﻿using ModelsLibrary;
using ModelsLibrary.RepositoryInterfaces;
using System.Data;
using System.Data.SqlClient;

namespace DataAccessLibrary.PurchaseOrderDetailRepository
{
    public class PurchaseOrderDetailRepository : IPurchaseOrderDetailRepository
    {
        private readonly IRelationalDataAccess _sqlDataAccess;

        public PurchaseOrderDetailRepository(IRelationalDataAccess sqlDataAccess)
        {
            _sqlDataAccess = sqlDataAccess;
        }

        public (IEnumerable<PurchaseOrderDetailModel>, string) GetAll()
        {
            List<PurchaseOrderDetailModel> purchaseOrderDetails = new List<PurchaseOrderDetailModel>();
            string? errorMessage = null;

            using (_sqlDataAccess.SQLConnection())
            {
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = _sqlDataAccess.SQLConnection();
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "dbo.usp_GetAllPurchaseOrderDetails";
                    command.Connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        //Check for errors
                        //Expecting more than one column if no error raised
                        if (reader.FieldCount > 1)
                        {
                            //Check for data
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    PurchaseOrderDetailModel purchaseOrderDetail = new PurchaseOrderDetailModel();
                                    purchaseOrderDetail.PurchaseOrderID = Convert.ToInt64(reader["PurchaseOrderID"]);
                                    purchaseOrderDetail.ProductID = Convert.ToInt32(reader["ProductID"]);
                                    purchaseOrderDetail.QuantityOrdered = Convert.ToInt32(reader["Quantity"]);
                                    purchaseOrderDetail.UnitCost = Convert.ToDecimal(reader["UnitCost"]);
                                    purchaseOrderDetail.UnitFreightCost = Convert.ToDecimal(reader["UnitFreightCost"]);
                                    purchaseOrderDetail.QuantityReceipted = Convert.ToInt32(reader["QuantityReceipted"]);
                                    purchaseOrderDetail.OrderLineStatusID = Convert.ToInt32(reader["OrderLineStatusID"]);

                                    purchaseOrderDetails.Add(purchaseOrderDetail);
                                }
                            }
                        }
                        else
                        {
                            //error raised
                            reader.Read();
                            errorMessage = reader["Message"].ToString();
                        }
                    }
                }
            }

            return (purchaseOrderDetails, errorMessage);//error message will be null if no error raised by database
        }

        public (IEnumerable<PurchaseOrderDetailModel>, string) GetByPurchaseOrderID(long id)
        {
            List<PurchaseOrderDetailModel> purchaseOrderDetails = new List<PurchaseOrderDetailModel>();
            string? errorMessage = null;

            using (_sqlDataAccess.SQLConnection())
            {
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = _sqlDataAccess.SQLConnection();
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "dbo.usp_GetPurchaseOrderDetailByPurchaseOrderID";
                    command.Parameters.Add("@PurchaseOrderID", SqlDbType.BigInt).Value = id;
                    command.Connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        //Check for errors
                        //Expecting more than one column if no error raised
                        if (reader.FieldCount > 1)
                        {
                            //Check for data
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    PurchaseOrderDetailModel purchaseOrderDetail = new PurchaseOrderDetailModel();
                                    purchaseOrderDetail.PurchaseOrderID = Convert.ToInt64(reader["PurchaseOrderID"]);
                                    purchaseOrderDetail.ProductID = Convert.ToInt32(reader["ProductID"]);
                                    purchaseOrderDetail.QuantityOrdered = Convert.ToInt32(reader["Quantity"]);
                                    purchaseOrderDetail.UnitCost = Convert.ToDecimal(reader["UnitCost"]);
                                    purchaseOrderDetail.UnitFreightCost = Convert.ToDecimal(reader["UnitFreightCost"]);
                                    purchaseOrderDetail.QuantityReceipted = Convert.ToInt32(reader["QuantityReceipted"]);
                                    purchaseOrderDetail.OrderLineStatusID = Convert.ToInt32(reader["OrderLineStatusID"]);

                                    purchaseOrderDetails.Add(purchaseOrderDetail);
                                }
                            }
                        }
                        else
                        {
                            //error raised
                            reader.Read();
                            errorMessage = reader["Message"].ToString();
                        }
                    }
                }
            }

            return (purchaseOrderDetails, errorMessage);//error message will be null if no error raised by database
        }

        public string Insert(PurchaseOrderDetailModel purchaseOrderDetail)
        {
            string? errorMessage = null;

            using (_sqlDataAccess.SQLConnection())
            {
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = _sqlDataAccess.SQLConnection();
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "dbo.usp_InsertPurchaseOrderDetail";
                    command.Parameters.Add("@PurchaseOrderID", SqlDbType.BigInt).Value = purchaseOrderDetail.PurchaseOrderID;
                    command.Parameters.Add("@ProductID", SqlDbType.Int).Value = purchaseOrderDetail.ProductID;
                    command.Parameters.Add("@Quantity", SqlDbType.Int).Value = purchaseOrderDetail.QuantityOrdered;
                    command.Parameters.Add("@UnitCost", SqlDbType.Money).Value = purchaseOrderDetail.UnitCost;
                    command.Parameters.Add("@UnitFreightCost", SqlDbType.Money).Value = purchaseOrderDetail.UnitFreightCost;
                    command.Parameters.Add("@OrderLineStatusID", SqlDbType.Int).Value = purchaseOrderDetail.OrderLineStatusID;
                    command.Connection.Open();

                    string returnedMessage = command.ExecuteScalar().ToString()!;

                    if (!returnedMessage.Equals("No Error"))
                    {
                        //Error raised
                        errorMessage = returnedMessage;
                    }
                }
            }

            return errorMessage; //error message will be null if no error raised by database
        }

        public string Update(PurchaseOrderDetailModel purchaseOrderDetail)
        {
            string? errorMessage = null;

            using (_sqlDataAccess.SQLConnection())
            {
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = _sqlDataAccess.SQLConnection();
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "dbo.usp_UpdatePurchaseOrderDetail";
                    command.Parameters.Add("@PurchaseOrderID", SqlDbType.BigInt).Value = purchaseOrderDetail.PurchaseOrderID;
                    command.Parameters.Add("@ProductID", SqlDbType.Int).Value = purchaseOrderDetail.ProductID;
                    command.Parameters.Add("@Quantity", SqlDbType.Int).Value = purchaseOrderDetail.QuantityOrdered;
                    command.Parameters.Add("@UnitCost", SqlDbType.Money).Value = purchaseOrderDetail.UnitCost;
                    command.Parameters.Add("@UnitFreightCost", SqlDbType.Money).Value = purchaseOrderDetail.UnitFreightCost;
                    command.Parameters.Add("@OrderLineStatusID", SqlDbType.Int).Value = purchaseOrderDetail.OrderLineStatusID;
                    command.Connection.Open();

                    string returnedMessage = command.ExecuteScalar().ToString()!;

                    if (!returnedMessage.Equals("No Error"))
                    {
                        //Error raised
                        errorMessage = returnedMessage;
                    }
                }
            }

            return errorMessage; //error message will be null if no error raised by database
        }
    }
}

using ModelsLibrary;
using ModelsLibrary.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLibrary.PurchaseOrderDetailRepository
{
    public class PurchaseOrderDetailRepository : IPurchaseOrderDetailRepository
    {
        private string _connectionString;

        public PurchaseOrderDetailRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public (IEnumerable<PurchaseOrderDetailModel>, string) GetAll()
        {
            List<PurchaseOrderDetailModel> purchaseOrderDetails = new List<PurchaseOrderDetailModel>();
            string? errorMessage = null;

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "dbo.usp_GetAllPurchaseOrderDetails";
                    connection.Open();

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
                                    purchaseOrderDetail.Quantity = Convert.ToInt32(reader["Quantity"]);
                                    purchaseOrderDetail.UnitCost = Convert.ToDecimal(reader["UnitCost"]);
                                    purchaseOrderDetail.UnitFreightCost = Convert.ToDecimal(reader["UnitFreightCost"]);
                                    purchaseOrderDetail.LineFilled = Convert.ToBoolean(reader["LineFilled"]);

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

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "dbo.usp_GetPurchaseOrderDetailByPurchaseOrderID";
                    command.Parameters.Add("@PurchaseOrderID", SqlDbType.BigInt).Value = id;
                    connection.Open();

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
                                    purchaseOrderDetail.Quantity = Convert.ToInt32(reader["Quantity"]);
                                    purchaseOrderDetail.UnitCost = Convert.ToDecimal(reader["UnitCost"]);
                                    purchaseOrderDetail.UnitFreightCost = Convert.ToDecimal(reader["UnitFreightCost"]);
                                    purchaseOrderDetail.LineFilled = Convert.ToBoolean(reader["LineFilled"]);

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

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "dbo.usp_InsertPurchaseOrderDetail";
                    command.Parameters.Add("@PurchaseOrderID", SqlDbType.BigInt).Value = purchaseOrderDetail.PurchaseOrderID;
                    command.Parameters.Add("@ProductID", SqlDbType.Int).Value = purchaseOrderDetail.ProductID;
                    command.Parameters.Add("@Quantity", SqlDbType.Int).Value = purchaseOrderDetail.Quantity;
                    command.Parameters.Add("@UnitCost", SqlDbType.Money).Value = purchaseOrderDetail.UnitCost;
                    command.Parameters.Add("@UnitFreightCost", SqlDbType.Money).Value = purchaseOrderDetail.UnitFreightCost;
                    command.Parameters.Add("@LineFilled", SqlDbType.Bit).Value = purchaseOrderDetail.LineFilled;
                    connection.Open();

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

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "dbo.usp_UpdatePurchaseOrderDetail";
                    command.Parameters.Add("@PurchaseOrderID", SqlDbType.BigInt).Value = purchaseOrderDetail.PurchaseOrderID;
                    command.Parameters.Add("@ProductID", SqlDbType.Int).Value = purchaseOrderDetail.ProductID;
                    command.Parameters.Add("@Quantity", SqlDbType.Int).Value = purchaseOrderDetail.Quantity;
                    command.Parameters.Add("@UnitCost", SqlDbType.Money).Value = purchaseOrderDetail.UnitCost;
                    command.Parameters.Add("@UnitFreightCost", SqlDbType.Money).Value = purchaseOrderDetail.UnitFreightCost;
                    command.Parameters.Add("@LineFilled", SqlDbType.Bit).Value = purchaseOrderDetail.LineFilled;
                    connection.Open();

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

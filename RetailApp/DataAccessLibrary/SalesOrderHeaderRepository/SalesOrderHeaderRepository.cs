using ModelsLibrary;
using ModelsLibrary.RepositoryInterfaces;
using System.Data;
using System.Data.SqlClient;

namespace DataAccessLibrary.SalesOrderHeaderRepository
{
    public class SalesOrderHeaderRepository : ISalesOrderHeaderRepository
    {
        private readonly IRelationalDataAccess _sqlDataAccess;

        public SalesOrderHeaderRepository(IRelationalDataAccess sqlDataAccess)
        {
            _sqlDataAccess = sqlDataAccess;
        }

        public (IEnumerable<SalesOrderHeaderModel>, string) GetAll()
        {
            List<SalesOrderHeaderModel> salesOrders = new List<SalesOrderHeaderModel>();
            string? errorMessage = null;

            using (SqlConnection connection = _sqlDataAccess.SQLConnection())
            {
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "dbo.usp_GetAllSalesOrderHeaders";
                    command.Connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        //check for errors
                        //expection more than one column if no error
                        if (reader.FieldCount > 1)
                        {
                            //No error
                            //Check for data
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    SalesOrderHeaderModel salesOrder = new SalesOrderHeaderModel();
                                    salesOrder.SalesOrderID = Convert.ToInt64(reader["SalesOrderID"]);
                                    salesOrder.CustomerID = Convert.ToInt32(reader["CustomerID"]);
                                    salesOrder.CustomerPurchaseOrder = reader["CustomerPurchaseOrder"].ToString()!;
                                    salesOrder.OrderDate = Convert.ToDateTime(reader["OrderDate"]);
                                    salesOrder.OrderAmount = Convert.ToDecimal(reader["OrderAmount"]);
                                    salesOrder.VATPercentage = Convert.ToDecimal(reader["VATPercentage"]);
                                    salesOrder.VATAmount = Convert.ToDecimal(reader["VATAmount"]);
                                    salesOrder.TotalAmount = Convert.ToDecimal(reader["TotalAmount"]);
                                    salesOrder.DeliveryDate = Convert.ToDateTime(reader["DeliveryDate"]);
                                    salesOrder.OrderStatusID = Convert.ToInt32(reader["OrderStatusID"]);

                                    salesOrders.Add(salesOrder);
                                }
                            }
                        }
                        else
                        {
                            //Error raised
                            reader.Read();
                            errorMessage = reader["Message"].ToString();
                        }
                    }
                }
            }

            return (salesOrders, errorMessage); //Error message will be null if no errors raised
        }

        public (SalesOrderHeaderModel, string) GetBySalesOrderID(long id)
        {
            SalesOrderHeaderModel salesOrder = new SalesOrderHeaderModel();
            string? errorMessage = null;

            using (SqlConnection connection = _sqlDataAccess.SQLConnection())
            {
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "dbo.usp_GetSalesOrderHeaderByID";
                    command.Parameters.Add("@SalesOrderID", SqlDbType.BigInt);
                    command.Connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        //check for errors
                        //expection more than one column if no error
                        if (reader.FieldCount > 1)
                        {
                            //No error
                            //Check for data
                            if (reader.HasRows)
                            {
                                reader.Read();
                                salesOrder.SalesOrderID = Convert.ToInt64(reader["SalesOrderID"]);
                                salesOrder.CustomerID = Convert.ToInt32(reader["CustomerID"]);
                                salesOrder.CustomerPurchaseOrder = reader["CustomerPurchaseOrder"].ToString()!;
                                salesOrder.OrderDate = Convert.ToDateTime(reader["OrderDate"]);
                                salesOrder.OrderAmount = Convert.ToDecimal(reader["OrderAmount"]);
                                salesOrder.VATPercentage = Convert.ToDecimal(reader["VATPercentage"]);
                                salesOrder.VATAmount = Convert.ToDecimal(reader["VATAmount"]);
                                salesOrder.TotalAmount = Convert.ToDecimal(reader["TotalAmount"]);
                                salesOrder.DeliveryDate = Convert.ToDateTime(reader["DeliveryDate"]);
                                salesOrder.OrderStatusID = Convert.ToInt32(reader["OrderStatusID"]);
                            }
                        }
                        else
                        {
                            //Error raised
                            reader.Read();
                            errorMessage = reader["Message"].ToString();
                        }
                    }
                }
            }

            return (salesOrder, errorMessage); //Error message will be null if no errors raised
        }

        public (SalesOrderHeaderModel, string) Insert(SalesOrderHeaderModel salesOrderHeader)
        {
            string? errorMessage = null;

            using (SqlConnection connection = _sqlDataAccess.SQLConnection())
            {
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "dbo.usp_InsertSalesOrderHeader";
                    command.Parameters.Add("@CustomerID", SqlDbType.Int).Value = salesOrderHeader.CustomerID;
                    command.Parameters.Add("@CustomerPurchaseOrder", SqlDbType.NVarChar).Value = salesOrderHeader.CustomerPurchaseOrder;
                    command.Parameters.Add("@OrderDate", SqlDbType.DateTime).Value = salesOrderHeader.OrderDate;
                    command.Parameters.Add("@OrderAmount", SqlDbType.Money).Value = salesOrderHeader.OrderAmount;
                    command.Parameters.Add("@VATPercentage", SqlDbType.Decimal).Value = salesOrderHeader.VATPercentage;
                    command.Parameters.Add("@VATAmount", SqlDbType.Money).Value = salesOrderHeader.VATAmount;
                    command.Parameters.Add("@TotalAmount", SqlDbType.Money).Value = salesOrderHeader.TotalAmount;
                    command.Parameters.Add("@DeliveryDate", SqlDbType.DateTime).Value = salesOrderHeader.DeliveryDate;
                    command.Parameters.Add("@OrderStatusID", SqlDbType.Int).Value = salesOrderHeader.OrderStatusID;
                    command.Connection.Open();

                    string returnedMessage = command.ExecuteScalar().ToString()!;
                    //check for errors
                    if (long.TryParse(returnedMessage, out _))
                    {
                        //no error as returned message can be parsed to a long
                        salesOrderHeader.SalesOrderID = long.Parse(returnedMessage);
                    }
                    else
                    {
                        //Error generated
                        errorMessage = returnedMessage;
                    }

                }
            }

            return (salesOrderHeader, errorMessage);//Error message will be null if no error raised
        }

        public string Update(SalesOrderHeaderModel salesOrderHeader)
        {
            string? errorMessage = null;

            using (SqlConnection connection = _sqlDataAccess.SQLConnection())
            {
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "dbo.usp_UpdateSalesOrderHeader";
                    command.Parameters.Add("@SalesOrderID", SqlDbType.BigInt).Value = salesOrderHeader.SalesOrderID;
                    command.Parameters.Add("@CustomerID", SqlDbType.Int).Value = salesOrderHeader.CustomerID;
                    command.Parameters.Add("@CustomerPurchaseOrder", SqlDbType.NVarChar).Value = salesOrderHeader.CustomerPurchaseOrder;
                    command.Parameters.Add("@OrderDate", SqlDbType.DateTime).Value = salesOrderHeader.OrderDate;
                    command.Parameters.Add("@OrderAmount", SqlDbType.Money).Value = salesOrderHeader.OrderAmount;
                    command.Parameters.Add("@VATPercentage", SqlDbType.Decimal).Value = salesOrderHeader.VATPercentage;
                    command.Parameters.Add("@VATAmount", SqlDbType.Money).Value = salesOrderHeader.VATAmount;
                    command.Parameters.Add("@TotalAmount", SqlDbType.Money).Value = salesOrderHeader.TotalAmount;
                    command.Parameters.Add("@DeliveryDate", SqlDbType.DateTime).Value = salesOrderHeader.DeliveryDate;
                    command.Parameters.Add("@OrderStatusID", SqlDbType.Int).Value = salesOrderHeader.OrderStatusID;
                    command.Connection.Open();

                    string returnedMessage = command.ExecuteScalar().ToString()!;
                    //check for errors
                    if (!returnedMessage.Equals("No Error"))
                    {
                        //Error generated
                        errorMessage = returnedMessage;
                    }

                }
            }

            return (errorMessage);//Error message will be null if no error raised
        }
    }
}

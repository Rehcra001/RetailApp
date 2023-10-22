using ModelsLibrary;
using ModelsLibrary.RepositoryInterfaces;
using System.Data;
using System.Data.SqlClient;

namespace DataAccessLibrary.SalesOrderDetailRepository
{
    public class SalesOrderDetailRepository : ISalesOrderDetailRepository
    {
        private readonly IRelationalDataAccess _sqlDataAccess;

        public SalesOrderDetailRepository(IRelationalDataAccess sqlDataAccess)
        {
            _sqlDataAccess = sqlDataAccess;
        }

        public (IEnumerable<SalesOrderDetailModel>, string) GetAll()
        {
            List<SalesOrderDetailModel> salesOrderDetails = new List<SalesOrderDetailModel>();
            string? errorMessage = null;

            using (SqlConnection connection = _sqlDataAccess.SQLConnection())
            {
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "dbo.usp_GetAllSalesOrderDetails";
                    command.Connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        //check for errors
                        //Expecting more than one column on no error
                        if (reader.FieldCount > 1)
                        {
                            //no errors
                            //check for data
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    SalesOrderDetailModel salesOrderDetail = new SalesOrderDetailModel();

                                    salesOrderDetail.SalesOrderID = Convert.ToInt64(reader["SalesOrderID"]);
                                    salesOrderDetail.ProductID = Convert.ToInt32(reader["ProductID"]);
                                    salesOrderDetail.QuantityOrdered = Convert.ToInt32(reader["Quantity"]);
                                    salesOrderDetail.UnitPrice = Convert.ToDecimal(reader["UnitPrice"]);
                                    salesOrderDetail.UnitCost = Convert.ToDecimal(reader["UnitCost"]);
                                    salesOrderDetail.Discount = Convert.ToDecimal(reader["Discount"]);
                                    salesOrderDetail.QuantityInvoiced = Convert.ToInt32(reader["QuantityInvoiced"]);
                                    salesOrderDetail.OrderLineStatusID = Convert.ToInt32(reader["OrderLineStatusID"]);

                                    salesOrderDetails.Add(salesOrderDetail);
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

                return (salesOrderDetails, errorMessage); //Error message will be null if no error raised
        }

        public (IEnumerable<SalesOrderDetailModel>, string) GetBySaleOrderID(long id)
        {
            List<SalesOrderDetailModel> salesOrderDetails = new List<SalesOrderDetailModel>();
            string? errorMessage = null;

            using (SqlConnection connection = _sqlDataAccess.SQLConnection())
            {
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "dbo.usp_GetSalesOrderDetailBySalesOrderID ";
                    command.Parameters.Add("@SalesOrderID", SqlDbType.BigInt).Value = id;
                    command.Connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        //check for errors
                        //Expecting more than one column on no error
                        if (reader.FieldCount > 1)
                        {
                            //no errors
                            //check for data
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    SalesOrderDetailModel salesOrderDetail = new SalesOrderDetailModel();

                                    salesOrderDetail.SalesOrderID = Convert.ToInt64(reader["SalesOrderID"]);
                                    salesOrderDetail.ProductID = Convert.ToInt32(reader["ProductID"]);
                                    salesOrderDetail.QuantityOrdered = Convert.ToInt32(reader["Quantity"]);
                                    salesOrderDetail.UnitPrice = Convert.ToDecimal(reader["UnitPrice"]);
                                    salesOrderDetail.UnitCost = Convert.ToDecimal(reader["UnitCost"]);
                                    salesOrderDetail.Discount = Convert.ToDecimal(reader["Discount"]);
                                    salesOrderDetail.QuantityInvoiced = Convert.ToInt32(reader["QuantityInvoiced"]);
                                    salesOrderDetail.OrderLineStatusID = Convert.ToInt32(reader["OrderLineStatusID"]);

                                    salesOrderDetails.Add(salesOrderDetail);
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

            return (salesOrderDetails, errorMessage); //Error message will be null if no error raised
        }

        public string Insert(SalesOrderDetailModel salesOrderDetail)
        {
            string? errorMessage = null;

            using (SqlConnection connection = _sqlDataAccess.SQLConnection())
            {
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "dbo.usp_InsertSalesOrderDetail";
                    command.Parameters.Add("@SalesOrderID", SqlDbType.BigInt).Value = salesOrderDetail.SalesOrderID;
                    command.Parameters.Add("@ProductID", SqlDbType.Int).Value = salesOrderDetail.ProductID;
                    command.Parameters.Add("@Quantity", SqlDbType.Int).Value = salesOrderDetail.QuantityOrdered;
                    command.Parameters.Add("@UnitPrice", SqlDbType.Money).Value = salesOrderDetail.UnitPrice;
                    command.Parameters.Add("@Discount", SqlDbType.Decimal).Value = salesOrderDetail.Discount;
                    command.Parameters.Add("@OrderLineStatusID", SqlDbType.Int).Value = salesOrderDetail.OrderLineStatusID;
                    command.Connection.Open();

                    string returnedMessage = command.ExecuteScalar().ToString()!;
                    if (!returnedMessage.Equals("No Error"))
                    {
                        //error raised
                        errorMessage = returnedMessage;
                    }
                }
            }

                return errorMessage; //Error message will be null if no error raised
        }

        public string Update(SalesOrderDetailModel salesOrderDetail)
        {
            string? errorMessage = null;

            using (SqlConnection connection = _sqlDataAccess.SQLConnection())
            {
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "dbo.usp_UpdateSalesOrderDetail";
                    command.Parameters.Add("@SalesOrderID", SqlDbType.BigInt).Value = salesOrderDetail.SalesOrderID;
                    command.Parameters.Add("@ProductID", SqlDbType.Int).Value = salesOrderDetail.ProductID;
                    command.Parameters.Add("@Quantity", SqlDbType.Int).Value = salesOrderDetail.QuantityOrdered;
                    command.Parameters.Add("@UnitPrice", SqlDbType.Money).Value = salesOrderDetail.UnitPrice;
                    command.Parameters.Add("@Discount", SqlDbType.Decimal).Value = salesOrderDetail.Discount;
                    command.Parameters.Add("@OrderLineStatusID", SqlDbType.Int).Value = salesOrderDetail.OrderLineStatusID;
                    command.Connection.Open();

                    string returnedMessage = command.ExecuteScalar().ToString()!;
                    if (!returnedMessage.Equals("No Error"))
                    {
                        //error raised
                        errorMessage = returnedMessage;
                    }
                }
            }

            return errorMessage; //Error message will be null if no error raised
        }
    }
}

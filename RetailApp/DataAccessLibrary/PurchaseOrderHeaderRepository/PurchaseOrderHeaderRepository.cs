using ModelsLibrary;
using ModelsLibrary.RepositoryInterfaces;
using System.Data;
using System.Data.SqlClient;

namespace DataAccessLibrary.PurchaseOrderHeaderRepository
{
    public class PurchaseOrderHeaderRepository : IPurchaseOrderHeaderRepository
    {
        private readonly IRelationalDataAccess _sqlDataAccess;

        public PurchaseOrderHeaderRepository(IRelationalDataAccess sqlDataAccess)
        {
            _sqlDataAccess = sqlDataAccess;
        }

        public (IEnumerable<PurchaseOrderHeaderModel>, string) GetALL()
        {
            List<PurchaseOrderHeaderModel> purchaseOrderHeaders = new List<PurchaseOrderHeaderModel>();
            string? errorMessage = null;

            using (_sqlDataAccess.SQLConnection())
            {
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = _sqlDataAccess.SQLConnection();
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "dbo.usp_GetAllPurchaseOrderHeaders";
                    command.Connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        //Check for errors
                        //Expecting more than one column on no error
                        if (reader.FieldCount > 1)
                        {
                            //no error
                            //check for data
                            if (reader.HasRows)
                            {
                                //Contains data
                                while (reader.Read())
                                {
                                    PurchaseOrderHeaderModel purchaseOrderHeader = new PurchaseOrderHeaderModel();
                                    purchaseOrderHeader.PurchaseOrderID = Convert.ToInt64(reader["PurchaseOrderID"]);
                                    purchaseOrderHeader.VendorID = Convert.ToInt32(reader["VendorID"]);
                                    purchaseOrderHeader.VendorReference = reader["VendorReference"].ToString();
                                    purchaseOrderHeader.OrderDate = Convert.ToDateTime(reader["OrderDate"]);
                                    purchaseOrderHeader.OrderAmount = Convert.ToDecimal(reader["OrderAmount"]);
                                    purchaseOrderHeader.VATPercentage = Convert.ToDecimal(reader["VATPercentage"]);
                                    purchaseOrderHeader.VATAmount = Convert.ToDecimal(reader["VATAmount"]);
                                    purchaseOrderHeader.TotalAmount = Convert.ToDecimal(reader["TotalAmount"]);
                                    purchaseOrderHeader.RequiredDate = (DateTime)reader["RequiredDate"];
                                    purchaseOrderHeader.OrderStatusID = Convert.ToInt32(reader["OrderStatusID"]);
                                    purchaseOrderHeader.IsImport = Convert.ToBoolean(reader["IsImport"]);

                                    purchaseOrderHeaders.Add(purchaseOrderHeader);
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

            return (purchaseOrderHeaders, errorMessage); //Error message will be null if no error raised by database
        }

        public (PurchaseOrderHeaderModel, string) GetByID(long id)
        {
            PurchaseOrderHeaderModel purchaseOrderHeader = new PurchaseOrderHeaderModel();
            string? errorMessage = null;

            using (_sqlDataAccess.SQLConnection())
            {
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = _sqlDataAccess.SQLConnection();
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "dbo.usp_GetPurchaseOrderHeaderByID";
                    command.Parameters.Add("@PurchaseOrderID", SqlDbType.BigInt).Value = id;
                    command.Connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        //Check for errors
                        //Expecting more than one column on no error
                        if (reader.FieldCount > 1)
                        {
                            //no error
                            //check for data
                            if (reader.HasRows)
                            {
                                //Contains data
                                reader.Read();
                                purchaseOrderHeader.PurchaseOrderID = Convert.ToInt64(reader["PurchaseOrderID"]);
                                purchaseOrderHeader.VendorID = Convert.ToInt32(reader["VendorID"]);
                                purchaseOrderHeader.VendorReference = reader["VendorReference"].ToString();
                                purchaseOrderHeader.OrderDate = Convert.ToDateTime(reader["OrderDate"]);
                                purchaseOrderHeader.OrderAmount = Convert.ToDecimal(reader["OrderAmount"]);
                                purchaseOrderHeader.VATPercentage = Convert.ToDecimal(reader["VATPercentage"]);
                                purchaseOrderHeader.VATAmount = Convert.ToDecimal(reader["VATAmount"]);
                                purchaseOrderHeader.TotalAmount = Convert.ToDecimal(reader["TotalAmount"]);
                                purchaseOrderHeader.RequiredDate = (DateTime)reader["RequiredDate"];
                                purchaseOrderHeader.OrderStatusID = Convert.ToInt32(reader["OrderStatusID"]);
                                purchaseOrderHeader.IsImport = Convert.ToBoolean(reader["IsImport"]);
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

            return (purchaseOrderHeader, errorMessage); //Error Message will be null if no error raised by database
        }

        public (IEnumerable<PurchaseOrderHeaderModel>, string) GetByOrderStatusID(int id)
        {
            List<PurchaseOrderHeaderModel> purchaseOrderHeaders = new List<PurchaseOrderHeaderModel>();
            string? errorMessage = null;

            using (_sqlDataAccess.SQLConnection())
            {
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = _sqlDataAccess.SQLConnection();
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "dbo.usp_GetPurchaseOrderHeaderByOrderStatus";
                    command.Parameters.Add("@OrderStatusID", SqlDbType.Int).Value = id;
                    command.Connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        //Check for errors
                        //Expecting more than one column on no error
                        if (reader.FieldCount > 1)
                        {
                            //no error
                            //check for data
                            if (reader.HasRows)
                            {
                                //Contains data
                                while (reader.Read())
                                {
                                    PurchaseOrderHeaderModel purchaseOrderHeader = new PurchaseOrderHeaderModel();
                                    purchaseOrderHeader.PurchaseOrderID = Convert.ToInt64(reader["PurchaseOrderID"]);
                                    purchaseOrderHeader.VendorID = Convert.ToInt32(reader["VendorID"]);
                                    purchaseOrderHeader.VendorReference = reader["VendorReference"].ToString();
                                    purchaseOrderHeader.OrderDate = Convert.ToDateTime(reader["OrderDate"]);
                                    purchaseOrderHeader.OrderAmount = Convert.ToDecimal(reader["OrderAmount"]);
                                    purchaseOrderHeader.VATPercentage = Convert.ToDecimal(reader["VATPercentage"]);
                                    purchaseOrderHeader.VATAmount = Convert.ToDecimal(reader["VATAmount"]);
                                    purchaseOrderHeader.TotalAmount = Convert.ToDecimal(reader["TotalAmount"]);
                                    purchaseOrderHeader.RequiredDate = (DateTime)reader["RequiredDate"];
                                    purchaseOrderHeader.OrderStatusID = Convert.ToInt32(reader["OrderStatusID"]);
                                    purchaseOrderHeader.IsImport = Convert.ToBoolean(reader["IsImport"]);

                                    purchaseOrderHeaders.Add(purchaseOrderHeader);
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

            return (purchaseOrderHeaders, errorMessage); //Error message will be null if no error raised by database
        }

        public (IEnumerable<PurchaseOrderHeaderModel>, string) GetByVendorID(int id)
        {
            List<PurchaseOrderHeaderModel> purchaseOrderHeaders = new List<PurchaseOrderHeaderModel>();
            string? errorMessage = null;

            using (_sqlDataAccess.SQLConnection())
            {
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = _sqlDataAccess.SQLConnection();
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "dbo.usp_GetPurchaseOrderHeaderVendorID";
                    command.Parameters.Add("@VendorID", SqlDbType.Int).Value = id;
                    command.Connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        //Check for errors
                        //Expecting more than one column on no error
                        if (reader.FieldCount > 1)
                        {
                            //no error
                            //check for data
                            if (reader.HasRows)
                            {
                                //Contains data
                                while (reader.Read())
                                {
                                    PurchaseOrderHeaderModel purchaseOrderHeader = new PurchaseOrderHeaderModel();
                                    purchaseOrderHeader.PurchaseOrderID = Convert.ToInt64(reader["PurchaseOrderID"]);
                                    purchaseOrderHeader.VendorID = Convert.ToInt32(reader["VendorID"]);
                                    purchaseOrderHeader.VendorReference = reader["VendorReference"].ToString();
                                    purchaseOrderHeader.OrderDate = Convert.ToDateTime(reader["OrderDate"]);
                                    purchaseOrderHeader.OrderAmount = Convert.ToDecimal(reader["OrderAmount"]);
                                    purchaseOrderHeader.VATPercentage = Convert.ToDecimal(reader["VATPercentage"]);
                                    purchaseOrderHeader.VATAmount = Convert.ToDecimal(reader["VATAmount"]);
                                    purchaseOrderHeader.TotalAmount = Convert.ToDecimal(reader["TotalAmount"]);
                                    purchaseOrderHeader.RequiredDate = (DateTime)reader["RequiredDate"];
                                    purchaseOrderHeader.OrderStatusID = Convert.ToInt32(reader["OrderStatusID"]);
                                    purchaseOrderHeader.IsImport = Convert.ToBoolean(reader["IsImport"]);

                                    purchaseOrderHeaders.Add(purchaseOrderHeader);
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

            return (purchaseOrderHeaders, errorMessage); //Error message will be null if no error raised by database
        }

        public (PurchaseOrderHeaderModel, string) Insert(PurchaseOrderHeaderModel purchaseOrderHeader)
        {
            string? errorMessage = null;

            using (_sqlDataAccess.SQLConnection())
            {
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = _sqlDataAccess.SQLConnection();
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "dbo.usp_InsertPurchaseOrderHeader";
                    command.Parameters.Add("@VendorID", SqlDbType.Int).Value = purchaseOrderHeader.VendorID;
                    command.Parameters.Add("@VendorReference", SqlDbType.NVarChar).Value = purchaseOrderHeader.VendorReference;
                    command.Parameters.Add("@OrderDate", SqlDbType.DateTime).Value = purchaseOrderHeader.OrderDate;
                    command.Parameters.Add("@OrderAmount", SqlDbType.Money).Value = purchaseOrderHeader.OrderAmount;
                    command.Parameters.Add("@VATPercentage", SqlDbType.Decimal).Value = purchaseOrderHeader.VATPercentage;
                    command.Parameters.Add("@VATAmount", SqlDbType.Money).Value = purchaseOrderHeader.VATAmount;
                    command.Parameters.Add("@TotalAmount", SqlDbType.Money).Value = purchaseOrderHeader.TotalAmount;
                    command.Parameters.Add("@RequiredDate", SqlDbType.Date).Value = purchaseOrderHeader.RequiredDate;
                    command.Parameters.Add("@OrderStatusID", SqlDbType.Int).Value = purchaseOrderHeader.OrderStatusID;
                    command.Parameters.Add("@IsImport", SqlDbType.Bit).Value = purchaseOrderHeader.IsImport;
                    command.Connection.Open();

                    string returnedMessage = command.ExecuteScalar().ToString()!;
                    //Check for errors
                    if (long.TryParse(returnedMessage, out _))
                    {
                        //no error
                        purchaseOrderHeader.PurchaseOrderID = long.Parse(returnedMessage);
                    }
                    else
                    {
                        //error raised
                        errorMessage = returnedMessage;
                    }
                }
            }
            return (purchaseOrderHeader, errorMessage);//Error message will be null if no error raised by database
        }

        public string Update(PurchaseOrderHeaderModel purchaseOrderHeader)
        {
            string? errorMessage = null;

            using (_sqlDataAccess.SQLConnection())
            {
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = _sqlDataAccess.SQLConnection();
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "dbo.usp_UpdatePurchaseOrderHeader";
                    command.Parameters.Add("@PurchaseOrderID", SqlDbType.BigInt).Value = purchaseOrderHeader.PurchaseOrderID;
                    command.Parameters.Add("@VendorID", SqlDbType.Int).Value = purchaseOrderHeader.VendorID;
                    command.Parameters.Add("@VendorReference", SqlDbType.NVarChar).Value = purchaseOrderHeader.VendorReference;
                    command.Parameters.Add("@OrderDate", SqlDbType.DateTime).Value = purchaseOrderHeader.OrderDate;
                    command.Parameters.Add("@OrderAmount", SqlDbType.Money).Value = purchaseOrderHeader.OrderAmount;
                    command.Parameters.Add("@VATPercentage", SqlDbType.Decimal).Value = purchaseOrderHeader.VATPercentage;
                    command.Parameters.Add("@VATAmount", SqlDbType.Money).Value = purchaseOrderHeader.VATAmount;
                    command.Parameters.Add("@TotalAmount", SqlDbType.Money).Value = purchaseOrderHeader.TotalAmount;
                    command.Parameters.Add("@RequiredDate", SqlDbType.Date).Value = purchaseOrderHeader.RequiredDate;
                    command.Parameters.Add("@OrderStatusID", SqlDbType.Int).Value = purchaseOrderHeader.OrderStatusID;
                    command.Parameters.Add("@IsImport", SqlDbType.Bit).Value = purchaseOrderHeader.IsImport;
                    command.Connection.Open();

                    string returnedMessage = command.ExecuteScalar().ToString()!;

                    if (!returnedMessage.Equals("No Error"))
                    {
                        //Error raised
                        errorMessage = returnedMessage;
                    }
                }
            }

            return errorMessage; //Will be null if no error raised by the database
        }
    }
}

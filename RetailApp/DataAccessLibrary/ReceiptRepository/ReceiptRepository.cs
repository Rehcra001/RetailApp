using ModelsLibrary;
using ModelsLibrary.RepositoryInterfaces;
using System.Data;
using System.Data.SqlClient;

namespace DataAccessLibrary.ReceiptRepository
{
    public class ReceiptRepository : IReceiptsRepository
    {
        private string _connectionString;

        public ReceiptRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public (IEnumerable<ReceiptModel>, string) GetByPurchaseOrderID(long id)
        {
            List<ReceiptModel> receipts = new List<ReceiptModel>();
            string? errorMessage = null;

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "dbo.ups_GetReceiptsByPurchaseOrderID";
                    command.Parameters.Add("@PurchaseOrderID", SqlDbType.BigInt).Value = id;
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        //check for error
                        //Expecting more than one column on no error
                        if (reader.FieldCount > 1)
                        {
                            //No error
                            //Check for data
                            if (reader.HasRows)
                            {
                                //read in receipts
                                while (reader.Read())
                                {
                                    ReceiptModel receipt = new ReceiptModel();
                                    receipt.ReceiptID = Convert.ToInt32(reader["ReceiptID"]);
                                    receipt.PurchaseOrderID = Convert.ToInt64(reader["PurchaseOrderID"]);
                                    receipt.ProductID = Convert.ToInt32(reader["ProductID"]);
                                    receipt.ReceiptDate = Convert.ToDateTime(reader["ReceiptDate"]);
                                    receipt.QuantityReceipted = Convert.ToInt32(reader["QuantityReceipted"]);
                                    receipt.UnitCost = Convert.ToDecimal(reader["UnitCost"]);

                                    receipts.Add(receipt);
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

            return (receipts, errorMessage);//Error message will be null if no error raised by database
        }

        public (ReceiptModel, string) Insert(ReceiptModel receipt)
        {
            string? errorMessage = null;
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "dbo.usp_InsertReceipt";
                    command.Parameters.Add("@PurchaseOrderID", SqlDbType.BigInt).Value = receipt.PurchaseOrderID;
                    command.Parameters.Add("@ProductID", SqlDbType.Int).Value = receipt.ProductID;
                    command.Parameters.Add("@QuantityReceipted", SqlDbType.Int).Value = receipt.QuantityReceipted;
                    command.Parameters.Add("@UnitCost", SqlDbType.Money).Value = receipt.UnitCost;
                    connection.Open();

                    string returnedMessage = command.ExecuteScalar().ToString()!;

                    //check for error
                    //Expecting ID back on no error
                    if (int.TryParse(returnedMessage, out _))
                    {
                        //ID Returned
                        receipt.ReceiptID = int.Parse(returnedMessage);
                    }
                    else
                    {
                        //Error raised
                        errorMessage = returnedMessage;
                    }
                }
            }
            return (receipt, errorMessage); //error message will be null if no error raised by database
        }
    }
}

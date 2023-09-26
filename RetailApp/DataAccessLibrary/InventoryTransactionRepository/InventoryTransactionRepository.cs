using ModelsLibrary;
using ModelsLibrary.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace DataAccessLibrary.InventoryTransactionRepository
{
    public class InventoryTransactionRepository : IInventoryTransactionRepository
    {
        private readonly string _connectionString;

        public InventoryTransactionRepository(string connectionString)
        {
            _connectionString = connectionString;
        }
        public (IEnumerable<InventoryTransactionModel>, string) GetByProductID(int id)
        {
            List<InventoryTransactionModel> inventoryTransactions = new List<InventoryTransactionModel>();
            string? errorMessage = null;

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "dbo.usp_GetInventoryTransactionsByProductID";
                    command.Parameters.Add("@ProductID", SqlDbType.Int).Value = id;
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        //Check if more than one column returned
                        //If only one returned then error raised
                        if (reader.FieldCount > 1)
                        {
                            //No error
                            //check if any data returned
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    InventoryTransactionModel inventoryTransaction = new InventoryTransactionModel();

                                    inventoryTransaction.TransactionID = Convert.ToInt32(reader["TransactionID"]);
                                    inventoryTransaction.TransactionType = reader["TransactionType"].ToString();
                                    inventoryTransaction.TransactionDate = Convert.ToDateTime(reader["TransactionDate"]);
                                    inventoryTransaction.ProductID = Convert.ToInt32(reader["ProductID"]);
                                    inventoryTransaction.OrderID = Convert.ToInt32(reader["OrderID"]);
                                    inventoryTransaction.Quantity = Convert.ToInt32(reader["Quantity"]);

                                    inventoryTransactions.Add(inventoryTransaction);
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

            return (inventoryTransactions, errorMessage); //Error message will be null if no error raised
        }
    }
}

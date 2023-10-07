using ModelsLibrary;
using ModelsLibrary.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLibrary.StatusRepository
{
    public class StatusRepository : IStatusRepository
    {
        private string _connectionString;

        public StatusRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public (IEnumerable<StatusModel>, string) GetAll()
        {
            List<StatusModel> orderStatuses = new List<StatusModel>();
            string? errorMessage = null;

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "dbo.usp_GetAllOrderStatus";
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        //Check for errors
                        //Expecting more than one column on no error
                        if (reader.FieldCount > 1)
                        {
                            //Check for data
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    StatusModel orderStatus = new StatusModel();
                                    orderStatus.StatusID = Convert.ToInt32(reader["OrderStatusID"]);
                                    orderStatus.Status = reader["OrderStatus"].ToString();

                                    orderStatuses.Add(orderStatus);
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

            return (orderStatuses, errorMessage); //Error message will be null if no error raised by database
        }

        public (StatusModel, string) GetByID(int id)
        {
            StatusModel orderStatus = new StatusModel();
            string? errorMessage = null;

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "dbo.usp_GetOrderStatusByID";
                    command.Parameters.Add("@OrderStatusID", SqlDbType.Int).Value = id;
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        //Check for errors
                        //Expecting more than one column on no error
                        if (reader.FieldCount > 1)
                        {
                            //Check for data
                            if (reader.HasRows)
                            {
                                reader.Read();
                                orderStatus.StatusID = Convert.ToInt32(reader["OrderStatusID"]);
                                orderStatus.Status = reader["OrderStatus"].ToString();
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

            return (orderStatus, errorMessage); //Error message will be null if no error raised by database
        }
    }
}

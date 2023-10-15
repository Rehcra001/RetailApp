using ModelsLibrary;
using ModelsLibrary.RepositoryInterfaces;
using System.Data;
using System.Data.SqlClient;

namespace DataAccessLibrary.StatusRepository
{
    public class StatusRepository : IStatusRepository
    {
        private readonly IRelationalDataAccess _sqlDataAccess;

        public StatusRepository(IRelationalDataAccess sqlDataAccess)
        {
            _sqlDataAccess = sqlDataAccess;
        }

        public (IEnumerable<StatusModel>, string) GetAll()
        {
            List<StatusModel> statuses = new List<StatusModel>();
            string? errorMessage = null;

            using (_sqlDataAccess.SQLConnection())
            {
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = _sqlDataAccess.SQLConnection();
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "dbo.usp_GetAllStatus";
                    command.Connection.Open();

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
                                    StatusModel status = new StatusModel();
                                    status.StatusID = Convert.ToInt32(reader["StatusID"]);
                                    status.Status = reader["Status"].ToString();

                                    statuses.Add(status);
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

            return (statuses, errorMessage); //Error message will be null if no error raised by database
        }

        public (StatusModel, string) GetByID(int id)
        {
            StatusModel status = new StatusModel();
            string? errorMessage = null;

            using (_sqlDataAccess.SQLConnection())
            {
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = _sqlDataAccess.SQLConnection();
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "dbo.usp_GetStatusByID";
                    command.Parameters.Add("@StatusID", SqlDbType.Int).Value = id;
                    command.Connection.Open();

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
                                status.StatusID = Convert.ToInt32(reader["StatusID"]);
                                status.Status = reader["Status"].ToString();
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

            return (status, errorMessage); //Error message will be null if no error raised by database
        }
    }
}

using ModelsLibrary;
using ModelsLibrary.RepositoryInterfaces;
using System.Data;
using System.Data.SqlClient;

namespace DataAccessLibrary.UnitsPerRepository
{
    public class UnitsPerRepository : IUnitsPerRepository
    {
        private string _connectionString;

        public UnitsPerRepository(string connectionString)
        {
            _connectionString = connectionString;
        }
        public (IEnumerable<UnitsPerModel>, string) GetAll()
        {
            string? errorMessage = null;
            List<UnitsPerModel> unitsPers = new List<UnitsPerModel>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "dbo.usp_GetAllUnits";
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.FieldCount > 1) //If more than one column is returned then no error raised
                        {
                            if (reader.HasRows) //Will return empty set if no units exist yet
                            {

                                while (reader.Read())
                                {
                                    UnitsPerModel unitsPer = new UnitsPerModel();
                                    unitsPer.UnitPerID = Convert.ToInt32(reader["UnitPerID"]);
                                    unitsPer.UnitPer = reader["UnitPer"].ToString();
                                    unitsPer.UnitPerDescription = reader["UnitPerDescription"].ToString();
                                    unitsPers.Add(unitsPer); 
                                }
                            }
                        }
                        else
                        {
                            reader.Read();
                            errorMessage = reader["Message"].ToString();
                        }
                    }
                }
            }

                return (unitsPers, errorMessage); //errorMessage will be null if no error raised by the database
        }

        public (UnitsPerModel, string) Insert(UnitsPerModel unitsPer)
        {
            string? errorMessage = null;

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "dbo.usp_InsertUnit";
                    command.Parameters.Add("@UnitPer", SqlDbType.NVarChar).Value = unitsPer.UnitPer;
                    command.Parameters.Add("@UnitPerDescription", SqlDbType.NVarChar).Value = unitsPer.UnitPerDescription;
                    connection.Open();

                    string returnMessage = command.ExecuteScalar().ToString()!;

                    if (int.TryParse(returnMessage, out _))
                    {
                        //UnitsPerID returned
                        unitsPer.UnitPerID = int.Parse(returnMessage);
                    }
                    else
                    {
                        //Error returned
                        errorMessage = returnMessage;
                    }
                }
            }

            return (unitsPer, errorMessage); //errorMessage will be null if no error is raised by the database
        }

        public string Update(UnitsPerModel unitsPer)
        {
            throw new NotImplementedException();
        }
    }
}

using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;

namespace DataAccessLibrary
{
    public class RelationalDataAccess : IRelationalDataAccess
    {
        private readonly IConfiguration _config;
        public RelationalDataAccess(IConfiguration config)
        {
            _config = config;
        }

        public SqlConnection SQLConnection()
        {
            SqlConnection connection = new SqlConnection(_config.GetConnectionString("SqlRetailAppDB"));

            return connection;
        }
    }
}

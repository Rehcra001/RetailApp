using System.Data.SqlClient;

namespace DataAccessLibrary
{
    public interface IRelationalDataAccess
    {
        SqlConnection SQLConnection();
    }
}
using ModelsLibrary;
using ModelsLibrary.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLibrary.IssuesRepository
{
    public class IssuesRepository : IIssueRepository
    {
        private readonly IRelationalDataAccess _sqlDataAccess;

        public IssuesRepository(IRelationalDataAccess sqlDataAccess)
        {
            _sqlDataAccess = sqlDataAccess;
        }

        public (IEnumerable<IssueModel>, string) GetBySalesOrderID(long id)
        {
            List<IssueModel> issues = new List<IssueModel>();
            string? errorMessage = null;

            using (SqlConnection connection = _sqlDataAccess.SQLConnection())
            {
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "dbo.usp_GetIssuesBySalesOrderID";
                    command.Parameters.Add("@SalesOrderID", SqlDbType.BigInt).Value = id;
                    command.Connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        //Check for errors
                        //Expecting more than one column on no error
                        if (reader.FieldCount > 1)
                        {
                            //no error
                            //checkf for data
                            if (reader.HasRows)
                            {
                                //read in data
                                while (reader.Read())
                                {
                                    IssueModel issue = new IssueModel();
                                    issue.IssueID = Convert.ToInt32(reader["IssuedID"]);
                                    issue.SalesOrderID = Convert.ToInt64(reader["SalesOrderID"]);
                                    issue.ProductID = Convert.ToInt32(reader["ProductID"]);
                                    issue.IssueDate = Convert.ToDateTime(reader["IssueDate"]);
                                    issue.QuantityIssued = Convert.ToInt32(reader["QuantityIssued"]);
                                    issue.UnitCost = Convert.ToDecimal(reader["UnitCost"]);
                                    if (reader["ReverseReferenceID"] is DBNull)
                                    {
                                        issue.ReverseReferenceID = 0;
                                    }
                                    else
                                    {
                                        issue.ReverseReferenceID = Convert.ToInt32(reader["ReverseReferenceID"]);
                                    }

                                    issues.Add(issue);
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
                return (issues, errorMessage);//error message will be null if no error raised
            }
        }

        public (IssueModel, string) Insert(IssueModel issue)
        {
            string? errorMessage = null;

            using (SqlConnection connection = _sqlDataAccess.SQLConnection())
            {
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "dbo.usp_InsertGoodsIssued";
                    command.Parameters.Add("@SalesOrderID", SqlDbType.BigInt).Value = issue.SalesOrderID;
                    command.Parameters.Add("@ProductID", SqlDbType.Int).Value = issue.ProductID;
                    command.Parameters.Add("@QuantityIssued", SqlDbType.Int).Value = issue.QuantityIssued;
                    command.Connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        //check for errors
                        //Expecting more than one column on no error
                        if (reader.FieldCount > 1)
                        {
                            //no error
                            //check for data
                            if (reader.HasRows)
                            {
                                //Only expecting one record returned
                                reader.Read();
                                //update the issue model with data returned
                                issue.IssueID = Convert.ToInt32(reader["IssuedID"]);
                                issue.IssueDate = Convert.ToDateTime(reader["IssueDate"]);
                                issue.UnitCost = Convert.ToDecimal(reader["UnitCost"]);
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
            return (issue, errorMessage); //Error message will be null if no error raised by datasource
        }

        public string ReverseByID(int id)
        {
            string? errorMessage = null;

            using (SqlConnection connection = _sqlDataAccess.SQLConnection())
            {
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "dbo.usp_ReverseIssueByID";
                    command.Parameters.Add("@IssueID", SqlDbType.Int).Value = id;
                    command.Connection.Open();

                    string returnedMessage = command.ExecuteScalar().ToString()!;

                    //check for errors
                    if (!returnedMessage.Equals("No Error"))
                    {
                        //error raised
                        errorMessage = returnedMessage;
                    }
                }
            }
            return (errorMessage); //will be null if no error raised by the datasource
        }
    }
}

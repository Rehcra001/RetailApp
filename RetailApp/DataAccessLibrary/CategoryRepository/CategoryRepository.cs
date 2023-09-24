using ModelsLibrary;
using ModelsLibrary.RepositoryInterfaces;
using System.Data;
using System.Data.SqlClient;

namespace DataAccessLibrary.CategoryRepository
{
    public class CategoryRepository : ICategoryRepository
    {
        private string _connectionSting;

        public CategoryRepository(string connectionSting)
        {
            _connectionSting = connectionSting;
        }

        public (IEnumerable<CategoryModel>, string) GetAll()
        {
            List<CategoryModel> categories = new List<CategoryModel>();
            string? errorMessage = null;

            using (SqlConnection connection = new SqlConnection(_connectionSting))
            {
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "dbo.usp_GetAllCategories";
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        //Check if more than one column returned - two expected on no error raised
                        if (reader.FieldCount > 1)
                        {
                            //No error
                            //Check if dataset empty
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    CategoryModel category = new CategoryModel();

                                    category.CategoryID = Convert.ToInt32(reader["CategoryID"]);
                                    category.CategoryName = reader["CategoryName"].ToString();

                                    categories.Add(category);
                                }
                            }
                        }
                        else
                        {
                            //Error raised
                            reader.Read();
                            errorMessage = reader["Message"].ToString();
                        }
                    }
                }
            }

            return (categories, errorMessage); //error message will be null if no error raised
        }

        public (CategoryModel, string) GetByID(int id)
        {
            CategoryModel category = new CategoryModel();
            string? errorMessage = null;

            using (SqlConnection connection = new SqlConnection(_connectionSting))
            {
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "dbo.usp_GetCategoryByID";
                    command.Parameters.Add("@CategoryID", SqlDbType.Int).Value = id;
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        //Check if dataset has more than one column - two expected if no error
                        if (reader.FieldCount > 1)
                        {
                            //Check if dataset has data
                            if (reader.HasRows)
                            {
                                //Only has a single row of data
                                reader.Read();
                                category.CategoryID = Convert.ToInt32(reader["CategoryID"]);
                                category.CategoryName = reader["CategoryName"].ToString();
                            }
                        }
                        else
                        {
                            //Error raised
                            reader.Read();
                            errorMessage = reader["Message"].ToString();
                        }
                    }
                }
            }
            return (category, errorMessage);// ErrorMessage will be null if no error is raised by database
        }

        public (CategoryModel, string) Insert(CategoryModel category)
        {
            string? errorMessage = null;

            using (SqlConnection connection = new SqlConnection(_connectionSting))
            {
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "dbo.usp_InsertCategory";
                    command.Parameters.Add("@CategoryName", SqlDbType.NVarChar).Value = category.CategoryName;
                    connection.Open();

                    //Only one column and row will be returned
                    string returnMessage = command.ExecuteScalar().ToString()!;

                    //Check if ID or error message returned
                    if (int.TryParse(returnMessage, out _))
                    {
                        //ID returned
                        category.CategoryID = int.Parse(returnMessage);
                    }
                    else
                    {
                        //error raised by database
                        errorMessage = returnMessage;
                    }
                }
            }
            return (category, errorMessage);//errorMessage will be null if no error raised by the database
        }

        public string Update(CategoryModel category)
        {
            string? errorMessage = null;

            using (SqlConnection connection = new SqlConnection(_connectionSting))
            {
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "dbo.usp_UpdateCategory";
                    command.Parameters.Add("@CategoryID", SqlDbType.Int).Value = category.CategoryID;
                    command.Parameters.Add("@CategoryName", SqlDbType.NVarChar).Value = category.CategoryName;
                    connection.Open();

                    //"No Error" returned on success.
                    //error message return on failiure
                    string returnMessage = command.ExecuteScalar().ToString()!;

                    if (!returnMessage.Equals("No Error"))
                    {
                        //Error raised
                        errorMessage = returnMessage;
                    }
                }
            }
            return errorMessage; //Will be null if no error raised by database
        }
    }
}

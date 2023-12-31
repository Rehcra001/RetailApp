﻿using ModelsLibrary;
using ModelsLibrary.RepositoryInterfaces;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography.X509Certificates;

namespace DataAccessLibrary.ProductRepository
{
    public class ProductRepository : IProductRepository
    {
        private readonly IRelationalDataAccess _sqlDataAccess;

        public ProductRepository(IRelationalDataAccess sqlDataAccess)
        {
            _sqlDataAccess = sqlDataAccess;
        }

        public (IEnumerable<ProductModel>, string) GetAll()
        {
            string? errorMessage = null;
            List<ProductModel> products = new List<ProductModel>();

            using (_sqlDataAccess.SQLConnection())
            {
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = _sqlDataAccess.SQLConnection();
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "dbo.usp_GetAllProducts";
                    command.Connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        //No error raised if more than one column returned
                        if (reader.FieldCount > 1)
                        {
                            //Check if any rows returned.
                            //Will not have any rows if no products exist yet
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    ProductModel product = new ProductModel();

                                    product.ProductID = Convert.ToInt32(reader["ProductID"]);
                                    product.ProductName = reader["ProductName"].ToString();
                                    product.ProductDescription = reader["ProductDescription"].ToString();
                                    product.VendorID = Convert.ToInt32(reader["VendorID"]);
                                    product.VendorProductName = reader["VendorProductName"].ToString();
                                    product.UnitPrice = Convert.ToDecimal(reader["UnitPrice"]);
                                    product.UnitCost = Convert.ToDecimal(reader["UnitCost"]);
                                    product.OnHand = Convert.ToInt32(reader["OnHand"]);
                                    product.OnOrder = Convert.ToInt32(reader["OnOrder"]);
                                    product.SalesDemand = Convert.ToInt32(reader["SalesDemand"]);
                                    product.ReorderPoint = Convert.ToInt32(reader["ReorderPoint"]);
                                    product.UnitPerID = Convert.ToInt32(reader["UnitPerID"]);
                                    product.UnitWeight = Convert.ToSingle(reader["UnitWeight"]);
                                    product.Obsolete = Convert.ToBoolean(reader["Obsolete"]);
                                    product.CategoryID = Convert.ToInt32(reader["CategoryID"]);
                                    products.Add(product);
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

                return (products, errorMessage);//errorMessage is null if no error raised by database
        }

        public (ProductModel, string) GetByID(int id)
        {
            string? errorMessage = null;
            ProductModel product = new ProductModel();

            using (_sqlDataAccess.SQLConnection())
            {
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = _sqlDataAccess.SQLConnection();
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "dbo.usp_GetProductByID";
                    command.Parameters.Add("@ProductID", SqlDbType.Int).Value = id;
                    command.Connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        //No error raised if more than one column returned
                        if (reader.FieldCount > 1)
                        {
                            //Check if any rows returned.
                            //Will not have any rows if no products exist yet
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    product.ProductID = Convert.ToInt32(reader["ProductID"]);
                                    product.ProductName = reader["ProductName"].ToString();
                                    product.ProductDescription = reader["ProductDescription"].ToString();
                                    product.VendorID = Convert.ToInt32(reader["VendorID"]);
                                    product.VendorProductName = reader["VendorProductName"].ToString();
                                    product.UnitPrice = Convert.ToDecimal(reader["UnitPrice"]);
                                    product.UnitCost = Convert.ToDecimal(reader["UnitCost"]);
                                    product.OnHand = Convert.ToInt32(reader["OnHand"]);
                                    product.OnOrder = Convert.ToInt32(reader["OnOrder"]);
                                    product.SalesDemand = Convert.ToInt32(reader["SalesDemand"]);
                                    product.ReorderPoint = Convert.ToInt32(reader["ReorderPoint"]);
                                    product.UnitPerID = Convert.ToInt32(reader["UnitPerID"]);
                                    product.UnitWeight = Convert.ToSingle(reader["UnitWeight"]);
                                    product.Obsolete = Convert.ToBoolean(reader["Obsolete"]);
                                    product.CategoryID = Convert.ToInt32(reader["CategoryID"]);
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

            return (product, errorMessage);//errorMessage is null if no error raised by database
        }

        public (ProductModel, string) Insert(ProductModel product)
        {
            string? errorMessage = null;

            using (_sqlDataAccess.SQLConnection())
            {
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = _sqlDataAccess.SQLConnection();
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "dbo.usp_InsertProduct";
                    command.Parameters.Add("@ProductName", SqlDbType.NVarChar).Value = product.ProductName;
                    command.Parameters.Add("@ProductDescription", SqlDbType.NVarChar).Value = product.ProductDescription;
                    command.Parameters.Add("@VendorID", SqlDbType.Int).Value = product.VendorID;
                    command.Parameters.Add("@VendorProductName", SqlDbType.NVarChar).Value = product.VendorProductName;
                    command.Parameters.Add("@UnitPrice", SqlDbType.Money).Value = product.UnitPrice;
                    command.Parameters.Add("@ReorderPoint", SqlDbType.Int).Value = product.ReorderPoint;
                    command.Parameters.Add("@UnitPerID", SqlDbType.Int).Value = product.UnitPerID;
                    command.Parameters.Add("@UnitWeight", SqlDbType.Float).Value = product.UnitWeight;
                    command.Parameters.Add("@CategoryID", SqlDbType.Int).Value = product.CategoryID;
                    command.Connection.Open();

                    string returnedMessage = command.ExecuteScalar().ToString()!;
                    if (int.TryParse(returnedMessage, out _))
                    {
                        //product ID returned
                        product.ProductID = int.Parse(returnedMessage);
                    }
                    else
                    {
                        //Error raised
                        errorMessage = returnedMessage;
                    }

                }
            }

                return (product, errorMessage);//errorMessage is null if no error raised by database
        }

        public string Update(ProductModel product)
        {
            string? errorMessage = null;

            using (_sqlDataAccess.SQLConnection())
            {
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = _sqlDataAccess.SQLConnection();
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "dbo.usp_UpdateProduct";
                    command.Parameters.Add("@ProductID", SqlDbType.Int).Value = product.ProductID;
                    command.Parameters.Add("@ProductName", SqlDbType.NVarChar).Value = product.ProductName;
                    command.Parameters.Add("@ProductDescription", SqlDbType.NVarChar).Value = product.ProductDescription;
                    command.Parameters.Add("@VendorID", SqlDbType.Int).Value = product.VendorID;
                    command.Parameters.Add("@VendorProductName", SqlDbType.NVarChar).Value = product.VendorProductName;
                    command.Parameters.Add("@UnitPrice", SqlDbType.Money).Value = product.UnitPrice;
                    command.Parameters.Add("@ReorderPoint", SqlDbType.Int).Value = product.ReorderPoint;
                    command.Parameters.Add("@UnitPerID", SqlDbType.Int).Value = product.UnitPerID;
                    command.Parameters.Add("@UnitWeight", SqlDbType.Float).Value = product.UnitWeight;
                    command.Parameters.Add("@CategoryID", SqlDbType.Int).Value = product.CategoryID;
                    command.Connection.Open();

                    string returnedMessage = command.ExecuteScalar().ToString()!;
                    if (!returnedMessage.Equals("No Error"))
                    {
                        //Error raised
                        errorMessage = returnedMessage;
                    }

                }
            }

            return (errorMessage);//errorMessage is null if no error raised by database
        }

        public (IEnumerable<ProductModel>, string) GetByVendorID(int id)
        {
            string? errorMessage = null;
            List<ProductModel> products = new List<ProductModel>();

            using (_sqlDataAccess.SQLConnection())
            {
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = _sqlDataAccess.SQLConnection();
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "dbo.usp_GetProductsByVendorID";
                    command.Parameters.Add("VendorID", SqlDbType.Int).Value = id;
                    command.Connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        //No error raised if more than one column returned
                        if (reader.FieldCount > 1)
                        {
                            //Check if any rows returned.
                            //Will not have any rows if no products exist yet
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    ProductModel product = new ProductModel();

                                    product.ProductID = Convert.ToInt32(reader["ProductID"]);
                                    product.ProductName = reader["ProductName"].ToString();
                                    product.ProductDescription = reader["ProductDescription"].ToString();
                                    product.VendorID = Convert.ToInt32(reader["VendorID"]);
                                    product.VendorProductName = reader["VendorProductName"].ToString();
                                    product.UnitPrice = Convert.ToDecimal(reader["UnitPrice"]);
                                    product.UnitCost = Convert.ToDecimal(reader["UnitCost"]);
                                    product.OnHand = Convert.ToInt32(reader["OnHand"]);
                                    product.OnOrder = Convert.ToInt32(reader["OnOrder"]);
                                    product.SalesDemand = Convert.ToInt32(reader["SalesDemand"]);
                                    product.ReorderPoint = Convert.ToInt32(reader["ReorderPoint"]);
                                    product.UnitPerID = Convert.ToInt32(reader["UnitPerID"]);
                                    product.UnitWeight = Convert.ToSingle(reader["UnitWeight"]);
                                    product.Obsolete = Convert.ToBoolean(reader["Obsolete"]);
                                    product.CategoryID = Convert.ToInt32(reader["CategoryID"]);
                                    products.Add(product);
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

            return (products, errorMessage);//errorMessage is null if no error raised by database
        }

    }
}

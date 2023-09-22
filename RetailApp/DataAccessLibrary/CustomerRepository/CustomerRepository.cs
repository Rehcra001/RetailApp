using ModelsLibrary;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLibrary.CustomerRepository
{
    public class CustomerRepository : ICustomerRepository
    {
        private string _connectionString;
        public CustomerRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        /// <summary>
        /// Deletes a customer
        /// </summary>
        /// <param name="customer">
        /// Takes in the customer model to be deleted
        /// </param>
        /// <returns>
        /// Returns a string error message if the delete was not successful - null otherwise
        /// </returns>
        public string Delete(CustomerModel customer)
        {
            string? errorMessage = null;

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "dbo.usp_DeleteCustomer";
                    command.Parameters.Add("@CustomerID", SqlDbType.Int).Value = customer.CustomerID;
                    connection.Open();

                    //if not error then "No Error" string is returned
                    //Otherwise an error message is returned
                    string returnMessage = command.ExecuteScalar().ToString()!;
                    if (!returnMessage.Equals("No Error"))
                    {
                        errorMessage = returnMessage;
                    }
                }
            }
            return errorMessage;
        }

        /// <summary>
        /// Gets a list of all customers
        /// </summary>
        /// <returns>
        /// Return an observable collection of customers if successfull - null otherwise
        /// Returns a string error message if not successfull - null otherwise
        /// </returns>
        public (IEnumerable<CustomerModel>, string) GetAll()
        {
            List<CustomerModel> customers = new List<CustomerModel>();
            string? errorMessage = null;

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "dbo.usp_GetAllCustomers";
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.FieldCount > 1) //Field count is 1 if an error is returned
                        {
                            //No errors
                            while (reader.Read())
                            {
                                CustomerModel customer = new CustomerModel();
                                customer.CustomerID = Convert.ToInt32(reader["CustomerID"]);
                                customer.FirstName = reader["FirstName"].ToString();
                                customer.LastName = reader["LastName"].ToString();
                                customer.CompanyName = reader["CompanyName"].ToString();
                                customer.VatRegistrationNumber = reader["VatRegistrationNumber"].ToString();
                                customer.AddressLine1 = reader["AddressLine1"].ToString();
                                customer.AddressLine2 = reader["AddressLine2"].ToString();
                                customer.City = reader["City"].ToString();
                                customer.Province = reader["Province"].ToString();
                                customer.PostalCode = reader["PostalCode"].ToString();
                                customer.EmailAddress = reader["EMailAddress"].ToString();
                                customer.PhoneAreaCode = reader["PhoneNumber"].ToString().Substring(0, 3);
                                customer.PhonePrefix = reader["PhoneNumber"].ToString().Substring(3, 3);
                                customer.PhoneSuffix = reader["PhoneNumber"].ToString().Substring(6, 4);
                                customers.Add(customer);
                            }
                        }
                        else
                        {
                            //Error retrieving Vendors
                            reader.Read();
                            errorMessage = reader["Message"].ToString();

                        }
                    }
                }
            }

            return (customers, errorMessage);
        }

        public (CustomerModel, string) GetByID(int id)
        {
            CustomerModel customer = new CustomerModel();
            string? errorMessage = null;

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "dbo.usp_GetCustomerByID";
                    command.Parameters.Add("@CustomerID", SqlDbType.Int).Value = id;
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.FieldCount > 1) //Field count is 1 if an error is returned
                        {
                            //No errors
                            reader.Read();
                            customer.CustomerID = Convert.ToInt32(reader["CustomerID"]);
                            customer.FirstName = reader["FirstName"].ToString();
                            customer.LastName = reader["LastName"].ToString();
                            customer.CompanyName = reader["CompanyName"].ToString();
                            customer.VatRegistrationNumber = reader["VatRegistrationNumber"].ToString();
                            customer.AddressLine1 = reader["AddressLine1"].ToString();
                            customer.AddressLine2 = reader["AddressLine2"].ToString();
                            customer.City = reader["City"].ToString();
                            customer.Province = reader["Province"].ToString();
                            customer.PostalCode = reader["PostalCode"].ToString();
                            customer.EmailAddress = reader["EMailAddress"].ToString();
                            customer.PhoneAreaCode = reader["PhoneNumber"].ToString().Substring(0, 3);
                            customer.PhonePrefix = reader["PhoneNumber"].ToString().Substring(3, 3);
                            customer.PhoneSuffix = reader["PhoneNumber"].ToString().Substring(6, 4);
                        }
                        else
                        {
                            //Error retrieving Vendors
                            reader.Read();
                            errorMessage = reader["Message"].ToString();

                        }
                    }
                }
            }

            return (customer, errorMessage);
        }

        /// <summary>
        /// Creates a new customer
        /// </summary>
        /// <param name="customer">
        /// Takes in the customer model to be created 
        /// </param>
        /// <returns>
        /// Returns a customer model with customer id if add was successful - otherwise id will zero 
        /// Returns a string error message if add not successful - null otherwise
        /// </returns>
        public (CustomerModel, string) Insert(CustomerModel customer)
        {
            string? errorMessage = null;

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "dbo.usp_InsertCustomer";
                    command.Parameters.Add("@FirstName", SqlDbType.NVarChar).Value = customer.FirstName;
                    command.Parameters.Add("@LastName", SqlDbType.NVarChar).Value = customer.LastName;
                    command.Parameters.Add("@CompanyName", SqlDbType.NVarChar).Value = customer.CompanyName;
                    command.Parameters.Add("@VatRegistrationNumber", SqlDbType.NVarChar).Value = customer.VatRegistrationNumber;
                    command.Parameters.Add("AddressLine1", SqlDbType.NVarChar).Value = customer.AddressLine1;
                    if (string.IsNullOrWhiteSpace(customer.AddressLine2))
                    {
                        command.Parameters.Add("@AddressLine2", SqlDbType.NVarChar).Value = DBNull.Value;
                    }
                    else
                    {
                        command.Parameters.Add("@AddressLine2", SqlDbType.NVarChar).Value = customer.AddressLine2;
                    }
                    command.Parameters.Add("@City", SqlDbType.NVarChar).Value = customer.City;
                    command.Parameters.Add("@Province", SqlDbType.NVarChar).Value = customer.Province;
                    command.Parameters.Add("@PostalCode", SqlDbType.NVarChar).Value = customer.PostalCode;
                    command.Parameters.Add("@EMailAddress", SqlDbType.NVarChar).Value = customer.EmailAddress;
                    command.Parameters.Add("@PhoneNumber", SqlDbType.NVarChar).Value = (customer.PhoneAreaCode + customer.PhonePrefix + customer.PhoneSuffix);
                    connection.Open();

                    //Database will return an ID if successful otherwise an error message
                    //ID is can be parsed to an int
                    string returnMessage = command.ExecuteScalar().ToString()!;
                    if (int.TryParse(returnMessage, out _))
                    {
                        //ID passed back
                        customer.CustomerID = int.Parse(returnMessage);
                    }
                    else
                    {
                        //Error passed back
                        errorMessage = returnMessage;
                    }
                }
            }

            return (customer, errorMessage);
        }

        // <summary>
        /// Updates an existing customer
        /// </summary>
        /// <param name="customer">
        /// Takes in the customer to be updated
        /// </param>
        /// <returns>
        /// Returns a string error message if the update was not successful - null otherwise
        /// </returns>
        public string Update(CustomerModel customer)
        {
            string? errorMessage = null;

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "dbo.usp_UpdateCustomer";
                    command.Parameters.Add("@CustomerID", SqlDbType.Int).Value = customer.CustomerID;
                    command.Parameters.Add("@FirstName", SqlDbType.NVarChar).Value = customer.FirstName;
                    command.Parameters.Add("@LastName", SqlDbType.NVarChar).Value = customer.LastName;
                    command.Parameters.Add("@CompanyName", SqlDbType.NVarChar).Value = customer.CompanyName;
                    command.Parameters.Add("@VatRegistrationNumber", SqlDbType.NVarChar).Value = customer.VatRegistrationNumber;
                    command.Parameters.Add("AddressLine1", SqlDbType.NVarChar).Value = customer.AddressLine1;
                    if (string.IsNullOrWhiteSpace(customer.AddressLine2))
                    {
                        command.Parameters.Add("@AddressLine2", SqlDbType.NVarChar).Value = DBNull.Value;
                    }
                    else
                    {
                        command.Parameters.Add("@AddressLine2", SqlDbType.NVarChar).Value = customer.AddressLine2;
                    }
                    command.Parameters.Add("@City", SqlDbType.NVarChar).Value = customer.City;
                    command.Parameters.Add("@Province", SqlDbType.NVarChar).Value = customer.Province;
                    command.Parameters.Add("@PostalCode", SqlDbType.NVarChar).Value = customer.PostalCode;
                    command.Parameters.Add("@EMailAddress", SqlDbType.NVarChar).Value = customer.EmailAddress;
                    command.Parameters.Add("@PhoneNumber", SqlDbType.NVarChar).Value = (customer.PhoneAreaCode + customer.PhonePrefix + customer.PhoneSuffix);
                    connection.Open();

                    //if not error then "No Error" string is returned
                    //Otherwise an error message is returned
                    string returnMessage = command.ExecuteScalar().ToString()!;
                    if (!returnMessage.Equals("No Error"))
                    {
                        //Error passed back
                        errorMessage = returnMessage;
                    }
                }
            }
            return errorMessage;
        }
    }
}

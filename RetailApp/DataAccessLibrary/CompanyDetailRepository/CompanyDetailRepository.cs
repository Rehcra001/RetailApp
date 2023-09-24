using ModelsLibrary;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelsLibrary.RepositoryInterfaces;

namespace DataAccessLibrary.CompanyDetailRepository
{
    public class CompanyDetailRepository : ICompanyDetailRepository
    {
        private string _connectionString;
        public CompanyDetailRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        /// <summary>
        /// Retrieves this companies details
        /// </summary>
        /// <returns>
        /// returns the company detail model if successfull - otherwise it return and error message
        /// </returns>
        public (CompanyDetailModel, string) Get()
        {
            CompanyDetailModel companyDetail = new CompanyDetailModel();
            string? errorMessage = null;

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "dbo.usp_GetCompanyDetail";
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.FieldCount > 1) //Field count is 1 if an error is returned
                        {
                            if (reader.HasRows)
                            {
                                //No errors
                                reader.Read();
                                companyDetail.CompanyID = Convert.ToInt32(reader["CompanyID"]);
                                companyDetail.FirstName = reader["FirstName"].ToString();
                                companyDetail.LastName = reader["LastName"].ToString();
                                companyDetail.CompanyName = reader["CompanyName"].ToString();
                                companyDetail.VatRegistrationNumber = reader["VatRegistrationNumber"].ToString();
                                companyDetail.AddressLine1 = reader["AddressLine1"].ToString();
                                companyDetail.AddressLine2 = reader["AddressLine2"].ToString();
                                companyDetail.City = reader["City"].ToString();
                                companyDetail.Province = reader["Province"].ToString();
                                companyDetail.PostalCode = reader["PostalCode"].ToString();
                                companyDetail.EmailAddress = reader["EMailAddress"].ToString();
                                companyDetail.PhoneAreaCode = reader["PhoneNumber"].ToString().Substring(0, 3);
                                companyDetail.PhonePrefix = reader["PhoneNumber"].ToString().Substring(3, 3);
                                companyDetail.PhoneSuffix = reader["PhoneNumber"].ToString().Substring(6, 4);
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

            return (companyDetail, errorMessage);
        }


        /// <summary>
        /// Creates a new companyDetail
        /// </summary>
        /// <param name="companyDetail">
        /// Takes in the companyDetail model to be created 
        /// </param>
        /// <returns>
        /// Returns a companydetail model with company id if add was successful - otherwise id will zero 
        /// Returns a string error message if add not successful - null otherwise
        /// </returns>
        public (CompanyDetailModel, string) Insert(CompanyDetailModel companyDetail)
        {
            string? errorMessage = null;

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "dbo.usp_InsertCompanyDetail";
                    command.Parameters.Add("@FirstName", SqlDbType.NVarChar).Value = companyDetail.FirstName;
                    command.Parameters.Add("@LastName", SqlDbType.NVarChar).Value = companyDetail.LastName;
                    command.Parameters.Add("@CompanyName", SqlDbType.NVarChar).Value = companyDetail.CompanyName;
                    command.Parameters.Add("@VatRegistrationNumber", SqlDbType.NVarChar).Value = companyDetail.VatRegistrationNumber;
                    command.Parameters.Add("AddressLine1", SqlDbType.NVarChar).Value = companyDetail.AddressLine1;
                    if (string.IsNullOrWhiteSpace(companyDetail.AddressLine2))
                    {
                        command.Parameters.Add("@AddressLine2", SqlDbType.NVarChar).Value = DBNull.Value;
                    }
                    else
                    {
                        command.Parameters.Add("@AddressLine2", SqlDbType.NVarChar).Value = companyDetail.AddressLine2;
                    }
                    command.Parameters.Add("@City", SqlDbType.NVarChar).Value = companyDetail.City;
                    command.Parameters.Add("@Province", SqlDbType.NVarChar).Value = companyDetail.Province;
                    command.Parameters.Add("@PostalCode", SqlDbType.NVarChar).Value = companyDetail.PostalCode;
                    command.Parameters.Add("@EMailAddress", SqlDbType.NVarChar).Value = companyDetail.EmailAddress;
                    command.Parameters.Add("@PhoneNumber", SqlDbType.NVarChar).Value = (companyDetail.PhoneAreaCode + companyDetail.PhonePrefix + companyDetail.PhoneSuffix);
                    connection.Open();

                    //Database will return an ID if successful otherwise an error message
                    //ID is can be parsed to an int
                    string returnMessage = command.ExecuteScalar().ToString()!;
                    if (int.TryParse(returnMessage, out _))
                    {
                        //ID passed back
                        companyDetail.CompanyID = int.Parse(returnMessage);
                    }
                    else
                    {
                        //Error passed back
                        errorMessage = returnMessage;
                    }
                }
            }

            return (companyDetail, errorMessage);
        }

        // <summary>
        /// Updates an existing customer
        /// </summary>
        /// <param name="companyDetail">
        /// Takes in the companyDetail to be updated
        /// </param>
        /// <returns>
        /// Returns a string error message if the update was not successful - null otherwise
        /// </returns>
        public string Update(CompanyDetailModel companyDetail)
        {
            string? errorMessage = null;

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "dbo.usp_UpdateCustomer";
                    command.Parameters.Add("@CustomerID", SqlDbType.Int).Value = companyDetail.CompanyID;
                    command.Parameters.Add("@FirstName", SqlDbType.NVarChar).Value = companyDetail.FirstName;
                    command.Parameters.Add("@LastName", SqlDbType.NVarChar).Value = companyDetail.LastName;
                    command.Parameters.Add("@CompanyName", SqlDbType.NVarChar).Value = companyDetail.CompanyName;
                    command.Parameters.Add("@VatRegistrationNumber", SqlDbType.NVarChar).Value = companyDetail.VatRegistrationNumber;
                    command.Parameters.Add("AddressLine1", SqlDbType.NVarChar).Value = companyDetail.AddressLine1;
                    if (string.IsNullOrWhiteSpace(companyDetail.AddressLine2))
                    {
                        command.Parameters.Add("@AddressLine2", SqlDbType.NVarChar).Value = DBNull.Value;
                    }
                    else
                    {
                        command.Parameters.Add("@AddressLine2", SqlDbType.NVarChar).Value = companyDetail.AddressLine2;
                    }
                    command.Parameters.Add("@City", SqlDbType.NVarChar).Value = companyDetail.City;
                    command.Parameters.Add("@Province", SqlDbType.NVarChar).Value = companyDetail.Province;
                    command.Parameters.Add("@PostalCode", SqlDbType.NVarChar).Value = companyDetail.PostalCode;
                    command.Parameters.Add("@EMailAddress", SqlDbType.NVarChar).Value = companyDetail.EmailAddress;
                    command.Parameters.Add("@PhoneNumber", SqlDbType.NVarChar).Value = (companyDetail.PhoneAreaCode + companyDetail.PhonePrefix + companyDetail.PhoneSuffix);
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

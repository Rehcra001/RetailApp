using ModelsLibrary;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLibrary.VendorRepository
{
    public class VendorRepository : IVendorRepository
    {
        private string _connectionString;
        public VendorRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        /// <summary>
        /// Deletes a vendor
        /// </summary>
        /// <param name="vendor">
        /// Takes in the vendor model to be deleted
        /// </param>
        /// <returns>
        /// Returns a string error message if the delete was not successful - null otherwise
        /// </returns>
        public string Delete(VendorModel vendor)
        {
            string? errorMessage = null;

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "dbo.usp_DeleteVendor";
                    command.Parameters.Add("@VendorID", SqlDbType.Int).Value = vendor.VendorID;
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
        /// Gets a list of all vendors
        /// </summary>
        /// <returns>
        /// Return an observable collection of vendors if successfull - null otherwise
        /// Returns a string error message if not successfull - null otherwise
        /// </returns>
        public (ObservableCollection<VendorModel>, string) GetAll()
        {
            ObservableCollection<VendorModel> vendors = new ObservableCollection<VendorModel>();
            string? errorMessage = null;

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "dbo.usp_GetAllVendors";
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.FieldCount > 1) //Field count is 1 if an error is returned
                        {
                            //No errors
                            while (reader.Read())
                            {
                                VendorModel vendor = new VendorModel();
                                vendor.VendorID = Convert.ToInt32(reader["VendorID"]);
                                vendor.FirstName = reader["FirstName"].ToString();
                                vendor.LastName = reader["LastName"].ToString();
                                vendor.CompanyName = reader["CompanyName"].ToString();
                                vendor.VatRegistrationNumber = reader["VatRegistrationNumber"].ToString();
                                vendor.AddressLine1 = reader["AddressLine1"].ToString();
                                vendor.AddressLine2 = reader["AddressLine2"].ToString();
                                vendor.City = reader["City"].ToString();
                                vendor.Province = reader["Province"].ToString();
                                vendor.PostalCode = reader["PostalCode"].ToString();
                                vendor.EmailAddress = reader["EMailAddress"].ToString();
                                vendor.PhoneAreaCode = reader["PhoneNumber"].ToString().Substring(0, 3);
                                vendor.PhonePrefix = reader["PhoneNumber"].ToString().Substring(3, 3);
                                vendor.PhoneSuffix = reader["PhoneNumber"].ToString().Substring(6, 4);
                                vendors.Add(vendor);
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

            return (vendors, errorMessage);
        }

        public (VendorModel, string) GetByID(int id)
        {
            VendorModel vendor = new VendorModel();
            string? errorMessage = null;

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "dbo.usp_GetVendorByID";
                    command.Parameters.Add("@VendorID", SqlDbType.Int).Value = id;
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.FieldCount > 1) //Field count is 1 if an error is returned
                        {
                            //No errors
                            reader.Read();
                            vendor.VendorID = Convert.ToInt32(reader["VendorID"]);
                            vendor.FirstName = reader["FirstName"].ToString();
                            vendor.LastName = reader["LastName"].ToString();
                            vendor.CompanyName = reader["CompanyName"].ToString();
                            vendor.VatRegistrationNumber = reader["VatRegistrationNumber"].ToString();
                            vendor.AddressLine1 = reader["AddressLine1"].ToString();
                            vendor.AddressLine2 = reader["AddressLine2"].ToString();
                            vendor.City = reader["City"].ToString();
                            vendor.Province = reader["Province"].ToString();
                            vendor.PostalCode = reader["PostalCode"].ToString();
                            vendor.EmailAddress = reader["EMailAddress"].ToString();
                            vendor.PhoneAreaCode = reader["PhoneNumber"].ToString().Substring(0, 3);
                            vendor.PhonePrefix = reader["PhoneNumber"].ToString().Substring(3, 3);
                            vendor.PhoneSuffix = reader["PhoneNumber"].ToString().Substring(6, 4);
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

            return (vendor, errorMessage);
        }

        /// <summary>
        /// Creates a new vendor
        /// </summary>
        /// <param name="vendor">
        /// Takes in the vendor model to be created 
        /// </param>
        /// <returns>
        /// Returns a vendor model with vendor id if add was successful - otherwise id will zero 
        /// Returns a string error message if add not successful - null otherwise
        /// </returns>
        public (VendorModel, string) Insert(VendorModel vendor)
        {
            string? errorMessage = null;

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "dbo.usp_InsertVendor";
                    command.Parameters.Add("@FirstName", SqlDbType.NVarChar).Value = vendor.FirstName;
                    command.Parameters.Add("@LastName", SqlDbType.NVarChar).Value = vendor.LastName;
                    command.Parameters.Add("@CompanyName", SqlDbType.NVarChar).Value = vendor.CompanyName;
                    command.Parameters.Add("@VatRegistrationNumber", SqlDbType.NVarChar).Value = vendor.VatRegistrationNumber;
                    command.Parameters.Add("AddressLine1", SqlDbType.NVarChar).Value = vendor.AddressLine1;
                    if (string.IsNullOrWhiteSpace(vendor.AddressLine2))
                    {
                        command.Parameters.Add("@AddressLine2", SqlDbType.NVarChar).Value = DBNull.Value;
                    }
                    else
                    {
                        command.Parameters.Add("@AddressLine2", SqlDbType.NVarChar).Value = vendor.AddressLine2;
                    }
                    command.Parameters.Add("@City", SqlDbType.NVarChar).Value = vendor.City;
                    command.Parameters.Add("@Province", SqlDbType.NVarChar).Value = vendor.Province;
                    command.Parameters.Add("@PostalCode", SqlDbType.NVarChar).Value = vendor.PostalCode;
                    command.Parameters.Add("@EMailAddress", SqlDbType.NVarChar).Value = vendor.EmailAddress;
                    command.Parameters.Add("@PhoneNumber", SqlDbType.NVarChar).Value = (vendor.PhoneAreaCode + vendor.PhonePrefix + vendor.PhoneSuffix);
                    connection.Open();

                    //Database will return an ID if successful otherwise an error message
                    //ID is can be parsed to an int
                    string returnMessage = command.ExecuteScalar().ToString()!;
                    if (int.TryParse(returnMessage, out _))
                    {
                        //ID passed back
                        vendor.VendorID = int.Parse(returnMessage);
                    }
                    else
                    {
                        //Error passed back
                        errorMessage = returnMessage;
                    }
                }
            }

            return (vendor, errorMessage);
        }

        // <summary>
        /// Updates an existing vendor
        /// </summary>
        /// <param name="vendor">
        /// Takes in the vendor to be updated
        /// </param>
        /// <returns>
        /// Returns a string error message if the update was not successful - null otherwise
        /// </returns>
        public string Update(VendorModel vendor)
        {
            string? errorMessage = null;

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "dbo.usp_UpdateVendor";
                    command.Parameters.Add("@VendorID", SqlDbType.Int).Value = vendor.VendorID;
                    command.Parameters.Add("@FirstName", SqlDbType.NVarChar).Value = vendor.FirstName;
                    command.Parameters.Add("@LastName", SqlDbType.NVarChar).Value = vendor.LastName;
                    command.Parameters.Add("@CompanyName", SqlDbType.NVarChar).Value = vendor.CompanyName;
                    command.Parameters.Add("@VatRegistrationNumber", SqlDbType.NVarChar).Value = vendor.VatRegistrationNumber;
                    command.Parameters.Add("AddressLine1", SqlDbType.NVarChar).Value = vendor.AddressLine1;
                    if (string.IsNullOrWhiteSpace(vendor.AddressLine2))
                    {
                        command.Parameters.Add("@AddressLine2", SqlDbType.NVarChar).Value = DBNull.Value;
                    }
                    else
                    {
                        command.Parameters.Add("@AddressLine2", SqlDbType.NVarChar).Value = vendor.AddressLine2;
                    }
                    command.Parameters.Add("@City", SqlDbType.NVarChar).Value = vendor.City;
                    command.Parameters.Add("@Province", SqlDbType.NVarChar).Value = vendor.Province;
                    command.Parameters.Add("@PostalCode", SqlDbType.NVarChar).Value = vendor.PostalCode;
                    command.Parameters.Add("@EMailAddress", SqlDbType.NVarChar).Value = vendor.EmailAddress;
                    command.Parameters.Add("@PhoneNumber", SqlDbType.NVarChar).Value = (vendor.PhoneAreaCode + vendor.PhonePrefix + vendor.PhoneSuffix);
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

﻿using ModelsLibrary;
using ModelsLibrary.RepositoryInterfaces;
using System.Data;
using System.Data.SqlClient;

namespace DataAccessLibrary.VATRepository
{
    public class VATRepository : IVATRepository
    {
        private readonly IRelationalDataAccess _sqlDataAccess;

        public VATRepository(IRelationalDataAccess sqlDataAccess)
        {
            _sqlDataAccess = sqlDataAccess;
        }

        public (VatModel, string) Get()
        {
            VatModel vat = new VatModel();
            string? errorMessage = null;

            using (_sqlDataAccess.SQLConnection())
            {
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = _sqlDataAccess.SQLConnection();
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "dbo.usp_GetVat";
                    command.Connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        //Check for errors
                        //Expecting two columns on no error
                        if (reader.FieldCount > 1)
                        {
                            //Check for data
                            if (reader.HasRows)
                            {
                                reader.Read();
                                vat.VAT = Convert.ToDecimal(reader["VAT"]);
                                vat.VatDecimal = Convert.ToDecimal(reader["VATDecimal"]);
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
            return (vat, errorMessage);//Error message will be null if no error raised by database
        }
    }
}

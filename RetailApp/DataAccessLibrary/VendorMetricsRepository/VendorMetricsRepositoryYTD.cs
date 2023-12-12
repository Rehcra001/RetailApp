using ChartModelsLibrary.ChartModels;
using System.Data;
using System.Data.SqlClient;

namespace DataAccessLibrary.VendorMetricsRepository
{
    public class VendorMetricsRepositoryYTD : IVendorMetricsRepositoryYTD
    {
        private readonly IRelationalDataAccess _relationalDataAccess;

        public VendorMetricsRepositoryYTD(IRelationalDataAccess relationalDataAccess)
        {
            _relationalDataAccess = relationalDataAccess;
        }

        public (HistogramModel, string) GetLeadTimeDaysCountByVendorIdYTD(int id)
        {
            HistogramModel leadTimes = new HistogramModel();
            List<decimal> daysCount = new List<decimal>();
            string? errorMessage = null;

            using (SqlConnection connection = _relationalDataAccess.SQLConnection())
            {
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "dbo.usp_GetLeadTimeDaysCountAllProductsByVendorYTD";
                    command.Parameters.Add("@VendorID", SqlDbType.Int).Value = id;

                    command.Connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        //Check for errors
                        if (reader.GetName(0).Equals("LeadTimeDays"))
                        {
                            //No Error
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    daysCount.Add(Convert.ToDecimal(reader["LeadTimeDays"]));
                                }
                                leadTimes.Observations = daysCount;
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
            return (leadTimes, errorMessage);//error message will be null if no error raised
        }

        public (HistogramModel, string) GetVendorDeliveryComplianceAllProducts(int id)
        {
            HistogramModel vendordelivery = new HistogramModel();
            string? errorMessage = null;
            List<decimal> observations = new List<decimal>();

            using (SqlConnection connection = _relationalDataAccess.SQLConnection())
            {
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "dbo.usp_GetVendorDeliveryComplianceDistributionYTD";
                    command.Parameters.Add("@VendorId", SqlDbType.Int).Value = id;

                    command.Connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        //check for errors
                        //expecting one column with name of DeliveryCompliance on no error
                        //expecting column name of Message on error
                        if (reader.GetName(0).Equals("DeliveryCompliance"))
                        {
                            //no error
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    observations.Add(Convert.ToDecimal(reader["DeliveryCompliance"]));
                                }
                            }

                            vendordelivery.Observations = observations;
                        }
                        else
                        {
                            reader.Read();
                            errorMessage = reader["Message"].ToString();
                        }
                    }
                }
            }
            return (vendordelivery, errorMessage); //error message will be null if no error raised
        }
    }
}

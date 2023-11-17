using ChartModelsLibrary.ChartModels;
using System.Data;
using System.Data.SqlClient;

namespace DataAccessLibrary.SalesMetricsRepository
{
    public class SalesMetricsYTD : ISalesMetricsYTD
    {
        IRelationalDataAccess _sqlDataAccess;

        public SalesMetricsYTD(IRelationalDataAccess sqlDataAccess)
        {
            this._sqlDataAccess = sqlDataAccess;
        }

        public (BarChartModel, string) GetMonthlyRevenueYTD()
        {
            BarChartModel barChart = new BarChartModel();
            string? errorMessage = null;
            List<decimal> monthlyRevenue = new List<decimal>();
            List<string> monthNames = new List<string>();

            using (SqlConnection connection = _sqlDataAccess.SQLConnection())
            {
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "dbo.usp_SalesRevenueYearToDatePerMonth";
                    command.Connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        //check for errors
                        //Expecting two columns if no error
                        if (reader.FieldCount > 1)
                        {
                            //check for data
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    monthlyRevenue.Add(Convert.ToDecimal(reader["YearToDateRevenue"]));
                                    monthNames.Add(reader["MonthName"].ToString()!);
                                }
                                barChart.Values = monthlyRevenue;
                                barChart.ValuesDescription = monthNames;
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
            }

            return (barChart, errorMessage); //error message will be null if no error raised
        }

        public (BarChartModel, string) GetTop10ProductsByRevenueYTD()
        {
            BarChartModel barChart = new BarChartModel();
            string? errorMessage = null;
            List<decimal> productRevenues = new List<decimal>();
            List<string> productNames = new List<string>();

            using (SqlConnection connection = _sqlDataAccess.SQLConnection())
            {
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "dbo.usp_SalesRevenueTop10ProductsYearToDate";
                    command.Connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        //Check for error
                        //Expecting two columns if no error
                        if (reader.FieldCount > 1)
                        {
                            //No error
                            //check for data
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    productNames.Add(reader["ProductName"].ToString()!);
                                    productRevenues.Add(Convert.ToDecimal(reader["YearToDateRevenue"]));
                                }
                                barChart.Values = productRevenues;
                                barChart.ValuesDescription = productNames;
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

            return (barChart, errorMessage); //error message will be null if no error raised
        }
    }
}

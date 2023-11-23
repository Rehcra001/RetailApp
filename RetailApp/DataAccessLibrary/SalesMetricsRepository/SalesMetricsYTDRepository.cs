using ChartModelsLibrary.ChartModels;
using ModelsLibrary;
using System.Data;
using System.Data.SqlClient;

namespace DataAccessLibrary.SalesMetricsRepository
{
    public class SalesMetricsYTDRepository : ISalesMetricsYTDRepository
    {
        IRelationalDataAccess _sqlDataAccess;

        public SalesMetricsYTDRepository(IRelationalDataAccess sqlDataAccess)
        {
            this._sqlDataAccess = sqlDataAccess;
        }

        public (BarChartModel, string) GetMonthlyRevenueYTDChart()
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

        public (decimal, string) GetRevenueYTD()
        {
            decimal value = 0; ;
            string? errorMessage = null;

            using (SqlConnection connection = _sqlDataAccess.SQLConnection())
            {
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "dbo.usp_SalesRevenueYearToDate";
                    command.Connection.Open();

                    string returned = command.ExecuteScalar().ToString()!;
                    //Check for errors
                    //expecting a decimal value back if no error
                    if (decimal.TryParse(returned, out _))
                    {
                        //no error
                        value = decimal.Parse(returned);
                    }
                    else
                    {
                        errorMessage = returned;
                    }
                }
            }

            return (value, errorMessage); //error message will be null if no error raised
        }

        public (BarChartModel, string) GetTop10ProductsByRevenueYTDChart()
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

        public (decimal, string) GetTop10ProductsRevenueYTD()
        {
            decimal value = 0;
            string? errorMessage = null;
            using (SqlConnection connection = _sqlDataAccess.SQLConnection())
            {
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "dbo.usp_Top10ProductsByRevenueYTD";
                    command.Connection.Open();

                    string returnedString = command.ExecuteScalar().ToString()!;
                    //Check for errors
                    //Expecting a single decimal value
                    if (decimal.TryParse(returnedString, out _))
                    {
                        //No error
                        value = decimal.Parse(returnedString);
                    }
                    else
                    {
                        //Error raised
                        errorMessage = returnedString;
                    }
                }
            }
            return (value, errorMessage); //error message will be null if no error raised
        }
    }
}

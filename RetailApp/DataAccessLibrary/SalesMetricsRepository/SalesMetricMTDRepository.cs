using ChartModelsLibrary.ChartModels;
using System.Data;
using System.Data.SqlClient;

namespace DataAccessLibrary.SalesMetricsRepository
{
    public class SalesMetricMTDRepository : ISalesMetricMTDRepository
    {
        private readonly IRelationalDataAccess _relationalDataAccess;

        public SalesMetricMTDRepository(IRelationalDataAccess relationalDataAccess)
        {
            _relationalDataAccess = relationalDataAccess;
        }

        public (decimal, string) GetCountOfCancelledSalesOrdersMTD()
        {
            decimal count = 0;
            string? errorMessage = null;

            using (SqlConnection connection = _relationalDataAccess.SQLConnection())
            {
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "dbo.usp_GetCountOfCancelledSalesOrdersMTD";
                    command.Connection.Open();

                    //Expecting one row and one column
                    string result = command.ExecuteScalar().ToString()!;

                    //Check for error
                    if (decimal.TryParse(result, out _))
                    {
                        //No error
                        count = decimal.Parse(result);
                    }
                    else
                    {
                        //error raised
                        errorMessage = result;
                    }
                }
            }
            return (count, errorMessage); //error message will be null if no error raised
        }

        public (decimal, string) GetCountOfOpenSalesOrdersMTD()
        {
            decimal count = 0;
            string? errorMessage = null;

            using (SqlConnection connection = _relationalDataAccess.SQLConnection())
            {
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "dbo.usp_GetCountOfOpenSalesOrdersMTD";
                    command.Connection.Open();

                    //Expecting one row and one column
                    string result = command.ExecuteScalar().ToString()!;

                    //Check for error
                    if (decimal.TryParse(result, out _))
                    {
                        //No error
                        count = decimal.Parse(result);
                    }
                    else
                    {
                        //error raised
                        errorMessage = result;
                    }
                }
            }
            return (count, errorMessage); //error message will be null if no error raised
        }

        public (decimal, string) GetCountOfSalesOrdersMTD()
        {
            decimal count = 0;
            string? errorMessage = null;

            using (SqlConnection connection = _relationalDataAccess.SQLConnection())
            {
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "dbo.usp_CountOfSalesOrdersMTD";
                    command.Connection.Open();

                    //Expecting one row and one column
                    string result = command.ExecuteScalar().ToString()!;

                    //Check for error
                    if (decimal.TryParse(result, out _))
                    {
                        //No error
                        count = decimal.Parse(result);
                    }
                    else
                    {
                        //error raised
                        errorMessage = result;
                    }
                }
            }
            return (count, errorMessage); //error message will be null if no error raised
        }

        public (HistogramModel, string) GetDaysCountToCloseSalesOrdersMTD()
        {
            HistogramModel daysCount = new HistogramModel();
            string? errorMessage = null;
            List<decimal> counts = new List<decimal>();

            using (SqlConnection connection = _relationalDataAccess.SQLConnection())
            {
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "dbo.usp_GetDaysCountToCloseSalesOrderMTD";
                    command.Connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        //Check for errors
                        string name = reader.GetName(0);
                        if (name.Equals("DaysToClose"))
                        {
                            //No error
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    counts.Add(Convert.ToDecimal(reader["DaysToClose"]));
                                }
                                daysCount.Observations = counts;
                            }
                        }
                        else
                        {
                            //error
                            reader.Read();
                            errorMessage = reader["Message"].ToString();
                        }
                    }
                }
            }
            return (daysCount, errorMessage);//error message will be null if no error raised
        }

        public (BarChartModel, string) GetProductsByRevenueMTD()
        {
            BarChartModel barChart = new BarChartModel();
            List<decimal> values = new List<decimal>();
            List<string> labels = new List<string>();
            string? errorMessage = null;

            using (SqlConnection connection = _relationalDataAccess.SQLConnection())
            {
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "dbo.usp_ProductsByRevenueMTD";
                    command.Connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        //check for errors
                        //Expecting two columns on no error
                        if (reader.FieldCount > 1)
                        {
                            //Check for data
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    values.Add(Convert.ToDecimal(reader["RevenueByProductMTD"]));
                                    labels.Add(reader["ProductName"].ToString()!);
                                }
                                barChart.Values = values;
                                barChart.ValuesDescription = labels;
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

        public (decimal, string) GetRevenueMTD()
        {
            decimal revenue = 0;
            string? errorMessage = null;

            using (SqlConnection connection = _relationalDataAccess.SQLConnection())
            {
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "dbo.usp_RevenueMTD";
                    command.Connection.Open();

                    //Expecting one row and one column
                    string result = command.ExecuteScalar().ToString()!;

                    //Check for error
                    if (decimal.TryParse(result, out _))
                    {
                        //No error
                        revenue = decimal.Parse(result);
                    }
                    else
                    {
                        //error raised
                        errorMessage = result;
                    }
                }
            }
            return (revenue, errorMessage); //error message will be null if no error raised
        }

        public (decimal, string) GetRevenueTop10ProductsMTD()
        {
            decimal revenue = 0;
            string? errorMessage = null;

            using (SqlConnection connection = _relationalDataAccess.SQLConnection())
            {
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "dbo.usp_GetRevenueTop10ProductsMTD";
                    command.Connection.Open();

                    //Expecting one row and one column
                    string result = command.ExecuteScalar().ToString()!;

                    //Check for error
                    if (decimal.TryParse(result, out _))
                    {
                        //No error
                        revenue = decimal.Parse(result);
                    }
                    else
                    {
                        //error raised
                        errorMessage = result;
                    }
                }
            }
            return (revenue, errorMessage); //error message will be null if no error raised
        }

        public (BarChartModel, string) GetTop10ProductsByRevenueMTD()
        {
            BarChartModel barChart = new BarChartModel();
            List<decimal> values = new List<decimal>();
            List<string> labels = new List<string>();
            string? errorMessage = null;

            using (SqlConnection connection = _relationalDataAccess.SQLConnection())
            {
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "dbo.usp_Top10ProductsByRevenueMTD";
                    command.Connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        //check for errors
                        //Expecting two columns on no error
                        if (reader.FieldCount > 1)
                        {
                            //Check for data
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    values.Add(Convert.ToDecimal(reader["RevenueByProductMTD"]));
                                    labels.Add(reader["ProductName"].ToString()!);
                                }
                                barChart.Values = values;
                                barChart.ValuesDescription = labels;
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

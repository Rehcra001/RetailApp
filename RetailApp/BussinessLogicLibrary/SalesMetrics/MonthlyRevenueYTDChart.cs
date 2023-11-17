using ChartModelsLibrary.ChartModels;
using DataAccessLibrary.SalesMetricsRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLogicLibrary.SalesMetrics
{
    public class MonthlyRevenueYTDChart : IMonthlyRevenueYTDChart
    {
        private ISalesMetricsYTDRepository _salesMetricsYTDRepository;

        public MonthlyRevenueYTDChart(ISalesMetricsYTDRepository salesMetricsYTDRepository)
        {
            _salesMetricsYTDRepository = salesMetricsYTDRepository;
        }

        public BarChartModel GetMonthlyRevenue()
        {
            Tuple<BarChartModel, string> monthlyRevenue = _salesMetricsYTDRepository.GetMonthlyRevenueYTDChart().ToTuple();

            //Check for errors
            if (monthlyRevenue.Item2 == null)
            {
                monthlyRevenue.Item1.ChartTitle = "Monthly Revenue YTD";
                monthlyRevenue.Item1.VerticalAxisTitle = "Revenue";
                monthlyRevenue.Item1.HorizontalAxisTitle = "Months";
                return monthlyRevenue.Item1;
            }
            else
            {
                //Error
                throw new Exception(monthlyRevenue.Item2);
            }

        }
    }
}

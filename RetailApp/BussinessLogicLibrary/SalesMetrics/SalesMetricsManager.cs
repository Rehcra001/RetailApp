using ChartModelsLibrary.ChartModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLogicLibrary.SalesMetrics
{
    public class SalesMetricsManager : ISalesMetricsManager
    {
        private ITop10ProductsByRevenueYTDChart _top10ProductSalesRevenueYTDChart;
        private IMonthlyRevenueYTDChart _monthlyRevenueYTDChart;

        public SalesMetricsManager(ITop10ProductsByRevenueYTDChart top10ProductSalesRevenueYTD,
                                   IMonthlyRevenueYTDChart monthlyRevenueYTDChart)
        {
            _top10ProductSalesRevenueYTDChart = top10ProductSalesRevenueYTD;
            _monthlyRevenueYTDChart = monthlyRevenueYTDChart;
        }

        public BarChartModel GetTop10ProductsRevenueYTDChart()
        {
            return _top10ProductSalesRevenueYTDChart.GetTop10ProductsRevenueYTD();
        }

        public decimal GetSalesRevenueYTD()
        {
            throw new NotImplementedException();
        }

        public BarChartModel GetMonthlyRevenueYTDChart()
        {
            return _monthlyRevenueYTDChart.GetMonthlyRevenue();
        }
    }
}

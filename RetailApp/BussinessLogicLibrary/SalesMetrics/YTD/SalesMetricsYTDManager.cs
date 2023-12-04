using ChartModelsLibrary.ChartModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLogicLibrary.SalesMetrics.YTD
{
    public class SalesMetricsYTDManager : ISalesMetricsYTDManager
    {
        private ITop10ProductsByRevenueYTDChart _top10ProductSalesRevenueYTDChart;
        private IMonthlyRevenueYTDChart _monthlyRevenueYTDChart;
        private ISalesRevenueYTD _salesRevenueYTD;
        private ITop10ProductsRevenueYTD _top10ProductsRevenueYTD;
        private IDaysCountToCloseSalesOrderYTD _daysCountToCloseSalesOrderYTD;
        private ICountOfSalesOrdersYTD _countOfSalesOrdersYTD;
        private ICountOfOpenSalesOrdersYTD _countOfOpenSalesOrders;
        private ICountOfCancelledSalesOrdersYTD _countOfCancelledSalesOrders;

        public SalesMetricsYTDManager(ITop10ProductsByRevenueYTDChart top10ProductSalesRevenueYTD,
                                   IMonthlyRevenueYTDChart monthlyRevenueYTDChart,
                                   ISalesRevenueYTD salesRevenueYTD,
                                   ITop10ProductsRevenueYTD top10ProductsRevenueYTD,
                                   IDaysCountToCloseSalesOrderYTD daysCountToCloseSalesOrderYTD,
                                   ICountOfSalesOrdersYTD countOfSalesOrders,
                                   ICountOfOpenSalesOrdersYTD countOfOpenSalesOrders,
                                   ICountOfCancelledSalesOrdersYTD countOfCancelledSalesOrders)
        {
            _top10ProductSalesRevenueYTDChart = top10ProductSalesRevenueYTD;
            _monthlyRevenueYTDChart = monthlyRevenueYTDChart;
            _salesRevenueYTD = salesRevenueYTD;
            _top10ProductsRevenueYTD = top10ProductsRevenueYTD;
            _daysCountToCloseSalesOrderYTD = daysCountToCloseSalesOrderYTD;
            _countOfSalesOrdersYTD = countOfSalesOrders;
            _countOfOpenSalesOrders = countOfOpenSalesOrders;
            _countOfCancelledSalesOrders = countOfCancelledSalesOrders;
        }

        public BarChartModel GetTop10ProductsRevenueYTDChart()
        {
            return _top10ProductSalesRevenueYTDChart.GetTop10ProductsRevenueYTD();
        }

        public decimal GetSalesRevenueYTD()
        {
            return _salesRevenueYTD.GetRevenueYTD();
        }

        public BarChartModel GetMonthlyRevenueYTDChart()
        {
            return _monthlyRevenueYTDChart.GetMonthlyRevenue();
        }

        public decimal GetTop10ProductsRevenueYTD()
        {
            return _top10ProductsRevenueYTD.GetTop10ProductsRevenue();
        }

        public HistogramModel GetDaysCountToCloseOrdersYTD()
        {
            return _daysCountToCloseSalesOrderYTD.GetDaysCountToCloseOrdersYTD();
        }

        public decimal GetCountOfOrdersYTD()
        {
            return _countOfSalesOrdersYTD.GetCountOfSalesOrdersYTD();
        }

        public decimal GetCountofOpenOrdersYTD()
        {
            return _countOfOpenSalesOrders.GetCountOfOpenSalesOrders();
        }

        public decimal GetCountOfCancelledOrders()
        {
            return _countOfCancelledSalesOrders.GetCountOfCancelledOrders();
        }
    }
}

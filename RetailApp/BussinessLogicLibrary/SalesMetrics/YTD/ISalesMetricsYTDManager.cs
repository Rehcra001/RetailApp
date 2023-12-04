using ChartModelsLibrary.ChartModels;

namespace BussinessLogicLibrary.SalesMetrics.YTD
{
    public interface ISalesMetricsYTDManager
    {
        decimal GetSalesRevenueYTD();
        BarChartModel GetTop10ProductsRevenueYTDChart();
        BarChartModel GetMonthlyRevenueYTDChart();
        decimal GetTop10ProductsRevenueYTD();
        HistogramModel GetDaysCountToCloseOrdersYTD();
        decimal GetCountOfOrdersYTD();
        decimal GetCountofOpenOrdersYTD();
        decimal GetCountOfCancelledOrders();

    }
}
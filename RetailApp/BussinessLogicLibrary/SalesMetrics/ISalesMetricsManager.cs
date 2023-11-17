using ChartModelsLibrary.ChartModels;

namespace BussinessLogicLibrary.SalesMetrics
{
    public interface ISalesMetricsManager
    {
        decimal GetSalesRevenueYTD();
        BarChartModel GetTop10ProductsRevenueYTDChart();
        BarChartModel GetMonthlyRevenueYTDChart();

    }
}
using ChartModelsLibrary.ChartModels;

namespace BussinessLogicLibrary.SalesMetrics.YTD
{
    public interface IMonthlyRevenueYTDChart
    {
        BarChartModel GetMonthlyRevenue();
    }
}
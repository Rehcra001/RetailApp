using ChartModelsLibrary.ChartModels;

namespace BussinessLogicLibrary.SalesMetrics
{
    public interface IMonthlyRevenueYTDChart
    {
        BarChartModel GetMonthlyRevenue();
    }
}
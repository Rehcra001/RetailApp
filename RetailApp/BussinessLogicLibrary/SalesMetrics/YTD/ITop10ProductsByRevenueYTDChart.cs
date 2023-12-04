using ChartModelsLibrary.ChartModels;

namespace BussinessLogicLibrary.SalesMetrics.YTD
{
    public interface ITop10ProductsByRevenueYTDChart
    {
        BarChartModel GetTop10ProductsRevenueYTD();
    }
}
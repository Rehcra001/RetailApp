using ChartModelsLibrary.ChartModels;

namespace BussinessLogicLibrary.SalesMetrics
{
    public interface ITop10ProductsByRevenueYTDChart
    {
        BarChartModel GetTop10ProductsRevenueYTD();
    }
}
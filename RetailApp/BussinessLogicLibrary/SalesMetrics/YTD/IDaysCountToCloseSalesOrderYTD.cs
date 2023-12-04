using ChartModelsLibrary.ChartModels;

namespace BussinessLogicLibrary.SalesMetrics.YTD
{
    public interface IDaysCountToCloseSalesOrderYTD
    {
        HistogramModel GetDaysCountToCloseOrdersYTD();
    }
}
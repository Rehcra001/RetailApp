using ChartModelsLibrary.ChartModels;

namespace BussinessLogicLibrary.SalesMetrics
{
    public interface IDaysCountToCloseSalesOrderYTD
    {
        HistogramModel GetDaysCountToCloseOrdersYTD();
    }
}
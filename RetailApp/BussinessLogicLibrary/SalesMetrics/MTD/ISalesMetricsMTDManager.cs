using ChartModelsLibrary.ChartModels;

namespace BussinessLogicLibrary.SalesMetrics.MTD
{
    public interface ISalesMetricsMTDManager
    {
        decimal GetCountOfCancelledSalesOrdersMTD();
        decimal GetCountOfOpenSalesOrdersMTD();
        decimal GetCountOfSalesOrdersMTD();
        BarChartModel GetProductsByRevenueMTD();
        decimal GetRevenueMTD();
        decimal GetRevenueTop10ProductsMTD();
        BarChartModel GetTop10ProductsByRevenueMTD();
        HistogramModel GetDaysCountToCloseSalesOrderMTD();
    }
}
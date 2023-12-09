using ChartModelsLibrary.ChartModels;

namespace BussinessLogicLibrary.VendorMetrics.YTD
{
    public interface IVendorMetricsManagerYTD
    {
        HistogramModel GetVendorLeadTimesYTD(int id);
    }
}

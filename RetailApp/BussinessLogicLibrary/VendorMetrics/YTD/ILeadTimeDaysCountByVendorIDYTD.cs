using ChartModelsLibrary.ChartModels;

namespace BussinessLogicLibrary.VendorMetrics.YTD
{
    public interface ILeadTimeDaysCountByVendorIDYTD
    {
        HistogramModel GetLeadTimeDaysCountByVendorIDYTD(int id);
    }
}

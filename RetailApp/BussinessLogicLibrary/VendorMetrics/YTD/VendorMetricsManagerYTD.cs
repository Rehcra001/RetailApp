using ChartModelsLibrary.ChartModels;

namespace BussinessLogicLibrary.VendorMetrics.YTD
{
    public class VendorMetricsManagerYTD : IVendorMetricsManagerYTD
    {
        private readonly ILeadTimeDaysCountByVendorIDYTD _leadTimeDaysCountByVendorIDYTD;

        public VendorMetricsManagerYTD(ILeadTimeDaysCountByVendorIDYTD leadTimeDaysCountByVendorIDYTD)
        {
            _leadTimeDaysCountByVendorIDYTD = leadTimeDaysCountByVendorIDYTD;
        }

        public HistogramModel GetVendorLeadTimesYTD(int id)
        {
            return _leadTimeDaysCountByVendorIDYTD.GetLeadTimeDaysCountByVendorIDYTD(id);
        }
    }
}

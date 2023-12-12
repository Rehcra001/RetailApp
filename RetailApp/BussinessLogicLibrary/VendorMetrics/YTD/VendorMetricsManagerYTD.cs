using ChartModelsLibrary.ChartModels;

namespace BussinessLogicLibrary.VendorMetrics.YTD
{
    public class VendorMetricsManagerYTD : IVendorMetricsManagerYTD
    {
        private readonly ILeadTimeDaysCountByVendorIDYTD _leadTimeDaysCountByVendorIDYTD;
        private readonly IVendorDeliveryComplianceYTD _vendorDeliveryComplianceYTD;

        public VendorMetricsManagerYTD(ILeadTimeDaysCountByVendorIDYTD leadTimeDaysCountByVendorIDYTD,
                                       IVendorDeliveryComplianceYTD vendorDeliveryComplianceYTD)
        {
            _leadTimeDaysCountByVendorIDYTD = leadTimeDaysCountByVendorIDYTD;
            _vendorDeliveryComplianceYTD = vendorDeliveryComplianceYTD;
        }

        public HistogramModel GetVendorDeliveryComplianceYTD(int id)
        {
            return _vendorDeliveryComplianceYTD.GetVendorDeliveryComplianceYTD(id);
        }

        public HistogramModel GetVendorLeadTimesYTD(int id)
        {
            return _leadTimeDaysCountByVendorIDYTD.GetLeadTimeDaysCountByVendorIDYTD(id);
        }
    }
}

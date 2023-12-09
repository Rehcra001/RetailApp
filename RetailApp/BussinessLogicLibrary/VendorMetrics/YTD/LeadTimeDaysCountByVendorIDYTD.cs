using ChartModelsLibrary.ChartModels;
using DataAccessLibrary.VendorMetricsRepository;

namespace BussinessLogicLibrary.VendorMetrics.YTD
{
    public class LeadTimeDaysCountByVendorIDYTD : ILeadTimeDaysCountByVendorIDYTD
    {
        private readonly IVendorMetricsRepositoryYTD _vendorMetricsRepositoryYTD;

        public LeadTimeDaysCountByVendorIDYTD(IVendorMetricsRepositoryYTD vendorMetricsRepository)
        {
            _vendorMetricsRepositoryYTD = vendorMetricsRepository;
        }

        public HistogramModel GetLeadTimeDaysCountByVendorIDYTD(int id)
        {
            Tuple<HistogramModel, string> leadTimes = _vendorMetricsRepositoryYTD.GetLeadTimeDaysCountByVendorIdYTD(id).ToTuple();

            //Check for errors
            if (leadTimes.Item2 == null)
            {
                //No error
                return leadTimes.Item1;
            }
            else
            {
                throw new Exception(leadTimes.Item2);
            }
        }
    }
}

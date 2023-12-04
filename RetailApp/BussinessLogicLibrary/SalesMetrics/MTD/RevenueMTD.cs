using DataAccessLibrary.SalesMetricsRepository;

namespace BussinessLogicLibrary.SalesMetrics.MTD
{
    public class RevenueMTD : IRevenueMTD
    {
        private readonly ISalesMetricMTDRepository _salesMetricMTDRepository;

        public RevenueMTD(ISalesMetricMTDRepository salesMetricMTDRepository)
        {
            _salesMetricMTDRepository = salesMetricMTDRepository;
        }

        public decimal GetRevenueMTD()
        {
            Tuple<decimal, string> revenue = _salesMetricMTDRepository.GetRevenueMTD().ToTuple();

            //Check for errors
            if (revenue.Item2 == null)
            {
                //No error
                return revenue.Item1;
            }
            else
            {
                throw new Exception(revenue.Item2);
            }
        }
    }
}

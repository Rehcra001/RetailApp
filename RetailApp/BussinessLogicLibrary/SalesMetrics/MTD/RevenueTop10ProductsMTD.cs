using DataAccessLibrary.SalesMetricsRepository;

namespace BussinessLogicLibrary.SalesMetrics.MTD
{
    public class RevenueTop10ProductsMTD : IRevenueTop10ProductsMTD
    {
        private readonly ISalesMetricMTDRepository _salesMetricMTDRepository;

        public RevenueTop10ProductsMTD(ISalesMetricMTDRepository salesMetricMTDRepository)
        {
            _salesMetricMTDRepository = salesMetricMTDRepository;
        }

        public decimal GetRevenueTop10ProductsMTD()
        {
            Tuple<decimal, string> revenue = _salesMetricMTDRepository.GetRevenueTop10ProductsMTD().ToTuple();

            //check for errors
            if (revenue.Item2 == null)
            {
                //no error
                return revenue.Item1;
            }
            else
            {
                //error
                throw new Exception(revenue.Item2);
            }
        }
    }
}

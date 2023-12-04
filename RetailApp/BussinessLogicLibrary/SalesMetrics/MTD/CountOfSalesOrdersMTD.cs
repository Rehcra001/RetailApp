using DataAccessLibrary.SalesMetricsRepository;

namespace BussinessLogicLibrary.SalesMetrics.MTD
{
    public class CountOfSalesOrdersMTD : ICountOfSalesOrdersMTD
    {
        private readonly ISalesMetricMTDRepository _salesMetricMTDRepository;

        public CountOfSalesOrdersMTD(ISalesMetricMTDRepository salesMetricMTDRepository)
        {
            _salesMetricMTDRepository = salesMetricMTDRepository;
        }

        public decimal GetCountOfSalesOrdersMTD()
        {
            Tuple<decimal, string> count = _salesMetricMTDRepository.GetCountOfSalesOrdersMTD().ToTuple();

            //check for errors
            if (count.Item2 == null)
            {
                //No Error
                return count.Item1;
            }
            else
            {
                //error
                throw new Exception(count.Item2);
            }
        }
    }
}

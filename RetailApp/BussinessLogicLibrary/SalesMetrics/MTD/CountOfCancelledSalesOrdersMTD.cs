using DataAccessLibrary.SalesMetricsRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLogicLibrary.SalesMetrics.MTD
{
    public class CountOfCancelledSalesOrdersMTD : ICountOfCancelledSalesOrdersMTD
    {
        private readonly ISalesMetricMTDRepository _salesMetricMTDRepository;

        public CountOfCancelledSalesOrdersMTD(ISalesMetricMTDRepository salesMetricMTDRepository)
        {
            _salesMetricMTDRepository = salesMetricMTDRepository;
        }

        public decimal GetCountOfCancelledSalesOrdersMTD()
        {
            Tuple<decimal, string> count = _salesMetricMTDRepository.GetCountOfCancelledSalesOrdersMTD().ToTuple();

            //check for errors
            if (count.Item2 == null)
            {
                //No error
                return count.Item1;
            }
            else
            {
                throw new Exception(count.Item2);
            }
        }
    }
}

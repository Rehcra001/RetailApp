using DataAccessLibrary.SalesMetricsRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLogicLibrary.SalesMetrics.MTD
{
    public class CountOfOpenSalesOrdersMTD : ICountOfOpenSalesOrdersMTD
    {
        private readonly ISalesMetricMTDRepository _salesMetricMTDRepository;

        public CountOfOpenSalesOrdersMTD(ISalesMetricMTDRepository salesMetricMTDRepository)
        {
            _salesMetricMTDRepository = salesMetricMTDRepository;
        }

        public decimal GetCountOfOpenSalesOrdersMTD()
        {
            Tuple<decimal, string> count = _salesMetricMTDRepository.GetCountOfOpenSalesOrdersMTD().ToTuple();

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

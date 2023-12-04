using DataAccessLibrary.SalesMetricsRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLogicLibrary.SalesMetrics.YTD
{
    public class CountOfSalesOrdersYTD : ICountOfSalesOrdersYTD
    {
        private readonly ISalesMetricsYTDRepository _salesMetricsYTDRepository;

        public CountOfSalesOrdersYTD(ISalesMetricsYTDRepository salesMetricsYTDRepository)
        {
            _salesMetricsYTDRepository = salesMetricsYTDRepository;
        }

        public decimal GetCountOfSalesOrdersYTD()
        {
            Tuple<decimal, string> count = _salesMetricsYTDRepository.GetCountOfOrdersYTD().ToTuple();

            //Check for errors
            if (count.Item2 == null)
            {
                //No error
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

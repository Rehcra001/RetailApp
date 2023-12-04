using DataAccessLibrary.SalesMetricsRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLogicLibrary.SalesMetrics.YTD
{
    public class CountOfCancelledSalesOrdersYTD : ICountOfCancelledSalesOrdersYTD
    {
        private readonly ISalesMetricsYTDRepository _salesMetricsYTDRepository;

        public CountOfCancelledSalesOrdersYTD(ISalesMetricsYTDRepository salesMetricsYTDRepository)
        {
            _salesMetricsYTDRepository = salesMetricsYTDRepository;
        }

        public decimal GetCountOfCancelledOrders()
        {
            Tuple<decimal, string> count = _salesMetricsYTDRepository.GetCountOfCancelledSalesOrders().ToTuple();

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

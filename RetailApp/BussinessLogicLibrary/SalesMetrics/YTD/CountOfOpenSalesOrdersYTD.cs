using DataAccessLibrary.SalesMetricsRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLogicLibrary.SalesMetrics.YTD
{
    public class CountOfOpenSalesOrdersYTD : ICountOfOpenSalesOrdersYTD
    {
        private readonly ISalesMetricsYTDRepository _salesMetricsYTDRepository;

        public CountOfOpenSalesOrdersYTD(ISalesMetricsYTDRepository salesMetricsYTDRepository)
        {
            _salesMetricsYTDRepository = salesMetricsYTDRepository;
        }

        public decimal GetCountOfOpenSalesOrders()
        {
            Tuple<decimal, string> count = _salesMetricsYTDRepository.GetCountOfOpenSalesOrders().ToTuple();

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

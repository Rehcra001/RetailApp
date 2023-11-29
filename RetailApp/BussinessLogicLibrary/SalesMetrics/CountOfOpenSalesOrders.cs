using DataAccessLibrary.SalesMetricsRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLogicLibrary.SalesMetrics
{
    public class CountOfOpenSalesOrders : ICountOfOpenSalesOrders
    {
        private readonly ISalesMetricsYTDRepository _salesMetricsYTDRepository;

        public CountOfOpenSalesOrders(ISalesMetricsYTDRepository salesMetricsYTDRepository)
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

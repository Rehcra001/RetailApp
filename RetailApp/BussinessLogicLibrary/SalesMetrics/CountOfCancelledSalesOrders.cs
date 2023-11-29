using DataAccessLibrary.SalesMetricsRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLogicLibrary.SalesMetrics
{
    public class CountOfCancelledSalesOrders : ICountOfCancelledSalesOrders
    {
        private readonly ISalesMetricsYTDRepository _salesMetricsYTDRepository;

        public CountOfCancelledSalesOrders(ISalesMetricsYTDRepository salesMetricsYTDRepository)
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

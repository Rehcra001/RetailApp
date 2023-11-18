using DataAccessLibrary.SalesMetricsRepository;
using ModelsLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLogicLibrary.SalesMetrics
{
    public class SalesRevenueYTD : ISalesRevenueYTD
    {
        private ISalesMetricsYTDRepository _salesMetricsYTDRepository;

        public SalesRevenueYTD(ISalesMetricsYTDRepository salesMetricsYTDRepository)
        {
            _salesMetricsYTDRepository = salesMetricsYTDRepository;
        }

        public decimal GetRevenueYTD()
        {
            Tuple<decimal, string> revenue = _salesMetricsYTDRepository.GetRevenueYTD().ToTuple();
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

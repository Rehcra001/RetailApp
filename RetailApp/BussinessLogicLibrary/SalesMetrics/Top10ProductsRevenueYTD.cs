using DataAccessLibrary.SalesMetricsRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLogicLibrary.SalesMetrics
{
    public class Top10ProductsRevenueYTD : ITop10ProductsRevenueYTD
    {
        ISalesMetricsYTDRepository _salesMetricsYTDRepository;

        public Top10ProductsRevenueYTD(ISalesMetricsYTDRepository salesMetricsYTDRepository)
        {
            _salesMetricsYTDRepository = salesMetricsYTDRepository;
        }

        public decimal GetTop10ProductsRevenue()
        {
            Tuple<decimal, string> Top10Revenue = _salesMetricsYTDRepository.GetTop10ProductsRevenueYTD().ToTuple();
            //check for errors
            if (Top10Revenue.Item2 == null)
            {
                //No Error
                return Top10Revenue.Item1;
            }
            else
            {
                throw new Exception(Top10Revenue.Item2);
            }
        }
    }
}

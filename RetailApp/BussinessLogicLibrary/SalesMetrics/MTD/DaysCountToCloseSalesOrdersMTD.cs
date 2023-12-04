using ChartModelsLibrary.ChartModels;
using DataAccessLibrary.SalesMetricsRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLogicLibrary.SalesMetrics.MTD
{
    public class DaysCountToCloseSalesOrdersMTD : IDaysCountToCloseSalesOrdersMTD
    {
        private readonly ISalesMetricMTDRepository _salesMetricMTDRepository;
        public DaysCountToCloseSalesOrdersMTD(ISalesMetricMTDRepository salesMetricMTDRepository)
        {
            _salesMetricMTDRepository = salesMetricMTDRepository;
        }
        public HistogramModel GetDaysCountToCloseSalesOrdersMTD()
        {
            Tuple<HistogramModel, string> counts = _salesMetricMTDRepository.GetDaysCountToCloseSalesOrdersMTD().ToTuple();

            //Check for errors
            if (counts.Item2 == null)
            {
                //No error
                return counts.Item1;
            }
            else
            {
                throw new Exception(counts.Item2);
            }
        }
    }
}

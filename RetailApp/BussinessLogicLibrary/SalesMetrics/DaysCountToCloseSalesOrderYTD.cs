using ChartModelsLibrary.ChartModels;
using DataAccessLibrary.SalesMetricsRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLogicLibrary.SalesMetrics
{
    public class DaysCountToCloseSalesOrderYTD : IDaysCountToCloseSalesOrderYTD
    {
        private readonly ISalesMetricsYTDRepository _salesMetricsYTDRepository;

        public DaysCountToCloseSalesOrderYTD(ISalesMetricsYTDRepository salesMetricsYTDRepository)
        {
            _salesMetricsYTDRepository = salesMetricsYTDRepository;
        }

        public HistogramModel GetDaysCountToCloseOrdersYTD()
        {
            Tuple<HistogramModel, string> histogram = _salesMetricsYTDRepository.GetDaysCountToCloseOrdersYTD().ToTuple();

            //Check for errors
            if (histogram.Item2 == null)
            {
                //no error
                return histogram.Item1;
            }
            else
            {
                throw new Exception(histogram.Item2);
            }
        }
    }
}

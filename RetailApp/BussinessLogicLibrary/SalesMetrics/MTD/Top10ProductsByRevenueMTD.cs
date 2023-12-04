using ChartModelsLibrary.ChartModels;
using DataAccessLibrary.SalesMetricsRepository;

namespace BussinessLogicLibrary.SalesMetrics.MTD
{
    public class Top10ProductsByRevenueMTD : ITop10ProductsByRevenueMTD
    {
        private readonly ISalesMetricMTDRepository _salesMetricMTDRepository;

        public Top10ProductsByRevenueMTD(ISalesMetricMTDRepository salesMetricMTDRepository)
        {
            _salesMetricMTDRepository = salesMetricMTDRepository;
        }

        public BarChartModel GetTop10ProductsByRevenueMTD()
        {
            Tuple<BarChartModel, string> products = _salesMetricMTDRepository.GetTop10ProductsByRevenueMTD().ToTuple();

            //Check for errors
            if (products.Item2 == null)
            {
                //No error
                return products.Item1;
            }
            else
            {
                //Error
                throw new Exception(products.Item2);
            }
        }

    }
}

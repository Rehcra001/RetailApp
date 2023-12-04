using ChartModelsLibrary.ChartModels;
using DataAccessLibrary.SalesMetricsRepository;

namespace BussinessLogicLibrary.SalesMetrics.MTD
{
    public class ProductsByRevenueMTD : IProductsByRevenueMTD
    {
        private readonly ISalesMetricMTDRepository _salesMetricMTDRepository;

        public ProductsByRevenueMTD(ISalesMetricMTDRepository salesMetricMTDRepository)
        {
            _salesMetricMTDRepository = salesMetricMTDRepository;
        }

        public BarChartModel GetProductsByRevenueMTD()
        {
            Tuple<BarChartModel, string> products = _salesMetricMTDRepository.GetProductsByRevenueMTD().ToTuple();

            //check for errors
            if (products.Item2 == null)
            {
                //No error
                return products.Item1;
            }
            else
            {
                throw new Exception(products.Item2);
            }
        }
    }
}

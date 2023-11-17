using ChartModelsLibrary.ChartModels;
using DataAccessLibrary.SalesMetricsRepository;

namespace BussinessLogicLibrary.SalesMetrics
{
    public class Top10ProductsByRevenueYTDChart : ITop10ProductsByRevenueYTDChart
    {
        ISalesMetricsYTDRepository _salesMetricsYTDRepository;

        public Top10ProductsByRevenueYTDChart(ISalesMetricsYTDRepository salesMetricsYTDRepository)
        {
            _salesMetricsYTDRepository = salesMetricsYTDRepository;
        }

        public BarChartModel GetTop10ProductsRevenueYTD()
        {
            Tuple<BarChartModel, string> productsRevenue = _salesMetricsYTDRepository.GetTop10ProductsByRevenueYTDChart().ToTuple();

            //check for errors
            if (productsRevenue.Item2 == null)
            {
                //No errors
                productsRevenue.Item1.ChartTitle = "Revenue for Top 10 Products YTD";
                productsRevenue.Item1.VerticalAxisTitle = "Revenue";
                productsRevenue.Item1.HorizontalAxisTitle = "Products";
                return productsRevenue.Item1;
            }
            else
            {
                //error
                throw new Exception(productsRevenue.Item2);
            }
        }
    }
}

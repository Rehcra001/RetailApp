using ChartModelsLibrary.ChartModels;

namespace BussinessLogicLibrary.SalesMetrics.MTD
{
    public class SalesMetricsMTDManager : ISalesMetricsMTDManager
    {
        private readonly IRevenueMTD _revenueMTD;
        private readonly ITop10ProductsByRevenueMTD _top10ProductsByRevenueMTD;
        private readonly IProductsByRevenueMTD _productsByRevenueMTD;
        private readonly IRevenueTop10ProductsMTD _revenueTop10ProductsMTD;
        private readonly ICountOfSalesOrdersMTD _countOfSalesOrdersMTD;
        private readonly ICountOfOpenSalesOrdersMTD _countOfOpenSalesOrdersMTD;
        private readonly ICountOfCancelledSalesOrdersMTD _countOfCancelledSalesMTD;
        private readonly IDaysCountToCloseSalesOrdersMTD _daysCountToCloseSalesOrdersMTD;

        public SalesMetricsMTDManager(IRevenueMTD revenueMTD,
                                      ITop10ProductsByRevenueMTD top10ProductsByRevenueMTD,
                                      IProductsByRevenueMTD productsByRevenueMTD,
                                      IRevenueTop10ProductsMTD revenueTop10ProductsMTD,
                                      ICountOfSalesOrdersMTD countOfSalesOrdersMTD,
                                      ICountOfOpenSalesOrdersMTD countOfOpenSalesOrdersMTD,
                                      ICountOfCancelledSalesOrdersMTD countOfCancelledSalesMTD,
                                      IDaysCountToCloseSalesOrdersMTD daysCountToCloseSalesOrdersMTD)
        {
            _revenueMTD = revenueMTD;
            _top10ProductsByRevenueMTD = top10ProductsByRevenueMTD;
            _productsByRevenueMTD = productsByRevenueMTD;
            _revenueTop10ProductsMTD = revenueTop10ProductsMTD;
            _countOfSalesOrdersMTD = countOfSalesOrdersMTD;
            _countOfOpenSalesOrdersMTD = countOfOpenSalesOrdersMTD;
            _countOfCancelledSalesMTD = countOfCancelledSalesMTD;
            _daysCountToCloseSalesOrdersMTD = daysCountToCloseSalesOrdersMTD;
        }

        public decimal GetRevenueMTD()
        {
            return _revenueMTD.GetRevenueMTD();
        }

        public BarChartModel GetTop10ProductsByRevenueMTD()
        {
            return _top10ProductsByRevenueMTD.GetTop10ProductsByRevenueMTD();
        }

        public BarChartModel GetProductsByRevenueMTD()
        {
            return _productsByRevenueMTD.GetProductsByRevenueMTD();
        }

        public decimal GetRevenueTop10ProductsMTD()
        {
            return _revenueTop10ProductsMTD.GetRevenueTop10ProductsMTD();
        }

        public decimal GetCountOfSalesOrdersMTD()
        {
            return _countOfSalesOrdersMTD.GetCountOfSalesOrdersMTD();
        }

        public decimal GetCountOfOpenSalesOrdersMTD()
        {
            return _countOfOpenSalesOrdersMTD.GetCountOfOpenSalesOrdersMTD();
        }

        public decimal GetCountOfCancelledSalesOrdersMTD()
        {
            return _countOfCancelledSalesMTD.GetCountOfCancelledSalesOrdersMTD();
        }

        public HistogramModel GetDaysCountToCloseSalesOrderMTD()
        {
            return _daysCountToCloseSalesOrdersMTD.GetDaysCountToCloseSalesOrdersMTD();
        }
    }
}

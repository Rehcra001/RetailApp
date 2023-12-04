using ChartModelsLibrary.ChartModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLibrary.SalesMetricsRepository
{
    public interface ISalesMetricMTDRepository
    {
        (decimal, string) GetRevenueMTD();
        (decimal, string) GetRevenueTop10ProductsMTD();
        (BarChartModel, string) GetProductsByRevenueMTD();
        (BarChartModel, string) GetTop10ProductsByRevenueMTD();
        (decimal, string) GetCountOfSalesOrdersMTD();
        (decimal, string) GetCountOfOpenSalesOrdersMTD();
        (decimal, string) GetCountOfCancelledSalesOrdersMTD();
        (HistogramModel, string) GetDaysCountToCloseSalesOrdersMTD();
    }
}

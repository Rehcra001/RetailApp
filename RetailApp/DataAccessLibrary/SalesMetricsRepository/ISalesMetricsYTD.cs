using ChartModelsLibrary.ChartModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLibrary.SalesMetricsRepository
{
    public interface ISalesMetricsYTD
    {
        (BarChartModel, string) GetMonthlyRevenueYTD();
        (BarChartModel, string) GetTop10ProductsByRevenueYTD();
    }
}

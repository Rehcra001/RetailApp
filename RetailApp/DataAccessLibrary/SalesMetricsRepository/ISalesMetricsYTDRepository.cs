using ChartModelsLibrary.ChartModels;
using ModelsLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLibrary.SalesMetricsRepository
{
    public interface ISalesMetricsYTDRepository
    {
        (ValueModel, string) GetRevenueYTD();
        (BarChartModel, string) GetMonthlyRevenueYTDChart();
        (BarChartModel, string) GetTop10ProductsByRevenueYTDChart();
    }
}

using ChartModelsLibrary.ChartModels;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLogicLibrary.SalesMetrics.MTD
{
    public interface IDaysCountToCloseSalesOrdersMTD
    {
        HistogramModel GetDaysCountToCloseSalesOrdersMTD();
    }
}

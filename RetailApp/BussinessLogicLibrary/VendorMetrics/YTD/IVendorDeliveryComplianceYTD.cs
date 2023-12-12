using ChartModelsLibrary.ChartModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLogicLibrary.VendorMetrics.YTD
{
    public interface IVendorDeliveryComplianceYTD
    {
        HistogramModel GetVendorDeliveryComplianceYTD(int id);
    }
}

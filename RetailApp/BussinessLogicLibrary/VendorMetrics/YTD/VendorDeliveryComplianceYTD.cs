using ChartModelsLibrary.ChartModels;
using DataAccessLibrary.VendorMetricsRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLogicLibrary.VendorMetrics.YTD
{
    public class VendorDeliveryComplianceYTD : IVendorDeliveryComplianceYTD
    {
        private readonly IVendorMetricsRepositoryYTD _vendorMetricsRepositoryYTD;

        public VendorDeliveryComplianceYTD(IVendorMetricsRepositoryYTD vendorMetricsRepositoryYTD)
        {
            _vendorMetricsRepositoryYTD = vendorMetricsRepositoryYTD;
        }

        public HistogramModel GetVendorDeliveryComplianceYTD(int id)
        {
            Tuple<HistogramModel, string> vendorDelivery = _vendorMetricsRepositoryYTD.GetVendorDeliveryComplianceAllProducts(id).ToTuple();

            //check for errors
            if (vendorDelivery.Item2 == null)
            {
                //No error
                return vendorDelivery.Item1;
            }
            else
            {
                throw new Exception(vendorDelivery.Item2);
            }
        }
    }
}

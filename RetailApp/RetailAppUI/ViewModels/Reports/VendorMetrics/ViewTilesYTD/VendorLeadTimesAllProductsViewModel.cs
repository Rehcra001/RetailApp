using BussinessLogicLibrary.VendorMetrics.YTD;
using ChartModelsLibrary.ChartModels;
using RetailAppUI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace RetailAppUI.ViewModels.Reports.VendorMetrics.ViewTilesYTD
{
    public class VendorLeadTimesAllProductsViewModel : BaseViewModel, IVendorLeadTimesAllProductsViewModel
    {
        private readonly IVendorMetricsManagerYTD _vendorMetricsManagerYTD;

        private ISharedDataService _sharedData;
        public ISharedDataService SharedData
        {
            get { return _sharedData; }
            set { _sharedData = value; }
        }

        private HistogramModel _vendorLeadTimes;
        public HistogramModel VendorLeadTimes
        {
            get { return _vendorLeadTimes; }
            set { _vendorLeadTimes = value; OnPropertyChanged(); }
        }

        public VendorLeadTimesAllProductsViewModel(IVendorMetricsManagerYTD vendorMetricsManagerYTD,
                                                   ISharedDataService sharedDataService)
        {
            _vendorMetricsManagerYTD = vendorMetricsManagerYTD;
            SharedData = sharedDataService;

            LoadLeadTimes();
        }

        private void LoadLeadTimes()
        {
            try
            {
                //VendorLeadTimes = new HistogramModel();
                List<decimal> obs = new List<decimal>
                {
                    20,30,12,15,15,16,22,4,0,0,0
                };
                VendorLeadTimes.Observations = obs;
                //    VendorLeadTimes = _vendorMetricsManagerYTD.GetVendorLeadTimesYTD(20001);
                //    if (SharedData.SharedData != null)
                //    {
                //        int vendorId = (int)SharedData.SharedData;
                //        VendorLeadTimes = _vendorMetricsManagerYTD.GetVendorLeadTimesYTD(20001);
                //    }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to retrieve the lead Times for selected vendor.\r\n\r\n" + ex.Message,
                    "Retrieval Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }
    }
}

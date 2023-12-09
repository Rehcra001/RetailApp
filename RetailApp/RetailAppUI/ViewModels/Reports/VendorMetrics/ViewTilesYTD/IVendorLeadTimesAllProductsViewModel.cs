using ChartModelsLibrary.ChartModels;
using RetailAppUI.Services;

namespace RetailAppUI.ViewModels.Reports.VendorMetrics.ViewTilesYTD
{
    public interface IVendorLeadTimesAllProductsViewModel
    {
        ISharedDataService SharedData { get; set; }
        HistogramModel VendorLeadTimes { get; set; }
    }
}
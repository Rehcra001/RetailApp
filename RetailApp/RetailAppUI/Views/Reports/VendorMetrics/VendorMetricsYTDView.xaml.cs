using ChartModelsLibrary.ChartModels;
using ChartsLibrary.Histogram;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace RetailAppUI.Views.Reports.VendorMetrics
{
    /// <summary>
    /// Interaction logic for VendorMetricsYTDView.xaml
    /// </summary>
    public partial class VendorMetricsYTDView : UserControl
    {
        public VendorMetricsYTDView()
        {
            InitializeComponent();
        }


        private void VendorComboBox_SourceUpdated(object sender, DataTransferEventArgs e)
        {
            //Vendor Lead Times
            VendorLeadTimes.Children.Clear();
            HistogramControl vendorLeadTimes = new HistogramControl();
            vendorLeadTimes.HistogramData = (HistogramModel)VendorLeadTimes.DataContext;
            vendorLeadTimes.TitlesFontColor = Brushes.White;
            vendorLeadTimes.ChartTitleContent = "Days Count of Vendor Lead Time (All Products)";
            vendorLeadTimes.VerticalAxisTitleContent = "Count of Orders";
            vendorLeadTimes.HorizontalAxisTitleContent = "Days";
            vendorLeadTimes.ChartTitleFontSize = 14;
            vendorLeadTimes.VerticalAxisTitleFontSize = 12;
            vendorLeadTimes.VerticalAxisLabelFontSize = 12;
            vendorLeadTimes.HorizontalAxisTitleFontSize = 12;
            vendorLeadTimes.HorizontalAxisLabelFontSize = 12;
            VendorLeadTimes.Children.Add(vendorLeadTimes);
            vendorLeadTimes.HistogramControl_Loaded(null, null);

            //Vendor Compliance
            vendorDeliveryCompliance.Children.Clear();
            HistogramControl vendorDelCompliance = new HistogramControl();
            vendorDelCompliance.HistogramData = (HistogramModel)vendorDeliveryCompliance.DataContext;
            vendorDelCompliance.TitlesFontColor = Brushes.White;
            vendorDelCompliance.ChartTitleContent = "Vendor Delivery Compliance (All Products)";
            vendorDelCompliance.VerticalAxisTitleContent = "Count of Order Lines";
            vendorDelCompliance.HorizontalAxisTitleContent = ("<--Early Deliveries (neagative) : (positive) Late Deliveries-->");
            vendorDelCompliance.ChartTitleFontSize = 14;
            vendorDelCompliance.VerticalAxisTitleFontSize = 12;
            vendorDelCompliance.VerticalAxisLabelFontSize = 12;
            vendorDelCompliance.HorizontalAxisTitleFontSize = 12;
            vendorDelCompliance.HorizontalAxisLabelFontSize = 12;
            vendorDeliveryCompliance.Children.Add(vendorDelCompliance);
            vendorDelCompliance.HistogramControl_Loaded(null, null);
        }
    }
}

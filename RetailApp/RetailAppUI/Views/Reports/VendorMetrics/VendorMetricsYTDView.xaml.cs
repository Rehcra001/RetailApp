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
            HistogramContent.Children.Clear();
            HistogramControl vendorsHistogram = new HistogramControl();
            vendorsHistogram.HistogramData = (HistogramModel)HistogramContent.DataContext;
            vendorsHistogram.TitlesFontColor = Brushes.White;
            vendorsHistogram.ChartTitleContent = "Days Count of Vendor Lead Time (All Products)";
            vendorsHistogram.VerticalAxisTitleContent = "Count of Orders";
            vendorsHistogram.HorizontalAxisTitleContent = "Days";
            HistogramContent.Children.Add(vendorsHistogram);
            vendorsHistogram.HistogramControl_Loaded(null, null);
        }
    }
}

using ModelsLibrary.ChartModels;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ChartsLibrary.BarCharts
{
    /// <summary>
    /// Interaction logic for BarChart.xaml
    /// </summary>
    public partial class BarChart : UserControl
    {

        public BarChartModel BarChartData
        {
            get { return (BarChartModel)GetValue(BarChartDataProperty); }
            set { SetValue(BarChartDataProperty, value); }
        }
        public static readonly DependencyProperty BarChartDataProperty =
            DependencyProperty.Register("BarChartData", typeof(BarChartModel), typeof(BarChart), new PropertyMetadata(null));



        public bool ShowChartTitle
        {
            get { return (bool)GetValue(ShowChartTitleProperty); }
            set { SetValue(ShowChartTitleProperty, value); }
        }
        public static readonly DependencyProperty ShowChartTitleProperty =
            DependencyProperty.Register("ShowChartTitle", typeof(bool), typeof(BarChart), new PropertyMetadata(true));



        public bool ShowVerticalAxisTitle
        {
            get { return (bool)GetValue(ShowVerticalAxisTitleProperty); }
            set { SetValue(ShowVerticalAxisTitleProperty, value); }
        }
        public static readonly DependencyProperty ShowVerticalAxisTitleProperty =
            DependencyProperty.Register("ShowVerticalAxisTitle", typeof(bool), typeof(BarChart), new PropertyMetadata(true));



        public bool ShowHorizontalAxisTitle
        {
            get { return (bool)GetValue(ShowHorizontalAxisTitleProperty); }
            set { SetValue(ShowHorizontalAxisTitleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ShowHorizontalAxisTitle.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ShowHorizontalAxisTitleProperty =
            DependencyProperty.Register("ShowHorizontalAxisTitle", typeof(bool), typeof(BarChart), new PropertyMetadata(true));





        public BarChart()
        {
            InitializeComponent();
        }


        private void AddChartTitle()
        {
            FormattedText str = new FormattedText(BarChartData.ChartTitle,
                                                  CultureInfo.GetCultureInfo("en-za"),
                                                  0,
                                                  new Typeface("NewTimesRoman"),
                                                  20,
                                                  Brushes.Black,
                                                  1);
            TextBlock chartTitle = new TextBlock();
            chartTitle.FontFamily = new FontFamily("NewTimeRoman");
            chartTitle.FontSize = 20;
            chartTitle.Text = str.Text;

            BarChartCanvas.Children.Add(chartTitle);
            double height = str.Height;
            double width = str.Width;
            double left = (BarChartCanvas.ActualWidth * 0.5) - width * 0.5;
            Canvas.SetTop(chartTitle, 5);
            Canvas.SetLeft(chartTitle, left);
        }

        
        private void AddVerticalAxisTitle()
        {
            FormattedText str = new FormattedText(BarChartData.VerticalAxisTitle,
                                                  CultureInfo.GetCultureInfo("en-za"),
                                                  0,
                                                  new Typeface("NewTimesRoman"),
                                                  18,
                                                  Brushes.Black,
                                                  1);
            TextBlock verticalAxisTitle = new TextBlock();
            verticalAxisTitle.FontFamily = new FontFamily("NewTimeRoman");
            verticalAxisTitle.FontSize = 18;
            verticalAxisTitle.Text = str.Text;
            RotateTransform rotate = new RotateTransform();
            rotate.Angle = 270;
            verticalAxisTitle.RenderTransform = rotate;

            BarChartCanvas.Children.Add(verticalAxisTitle);
            double height = str.Height;
            double width = str.Width;
            double top = (BarChartCanvas.ActualHeight * 0.5) + width * 0.5;
            Canvas.SetTop(verticalAxisTitle, top);
            Canvas.SetLeft(verticalAxisTitle, 5);
        }

        private void AddHorizontalAxisTitle()
        {
            FormattedText str = new FormattedText(BarChartData.HorizontalAxisTitle,
                                                  CultureInfo.GetCultureInfo("en-za"),
                                                  0,
                                                  new Typeface("NewTimesRoman"),
                                                  18,
                                                  Brushes.Black,
                                                  1);
            TextBlock horizontalAxisTitle = new TextBlock();
            horizontalAxisTitle.FontFamily = new FontFamily("NewTimeRoman");
            horizontalAxisTitle.FontSize = 18;
            horizontalAxisTitle.Text = str.Text;

            BarChartCanvas.Children.Add(horizontalAxisTitle);
            double height = str.Height;
            double width = str.Width;
            double left = (BarChartCanvas.ActualWidth * 0.5) - width * 0.5;
            double bottom = (BarChartCanvas.ActualHeight - 5) - height;
            Canvas.SetTop(horizontalAxisTitle, bottom);
            Canvas.SetLeft(horizontalAxisTitle, left);
        }

        private void BarChart_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (BarChartData is not null)
            {
                BarChartCanvas.Children.Clear();
                if (ShowChartTitle)
                {
                    AddChartTitle();
                }

                if (ShowVerticalAxisTitle)
                {
                    AddVerticalAxisTitle();
                }
                
                if (ShowHorizontalAxisTitle)
                {
                    AddHorizontalAxisTitle();
                }
                
            }
        }
    }
}

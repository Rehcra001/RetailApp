using ModelsLibrary.ChartModels;
using System;
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
        private double ChartTitleHeight { get; set; }
        private double VerticalAxisTitleWidth { get; set; }
        private double HorizontalAxisTitleHeight { get; set; }
        private double VerticalAxisWidth { get; set; }
        private double HorizontalAxisHeight { get; set; }
        private double ChartWidth { get; set; }
        private double ChartHeight { get; set; }
        private double ChartAreaHeight { get; set; }
        private double ChartAreaWidth { get; set; }
        private double MaxValue { get; set; }
        private int MaxValueDescriptionLength { get; set; }

        private const int MARGIN = 5;

        #region Bar Chart Model Data
        public BarChartModel BarChartData
        {
            get { return (BarChartModel)GetValue(BarChartDataProperty); }
            set { SetValue(BarChartDataProperty, value); }
        }
        public static readonly DependencyProperty BarChartDataProperty =
            DependencyProperty.Register("BarChartData", typeof(BarChartModel), typeof(BarChart), new PropertyMetadata(null));

        #endregion

        #region Titles DP
        public string TitlesFontFamily
        {
            get { return (string)GetValue(ChartTitleFontFamilyProperty); }
            set { SetValue(ChartTitleFontFamilyProperty, value); }
        }
        public static readonly DependencyProperty ChartTitleFontFamilyProperty =
            DependencyProperty.Register("ChartTitleFontFamily", typeof(string), typeof(BarChart), new PropertyMetadata("NewTimesRoman"));

        public Brush TitlesFontColor
        {
            get { return (Brush)GetValue(ChartTitleFontColorProperty); }
            set { SetValue(ChartTitleFontColorProperty, value); }
        }
        public static readonly DependencyProperty ChartTitleFontColorProperty =
            DependencyProperty.Register("ChartTitleFontColor", typeof(Brush), typeof(BarChart), new PropertyMetadata(Brushes.Black));

        #endregion

        #region Chart Title DP

        public bool ShowChartTitle
        {
            get { return (bool)GetValue(ShowChartTitleProperty); }
            set { SetValue(ShowChartTitleProperty, value); }
        }
        public static readonly DependencyProperty ShowChartTitleProperty =
            DependencyProperty.Register("ShowChartTitle", typeof(bool), typeof(BarChart), new PropertyMetadata(true));


        public int ChartTitleFontSize
        {
            get { return (int)GetValue(ChartTitleFontSizeProperty); }
            set { SetValue(ChartTitleFontSizeProperty, value); }
        }
        public static readonly DependencyProperty ChartTitleFontSizeProperty =
            DependencyProperty.Register("ChartTitleFontSize", typeof(int), typeof(BarChart), new PropertyMetadata(16));

        #endregion

        #region Vertical Axis Title
        public bool ShowVerticalAxisTitle
        {
            get { return (bool)GetValue(ShowVerticalAxisTitleProperty); }
            set { SetValue(ShowVerticalAxisTitleProperty, value); }
        }
        public static readonly DependencyProperty ShowVerticalAxisTitleProperty =
            DependencyProperty.Register("ShowVerticalAxisTitle", typeof(bool), typeof(BarChart), new PropertyMetadata(true));

        public int VerticalAxisTitleFontSize
        {
            get { return (int)GetValue(VerticalAxisTitleFontSizeProperty); }
            set { SetValue(VerticalAxisTitleFontSizeProperty, value); }
        }
        public static readonly DependencyProperty VerticalAxisTitleFontSizeProperty =
            DependencyProperty.Register("VerticalAxisTitleFontSize", typeof(int), typeof(BarChart), new PropertyMetadata(16));
        #endregion

        #region Horizontal Axis Title
        public bool ShowHorizontalAxisTitle
        {
            get { return (bool)GetValue(ShowHorizontalAxisTitleProperty); }
            set { SetValue(ShowHorizontalAxisTitleProperty, value); }
        }
        public static readonly DependencyProperty ShowHorizontalAxisTitleProperty =
            DependencyProperty.Register("ShowHorizontalAxisTitle", typeof(bool), typeof(BarChart), new PropertyMetadata(true));


        public int HorizontalAxisFontSize
        {
            get { return (int)GetValue(HorizontalAxisFontSizeProperty); }
            set { SetValue(HorizontalAxisFontSizeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for HorizontalAxisFontSize.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HorizontalAxisFontSizeProperty =
            DependencyProperty.Register("HorizontalAxisFontSize", typeof(int), typeof(BarChart), new PropertyMetadata(16));


        #endregion



        public BarChart()
        {
            InitializeComponent();
        }

        private void BarChart_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (BarChartData is not null)
            {
                ChartHeight = BarChartCanvas.ActualHeight;
                ChartWidth = BarChartCanvas.ActualWidth;

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
                AddAxes();
            }
        }

        private void AddAxes()
        {
            //CalcVerticalAxisWidth();
            //CalcHorizontalAxisHeight();
            //CalcChartAreaWidth();
            //CalcChartAreaHeight();
            

            //AddVerticalAxis();
            //AddHorizontalAxis();
        }


        private void AddVerticalAxis()
        {
            throw new NotImplementedException();
        }

        private void AddHorizontalAxis()
        {
            throw new NotImplementedException();
        }      
        
        private void CalcChartAreaHeight()
        {
            throw new NotImplementedException();
        }
        private void CalcChartAreaWidth()
        {
            throw new NotImplementedException();
        }

        private void CalcHorizontalAxisHeight()
        {
            throw new NotImplementedException();
        }

        private void CalcVerticalAxisWidth()
        {
            throw new NotImplementedException();
        }

        private void AddChartTitle()
        {
            FormattedText str = new FormattedText(BarChartData.ChartTitle,
                                                  CultureInfo.GetCultureInfo("en-za"),
                                                  FlowDirection.LeftToRight,
                                                  new Typeface(TitlesFontFamily),
                                                  ChartTitleFontSize,
                                                  TitlesFontColor,
                                                  1);
            TextBlock chartTitle = new TextBlock();
            chartTitle.FontFamily = new FontFamily(TitlesFontFamily);
            chartTitle.FontSize = ChartTitleFontSize;
            chartTitle.Foreground = TitlesFontColor;
            chartTitle.Text = str.Text;

            BarChartCanvas.Children.Add(chartTitle);

            //Position Chart Title
            double height = str.Height;
            double width = str.Width;
            double left = (ChartWidth * 0.5) - width * 0.5;
            Canvas.SetTop(chartTitle, MARGIN);
            Canvas.SetLeft(chartTitle, left);

            ChartTitleHeight = height + MARGIN * 2;
        }

        private void AddVerticalAxisTitle()
        {
            FormattedText str = new FormattedText(BarChartData.VerticalAxisTitle,
                                                  CultureInfo.GetCultureInfo("en-za"),
                                                  0,
                                                  new Typeface("NewTimesRoman"),
                                                  VerticalAxisTitleFontSize,
                                                  TitlesFontColor,
                                                  1);
            TextBlock verticalAxisTitle = new TextBlock();
            verticalAxisTitle.FontFamily = new FontFamily("NewTimeRoman");
            verticalAxisTitle.FontSize = VerticalAxisTitleFontSize;
            verticalAxisTitle.Foreground = TitlesFontColor;
            verticalAxisTitle.Text = str.Text;
            RotateTransform rotate = new RotateTransform();
            rotate.Angle = 270;
            verticalAxisTitle.RenderTransform = rotate;

            BarChartCanvas.Children.Add(verticalAxisTitle);

            //Position Vertical Axis Title
            double height = str.Height;
            double width = str.Width;
            double top = (ChartHeight * 0.5) + width * 0.5;
            Canvas.SetTop(verticalAxisTitle, top);
            Canvas.SetLeft(verticalAxisTitle, MARGIN);

            VerticalAxisTitleWidth = height + MARGIN * 2;
        }

        private void AddHorizontalAxisTitle()
        {
            FormattedText str = new FormattedText(BarChartData.HorizontalAxisTitle,
                                                  CultureInfo.GetCultureInfo("en-za"),
                                                  0,
                                                  new Typeface("NewTimesRoman"),
                                                  HorizontalAxisFontSize,
                                                  Brushes.Black,
                                                  1);
            TextBlock horizontalAxisTitle = new TextBlock();
            horizontalAxisTitle.FontFamily = new FontFamily("NewTimeRoman");
            horizontalAxisTitle.FontSize = HorizontalAxisFontSize;
            horizontalAxisTitle.Foreground = TitlesFontColor;
            horizontalAxisTitle.Text = str.Text;

            BarChartCanvas.Children.Add(horizontalAxisTitle);

            //Position Horizontal Axis Title
            double height = str.Height; 
            double width = str.Width;
            double left = (ChartWidth * 0.5) - width * 0.5;
            double bottom = (ChartHeight - MARGIN) - height;
            Canvas.SetTop(horizontalAxisTitle, bottom);
            Canvas.SetLeft(horizontalAxisTitle, left);

            HorizontalAxisTitleHeight = height + MARGIN * 2;
        }

        
    }
}

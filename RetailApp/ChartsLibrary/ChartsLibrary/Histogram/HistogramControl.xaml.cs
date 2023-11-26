using ChartModelsLibrary.ChartModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ChartsLibrary.Histogram
{
    /// <summary>
    /// Interaction logic for HistogramControl.xaml
    /// </summary>
    public partial class HistogramControl : UserControl
    {
        private bool _isLoaded = false;

        private double ChartHeight { get; set; }
        private double ChartWidth { get; set; }
        private double ChartTitleHeight { get; set; }
        private double ChartVerticalAxisTitleWidth { get; set; }
        private double ChartHorizontalAxisTitleHeight { get; set; }
        private double VerticalAxisWidth { get; set; }
        private double HorizontalAxisHeight { get; set; }
        private double ChartAreaHeight { get; set; }
        private double ChartAreaWidth { get; set; }
        private double BarWidth { get; set; }
        private int[] Bins { get; set; }
        private decimal LowerBound { get; set; }
        private decimal UpperBound { get; set; }
        private decimal BinWidth { get; set; }
        private List<string> VerticalAxisLabels { get; set; }
        private List<string> HorizontalAxisLabels { get; set; }

        private const double MARGIN = 5;

        #region Histogram Data Model

        public HistogramModel HistogramData
        {
            get { return (HistogramModel)GetValue(HistogramDataProperty); }
            set { SetValue(HistogramDataProperty, value); }
        }
        public static readonly DependencyProperty HistogramDataProperty =
            DependencyProperty.Register("HistogramData", typeof(HistogramModel), typeof(HistogramControl), new PropertyMetadata(null));

        #endregion

        #region Histogram DP
        public CultureInfo HistogramCulture
        {
            get { return (CultureInfo)GetValue(HistogramCultureProperty); }
            set { SetValue(HistogramCultureProperty, value); }
        }
        public static readonly DependencyProperty HistogramCultureProperty =
            DependencyProperty.Register("HistogramCulture", typeof(CultureInfo), typeof(HistogramControl), new PropertyMetadata(new CultureInfo("en-za")));

        public int NumberOfBins
        {
            get { return (int)GetValue(NumberOfBinsProperty); }
            set { SetValue(NumberOfBinsProperty, value); }
        }
        public static readonly DependencyProperty NumberOfBinsProperty =
            DependencyProperty.Register("NumberOfBins", typeof(int), typeof(HistogramControl), new PropertyMetadata(0));

        #endregion

        #region Titles DP


        public string TitlesFontFamily
        {
            get { return (string)GetValue(TitlesFontFamilyProperty); }
            set { SetValue(TitlesFontFamilyProperty, value); }
        }
        public static readonly DependencyProperty TitlesFontFamilyProperty =
            DependencyProperty.Register("TitlesFontFamily", typeof(string), typeof(HistogramControl), new PropertyMetadata("Courier"));

        public Brush TitlesFontColor
        {
            get { return (Brush)GetValue(TitlesFontColorProperty); }
            set { SetValue(TitlesFontColorProperty, value); }
        }
        public static readonly DependencyProperty TitlesFontColorProperty =
            DependencyProperty.Register("TitlesFontColor", typeof(Brush), typeof(HistogramControl), new PropertyMetadata(Brushes.Black));

        #endregion

        #region Chart Title DP
        public bool ShowChartTitle
        {
            get { return (bool)GetValue(ShowChartTitleProperty); }
            set { SetValue(ShowChartTitleProperty, value); }
        }
        public static readonly DependencyProperty ShowChartTitleProperty =
            DependencyProperty.Register("ShowChartTitle", typeof(bool), typeof(HistogramControl), new PropertyMetadata(true));


        public int ChartTitleFontSize
        {
            get { return (int)GetValue(ChartTitleFontSizeProperty); }
            set { SetValue(ChartTitleFontSizeProperty, value); }
        }
        public static readonly DependencyProperty ChartTitleFontSizeProperty =
            DependencyProperty.Register("ChartTitleFontSize", typeof(int), typeof(HistogramControl), new PropertyMetadata(16));



        #endregion

        #region Chart Vertical Axis Title DP


        public bool ShowVerticalAxisTitle
        {
            get { return (bool)GetValue(ShowVerticalAxisTitleProperty); }
            set { SetValue(ShowVerticalAxisTitleProperty, value); }
        }
        public static readonly DependencyProperty ShowVerticalAxisTitleProperty =
            DependencyProperty.Register("ShowVerticalAxisTitle", typeof(bool), typeof(HistogramControl), new PropertyMetadata(true));


        public int VerticalAxisTitleFontSize
        {
            get { return (int)GetValue(VerticalAxisTitleFontSizeProperty); }
            set { SetValue(VerticalAxisTitleFontSizeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for VerticalAxisTitleFontSize.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty VerticalAxisTitleFontSizeProperty =
            DependencyProperty.Register("VerticalAxisTitleFontSize", typeof(int), typeof(HistogramControl), new PropertyMetadata(16));

        #endregion

        #region Chart Horizontal Axis Title DP


        public bool ShowHorizontalAxisTitle
        {
            get { return (bool)GetValue(ShowHorizontalAxisTitleProperty); }
            set { SetValue(ShowHorizontalAxisTitleProperty, value); }
        }
        public static readonly DependencyProperty ShowHorizontalAxisTitleProperty =
            DependencyProperty.Register("ShowHorizontalAxisTitle", typeof(bool), typeof(HistogramControl), new PropertyMetadata(true));



        public int HorizontalAxisTitleFontSize
        {
            get { return (int)GetValue(HorizontalAxisTitleFontSizeProperty); }
            set { SetValue(HorizontalAxisTitleFontSizeProperty, value); }
        }
        public static readonly DependencyProperty HorizontalAxisTitleFontSizeProperty =
            DependencyProperty.Register("HorizontalAxisTitleFontSize", typeof(int), typeof(HistogramControl), new PropertyMetadata(16));

        #endregion

        #region Vertical Axis DP
        public bool ShowVerticalAxis
        {
            get { return (bool)GetValue(ShowVerticalAxisProperty); }
            set { SetValue(ShowVerticalAxisProperty, value); }
        }
        public static readonly DependencyProperty ShowVerticalAxisProperty =
            DependencyProperty.Register("ShowVerticalAxis", typeof(bool), typeof(HistogramControl), new PropertyMetadata(true));

        public int NumberOfVerticalAxisLabels
        {
            get { return (int)GetValue(NumberOfVerticalAxisLabelsProperty); }
            set { SetValue(NumberOfVerticalAxisLabelsProperty, value); }
        }
        public static readonly DependencyProperty NumberOfVerticalAxisLabelsProperty =
            DependencyProperty.Register("NumberOfVericalAxisLabels", typeof(int), typeof(HistogramControl), new PropertyMetadata(8));


        public int VerticalAxisLabelFontSize
        {
            get { return (int)GetValue(VerticalAxisLabelFontSizeProperty); }
            set { SetValue(VerticalAxisLabelFontSizeProperty, value); }
        }
        public static readonly DependencyProperty VerticalAxisLabelFontSizeProperty =
            DependencyProperty.Register("VerticalAxisLabelFontSize", typeof(int), typeof(HistogramControl), new PropertyMetadata(16));

        #endregion

        #region Horizontal Axis DP

        public bool ShowHorizontalAxis
        {
            get { return (bool)GetValue(ShowHorizontalAxisProperty); }
            set { SetValue(ShowHorizontalAxisProperty, value); }
        }
        public static readonly DependencyProperty ShowHorizontalAxisProperty =
            DependencyProperty.Register("ShowHorizontalAxis", typeof(bool), typeof(HistogramControl), new PropertyMetadata(true));

        public int HorizontalAxisLabelFontSize
        {
            get { return (int)GetValue(HorizontalAxisLabelFontSizeProperty); }
            set { SetValue(HorizontalAxisLabelFontSizeProperty, value); }
        }
        public static readonly DependencyProperty HorizontalAxisLabelFontSizeProperty =
            DependencyProperty.Register("HorizontalAxisLabelFontSize", typeof(int), typeof(HistogramControl), new PropertyMetadata(16));

        #endregion

        #region ChartArea DP

        #endregion

        public HistogramControl()
        {
            InitializeComponent();
        }

        private void HistogramControl_Loaded(object sender, RoutedEventArgs e)
        {
            _isLoaded = true;
            if (HistogramData is not null)
            {
                CalcConstantProperties();
                DrawHistogram();
            }
        }

        private void HistogramControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (_isLoaded)
            {
                DrawHistogram();
            }
        }

        private void DrawHistogram()
        {
            CalcVariableProperties();
        }
        private void CalcVariableProperties()
        {
            //Chart Height
            
            //Chart Width

            //Chart Area Height

            //Chart Area Width

            //Bar Width
            throw new NotImplementedException();
        }

        private void CalcConstantProperties()
        {
            //Chart Title
            if (!String.IsNullOrWhiteSpace(HistogramData.ChartTitle) && ShowChartTitle)
            {
                SetChartTitleHeight();
            }
            //Chart Vertical Axis Title
            if (!String.IsNullOrWhiteSpace(HistogramData.VerticalAxisTitle) && ShowVerticalAxisTitle)
            {
                SetVerticalAxisTitleWidth();
            }
            //Chart Horizontal Axis Title
            if (!String.IsNullOrWhiteSpace(HistogramData.HorizontalAxisTitle) && ShowHorizontalAxisTitle)
            {
                SetHorizontalAxisTitleHeight();
            }
            //Fill the bins
            FillBins();

            //Chart Vertical Axis
            if (ShowVerticalAxis)
            {
                //Generate list of vertical axis labels
                GenerateVerticalAxisLabels();
                SetVerticalAxisWidth();
            }

            //Chart Horizontal Axis
            if (ShowHorizontalAxis)
            {
                GenerateHorizontalAxisLabels();
                SetHorizontalAxisHeight();
            }

        }

        private void SetHorizontalAxisHeight()
        {
            List<double> labelWidths = new List<double>();
            foreach (string label in HorizontalAxisLabels)
            {
                //Will be rotated so width will be the height
                double width = CalcStringHeightAndWidth(label, TitlesFontFamily, HorizontalAxisLabelFontSize, TitlesFontColor).Width;
                labelWidths.Add(width);
            }
            HorizontalAxisHeight = labelWidths.Max() + MARGIN * 2;
        }

        private void GenerateHorizontalAxisLabels()
        {
            HorizontalAxisLabels = new List<string>();
            for (int i = 0; i <= Bins.Length; i++)
            {
                HorizontalAxisLabels.Add(((LowerBound) + BinWidth * i).ToString());
            }
        }

        private void SetVerticalAxisWidth()
        {
            List<double> labelWidths = new List<double>();
            foreach (string label in VerticalAxisLabels)
            {
                double width = CalcStringHeightAndWidth(label, TitlesFontFamily, VerticalAxisLabelFontSize, TitlesFontColor).Width;
                labelWidths.Add(width);
            }
            VerticalAxisWidth = labelWidths.Max() + MARGIN * 2;
        }

        private void GenerateVerticalAxisLabels()
        {
            int max = Bins.Max();
            //find next highest value that has mod of zero given the number of labels
            int maxLabelValue = max;
            while (maxLabelValue % NumberOfVerticalAxisLabels != 0)
            {
                maxLabelValue++;
            }

            int labelDivision = maxLabelValue / NumberOfVerticalAxisLabels;

            VerticalAxisLabels = new List<string>();
            for (int i = 0; i <= NumberOfVerticalAxisLabels; i++)
            {
                VerticalAxisLabels.Add((labelDivision * i).ToString("N2"));
            }
        }

        private void FillBins()
        {
            //Determine lower bound
            LowerBound = Math.Floor(HistogramData.Observations!.Min());
            //Determine upper bound
            UpperBound = Math.Ceiling(HistogramData.Observations!.Max());
            //Determine number of bins
            CalcNumberOfBins();
            //Determine bin width
            BinWidth = CalcBinWidth();
            //Create bins
            Bins = new int[NumberOfBins];
            //Fill the bins with observations
            AddToBins();
        }

        private void AddToBins()
        {
            foreach (decimal observation in HistogramData.Observations!)
            {
                int index = (int)((observation - LowerBound) / BinWidth);
                if (index > Bins.Length - 1)
                {
                    index = Bins.Length - 1;
                }
                if (index < 0)
                {
                    index = 0;
                }
                Bins[index]++;
            }
        }

        private decimal CalcBinWidth()
        {
            return (UpperBound - LowerBound) / (decimal)NumberOfBins;
        }

        private void CalcNumberOfBins()
        {
            //Check if NumberOfBins is greater than zero
            //if yes then use NumberOfBins
            //if no then calculate number of bins using Sturges formula
            if (NumberOfBins == 0)
            {
                NumberOfBins = (int)Math.Ceiling(1 + (3.3 * Math.Log10(HistogramData.Observations!.Count())));
            }
        }

        private void SetHorizontalAxisTitleHeight()
        {
            string str = HistogramData.HorizontalAxisTitle;
            ChartHorizontalAxisTitleHeight = CalcStringHeightAndWidth(str, TitlesFontFamily, HorizontalAxisTitleFontSize, TitlesFontColor).Height + MARGIN * 2;
        }

        private void SetVerticalAxisTitleWidth()
        {
            string str = HistogramData.VerticalAxisTitle;
            //Text will be vertical so height becomes the width
            ChartVerticalAxisTitleWidth = CalcStringHeightAndWidth(str, TitlesFontFamily, VerticalAxisTitleFontSize, TitlesFontColor).Height + MARGIN * 2;
        }

        private void SetChartTitleHeight()
        {
            string str = HistogramData.ChartTitle;
            ChartTitleHeight = CalcStringHeightAndWidth(str, TitlesFontFamily, ChartTitleFontSize, TitlesFontColor).Height + MARGIN * 2;
        }        

        private (double Height, double Width) CalcStringHeightAndWidth(string str, string fontFamily, int fontSize, Brush fontColor, double pixelsPerDip = 1)
        {
            FormattedText s = new FormattedText(str, HistogramCulture, FlowDirection.LeftToRight, new Typeface(fontFamily), fontSize, fontColor, pixelsPerDip);

            return (s.Height, s.Width);
        }
    }
}

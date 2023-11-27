using ChartModelsLibrary.ChartModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
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
        private double ChartTitleWidth { get; set; }
        private double VerticalAxisTitleHeight { get; set; }
        private double VerticalAxisTitleWidth { get; set; }
        private double HorizontalAxisTitleHeight { get; set; }
        private double HorizontalAxisTitleWidth { get; set; }
        private double VerticalAxisWidth { get; set; }
        private double HorizontalAxisHeight { get; set; }
        private double ChartAreaHeight { get; set; }
        private double ChartAreaWidth { get; set; }
        private double BarWidth { get; set; }

        private int[] Bins { get; set; }
        private decimal LowerBound { get; set; }
        private decimal UpperBound { get; set; }
        private decimal BinWidth { get; set; }

        private double MaxLabelValue { get; set; }
        private List<string> VerticalAxisLabels { get; set; }
        private List<double> VerticalAxisLabelWidths { get; set; }
        private List<TextBlock> VerticalAxisLabelTextBlocks { get; set; }
        private double VerticalAxisLabelHeight { get; set; }

        private List<string> HorizontalAxisLabels { get; set; }
        private List<double> HorizontalAxisLabelHeights { get; set; }
        private List<TextBlock> HorizontalAxisLabelTextBlocks { get; set; }
        private double HorizontalAxisLabelWidth { get; set; }

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

        public TextBlock ChartTitleTextBlock
        {
            get { return (TextBlock)GetValue(ChartTitleTextBlockProperty); }
            set { SetValue(ChartTitleTextBlockProperty, value); }
        }
        public static readonly DependencyProperty ChartTitleTextBlockProperty =
            DependencyProperty.Register("ChartTitleTextBlock", typeof(TextBlock), typeof(HistogramControl), new PropertyMetadata(null));

        public string ChartTitleContent
        {
            get { return (string)GetValue(ChartTitleContentProperty); }
            set { SetValue(ChartTitleContentProperty, value); }
        }
        public static readonly DependencyProperty ChartTitleContentProperty =
            DependencyProperty.Register("ChartTitleContent", typeof(string), typeof(HistogramControl), new PropertyMetadata(""));



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
        public static readonly DependencyProperty VerticalAxisTitleFontSizeProperty =
            DependencyProperty.Register("VerticalAxisTitleFontSize", typeof(int), typeof(HistogramControl), new PropertyMetadata(16));



        public TextBlock VerticalAxisTitleTextBlock
        {
            get { return (TextBlock)GetValue(VerticalAxisTitleTextBlockProperty); }
            set { SetValue(VerticalAxisTitleTextBlockProperty, value); }
        }
        public static readonly DependencyProperty VerticalAxisTitleTextBlockProperty =
            DependencyProperty.Register("VerticalAxisTitleTextBlock", typeof(TextBlock), typeof(HistogramControl), new PropertyMetadata(null));



        public string VerticalAxisTitleContent
        {
            get { return (string)GetValue(VerticalAxisTitleContentProperty); }
            set { SetValue(VerticalAxisTitleContentProperty, value); }
        }
        public static readonly DependencyProperty VerticalAxisTitleContentProperty =
            DependencyProperty.Register("VerticalAxisTitleContent", typeof(string), typeof(HistogramControl), new PropertyMetadata(""));

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

        public TextBlock HorizontalAxisTitleTextBlock
        {
            get { return (TextBlock)GetValue(HorizontalAxisTitleTextBlockProperty); }
            set { SetValue(HorizontalAxisTitleTextBlockProperty, value); }
        }
        public static readonly DependencyProperty HorizontalAxisTitleTextBlockProperty =
            DependencyProperty.Register("HorizontalAxisTitleTextBlock", typeof(TextBlock), typeof(HistogramControl), new PropertyMetadata(null));

        public string HorizontalAxisTitleContent
        {
            get { return (string)GetValue(HorizontalAxisTitleContentProperty); }
            set { SetValue(HorizontalAxisTitleContentProperty, value); }
        }
        public static readonly DependencyProperty HorizontalAxisTitleContentProperty =
            DependencyProperty.Register("HorizontalAxisTitleContent", typeof(string), typeof(HistogramControl), new PropertyMetadata(""));





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
            DependencyProperty.Register("NumberOfVericalAxisLabels", typeof(int), typeof(HistogramControl), new PropertyMetadata(5));


        public int VerticalAxisLabelFontSize
        {
            get { return (int)GetValue(VerticalAxisLabelFontSizeProperty); }
            set { SetValue(VerticalAxisLabelFontSizeProperty, value); }
        }
        public static readonly DependencyProperty VerticalAxisLabelFontSizeProperty =
            DependencyProperty.Register("VerticalAxisLabelFontSize", typeof(int), typeof(HistogramControl), new PropertyMetadata(12));

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
            DependencyProperty.Register("HorizontalAxisLabelFontSize", typeof(int), typeof(HistogramControl), new PropertyMetadata(12));

        #endregion

        #region ChartArea DP


        public bool ShowChartAreaBorder
        {
            get { return (bool)GetValue(ShowChartAreaBorderProperty); }
            set { SetValue(ShowChartAreaBorderProperty, value); }
        }
        public static readonly DependencyProperty ShowChartAreaBorderProperty =
            DependencyProperty.Register("ShowChartAreaBorder", typeof(bool), typeof(HistogramControl), new PropertyMetadata(true));


        public bool ShowChartAreaGridLines
        {
            get { return (bool)GetValue(ShowChartAreaGridLinesProperty); }
            set { SetValue(ShowChartAreaGridLinesProperty, value); }
        }
        public static readonly DependencyProperty ShowChartAreaGridLinesProperty =
            DependencyProperty.Register("ShowChartAreaGridLines", typeof(bool), typeof(HistogramControl), new PropertyMetadata(true));

        public Brush ChartAreaGridLinesColor
        {
            get { return (Brush)GetValue(ChartAreaGridLinesColorProperty); }
            set { SetValue(ChartAreaGridLinesColorProperty, value); }
        }
        public static readonly DependencyProperty ChartAreaGridLinesColorProperty =
            DependencyProperty.Register("ChartAreaGridLinesColor", typeof(Brush), typeof(HistogramControl), new PropertyMetadata(Brushes.Aqua));

        public Brush BarColor
        {
            get { return (Brush)GetValue(BarColorProperty); }
            set { SetValue(BarColorProperty, value); }
        }
        public static readonly DependencyProperty BarColorProperty =
            DependencyProperty.Register("BarColor", typeof(Brush), typeof(HistogramControl), new PropertyMetadata(Brushes.DarkCyan));
        #endregion

        public HistogramControl()
        {
            InitializeComponent();
        }

        private void HistogramControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (HistogramData is not null)
            {
                //Properties that do not change when window is resized
                CalcConstantProperties();
                _isLoaded = true;
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
            //Clear the canvas
            histogramCanvas.Children.Clear();

            //Properties that change when window is resized
            CalcVariableProperties();

            //Add Chart Title
            if (ShowChartTitle)
            {
                AddChartTitle();
            }
            //Add Vertical Axis Title
            if (ShowVerticalAxisTitle)
            {
                AddVerticalAxisTitle();
            }
            //Add Vertical Axis
            if (ShowVerticalAxis)
            {
                AddVerticalAxis();
            }
            //Add Horizontal Axis Title
            if (ShowHorizontalAxisTitle)
            {
                AddHorizontalAxisTitle();
            }
            //Add horizontal grid lines
            if (ShowChartAreaGridLines)
            {
                AddChartAreaGridLines();
            }
            //Add Chart Area Border
            if (ShowChartAreaBorder)
            {
                AddChartAreaBorder();
            }
            //Add Chart Area Bars
            AddChartAreaBars();
            //Add Horizontal Axis
            //Add last as horizontal axis labels
            //May be required in the chart area
            if (ShowHorizontalAxis)
            {
                AddHorizontalAxis();
            }
        }

        private void AddChartAreaBars()
        {
            double leftStart = VerticalAxisTitleWidth + VerticalAxisWidth;
            double bottomStart = HorizontalAxisTitleHeight + HorizontalAxisHeight;

            for (int i = 0; i < Bins.Length; i++)
            {
                Rectangle bar = new Rectangle();
                bar.Width = BarWidth;
                bar.Height = ChartAreaHeight * ((double)Bins[i] / MaxLabelValue);
                bar.Fill = BarColor;

                histogramCanvas.Children.Add(bar);
                Canvas.SetLeft(bar, leftStart + BarWidth * i);
                Canvas.SetBottom(bar, bottomStart);
            }
        }

        private void AddChartAreaGridLines()
        {
            double spacing = ChartAreaHeight / NumberOfVerticalAxisLabels;
            double leftStart = VerticalAxisTitleWidth + VerticalAxisWidth;
            double topStart = ChartTitleHeight + spacing ;


            for (int i = 0; i < NumberOfVerticalAxisLabels - 1; i++)
            {
                Line line = new Line();
                line.X1 = leftStart;
                line.Y1 = topStart + spacing * i;
                line.X2 = ChartAreaWidth + leftStart;
                line.Y2 = line.Y1;
                line.Stroke = ChartAreaGridLinesColor;
                line.StrokeThickness = 0.25;
                
                histogramCanvas.Children.Add(line);
            }
        }

        private void AddHorizontalAxis()
        {
            double leftStart = VerticalAxisTitleWidth + VerticalAxisWidth - HorizontalAxisLabelWidth / 2;
            double topStart = ChartTitleHeight + ChartAreaHeight + MARGIN;
            double spacing = ChartAreaWidth / (HorizontalAxisLabels.Count - 1);

            for (int i = 0; i < HorizontalAxisLabelTextBlocks.Count; i++)
            {
                histogramCanvas.Children.Add(HorizontalAxisLabelTextBlocks[i]);
                Canvas.SetLeft(HorizontalAxisLabelTextBlocks[i], leftStart + spacing * i );
                Canvas.SetTop(HorizontalAxisLabelTextBlocks[i], topStart + HorizontalAxisLabelHeights[i]);
            }
        }

        private void AddHorizontalAxisTitle()
        {
            double leftStart = VerticalAxisTitleWidth + VerticalAxisWidth + (ChartAreaWidth - HorizontalAxisTitleWidth) / 2;
            double TopStart = ChartTitleHeight + ChartAreaHeight + HorizontalAxisHeight + MARGIN;

            histogramCanvas.Children.Add(HorizontalAxisTitleTextBlock);
            Canvas.SetLeft(HorizontalAxisTitleTextBlock, leftStart);
            Canvas.SetTop(HorizontalAxisTitleTextBlock, TopStart);
        }

        private void AddVerticalAxis()
        {
            //determine label spacing
            double labelSpacing = ChartAreaHeight / NumberOfVerticalAxisLabels;

            double topStart = ChartTitleHeight + ChartAreaHeight - VerticalAxisLabelHeight / 2;
            double leftStart = VerticalAxisTitleWidth + VerticalAxisWidth;

            for (int i = 0; i < VerticalAxisLabelTextBlocks.Count; i++)
            {
                histogramCanvas.Children.Add(VerticalAxisLabelTextBlocks[i]);
                Canvas.SetLeft(VerticalAxisLabelTextBlocks[i], leftStart - VerticalAxisLabelWidths[i] - MARGIN);
                Canvas.SetTop(VerticalAxisLabelTextBlocks[i], topStart - (labelSpacing * i) );
            }
        }

        private void AddVerticalAxisTitle()
        {
            double topStart = (ChartTitleHeight + ChartAreaHeight) - (ChartAreaHeight - VerticalAxisTitleHeight) / 2;
            double leftStart =  MARGIN;

            histogramCanvas.Children.Add(VerticalAxisTitleTextBlock);
            Canvas.SetTop(VerticalAxisTitleTextBlock, topStart);
            Canvas.SetLeft(VerticalAxisTitleTextBlock, leftStart);
        }

        private void AddChartTitle()
        {

            double topStart = MARGIN;
            double leftStart = (VerticalAxisTitleWidth + VerticalAxisWidth) + ((ChartAreaWidth - ChartTitleWidth) / 2);

            histogramCanvas.Children.Add(ChartTitleTextBlock);
            Canvas.SetTop(ChartTitleTextBlock, topStart);
            Canvas.SetLeft(ChartTitleTextBlock, leftStart);
        }

        private void AddChartAreaBorder()
        {
            Rectangle chartAreaBorder = new Rectangle();
            chartAreaBorder.Height = ChartAreaHeight;
            chartAreaBorder.Width = ChartAreaWidth;
            chartAreaBorder.Stroke = Brushes.Aqua;
            chartAreaBorder.StrokeThickness = 1;

            histogramCanvas.Children.Add(chartAreaBorder);
            double leftStart = VerticalAxisTitleWidth + VerticalAxisWidth;
            double topStart = ChartTitleHeight;
            Canvas.SetLeft(chartAreaBorder, leftStart);
            Canvas.SetTop(chartAreaBorder, topStart);

        }

        private void CalcVariableProperties()
        {
            //Chart Height
            SetChartHeight();
            //Chart Width
            SetChartWidth();
            //Chart Area Height
            SetChartAreaHeight();
            //Chart Area Width
            SetChartAreaWidth();
            //Bar Width
            SetBarWidth();
        }

        private void SetBarWidth()
        {
            BarWidth = ChartAreaWidth / (Bins.Length);
        }

        private void SetChartAreaWidth()
        {
            ChartAreaWidth = ChartWidth - VerticalAxisWidth - VerticalAxisTitleWidth - MARGIN;
        }

        private void SetChartAreaHeight()
        {
            ChartAreaHeight = ChartHeight - ChartTitleHeight - HorizontalAxisHeight - HorizontalAxisTitleHeight;
        }

        private void SetChartWidth()
        {
            ChartWidth = histogramCanvas.ActualWidth;
        }

        private void SetChartHeight()
        {
            ChartHeight = histogramCanvas.ActualHeight;
        }

        private void CalcConstantProperties()
        {
            //Chart Title
            if (!String.IsNullOrWhiteSpace(HistogramData.ChartTitle) && ShowChartTitle)
            {
                SetChartTitleHeight();
                SetChartTitleWidth();
                SetChartTitleTextBlock();
            }

            //Chart Vertical Axis Title
            if (!String.IsNullOrWhiteSpace(HistogramData.VerticalAxisTitle) && ShowVerticalAxisTitle)
            {
                SetVerticalAxisTitleWidth();
                SetVerticalAxisTitleHeight();
                SetVerticalAxisTitleTextBlock();
            }

            //Chart Horizontal Axis Title
            if (!String.IsNullOrWhiteSpace(HistogramData.HorizontalAxisTitle) && ShowHorizontalAxisTitle)
            {
                SetHorizontalAxisTitleHeight();
                SetHorizontalAxisTitleWidth();
                SetHorizontalAxisTitleTextBlock();
            }

            //Fill the bins
            FillBins();

            //Chart Vertical Axis
            if (ShowVerticalAxis)
            {
                //Generate list of vertical axis labels
                GenerateVerticalAxisLabels();
                GenerateVerticalAxisLabelWidths();
                GenerateVerticalAxisTextBlocks();
                SetVerticalAxisLabelHeight();
                SetVerticalAxisWidth();
            }

            //Chart Horizontal Axis
            if (ShowHorizontalAxis)
            {
                GenerateHorizontalAxisLabels();
                GenerateHorizontalAxisLabelHeights();
                GenerateHorizontalAxisTextBlocks();
                SetHorizontalAxisLabelWidth();
                SetHorizontalAxisHeight();
            }
        }

        private void SetHorizontalAxisLabelWidth()
        {
            string str = HorizontalAxisLabels[0];
            //Will be rotated so height becomes width
            double width = CalcStringHeightAndWidth(str, TitlesFontFamily, HorizontalAxisLabelFontSize, TitlesFontColor).Height;
            HorizontalAxisLabelWidth = width;
        }

        private void GenerateHorizontalAxisTextBlocks()
        {
            HorizontalAxisLabelTextBlocks = new List<TextBlock>();

            RotateTransform rotate = new RotateTransform();
            rotate.Angle = 270;

            foreach (string label in HorizontalAxisLabels)
            {
                TextBlock textBlock = new TextBlock();

                textBlock.Text = label;
                textBlock.FontFamily = new FontFamily(TitlesFontFamily);
                textBlock.FontSize = HorizontalAxisLabelFontSize;
                textBlock.Foreground = TitlesFontColor;

                //rotate the text block
                textBlock.RenderTransform = rotate;

                HorizontalAxisLabelTextBlocks.Add(textBlock);
            }
        }

        private void GenerateHorizontalAxisLabelHeights()
        {
            HorizontalAxisLabelHeights = new List<double>();

            foreach (string label in HorizontalAxisLabels)
            {
                double height = CalcStringHeightAndWidth(label, TitlesFontFamily, HorizontalAxisLabelFontSize, TitlesFontColor).Width;
                HorizontalAxisLabelHeights.Add(height);
            }
        }

        private void SetHorizontalAxisHeight()
        {            
            HorizontalAxisHeight = HorizontalAxisLabelHeights.Max() + MARGIN * 2;
        }

        private void GenerateHorizontalAxisLabels()
        {
            HorizontalAxisLabels = new List<string>();
            for (int i = 0; i <= Bins.Length; i++)
            {
                HorizontalAxisLabels.Add(((LowerBound) + BinWidth * i).ToString("N2"));
            }
        }

        private void SetVerticalAxisWidth()
        {            
            VerticalAxisWidth = VerticalAxisLabelWidths.Max() + MARGIN * 2;
        }
        private void SetVerticalAxisLabelHeight()
        {
            string str = VerticalAxisLabels[0];
            VerticalAxisLabelHeight = CalcStringHeightAndWidth(str, TitlesFontFamily, VerticalAxisLabelFontSize, TitlesFontColor).Height;
        }

        private void GenerateVerticalAxisTextBlocks()
        {
            VerticalAxisLabelTextBlocks = new List<TextBlock>();
            foreach (string label in VerticalAxisLabels)
            {
                TextBlock textBlock = new TextBlock();
                textBlock.Text = label;
                textBlock.FontFamily = new FontFamily(TitlesFontFamily);
                textBlock.FontSize = VerticalAxisLabelFontSize;
                textBlock.Foreground = TitlesFontColor;

                VerticalAxisLabelTextBlocks.Add(textBlock);
            }
        }

        private void GenerateVerticalAxisLabelWidths()
        {
            VerticalAxisLabelWidths = new List<double>();
            foreach (string label in VerticalAxisLabels)
            {
                double width = CalcStringHeightAndWidth(label, TitlesFontFamily, VerticalAxisLabelFontSize, TitlesFontColor).Width;
                VerticalAxisLabelWidths.Add(width);
            }
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

            //set max label value
            MaxLabelValue = (double)maxLabelValue;

            int labelDivision = maxLabelValue / NumberOfVerticalAxisLabels;

            VerticalAxisLabels = new List<string>();
            for (int i = 0; i <= NumberOfVerticalAxisLabels; i++)
            {
                VerticalAxisLabels.Add((labelDivision * i).ToString("N0"));
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

        private void SetHorizontalAxisTitleTextBlock()
        {
            HorizontalAxisTitleTextBlock = new TextBlock();

            HorizontalAxisTitleTextBlock.FontFamily = new FontFamily(TitlesFontFamily);
            HorizontalAxisTitleTextBlock.FontSize = HorizontalAxisTitleFontSize;
            HorizontalAxisTitleTextBlock.Foreground = TitlesFontColor;

            if (!String.IsNullOrWhiteSpace(HorizontalAxisTitleContent))
            {
                HorizontalAxisTitleTextBlock.Text = HorizontalAxisTitleContent;
            }
            else
            {
                HorizontalAxisTitleTextBlock.Text = HistogramData.HorizontalAxisTitle;
            }
        }

        private void SetHorizontalAxisTitleWidth()
        {
            string str;
            if (!String.IsNullOrWhiteSpace(HorizontalAxisTitleContent))
            {
                str = HorizontalAxisTitleContent;
            }
            else
            {
                str = HistogramData.HorizontalAxisTitle;
            }

            HorizontalAxisTitleWidth = CalcStringHeightAndWidth(str, TitlesFontFamily, HorizontalAxisTitleFontSize, TitlesFontColor).Width;
        }

        private void SetHorizontalAxisTitleHeight()
        {
            string str;
            if (!String.IsNullOrWhiteSpace(HorizontalAxisTitleContent))
            {
                str = HorizontalAxisTitleContent;
            }
            else
            {
                str = HistogramData.HorizontalAxisTitle;
            }

            HorizontalAxisTitleHeight = CalcStringHeightAndWidth(str, TitlesFontFamily, HorizontalAxisTitleFontSize, TitlesFontColor).Height + MARGIN * 2;
        }

        private void SetVerticalAxisTitleWidth()
        {
            string str;
            if (!String.IsNullOrWhiteSpace(VerticalAxisTitleContent))
            {
                str = VerticalAxisTitleContent;
            }
            else
            {
                str = HistogramData.VerticalAxisTitle;
            }
            //Text will be vertical so height becomes the width
            VerticalAxisTitleWidth = CalcStringHeightAndWidth(str, TitlesFontFamily, VerticalAxisTitleFontSize, TitlesFontColor).Height + MARGIN * 2;
        }
        private void SetVerticalAxisTitleHeight()
        {
            string str;
            if (!String.IsNullOrWhiteSpace(VerticalAxisTitleContent))
            {
                str = VerticalAxisTitleContent;
            }
            else
            {
                str = HistogramData.VerticalAxisTitle;
            }
            //Text will be vertical so width becomes the height
            VerticalAxisTitleHeight = CalcStringHeightAndWidth(str, TitlesFontFamily, VerticalAxisTitleFontSize, TitlesFontColor).Width;
        }
        private void SetVerticalAxisTitleTextBlock()
        {
            VerticalAxisTitleTextBlock = new TextBlock();

            VerticalAxisTitleTextBlock.FontFamily = new FontFamily(TitlesFontFamily);
            VerticalAxisTitleTextBlock.FontSize = VerticalAxisTitleFontSize;
            VerticalAxisTitleTextBlock.Foreground = TitlesFontColor;

            if (String.IsNullOrWhiteSpace(VerticalAxisTitleContent))
            {
                VerticalAxisTitleTextBlock.Text = HistogramData.VerticalAxisTitle;
            }
            else
            {
                VerticalAxisTitleTextBlock.Text = VerticalAxisTitleContent;
            }
            //rotated label
            RotateTransform rotate = new RotateTransform();
            rotate.Angle = 270;
            VerticalAxisTitleTextBlock.RenderTransform = rotate;
        }

        private void SetChartTitleHeight()
        {
            string str;
            if (!String.IsNullOrWhiteSpace(ChartTitleContent))
            {
                str = ChartTitleContent;
            }
            else
            {
                str = HistogramData.ChartTitle;
            }

            ChartTitleHeight = CalcStringHeightAndWidth(str, TitlesFontFamily, ChartTitleFontSize, TitlesFontColor).Height + MARGIN * 2;
        }
        private void SetChartTitleWidth()
        {
            string str;
            if (!String.IsNullOrWhiteSpace(ChartTitleContent))
            {
                str = ChartTitleContent;
            }
            else
            {
                str = HistogramData.ChartTitle;
            }

            ChartTitleWidth = CalcStringHeightAndWidth(str, TitlesFontFamily, ChartTitleFontSize, TitlesFontColor).Width;
        }
        private void SetChartTitleTextBlock()
        {
            ChartTitleTextBlock = new TextBlock();

            ChartTitleTextBlock.FontFamily = new FontFamily(TitlesFontFamily);
            ChartTitleTextBlock.FontSize = ChartTitleFontSize;
            ChartTitleTextBlock.Foreground = TitlesFontColor;

            if (String.IsNullOrWhiteSpace(ChartTitleContent))
            {
                ChartTitleTextBlock.Text = HistogramData.ChartTitle;
            }
            else
            {
                ChartTitleTextBlock.Text = ChartTitleContent;
            }
        }

        private (double Height, double Width) CalcStringHeightAndWidth(string str, string fontFamily, int fontSize, Brush fontColor, double pixelsPerDip = 1)
        {
            FormattedText s = new FormattedText(str, HistogramCulture, FlowDirection.LeftToRight, new Typeface(fontFamily), fontSize, fontColor, pixelsPerDip);

            return (s.Height, s.Width);
        }
    }
}

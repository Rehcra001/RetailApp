using ModelsLibrary.ChartModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

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
        private decimal MaxValue { get; set; }
        private string MaxValueDescriptionWidth { get; set; }
        private List<string> VerticalAxisLabels { get; set; }
        private decimal VerticalAxisDivisionValue { get; set; }
        private double MaxBarWidth { get; set; }

        private bool isLoaded = false;

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


        public int HorizontalAxisTitleFontSize
        {
            get { return (int)GetValue(HorizontalAxisFontSizeProperty); }
            set { SetValue(HorizontalAxisFontSizeProperty, value); }
        }
        public static readonly DependencyProperty HorizontalAxisFontSizeProperty =
            DependencyProperty.Register("HorizontalAxisFontSize", typeof(int), typeof(BarChart), new PropertyMetadata(16));


        #endregion

        #region Vertical Axis DP
        public bool ShowVerticalAxis
        {
            get { return (bool)GetValue(ShowVerticalAxisProperty); }
            set { SetValue(ShowVerticalAxisProperty, value); }
        }
        public static readonly DependencyProperty ShowVerticalAxisProperty =
            DependencyProperty.Register("ShowVerticalAxis", typeof(bool), typeof(BarChart), new PropertyMetadata(true));


        public int NumberOfVerticalGridLines
        {
            get { return (int)GetValue(NumberOfVerticalGridLinesProperty); }
            set { SetValue(NumberOfVerticalGridLinesProperty, value); }
        }
        public static readonly DependencyProperty NumberOfVerticalGridLinesProperty =
            DependencyProperty.Register("NumberOfVerticalGridLines", typeof(int), typeof(BarChart), new PropertyMetadata(5));


        public int VericalAxisLabelFontSize
        {
            get { return (int)GetValue(VericalAxisLabelFontSizeProperty); }
            set { SetValue(VericalAxisLabelFontSizeProperty, value); }
        }
        public static readonly DependencyProperty VericalAxisLabelFontSizeProperty =
            DependencyProperty.Register("VericalAxisLabelFontSize", typeof(int), typeof(BarChart), new PropertyMetadata(12));



        public int VerticalAxisValueScale
        {
            get { return (int)GetValue(VerticalAxisValueScaleProperty); }
            set { SetValue(VerticalAxisValueScaleProperty, value); }
        }
        public static readonly DependencyProperty VerticalAxisValueScaleProperty =
            DependencyProperty.Register("VerticalAxisValueScale", typeof(int), typeof(BarChart), new PropertyMetadata(1));
        #endregion

        #region Horizontal Axis DP
        public bool ShowHorizontalAxis
        {
            get { return (bool)GetValue(ShowHorizontalAxisProperty); }
            set { SetValue(ShowHorizontalAxisProperty, value); }
        }
        public static readonly DependencyProperty ShowHorizontalAxisProperty =
            DependencyProperty.Register("ShowHorizontalAxis", typeof(bool), typeof(BarChart), new PropertyMetadata(true));


        public int HorizontalAxisLabelFontSize
        {
            get { return (int)GetValue(HorizontalAxisLabelFontSizeProperty); }
            set { SetValue(HorizontalAxisLabelFontSizeProperty, value); }
        }
        public static readonly DependencyProperty HorizontalAxisLabelFontSizeProperty =
            DependencyProperty.Register("HorizontalAxisLabelFontSize", typeof(int), typeof(BarChart), new PropertyMetadata(12));
        #endregion

        public BarChart()
        {
            InitializeComponent();
        }

        private void BarChart_Loaded(object sender, RoutedEventArgs e)
        {
            CalcConstantDimensions();
            CalcVariableDimensions();
            isLoaded = true;
            DrawBarChart();
        }

        private void BarChart_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            DrawBarChart();
        }

        private void DrawBarChart()
        {
            if (isLoaded && BarChartData is not null)
            {
                CalcVariableDimensions();

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
                AddVerticalAxis();
                AddHorizontalAxis();
                AddChartArea();
            }
        }

        private void CalcConstantDimensions()
        {
            //These properties will remain as calculated 
            //Through out the life of the bar chart

            CalcChartTitleHeight();

            CalcVerticalAxisTitleWidth();

            CalcHorizontalTitleAxisHeight();

            GenerateVerticalAxisLabels();
            CalcVerticalAxisWidth();

            //Labels will be vertical
            CalcHorizontalAxisHeight();

        }

        private void CalcVariableDimensions()
        {
            //These properties will change as 
            //the view size is altered
            CalcChartHeight();
            CalcChartWidth();

            CalcChartAreaWidth();
            CalcChartAreaHeight();
            //CalcMaxBarWidth();
        }

        private (double Height, double Width) CalcStringHeightAndWidth(string str, string fontFamily, int fontSize, Brush color)
        {
            FormattedText s = new FormattedText(str,
                                                CultureInfo.GetCultureInfo("en-za"),
                                                FlowDirection.LeftToRight,
                                                new Typeface(fontFamily),
                                                fontSize,
                                                color,
                                                1);
            return (s.Height, s.Width);
        }

        private void CalcChartTitleHeight()
        {
            if (ShowChartTitle)
            {
                double height = CalcStringHeightAndWidth(BarChartData.ChartTitle,
                                                     TitlesFontFamily,
                                                     ChartTitleFontSize,
                                                     TitlesFontColor).Height;
                ChartTitleHeight = height + MARGIN * 2;
            }
            else
            {
                ChartTitleHeight = 0;
            }

        }

        private void CalcVerticalAxisTitleWidth()
        {
            if (ShowVerticalAxisTitle)
            {
                double height = CalcStringHeightAndWidth(BarChartData.VerticalAxisTitle,
                                                                             TitlesFontFamily,
                                                                             VerticalAxisTitleFontSize,
                                                                             TitlesFontColor).Height;
                //height as the text is rotated 90°
                VerticalAxisTitleWidth = height + MARGIN * 2;
            }
            else
            {
                VerticalAxisTitleWidth = 0;
            }
        }

        private void CalcHorizontalTitleAxisHeight()
        {
            if (ShowHorizontalAxisTitle)
            {
                double height = CalcStringHeightAndWidth(BarChartData.ChartTitle,
                                                                     TitlesFontFamily,
                                                                     ChartTitleFontSize,
                                                                     TitlesFontColor).Height;
                HorizontalAxisTitleHeight = height + MARGIN * 2;
            }
            else
            {
                HorizontalAxisTitleHeight = 0;
            }

        }

        private void GenerateVerticalAxisLabels()
        {
            if (ShowVerticalAxis)
            {
                //Get the max value from Values
                SetMaxValue();
                //Determine the increment value of each horizontal grid line
                //given the number of grid lines required
                CalcVerticalGridLineIncrementValue();

                //generate labels
                VerticalAxisLabels = new List<string>();

                for (int i = 0; i <= NumberOfVerticalGridLines; i++)
                {
                    VerticalAxisLabels.Add((VerticalAxisDivisionValue * i).ToString());
                }
            }
        }

        private void SetMaxValue()
        {
            if (BarChartData.Values is not null)
            {
                MaxValue = BarChartData.Values.Max();
            }
        }

        private void CalcVerticalGridLineIncrementValue()
        {
            VerticalAxisDivisionValue = CalcMaxVerticalAxisLabelValue() / NumberOfVerticalGridLines;
        }

        private decimal CalcMaxVerticalAxisLabelValue()
        {
            decimal val = MaxValue; // divide by number of grid lines
            StringBuilder sb = new StringBuilder(val.ToString());

            //Deal with values greater than or equal to 10 first
            if (val >= 10)
            {
                //add one to the second digit
                if (sb[1] < '9')
                {
                    sb[1] = (char)((int)sb[1] + 1);
                }
                else
                {
                    //sb[1] is 9 check first digit
                    //if less than 9 add 1 to first digit 
                    //else make sb[0] and sb[1] zero
                    //insert 1 to the front of sb
                    if (sb[0] < '9')
                    {
                        sb[0] = (char)((int)sb[0] + 1);
                        sb[1] = '0';
                    }
                    else
                    {
                        sb[0] = '0';
                        sb[1] = '0';
                        sb.Insert(0, '1');
                    }
                }
                //set any trailing digits to zero
                for (int i = 2; i < sb.Length; i++)
                {
                    if (sb[i] != '.')
                    {
                        sb[i] = '0';
                    }
                }
            }
            else
            {
                //less than 9 just add 1 to first digit
                if (val < 9)
                {
                    sb[0] = (char)((int)sb[0] + 1);
                }
                else
                {
                    sb[0] = '0';
                    sb.Insert(0, '1');
                }

                //set any trailing digits to zero
                for (int i = 1; i < sb.Length; i++)
                {
                    if (sb[i] != '.')
                    {
                        sb[i] = '0';
                    }
                }
            }

            return decimal.Parse(sb.ToString());
        }

        private void CalcVerticalAxisWidth()
        {
            if (ShowVerticalAxis)
            {
                double maxWidth = 0;
                foreach (string str in VerticalAxisLabels)
                {
                    double width = CalcStringHeightAndWidth(str, TitlesFontFamily, VericalAxisLabelFontSize, TitlesFontColor).Width;
                    if (maxWidth < width)
                    {
                        maxWidth = width;
                    }
                }

                VerticalAxisWidth = maxWidth + MARGIN * 2;
            }
            else
            {
                VerticalAxisWidth = 0;
            }

        }

        private void CalcHorizontalAxisHeight()
        {
            //Labels will be vertically placed
            if (ShowHorizontalAxis)
            {
                double maxWidth = 0;
                if (BarChartData.ValuesDescription is not null)
                {
                    foreach (string str in BarChartData.ValuesDescription)
                    {
                        double width = CalcStringHeightAndWidth(str, TitlesFontFamily, HorizontalAxisLabelFontSize, TitlesFontColor).Width;
                        if (maxWidth < width)
                        {
                            maxWidth = width;
                        }
                    }
                }

                HorizontalAxisHeight = maxWidth + MARGIN * 2;
            }
            else
            {
                HorizontalAxisHeight = 0;
            }

        }

        private void CalcChartHeight()
        {
            ChartHeight = BarChartCanvas.ActualHeight;
        }

        private void CalcChartWidth()
        {
            ChartWidth = BarChartCanvas.ActualWidth;
        }

        private void CalcChartAreaWidth()
        {
            ChartAreaWidth = ChartWidth - VerticalAxisTitleWidth - VerticalAxisWidth - (double)MARGIN * 2;
        }

        private void CalcChartAreaHeight()
        {
            ChartAreaHeight = ChartHeight - ChartTitleHeight - HorizontalAxisTitleHeight - HorizontalAxisHeight;
        }

        private void CalcMaxBarWidth()
        {
            throw new NotImplementedException();
        }


        private void AddChartTitle()
        {
            TextBlock chartTitle = new TextBlock();
            chartTitle.FontFamily = new FontFamily(TitlesFontFamily);
            chartTitle.FontSize = ChartTitleFontSize;
            chartTitle.Foreground = TitlesFontColor;
            chartTitle.Text = BarChartData.ChartTitle;

            BarChartCanvas.Children.Add(chartTitle);

            //Position Chart Title
            (double height, double width) = CalcStringHeightAndWidth(BarChartData.ChartTitle,
                                                                     TitlesFontFamily,
                                                                     ChartTitleFontSize,
                                                                     TitlesFontColor);
            double left = (ChartWidth * 0.5) - width * 0.5;
            Canvas.SetTop(chartTitle, MARGIN);
            Canvas.SetLeft(chartTitle, left);
        }

        private void AddVerticalAxisTitle()
        {
            TextBlock verticalAxisTitle = new TextBlock();
            verticalAxisTitle.FontFamily = new FontFamily(TitlesFontFamily);
            verticalAxisTitle.FontSize = VerticalAxisTitleFontSize;
            verticalAxisTitle.Foreground = TitlesFontColor;
            verticalAxisTitle.Text = BarChartData.ChartTitle;
            RotateTransform rotate = new RotateTransform();
            rotate.Angle = 270;
            verticalAxisTitle.RenderTransform = rotate;

            BarChartCanvas.Children.Add(verticalAxisTitle);

            //Position Vertical Axis Title
            (double height, double width) = CalcStringHeightAndWidth(BarChartData.VerticalAxisTitle,
                                                                     TitlesFontFamily,
                                                                     VerticalAxisTitleFontSize,
                                                                     TitlesFontColor);
            double top = (ChartHeight * 0.5) + width * 0.5;
            Canvas.SetTop(verticalAxisTitle, top);
            Canvas.SetLeft(verticalAxisTitle, MARGIN);


        }

        private void AddHorizontalAxisTitle()
        {
            TextBlock horizontalAxisTitle = new TextBlock();
            horizontalAxisTitle.FontFamily = new FontFamily(TitlesFontFamily);
            horizontalAxisTitle.FontSize = HorizontalAxisTitleFontSize;
            horizontalAxisTitle.Foreground = TitlesFontColor;
            horizontalAxisTitle.Text = BarChartData.HorizontalAxisTitle;

            BarChartCanvas.Children.Add(horizontalAxisTitle);

            //Position Horizontal Axis Title
            (double height, double width) = CalcStringHeightAndWidth(BarChartData.HorizontalAxisTitle,
                                                                     TitlesFontFamily,
                                                                     HorizontalAxisTitleFontSize,
                                                                     TitlesFontColor);
            double left = (ChartWidth * 0.5) - width * 0.5;
            double bottom = (ChartHeight - MARGIN) - height;
            Canvas.SetTop(horizontalAxisTitle, bottom);
            Canvas.SetLeft(horizontalAxisTitle, left);
        }
        private void AddVerticalAxis()
        {
            // TODO - Delete testing rectangle
            // For testing draw rectangle in this area
            Rectangle rectangle = new Rectangle();
            rectangle.Height = ChartAreaHeight;
            rectangle.Width = VerticalAxisWidth;
            rectangle.Stroke = Brushes.Aqua;
            rectangle.StrokeThickness = 1;
            BarChartCanvas.Children.Add(rectangle);
            Canvas.SetTop(rectangle, ChartTitleHeight);
            Canvas.SetLeft(rectangle, VerticalAxisTitleWidth);

            //throw new NotImplementedException();
        }

        private void AddHorizontalAxis()
        {
            // TODO - Delete testing rectangle
            // For testing draw rectangle in this area
            Rectangle rectangle = new Rectangle();
            rectangle.Height = HorizontalAxisHeight;
            rectangle.Width = ChartAreaWidth;
            rectangle.Stroke = Brushes.Aqua;
            rectangle.StrokeThickness = 1;
            BarChartCanvas.Children.Add(rectangle);
            Canvas.SetTop(rectangle, ChartTitleHeight + ChartAreaHeight);
            Canvas.SetLeft(rectangle, VerticalAxisTitleWidth + VerticalAxisWidth);

            //throw new NotImplementedException();
        }

        private void AddChartArea()
        {
            // TODO - Create separate method for the chart are border with option to show or not show
            // For testing draw rectangle in this area
            Rectangle rectangle = new Rectangle();
            rectangle.Height = ChartAreaHeight;
            rectangle.Width = ChartAreaWidth;
            rectangle.Stroke = Brushes.Aqua;
            rectangle.StrokeThickness = 1;
            rectangle.RadiusX = 10;
            rectangle.RadiusY = 10;
            BarChartCanvas.Children.Add(rectangle);
            Canvas.SetTop(rectangle, ChartTitleHeight);
            Canvas.SetLeft(rectangle, VerticalAxisTitleWidth + VerticalAxisWidth);
        }
    }
}

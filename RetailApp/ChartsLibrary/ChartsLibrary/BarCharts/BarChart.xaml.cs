﻿using ModelsLibrary.ChartModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Media3D;

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
            DependencyProperty.Register("NumberOfVerticalGridLines", typeof(int), typeof(BarChart), new PropertyMetadata(10));


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


        public BarChart()
        {
            InitializeComponent();
        }

        private void BarChart_Loaded(object sender, RoutedEventArgs e)
        {
            AddAxes();
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
            }
        }


        private void AddAxes()
        {

            CalcChartTitleHeight();
            CalcVerticalAxisTitleWidth();
            CalcHorizontalAxisTitleHeight();

            GenerateVerticalAxisLabels();

            //CalcVerticalAxisWidth();
            //CalcHorizontalAxisHeight();
            //CalcChartAreaWidth();
            //CalcChartAreaHeight();


            //AddVerticalAxis();
            //AddHorizontalAxis();
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
            double height = CalcStringHeightAndWidth(BarChartData.ChartTitle,
                                                     TitlesFontFamily,
                                                     ChartTitleFontSize,
                                                     TitlesFontColor).Height;
            ChartTitleHeight = height + MARGIN * 2;
        }

        private void CalcVerticalAxisTitleWidth()
        {
            double height = CalcStringHeightAndWidth(BarChartData.VerticalAxisTitle,
                                                                     TitlesFontFamily,
                                                                     VerticalAxisTitleFontSize,
                                                                     TitlesFontColor).Height;
            //height as the text is rotated 90°
            VerticalAxisTitleWidth = height + MARGIN * 2;
        }

        private void CalcHorizontalAxisTitleHeight()
        {
            double height = CalcStringHeightAndWidth(BarChartData.ChartTitle,
                                                                     TitlesFontFamily,
                                                                     ChartTitleFontSize,
                                                                     TitlesFontColor).Height;
            HorizontalAxisTitleHeight = height + MARGIN * 2;
        }

        private void GenerateVerticalAxisLabels()
        {
            //I need to know the max value in Values
            //using the max value divide up the vertical axis
            //by NumberOfVerticalGridLines 21360 / 10
            //round down to lowest eg. 2136 => 2000
            //every grid line will then increment by 2000 for this example
            //Calculate the max verical axis label value
            //2000 * 10 + 2000
            //lowest grid line will always be zero

            SetMaxValue();
            CalcVerticalGridLineIncrementValue();
            //generate labels
            VerticalAxisLabels = new List<string>();

            for (int i = 0; i <= NumberOfVerticalGridLines; i++)
            {
                VerticalAxisLabels.Add((VerticalAxisDivisionValue * i).ToString());
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
            VerticalAxisDivisionValue = CalcMaxVerticalAxisValue() / NumberOfVerticalGridLines;
        }

        private decimal CalcMaxVerticalAxisValue()
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

        

        private void CalcVerticalAxisWidth()
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

        
    }
}

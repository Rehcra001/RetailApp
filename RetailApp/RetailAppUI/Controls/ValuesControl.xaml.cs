using ModelsLibrary;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace RetailAppUI.Controls
{
    /// <summary>
    /// Interaction logic for ValuesControl.xaml
    /// </summary>
    public partial class ValuesControl : UserControl
    {
        #region ValueData Model DP
        public ValueModel ValueData
        {
            get { return (ValueModel)GetValue(ValueDataProperty); }
            set { SetValue(ValueDataProperty, value); }
        }
        public static readonly DependencyProperty ValueDataProperty =
            DependencyProperty.Register("ValueData", typeof(ValueModel), typeof(ValuesControl), new PropertyMetadata(null));

        #endregion

        #region Border DP
        //Border color
        public Brush BorderColor
        {
            get { return (Brush)GetValue(BorderColorProperty); }
            set { SetValue(BorderColorProperty, value); }
        }
        public static readonly DependencyProperty BorderColorProperty =
            DependencyProperty.Register("BorderColor", typeof(Brush), typeof(ValuesControl), new PropertyMetadata(Brushes.Aqua));

        //Border thickness
        public Thickness BorderLineWidth
        {
            get { return (Thickness)GetValue(BorderLineWidthProperty); }
            set { SetValue(BorderLineWidthProperty, value); }
        }
        public static readonly DependencyProperty BorderLineWidthProperty =
            DependencyProperty.Register("BorderLineWidth", typeof(Thickness), typeof(ValuesControl), new PropertyMetadata(new Thickness(1)));

        //Border corner radius
        public CornerRadius BorderCornerRadius
        {
            get { return (CornerRadius)GetValue(BorderCornerRadiusProperty); }
            set { SetValue(BorderCornerRadiusProperty, value); }
        }
        public static readonly DependencyProperty BorderCornerRadiusProperty =
            DependencyProperty.Register("BorderCornerRadius", typeof(CornerRadius), typeof(ValuesControl), new PropertyMetadata(new CornerRadius(1)));

        //Border BackGround color
        public Brush BorderBackGroundColor
        {
            get { return (Brush)GetValue(BorderBackGroundColorProperty); }
            set { SetValue(BorderBackGroundColorProperty, value); }
        }
        public static readonly DependencyProperty BorderBackGroundColorProperty =
            DependencyProperty.Register("BorderBackGroundColor", typeof(Brush), typeof(ValuesControl), new PropertyMetadata(Brushes.Transparent));


        public bool ShowBorder
        {
            get { return (bool)GetValue(ShowBorderProperty); }
            set { SetValue(ShowBorderProperty, value); }
        }
        public static readonly DependencyProperty ShowBorderProperty =
            DependencyProperty.Register("ShowBorder", typeof(bool), typeof(ValuesControl), new PropertyMetadata(true));

        #endregion

        #region Title DP

        //Title Font-Family
        public FontFamily TitleFontFamily
        {
            get { return (FontFamily)GetValue(TitleFontFamilyProperty); }
            set { SetValue(TitleFontFamilyProperty, value); }
        }
        public static readonly DependencyProperty TitleFontFamilyProperty =
            DependencyProperty.Register("TitleFontFamily", typeof(FontFamily), typeof(ValuesControl), new PropertyMetadata(new FontFamily("New Times Roman")));

        //Title Font-Size
        public int TitleFontSize
        {
            get { return (int)GetValue(TitleFontSizeProperty); }
            set { SetValue(TitleFontSizeProperty, value); }
        }
        public static readonly DependencyProperty TitleFontSizeProperty =
            DependencyProperty.Register("TitleFontSize", typeof(int), typeof(ValuesControl), new PropertyMetadata(16));

        //Title Font-Weight
        public FontWeight TitleFontWeight
        {
            get { return (FontWeight)GetValue(TitleFontWeightProperty); }
            set { SetValue(TitleFontWeightProperty, value); }
        }
        public static readonly DependencyProperty TitleFontWeightProperty =
            DependencyProperty.Register("TitleFontWeight", typeof(FontWeight), typeof(ValuesControl), new PropertyMetadata(FontWeights.Normal));

        //Title Foreground Color
        public Brush TitleColor
        {
            get { return (Brush)GetValue(TitleColorProperty); }
            set { SetValue(TitleColorProperty, value); }
        }
        public static readonly DependencyProperty TitleColorProperty =
            DependencyProperty.Register("TitleColor", typeof(Brush), typeof(ValuesControl), new PropertyMetadata(Brushes.White));

        #endregion

        #region Value DP

        //Value Font-Family
        public FontFamily ValueFontFamily
        {
            get { return (FontFamily)GetValue(ValueFontFamilyProperty); }
            set { SetValue(ValueFontFamilyProperty, value); }
        }
        public static readonly DependencyProperty ValueFontFamilyProperty =
            DependencyProperty.Register("ValueFontFamily", typeof(FontFamily), typeof(ValuesControl), new PropertyMetadata(new FontFamily("New Times Roman")));

        //Value Font-Size
        public int ValueFontSize
        {
            get { return (int)GetValue(ValueFontSizeProperty); }
            set { SetValue(ValueFontSizeProperty, value); }
        }
        public static readonly DependencyProperty ValueFontSizeProperty =
            DependencyProperty.Register("ValueFontSize", typeof(int), typeof(ValuesControl), new PropertyMetadata(20));

        //Value Font-Weight
        public FontWeight ValueFontWeight
        {
            get { return (FontWeight)GetValue(ValueFontWeightProperty); }
            set { SetValue(ValueFontWeightProperty, value); }
        }
        public static readonly DependencyProperty ValueFontWeightProperty =
            DependencyProperty.Register("ValueFontWeight", typeof(FontWeight), typeof(ValuesControl), new PropertyMetadata(FontWeights.Normal));

        //Value Foreground Color
        public Brush ValueFontColor
        {
            get { return (Brush)GetValue(ValueFontColorProperty); }
            set { SetValue(ValueFontColorProperty, value); }
        }
        public static readonly DependencyProperty ValueFontColorProperty =
            DependencyProperty.Register("ValueFontColor", typeof(Brush), typeof(ValuesControl), new PropertyMetadata(Brushes.White));


        //Value number of decimals
        public int ValueNumberOfDecimals
        {
            get { return (int)GetValue(ValueNumberOfDecimalsProperty); }
            set { SetValue(ValueNumberOfDecimalsProperty, value); }
        }
        public static readonly DependencyProperty ValueNumberOfDecimalsProperty =
            DependencyProperty.Register("ValueNumberOfDecimals", typeof(int), typeof(ValuesControl), new PropertyMetadata(2));


        //Value Currency symbol/culture info
        public string ValueCultureInfo
        {
            get { return (string)GetValue(ValueCultureInfoProperty); }
            set { SetValue(ValueCultureInfoProperty, value); }
        }
        public static readonly DependencyProperty ValueCultureInfoProperty =
            DependencyProperty.Register("ValueCultureInfo", typeof(string), typeof(ValuesControl), new PropertyMetadata("en-za"));

        //Display a prefix - default is false
        public bool ShowValuePrefix
        {
            get { return (bool)GetValue(ShowValuePrefixProperty); }
            set { SetValue(ShowValuePrefixProperty, value); }
        }
        public static readonly DependencyProperty ShowValuePrefixProperty =
            DependencyProperty.Register("ShowValuePrefix", typeof(bool), typeof(ValuesControl), new PropertyMetadata(false));

        //Display a suffix
        public bool ShowValueSuffix
        {
            get { return (bool)GetValue(ShowValueSuffixProperty); }
            set { SetValue(ShowValueSuffixProperty, value); }
        }
        public static readonly DependencyProperty ShowValueSuffixProperty =
            DependencyProperty.Register("ShowValueSuffix", typeof(bool), typeof(ValuesControl), new PropertyMetadata(false));

        //Value prefix symbol eg 'R' for Rand


        public string ValuePrefix
        {
            get { return (string)GetValue(ValuePrefixProperty); }
            set { SetValue(ValuePrefixProperty, value); }
        }
        public static readonly DependencyProperty ValuePrefixProperty =
            DependencyProperty.Register("ValuePrefix", typeof(string), typeof(ValuesControl), new PropertyMetadata(""));

        //Value suffix symbol eg '%' if value is a percentage
        public string ValueSuffix
        {
            get { return (string)GetValue(ValueSuffixProperty); }
            set { SetValue(ValueSuffixProperty, value); }
        }
        public static readonly DependencyProperty ValueSuffixProperty =
            DependencyProperty.Register("ValueSuffix", typeof(string), typeof(ValuesControl), new PropertyMetadata(""));

        #endregion

        public ValuesControl()
        {
            InitializeComponent();
        }

        private void Values_Loaded(object sender, RoutedEventArgs e)
        {
            if (ValueData is not null)
            {
                if (ShowBorder)
                {
                    //Border properties
                    border.BorderBrush = BorderColor;
                    border.BorderThickness = BorderLineWidth;
                    border.CornerRadius = BorderCornerRadius;
                    border.Background = BorderBackGroundColor;
                }
                

                //Title properties
                title.Text = ValueData.Title;
                title.HorizontalAlignment = HorizontalAlignment.Center;
                title.VerticalAlignment = VerticalAlignment.Center;
                title.FontFamily = TitleFontFamily;
                title.FontSize = TitleFontSize;
                title.FontWeight = TitleFontWeight;
                title.Foreground = TitleColor;


                //Value properties
                string numDecimals = "N" + ValueNumberOfDecimals.ToString();
                string str = ValueData.Value.ToString(numDecimals, CultureInfo.CreateSpecificCulture(ValueCultureInfo));
                if (ShowValuePrefix)
                {
                    str = ValuePrefix + str;
                }
                if (ShowValueSuffix)
                {
                    str += ValueSuffix;
                }
                value.Text = str;
                value.HorizontalAlignment = HorizontalAlignment.Center;
                value.VerticalAlignment = VerticalAlignment.Center;
                value.FontFamily = ValueFontFamily;
                value.FontSize = ValueFontSize;
                value.FontWeight = ValueFontWeight;
                value.Foreground = ValueFontColor;
            }
            else
            {
                if (ShowBorder)
                {
                    //Border properties
                    border.BorderBrush = BorderColor;
                    border.BorderThickness = BorderLineWidth;
                    border.CornerRadius = BorderCornerRadius;
                    border.Background = BorderBackGroundColor;
                }

                title.Text = "No Title Available";
                title.Foreground = Brushes.Red;
                title.HorizontalAlignment = HorizontalAlignment.Center;
                title.VerticalAlignment = VerticalAlignment.Center;
                title.FontFamily = TitleFontFamily;
                title.FontSize = TitleFontSize;
                title.FontWeight = TitleFontWeight;


                //value.Text = "No Value Available";
                decimal val = 0M;
                string numDecimals = "N" + ValueNumberOfDecimals.ToString();
                string str = val.ToString(numDecimals, CultureInfo.CreateSpecificCulture(ValueCultureInfo));
                if (ShowValuePrefix)
                {
                    str = ValuePrefix + str;
                }
                if (ShowValueSuffix)
                {
                    str += ValueSuffix;
                }
                value.Text = str;
                //value.Text = "No Value Available";
                value.Foreground = Brushes.Red;
                value.HorizontalAlignment = HorizontalAlignment.Center;
                value.VerticalAlignment = VerticalAlignment.Center;
                value.FontFamily = ValueFontFamily;
                value.FontSize = ValueFontSize;
            }
        }
    }
}

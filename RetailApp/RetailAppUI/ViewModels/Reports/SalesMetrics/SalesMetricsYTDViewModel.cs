using BussinessLogicLibrary.SalesMetrics;
using ChartModelsLibrary.ChartModels;
using ModelsLibrary;
using RetailAppUI.Commands;
using RetailAppUI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Windows;

namespace RetailAppUI.ViewModels.Reports.SalesMetrics
{
    public class SalesMetricsYTDViewModel : BaseViewModel
    {
        private ISalesMetricsManager _salesMetricsManager;

        private INavigationService _navigation;
        public INavigationService Navigation
        {
            get { return _navigation; }
            set { _navigation = value; OnPropertyChanged(); }
        }

        private BarChartModel _salesRevenueByMonth;
        public BarChartModel SalesRevenueByMonth
        {
            get { return _salesRevenueByMonth; }
            set { _salesRevenueByMonth = value; OnPropertyChanged(); }
        }

        private BarChartModel _top10ProductsByRevenue;
        public BarChartModel Top10ProductsByRevenue
        {
            get { return _top10ProductsByRevenue; }
            set { _top10ProductsByRevenue = value; OnPropertyChanged(); }
        }

        private ValueModel _revenueYTD;
        public ValueModel RevenueYTD
        {
            get { return _revenueYTD; }
            set { _revenueYTD = value; OnPropertyChanged(); }
        }

        private ValueModel _top10RevenueYTD;
        public ValueModel Top10RevenueYTD
        {
            get { return _top10RevenueYTD; }
            set { _top10RevenueYTD = value; OnPropertyChanged(); }
        }

        private HistogramModel _test;

        public HistogramModel Test
        {
            get { return _test; }
            set { _test = value; OnPropertyChanged(); }
        }



        //Commands
        public RelayCommand CloseViewCommand { get; set; }

        public SalesMetricsYTDViewModel(INavigationService navigation, ISalesMetricsManager salesMetricsManager)
        {
            Navigation = navigation;
            _salesMetricsManager = salesMetricsManager;

            //Add Bar Chart for YDT monthly revenue
            LoadMonthlyRevenueYTD();
            //Add Bar Chart for YDT Top 10 Products by revenue
            LoadTop10ProductsByRevenue();
            //Load ValueModel for revenue YTD
            LoadRevenueYTD();
            //Load ValueModel for Top 10 product revenue
            LoadTop10ProductsRevenue();

            //*******************************
            //For Testing to be deleted
            LoadTest();
            // TODO - Delete test functions
            //*******************************

            //Instantiate commands
            CloseViewCommand = new RelayCommand(CloseView, CanCloseView);
        }

        private void LoadTest()
        {
            Test = new HistogramModel();
            Test.Observations = LoadObservations();
        }

        private IEnumerable<decimal> LoadObservations()
        {
            List<double> obs = new List<double>
            {
                0.595782816,0.695390165,0.379609971,-0.661299639,-1.33241481,1.28276084,-0.671623782,-1.894314112,-1.244328262,-0.017189845,-1.512825038,0.403447937,0.092300715,-2.055760879,-0.158595083,1.141910003,-0.614017329,-0.999446666,-1.64134545,-0.112569302,-1.837241675,-0.98607672,-1.539998369,-1.656100167,0.044500165,0.986544681,0.26030039,0.644126655,1.259143184,0.246600068,0.016837967,-0.465570482,1.415710989,0.930747361,-0.836827232,0.661819745,-0.778671494,0.792724773,1.115212363,-2.202736723,0.53519849,1.222838293,-1.02438793,-0.6620457,0.842739448,0.286283344,1.415572639,1.612865364,-0.238948765,0.382218077,1.298685388,0.24941132,0.438277674,-0.860481035,-1.678749389,-0.793649077,1.823212386,-1.36931993,-0.906007881,0.441412043,0.834382706,-2.202801432,-0.546022958,0.542739006,-1.050765295,0.549834605,0.477147857,-1.36080886,-0.414994179,2.322800799,-1.01337982,-0.607424919,-0.233565714,-0.104453534,1.622632172,-1.076191811,0.267937004,0.382009043,-0.112652958,-0.644947968,2.469867762,-1.626188112,-1.209413665,-0.044534277,1.494628181,0.780778964,-0.367290646,-1.08756752,-0.263467654,1.75688363,1.518845949,-0.410090896,-0.207090435,-0.618194593,-0.121977211,-0.005424459,1.853547499,-1.082840532,2.136283331,0.118505543,0.59118692,0.022099428,1.157655082,2.938308034,0.275144516,-1.709372018,0.166625108,0.255299506,1.389825261,-1.382367792,-0.211088266,1.166253821,-1.665565042,-0.210780649,-0.281220846,0.053967179,-0.816725419,-1.15989063,-0.686655603,0.979027437,0.110481235,-2.346759768,-0.205016931,-0.479029206,0.571278571,-0.613159009,-0.045131858,0.130418481,-0.789784245,-0.021589905,0.803214978,1.030713535,-0.519543598,-0.003249275,0.021723744,0.814589639,0.228396628,-0.581815082,0.140484304,-0.035119387,-0.471241178,-1.301465097,0.457984201,0.298226009,-1.638297956,1.014820349,-0.030696996,0.560376518,-0.560520034,1.438913504,-0.709015146,-0.350384997,1.242240361,1.139062012,0.336638306,0.013219843,1.231078007,-0.592025232,-0.128939976,-1.484539032,-0.897194694,0.243087395,-1.601407433,-2.930420875,-1.337107431,0.435859098,-0.454216305,0.319136329,-0.813443592,2.588684287,0.417095107,0.689948916,-0.195528073,-0.926100394,0.606217781,-0.392099028,0.398953481,-0.362765422,-0.684106486,-0.103307101,-2.297729206,-0.906748378,-1.131531219,-0.323098922,0.728523541,-0.670509838,0.794768905,-1.218271379,-0.231996517,0.211606713,-0.975055154,0.610067035,2.674132893,-0.286802936,0.28125262,-0.193467045,0.163809713,2.04493286,0.819289806,1.077538055,0.770949646,0.584092732,-0.09930035,1.871255911,-1.486904088,0.085240717,-1.141823793,-0.964355002,0.626691803,1.416419704,-1.507255185,0.26502214,-0.024890335,0.808312501,-0.149907399,-0.009711073,-1.213189384,-0.756286517,1.698722333,-0.675536837,0.656794442,-0.103087713,-0.529257778,-1.041403351,-1.068231061,1.435074544,-0.224944094,-0.501250285,0.247817955,0.852623745,-0.851090181,0.326463613,2.119379618,-1.588978419,0.08123221,-1.752268818,-0.753845557,1.110360077,-0.308297017,0.682708125,-0.406895059,0.21886009,1.172705679,2.115782474,-0.350444999,0.044996253,-2.158729975,-0.468379742,-1.193075946,-0.147740181,0.408253219,0.845180389,-0.580946385,-1.992348979,-1.622416722,0.217848161,-0.849892425,0.146403268,-0.457308332,0.06163996,0.225116172,-1.365065377,-0.773597026,-0.64369944,0.606562138,-0.232829481,-0.210589098,1.450720365,-0.605919496,-1.122887574,-0.407962466,-0.45880645,0.979636119,0.065277809,-0.416497425,-0.087930291,1.028652064,0.232155223,-0.946651835,1.214545801,0.415512621,-0.024964867,0.238187602,-0.570068996,-0.969213032,-0.443964185,-0.402321011,-0.209303992,0.506625863,0.759801393


            };

            List<decimal> o = new List<decimal>();

            for (int i = 0; i < obs.Count; i++)
            {
                o.Add((decimal)obs[i]);
            }
            return o;
        }

        private IEnumerable<decimal> GenerateNormal()
        {
            List<double> sample = new List<double>();
            Random random = new Random();

            for (int i = 0; i < 1000; i++)
            {
                //sample.Add(-1 * (1 / lambda) * Math.Log(random.NextDouble()));
                sample.Add(Math.Pow(Math.Log10((double)1 - random.NextDouble()),2));
            }
            List<decimal> obs = new List<decimal>();

            for (int i = 0; i < sample.Count; i++)
            {
                obs.Add((decimal)sample[i]);
            }
            return obs;
        }


        private void LoadTop10ProductsByRevenue()
        {
            //Add Bar Chart for YDT Top 10 Products by revenue

            try
            {
                Top10ProductsByRevenue = _salesMetricsManager.GetTop10ProductsRevenueYTDChart();
            }
            catch (Exception ex)
            {

                MessageBox.Show("Unable to load the Top 10 products by revenue YTD. \r\n\r\n" + ex.Message,
                                "Retrieval Error",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
                Navigation.NavigateTo<ReportsSwitchboardViewModel>();
            }
        }

        private void LoadRevenueYTD()
        {
            RevenueYTD = new ValueModel();
            RevenueYTD.Title = "REVENUE YEAR TO DATE";
            RevenueYTD.Value = _salesMetricsManager.GetSalesRevenueYTD();
        }

        private void LoadMonthlyRevenueYTD()
        {
            try
            {
                SalesRevenueByMonth = _salesMetricsManager.GetMonthlyRevenueYTDChart();
            }
            catch (Exception ex)
            {

                MessageBox.Show("Unable to load the monthly revenue YTD. \r\n\r\n" + ex.Message,
                                "Retrieval Error",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
                Navigation.NavigateTo<ReportsSwitchboardViewModel>();
            }
        }

        private void LoadTop10ProductsRevenue()
        {
            try
            {
                Top10RevenueYTD = new ValueModel();
                Top10RevenueYTD.Title = "Top 10 Products Revenue YTD";
                Top10RevenueYTD.Value = _salesMetricsManager.GetTop10ProductsRevenueYTD();
            }
            catch (Exception ex)
            {

                MessageBox.Show("Unable to load the Top 10 products revenue YTD. \r\n\r\n" + ex.Message,
                                "Retrieval Error",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
                Navigation.NavigateTo<ReportsSwitchboardViewModel>();
            }
        }

        private bool CanCloseView(object obj)
        {
            return true;
        }

        private void CloseView(object obj)
        {
            Navigation.NavigateTo<ReportsSwitchboardViewModel>();
        }
    }
}

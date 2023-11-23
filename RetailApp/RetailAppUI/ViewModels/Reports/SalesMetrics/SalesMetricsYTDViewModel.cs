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

            //Instantiate commands
            CloseViewCommand = new RelayCommand(CloseView, CanCloseView);
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

using BussinessLogicLibrary.SalesMetrics.MTD;
using ChartModelsLibrary.ChartModels;
using ModelsLibrary;
using RetailAppUI.Commands;
using RetailAppUI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetailAppUI.ViewModels.Reports.SalesMetrics
{
    public class SalesMetricsMTDViewModel : BaseViewModel
    {
        private readonly ISalesMetricsMTDManager _salesMetricsMTDManager;

        private INavigationService _navigation;

        public INavigationService Navigation
        {
            get { return _navigation; }
            set { _navigation = value; OnPropertyChanged(); }
        }

        private ValueModel _revenueMTD;
        public ValueModel RevenueMTD
        {
            get { return _revenueMTD; }
            set { _revenueMTD = value; OnPropertyChanged(); }
        }

        private BarChartModel _salesRevenueByProduct;
        public BarChartModel SalesRevenueByProduct
        {
            get { return _salesRevenueByProduct; }
            set { _salesRevenueByProduct = value; OnPropertyChanged(); }
        }

        private ValueModel _top10RevenueMTD;
        public ValueModel Top10RevenueMTD
        {
            get { return _top10RevenueMTD; }
            set { _top10RevenueMTD = value; OnPropertyChanged(); }
        }


        private BarChartModel _top10ProductsByRevenue;
        public BarChartModel Top10ProductsByRevenue
        {
            get { return _top10ProductsByRevenue; }
            set { _top10ProductsByRevenue = value; OnPropertyChanged(); }
        }

        private HistogramModel _daysCountToCloseOrderMTD;
        public HistogramModel DaysCountToCloseOrderMTD
        {
            get { return _daysCountToCloseOrderMTD; }
            set { _daysCountToCloseOrderMTD = value; OnPropertyChanged(); }
        }

        private ValueModel _countOfOrdersMTD;
        public ValueModel CountOfOrdersMTD
        {
            get { return _countOfOrdersMTD; }
            set { _countOfOrdersMTD = value; OnPropertyChanged(); }
        }

        private ValueModel _countOfOpenOrdersMTD;
        public ValueModel CountOfOpenOrdersMTD
        {
            get { return _countOfOpenOrdersMTD; }
            set { _countOfOpenOrdersMTD = value; OnPropertyChanged(); }
        }

        private ValueModel _countOfCancelledOrdersMTD;
        public ValueModel CountOfCancelledOrdersMTD
        {
            get { return _countOfCancelledOrdersMTD; }
            set { _countOfCancelledOrdersMTD = value; OnPropertyChanged(); }
        }


        public RelayCommand CloseViewCommand { get; set; }

        public SalesMetricsMTDViewModel(ISalesMetricsMTDManager salesMetricsMTDManager, INavigationService navigationService)
        {
            _salesMetricsMTDManager = salesMetricsMTDManager;
            Navigation = navigationService;


            //instantiate commands
            CloseViewCommand = new RelayCommand(CloseView, CanCloseView);

            //Load data
            LoadRevenueMTD();
            LoadSalesRevenueByProduct();
            LoadTop10RevenueMTD();
            LoadTop10ProductsByRevenue();
            LoadDaysCountToCloseOrderMTD();
            LoadCountOfOrdersMTD();
            LoadCountOfOpenOrdersMTD();
            LoadCountOfCancelledOrdersMTD();
        }

        private void LoadCountOfCancelledOrdersMTD()
        {
            CountOfCancelledOrdersMTD = new ValueModel();
            CountOfCancelledOrdersMTD.Value = _salesMetricsMTDManager.GetCountOfCancelledSalesOrdersMTD();
            CountOfCancelledOrdersMTD.Title = "Count of Cancelled Orders MTD";
        }

        private void LoadCountOfOpenOrdersMTD()
        {
            CountOfOpenOrdersMTD = new ValueModel();
            CountOfOpenOrdersMTD.Value = _salesMetricsMTDManager.GetCountOfOpenSalesOrdersMTD();
            CountOfOpenOrdersMTD.Title = "Count of Open Orders MTD";
        }

        private void LoadCountOfOrdersMTD()
        {
            CountOfOrdersMTD = new ValueModel();
            CountOfOrdersMTD.Value = _salesMetricsMTDManager.GetCountOfSalesOrdersMTD();
            CountOfOrdersMTD.Title = "Count of Orders MTD";
        }

        private void LoadDaysCountToCloseOrderMTD()
        {
            DaysCountToCloseOrderMTD = _salesMetricsMTDManager.GetDaysCountToCloseSalesOrderMTD();
        }

        private void LoadTop10RevenueMTD()
        {
            Top10RevenueMTD = new ValueModel();
            Top10RevenueMTD.Value = _salesMetricsMTDManager.GetRevenueTop10ProductsMTD();
            Top10RevenueMTD.Title = "Revenue of Top 10 Products MTD";
        }

        private void LoadTop10ProductsByRevenue()
        {
            Top10ProductsByRevenue = _salesMetricsMTDManager.GetTop10ProductsByRevenueMTD();
        }

        private void LoadSalesRevenueByProduct()
        {
            SalesRevenueByProduct = _salesMetricsMTDManager.GetProductsByRevenueMTD();
        }

        private void LoadRevenueMTD()
        {
            RevenueMTD = new ValueModel();
            RevenueMTD.Value = _salesMetricsMTDManager.GetRevenueMTD();
            RevenueMTD.Title = "Revenue MTD";
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

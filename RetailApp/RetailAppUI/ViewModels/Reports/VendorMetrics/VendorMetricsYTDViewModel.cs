using BussinessLogicLibrary.Products;
using BussinessLogicLibrary.VendorMetrics.YTD;
using BussinessLogicLibrary.Vendors;
using ChartModelsLibrary.ChartModels;
using ModelsLibrary;
using RetailAppUI.Commands;
using RetailAppUI.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace RetailAppUI.ViewModels.Reports.VendorMetrics
{
    public class VendorMetricsYTDViewModel : BaseViewModel
    {
        private readonly IVendorManager _vendorManager;
        private readonly IProductsManager _productsManager;
        private readonly IVendorMetricsManagerYTD _vendorMetricsManagerYTD;


        private INavigationService _navigation;
        public INavigationService Navigation
        {
            get { return _navigation; }
            set { _navigation = value; OnPropertyChanged(); }
        }


        private ObservableCollection<VendorModel> _vendors;
        public ObservableCollection<VendorModel> Vendors
        {
            get { return _vendors; }
            set { _vendors = value; OnPropertyChanged(); }
        }

        private VendorModel _selectedVendor;
        public VendorModel SelectedVendor
        {
            get { return _selectedVendor; }
            set { _selectedVendor = value; 
                  LoadVendorProducts();
                  LoadSelectedVendorMetricsDataYTD(); 
                  OnPropertyChanged(); }
        }

        private IEnumerable<ProductModel> _products;
        public IEnumerable<ProductModel> Products
        {
            get { return _products; }
            set { _products = value; OnPropertyChanged(); }
        }

        private ObservableCollection<ProductModel> _selectedVendorProducts;
        public ObservableCollection<ProductModel> SelectedVendorProducts
        {
            get { return _selectedVendorProducts; }
            set { _selectedVendorProducts = value; OnPropertyChanged(); }
        }

        private ProductModel _selectedProduct;
        public ProductModel SelectedProduct
        {
            get { return _selectedProduct; }
            set { _selectedProduct = value; OnPropertyChanged(); }
        }

        private HistogramModel _vendorsLeadTimes;
        public HistogramModel VendorLeadTimes
        {
            get { return _vendorsLeadTimes; }
            set { _vendorsLeadTimes = value; OnPropertyChanged(); }
        }

        private HistogramModel _vendorDeliveryCompliance;
        public HistogramModel VendorDeliveryCompliance
        {
            get { return _vendorDeliveryCompliance; }
            set { _vendorDeliveryCompliance = value; OnPropertyChanged(); }
        }

        public RelayCommand CloseViewCommand { get; set; }

        public VendorMetricsYTDViewModel(INavigationService navigationService,
                                         IVendorManager vendorManager,
                                         IProductsManager productsManager,
                                         IVendorMetricsManagerYTD vendorMetricsManagerYTD)
        {
            Navigation = navigationService;
            _vendorManager = vendorManager;
            _productsManager = productsManager;
            _vendorMetricsManagerYTD = vendorMetricsManagerYTD;

            //Instantiate commands
            CloseViewCommand = new RelayCommand(CloseView, CanCloseViewCommand);

            //Load initial data
            LoadVendors();
            LoadProducts();


        }

        

        private void LoadSelectedVendorMetricsDataYTD()
        {
            LoadSelectedVendorHistogram();
            LoadSelectedVendorDeliveryComplianceYTD();
        }

        private void LoadSelectedVendorDeliveryComplianceYTD()
        {
            VendorDeliveryCompliance = _vendorMetricsManagerYTD.GetVendorDeliveryComplianceYTD(SelectedVendor.VendorID);
        }

        private void LoadSelectedVendorHistogram()
        {
            VendorLeadTimes = _vendorMetricsManagerYTD.GetVendorLeadTimesYTD(SelectedVendor.VendorID);
        }

        private void LoadVendors()
        {
            try
            {
                Vendors = new ObservableCollection<VendorModel>(_vendorManager.GetAll());
            }
            catch (Exception ex)
            {
                string message = "Unable to retrieve Vendors\r\n\r\n";
                MessageBox.Show(message + ex.Message, "Retrieval Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Navigation.NavigateTo<ReportsSwitchboardViewModel>();
            }

        }

        private void LoadProducts()
        {
            try
            {
                Products = _productsManager.GetAll();
            }
            catch (Exception ex)
            {
                string message = "Unable to retrieve Products\r\n\r\n";
                MessageBox.Show(message + ex.Message, "Retrieval Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Navigation.NavigateTo<ReportsSwitchboardViewModel>();
            }

        }

        void LoadVendorProducts()
        {
            List<ProductModel> vendorProducts = Products.Where(x => x.VendorID == SelectedVendor.VendorID).ToList();
            SelectedVendorProducts = new ObservableCollection<ProductModel>(vendorProducts);
        }

        private bool CanCloseViewCommand(object obj)
        {
            return true;
        }

        private void CloseView(object obj)
        {
            Navigation.NavigateTo<ReportsSwitchboardViewModel>();
        }
    }
}

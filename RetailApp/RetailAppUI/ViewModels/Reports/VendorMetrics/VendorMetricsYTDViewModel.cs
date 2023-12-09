using BussinessLogicLibrary.Products;
using BussinessLogicLibrary.VendorMetrics.YTD;
using BussinessLogicLibrary.Vendors;
using ModelsLibrary;
using RetailAppUI.Services;
using RetailAppUI.ViewModels.Reports.VendorMetrics.ViewTilesYTD;
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

        private ISharedDataService _sharedData;
        public ISharedDataService SharedData
        {
            get { return _sharedData; }
            set { _sharedData = value; }
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
            set { _selectedVendor = value; LoadSelectedVendorLeadTimesAllProducts(); LoadVendorProducts(); OnPropertyChanged(); }
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

        private VendorLeadTimesAllProductsViewModel selectedVendorLeadTimesAllProducts;
        public VendorLeadTimesAllProductsViewModel SelectedVendorLeadTimesAllProductsViewModel
        {
            get { return selectedVendorLeadTimesAllProducts; }
            set { selectedVendorLeadTimesAllProducts = value; OnPropertyChanged(); }
        }


        public VendorMetricsYTDViewModel(INavigationService navigationService,
                                         ISharedDataService sharedDataService,
                                         IVendorManager vendorManager,
                                         IProductsManager productsManager,
                                         IVendorMetricsManagerYTD vendorMetricsManagerYTD)
        {
            Navigation = navigationService;
            SharedData = sharedDataService;
            _vendorManager = vendorManager;
            _productsManager = productsManager;
            _vendorMetricsManagerYTD = vendorMetricsManagerYTD;
            //Load initial data
            LoadVendors();
            LoadProducts();
            
        }

        private void LoadSelectedVendorLeadTimesAllProducts()
        {
            SharedData.SharedData = SelectedVendor.VendorID;
            if (SharedData.SharedData is not null)
            {
                
                SelectedVendorLeadTimesAllProductsViewModel = new VendorLeadTimesAllProductsViewModel(_vendorMetricsManagerYTD, SharedData);
            }
            
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
    }
}

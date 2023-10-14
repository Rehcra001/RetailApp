using BussinessLogicLibrary.Products;
using ModelsLibrary;
using ModelsLibrary.RepositoryInterfaces;
using RetailAppUI.Commands;
using RetailAppUI.Services;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Data;

namespace RetailAppUI.ViewModels.Products
{
    public class ProductsSwitchboardViewModel : BaseViewModel
    {
		private readonly ProductsManager _productsManager;
        private IVendorRepository _vendorRepository;

        private string _groupByState;

        #region Collection View Property
        public ICollectionView ProductsCollectionView { get; set; }

        private ICurrentViewService	_currentView;
		public ICurrentViewService CurrentView
		{
			get { return _currentView; }
			set { _currentView = value; OnPropertyChanged(); }
		}
        #endregion Collection View Property

        #region Service Properties
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

        private IConnectionStringService _connectionString;
        public IConnectionStringService ConnectionString { get => _connectionString; set => _connectionString = value; }
        #endregion Service Properties

        #region Product Properties
        private ObservableCollection<ProductModel> _products;       
        public ObservableCollection<ProductModel> Products
		{
			get { return _products; }
			set { _products = value; OnPropertyChanged(); }
		}

		private ProductModel _product;
		public ProductModel Product
		{
			get { return _product; }
			set { _product = value; OnPropertyChanged(); }
		}
        #endregion Product Properties

        #region Product Filter Properties
        private string _productNameFilter = string.Empty;
		public string ProductNameFilter
		{
			get { return _productNameFilter; }
			set { _productNameFilter = value; OnPropertyChanged(); ProductsCollectionView.Refresh(); }
		}
        #endregion Product Filter Properties

        #region Relay Command Properties
        //commands
        public RelayCommand CloseViewCommand { get; set; }
		public RelayCommand NavigateToAddNewProductViewCommand { get; set; }
        public RelayCommand NavigateToProductViewCommand { get; set; }
        public RelayCommand GroupByCategoryCommand { get; set; }
        public RelayCommand GroupByVendorCommand { get; set; }
        public RelayCommand ClearGroupByCommand { get; set; }
        #endregion Relay Command Properties

        #region Constructor
        public ProductsSwitchboardViewModel(INavigationService navigation, ICurrentViewService currentView, ISharedDataService sharedData, IConnectionStringService connectionString, IVendorRepository vendorRepository)
        {
			Navigation = navigation;
			CurrentView = currentView;
			SharedData = sharedData;
			ConnectionString = connectionString;
            _vendorRepository = vendorRepository;

			//Fetch a list of products
            _productsManager = new ProductsManager(ConnectionString.GetConnectionString(), _vendorRepository);
            GetProductsList();

			CloseViewCommand = new RelayCommand(CloseView, CanCloseView);
			NavigateToAddNewProductViewCommand = new RelayCommand(NavigateToAddNewProductView, CanNavigateToAddNewProductView);
			NavigateToProductViewCommand = new RelayCommand(NavigateToProductView, CanNavigateToProductView);
			GroupByCategoryCommand = new RelayCommand(GroupByCategory, CanGroupByCategory);
			GroupByVendorCommand = new RelayCommand(GroupByVendor, CanGroupByVendor);
			ClearGroupByCommand = new RelayCommand(ClearGroupBy, CanClearGroupBy);

			SetGroupByState("Clear");
        }
        #endregion Constructor


        #region GroupBy Methods
        private bool CanGroupByVendor(object obj)
        {
			return !_groupByState.Equals("Vendor");
        }

        private void GroupByVendor(object obj)
        {
			SetGroupByState("Vendor");
        }

        private bool CanGroupByCategory(object obj)
        {
			return !_groupByState.Equals("Category");
        }

        private void GroupByCategory(object obj)
        {
			SetGroupByState("Category");
        }

        private bool CanClearGroupBy(object obj)
        {
			return !_groupByState.Equals("Clear");
        }

        private void ClearGroupBy(object obj)
        {
			SetGroupByState("Clear");
			
        }

        private void SetGroupByState(string state)
		{
			_groupByState = state;

			switch (_groupByState)
			{
				case "Clear":
					if (ProductsCollectionView.GroupDescriptions.Count > 0)
					{
                        ProductsCollectionView.GroupDescriptions.Clear();
                    }                    
                    break;
				case "Category":
                    if (ProductsCollectionView.GroupDescriptions.Count > 0)
                    {
                        ProductsCollectionView.GroupDescriptions.Clear();
                    }
					ProductsCollectionView.GroupDescriptions.Add(new PropertyGroupDescription("Category.CategoryName"));
                    break;
                case "Vendor":
                    if (ProductsCollectionView.GroupDescriptions.Count > 0)
                    {
                        ProductsCollectionView.GroupDescriptions.Clear();
                    }
                    ProductsCollectionView.GroupDescriptions.Add(new PropertyGroupDescription("Vendor.CompanyName"));
                    break;
            }
		}
        #endregion GroupBy Methods

        #region Products List Methods
        private void GetProductsList()
		{
			try
			{
                Products = new ObservableCollection<ProductModel>(_productsManager.GetAll());
				SetProductCollectionView();
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "Error Retrieving Products", MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}
        }

		private void SetProductCollectionView()
		{
			ProductsCollectionView = CollectionViewSource.GetDefaultView(Products);
			//Add a filter
			ProductsCollectionView.Filter = FilterProductName;
		}
        #endregion Products List Methods

        #region Filter Methods
        private bool FilterProductName(object obj)
        {
            if (obj is ProductModel product)
			{
				return product.ProductName.Contains(ProductNameFilter, StringComparison.InvariantCultureIgnoreCase);
			}
			return false;
        }
        #endregion Filter Methods

        #region Navigate To Views
        private bool CanNavigateToAddNewProductView(object obj)
        {
			return true;
        }

        private void NavigateToAddNewProductView(object obj)
        {

			Navigation.NavigateTo<AddNewProductViewModel>();
        }

        private bool CanNavigateToProductView(object obj)
        {
			return Product != null;
        }

        private void NavigateToProductView(object obj)
        {
            //ProductView will need this product id
            SharedData.SharedData = Product.ProductID;
			Navigation.NavigateTo<ProductViewModel>();
        }
        #endregion Navigate To Views

        #region Close View
        private bool CanCloseView(object obj)
        {
            return true;
        }

        private void CloseView(object obj)
        {
            CurrentView.CurrentView = "HomeView";
            Navigation.NavigateTo<HomeViewModel>();
        }
        #endregion Close View
    }
}

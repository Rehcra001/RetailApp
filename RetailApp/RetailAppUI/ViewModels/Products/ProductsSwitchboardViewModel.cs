using BussinessLogicLibrary.Products;
using ModelsLibrary;
using RetailAppUI.Commands;
using RetailAppUI.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace RetailAppUI.ViewModels.Products
{
    public class ProductsSwitchboardViewModel : BaseViewModel
    {
		private readonly ProductsManager _productsManager;

		public ICollectionView ProductsCollectionView { get; set; }

        private ICurrentViewService	_currentView;
		public ICurrentViewService CurrentView
		{
			get { return _currentView; }
			set { _currentView = value; OnPropertyChanged(); }
		}

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


		public RelayCommand CloseViewCommand { get; set; }
		public RelayCommand NavigateToAddNewProductViewCommand { get; set; }

        public ProductsSwitchboardViewModel(INavigationService navigation, ICurrentViewService currentView, ISharedDataService sharedData, IConnectionStringService connectionString)
        {
			Navigation = navigation;
			CurrentView = currentView;
			SharedData = sharedData;
			ConnectionString = connectionString;

			//Fetch a list of products
            _productsManager = new ProductsManager(ConnectionString.GetConnectionString());
            GetProductsList();

			CloseViewCommand = new RelayCommand(CloseView, CanCloseView);
			NavigateToAddNewProductViewCommand = new RelayCommand(NavigateToAddNewProductView, CanNavigateToAddNewProductView);
        }

        #region ProductsMethods
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
			SetProductGrouping();
		}

		private void SetProductGrouping()
		{
			ProductsCollectionView.GroupDescriptions.Add(new PropertyGroupDescription("Vendor.CompanyName"));
		}
        #endregion ProductsMethods

        private bool CanNavigateToAddNewProductView(object obj)
        {
			return true;
        }

        private void NavigateToAddNewProductView(object obj)
        {

			Navigation.NavigateTo<AddNewProductViewModel>();
        }

        private bool CanCloseView(object obj)
        {
            return true;
        }

        private void CloseView(object obj)
        {
            CurrentView.CurrentView = "HomeView";
            Navigation.NavigateTo<HomeViewModel>();
        }
    }
}

using BussinessLogicLibrary.Products;
using ModelsLibrary;
using RetailAppUI.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using DataAccessLibrary.UnitsPerRepository;
using RetailAppUI.Commands;
using BussinessLogicLibrary.Vendors;
using BussinessLogicLibrary.Categories;
using BussinessLogicLibrary.UnitPers;

namespace RetailAppUI.ViewModels.Products
{
    public class AddNewProductViewModel : BaseViewModel
    {
        private readonly ProductManager _productManager;
        private readonly IVendorManager _vendorManager;
        private readonly ICategoryManager _categoryManager;
        private readonly IUnitPerManager _unitPerManager;

        private ProductModel? _product;
        public ProductModel? Product
        {
            get => _product;
            set { _product = value; OnPropertyChanged(); }
        }

        private ObservableCollection<VendorModel> _vendors;
        public ObservableCollection<VendorModel> Vendors
        {
            get => _vendors;
            set { _vendors = value; OnPropertyChanged(); }
        }

        private ObservableCollection<UnitsPerModel> _unitPers;
        public ObservableCollection<UnitsPerModel> UnitPers
        {
            get => _unitPers;
            set { _unitPers = value; OnPropertyChanged(); }
        }

        private ObservableCollection<CategoryModel> _categories;
        public ObservableCollection<CategoryModel> Categories
        {
            get { return _categories; }
            set { _categories = value; OnPropertyChanged(); }
        }



        private IConnectionStringService  _connectionString;
        public IConnectionStringService ConnectionString
        {
            get => _connectionString;
            set { _connectionString = value; }
        }

        private INavigationService _navigation;
        public INavigationService Navigation
        {
            get => _navigation;
            set { _navigation = value; OnPropertyChanged(); }
        }

        private ICurrentViewService _currentView;

        public ICurrentViewService CurrentView
        {
            get => _currentView;
            set { _currentView = value; OnPropertyChanged(); }
        }


        //Commands
        public RelayCommand CloseViewCommand { get; set; }
        public RelayCommand CancelActionCommand { get; set; }
        public RelayCommand SaveProductCommand { get; set; }


        public AddNewProductViewModel(INavigationService navigation,
                                      IConnectionStringService connectionString,
                                      ICurrentViewService currentView,
                                      IVendorManager vendorManager,
                                      ICategoryManager categoryManager,
                                      IUnitPerManager unitPerManager)
        {
            Navigation = navigation;
            ConnectionString = connectionString;
            CurrentView = currentView;
            _vendorManager = vendorManager;
            _categoryManager = categoryManager;
            _unitPerManager = unitPerManager;

            //Set product to a new product model
            _productManager = new ProductManager(ConnectionString.GetConnectionString(), _vendorManager, _categoryManager, _unitPerManager);
            Product = _productManager.Product;

            //Add list of vendor to Vendors
            try
            {
                Vendors = new ObservableCollection<VendorModel>(_vendorManager.GetAll());
            }
            catch (Exception ex)
            {
                //error retrieving vendors
                MessageBox.Show("Unable to retrieve the vendors.\r\n\r\n" + ex.Message, "Retrieval Errror",
                                MessageBoxButton.OK, MessageBoxImage.Error);
                Navigation.NavigateTo<ProductsSwitchboardViewModel>();
            }

            //Add list of unit pers to UnitPers
            try
            {
                UnitPers = new ObservableCollection<UnitsPerModel>(_unitPerManager.GetAll());
            }
            catch (Exception ex)
            {
                //error retrieving unit pers
                MessageBox.Show("Unable to retrieve the Units.\r\n\r\n" + ex.Message, "Retrieval Error",
                                MessageBoxButton.OK, MessageBoxImage.Error);
                Navigation.NavigateTo<ProductsSwitchboardViewModel>();
            }

            //Add list of categories to Categories
            try
            {
                Categories = new ObservableCollection<CategoryModel>(_categoryManager.GetAll());
            }
            catch (Exception ex)
            {
                //error retrieving Categories
                MessageBox.Show("Unable to retrieve the categories.\r\n\r\n" + ex.Message, "Categories Errror",
                                MessageBoxButton.OK, MessageBoxImage.Error);
                Navigation.NavigateTo<ProductsSwitchboardViewModel>();
            }


            //Instantiate commands
            CloseViewCommand = new RelayCommand(CloseView, CanCloseView);
            CancelActionCommand = new RelayCommand(CancelAction, CanCancelAction);
            SaveProductCommand = new RelayCommand(SaveProduct, CanSaveProduct);
        }

        private bool CanSaveProduct(object obj)
        {
            return true;
        }

        private void SaveProduct(object obj)
        {            
            //Make sure vendor, unit per and category are not null
            if (Product!.Vendor == null)
            {
                MessageBox.Show("Please select a vendor.", "Vendor Missing", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (Product.Unit == null)
            {
                MessageBox.Show("Please select a unit per.", "Unit Per Missing", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (Product.Category == null)
            {
                MessageBox.Show("Please select a category.", "Category Missing", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            try
            {
                _productManager.Insert();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error Saving", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            Navigation.NavigateTo<ProductsSwitchboardViewModel>();
        }

        private bool CanCancelAction(object obj)
        {
            return true;
        }

        private void CancelAction(object obj)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to cancel this add. \r\n Changes made will not be saved!\r\n",
                                                      "Cancel Add New Product", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);
            if (result == MessageBoxResult.Yes)
            {
                Navigation.NavigateTo<ProductsSwitchboardViewModel>();
            }
        }

        private bool CanCloseView(object obj)
        {
            return true;
        }

        private void CloseView(object obj)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to close this view. \r\n No changes made will be saved!\r\n",
                                                      "Close View", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);
            if (result == MessageBoxResult.Yes)
            {
                Navigation.NavigateTo<ProductsSwitchboardViewModel>();
            }
        }
    }
}

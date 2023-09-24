using BussinessLogicLibrary.Products;
using ModelsLibrary;
using RetailAppUI.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLibrary.VendorRepository;
using System.Windows;
using DataAccessLibrary.UnitsPerRepository;
using RetailAppUI.Commands;
using System.Text.RegularExpressions;
using DataAccessLibrary.CategoryRepository;

namespace RetailAppUI.ViewModels.Products
{
    public class AddNewProductViewModel : BaseViewModel
    {
        private ProductManager _productManager;

        private ProductModel? _product;
        public ProductModel? Product
        {
            get => _product;
            set { _product = value; OnPropertyChanged(); }
        }

        private VendorModel? _vendor;
        public VendorModel? Vendor
        {
            get => _vendor;
            set { _vendor = value; OnPropertyChanged(); }
        }

        private ObservableCollection<VendorModel> _vendors;
        public ObservableCollection<VendorModel> Vendors
        {
            get => _vendors;
            set { _vendors = value; OnPropertyChanged(); }
        }

        private UnitsPerModel? _unitPer;
        public UnitsPerModel? UnitPer
        {
            get => _unitPer;
            set { _unitPer = value; OnPropertyChanged(); }
        }

        private ObservableCollection<UnitsPerModel> _unitPers;
        public ObservableCollection<UnitsPerModel> UnitPers
        {
            get => _unitPers;
            set { _unitPers = value; OnPropertyChanged(); }
        }

        private CategoryModel _category;
        public CategoryModel Category
        {
            get { return _category; }
            set { _category = value; OnPropertyChanged(); }
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


        public AddNewProductViewModel(INavigationService navigation, IConnectionStringService connectionString, ICurrentViewService currentView)
        {
            Navigation = navigation;
            ConnectionString = connectionString;
            CurrentView = currentView;

            //Set product to a new product model
            _productManager = new ProductManager(ConnectionString.GetConnectionString());
            Product = _productManager.Product;

            //Add list of vendor to Vendors
            Tuple<IEnumerable<VendorModel>, string> vendors = new VendorRepository(ConnectionString.GetConnectionString()).GetAll().ToTuple();
            //Check for errors
            if (vendors.Item2 == null)//null if no errors raised
            {
                Vendors = new ObservableCollection<VendorModel>(vendors.Item1);
            }
            else
            {
                //error retrieving vendors
                MessageBox.Show("Unable to retrieve the vendors.\r\n" + vendors.Item2, "Vendors Errror",
                                MessageBoxButton.OK, MessageBoxImage.Error);
                // TODO - Add return to products switch board on error
            }

            //Add list of unit pers to UnitPers
            Tuple<IEnumerable<UnitsPerModel>, string> unitPers = new UnitsPerRepository(ConnectionString.GetConnectionString()).GetAll().ToTuple();
            //Check for errors
            if (unitPers.Item2 == null)//null if no errors raised
            {
                UnitPers = new ObservableCollection<UnitsPerModel>(unitPers.Item1);
            }
            else
            {
                //error retrieving unit pers
                MessageBox.Show("Unable to retrieve the Units.\r\n" + unitPers.Item2, "Units Errror",
                                MessageBoxButton.OK, MessageBoxImage.Error);
                // TODO - Add return to products switch board on error
            }

            //Add list of categories to Categories
            Tuple<IEnumerable<CategoryModel>, string> categories = new CategoryRepository(ConnectionString.GetConnectionString()).GetAll().ToTuple();
            //Check for errors
            if (categories.Item2 == null)//null if no error raised
            {
                Categories = new ObservableCollection<CategoryModel>(categories.Item1);
            }
            else
            {
                //error retrieving Categories
                MessageBox.Show("Unable to retrieve the categories.\r\n" + categories.Item2, "Categories Errror",
                                MessageBoxButton.OK, MessageBoxImage.Error);
                // TODO - Add return to products switch board on error
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
            if (Vendor == null)
            {
                MessageBox.Show("Please select a vendor.", "Vendor Missing", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (UnitPer == null)
            {
                MessageBox.Show("Please select a unit per.", "Unit Per Missing", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (Category == null)
            {
                MessageBox.Show("Please select a category.", "Category Missing", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            try
            {
                _productManager.Insert(Vendor, UnitPer, Category);
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
            MessageBoxResult result = MessageBox.Show("Are you sure you want to cancel this add. \r\n No changes made will be saved!\r\n",
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

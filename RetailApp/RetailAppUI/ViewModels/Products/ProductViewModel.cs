using BussinessLogicLibrary.Categories;
using BussinessLogicLibrary.Products;
using BussinessLogicLibrary.UnitPers;
using BussinessLogicLibrary.Vendors;
using DataAccessLibrary.UnitsPerRepository;
using ModelsLibrary;
using RetailAppUI.Commands;
using RetailAppUI.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace RetailAppUI.ViewModels.Products
{
    public class ProductViewModel : BaseViewModel
    {
		private string _state;
		private readonly ProductManager _productManager;
		private readonly IVendorManager _vendorManager;
		private readonly ICategoryManager _categoryManager;
		private readonly IUnitPerManager _unitPerManager;

        #region Service Properties
        private ISharedDataService _sharedData;
		public ISharedDataService SharedData
		{
			get { return _sharedData; }
			set { _sharedData = value; }
		}


		private IConnectionStringService _connectionString;
		public IConnectionStringService ConnectionString
		{
			get { return _connectionString; }
			set { _connectionString = value; }
		}

		private INavigationService _navigation;

		public INavigationService Navigation
		{
			get { return _navigation; }
			set { _navigation = value; OnPropertyChanged(); }
		}

        #endregion Service Properties

        #region Product Property
        public ProductModel ProductUndoEdit { get; set; }

        private ProductModel _product;
		public ProductModel Product
		{
			get { return _product; }
			set { _product = value; OnPropertyChanged(); }
		}
        #endregion Product Property

        #region Vendor Properties

		private ObservableCollection<VendorModel> _vendors;
		public ObservableCollection<VendorModel> Vendors
		{
			get { return _vendors; }
			set { _vendors = value; OnPropertyChanged(); }
		}
        #endregion Vendor Properties

        #region Category Properties

		private ObservableCollection<CategoryModel> _categories;
		public ObservableCollection<CategoryModel> Categories
		{
			get { return _categories; }
			set { _categories = value; OnPropertyChanged(); }
		}
        #endregion Category Properties

        #region Unit Per Properties

		private ObservableCollection<UnitsPerModel> _unitPers;
		public ObservableCollection<UnitsPerModel> UnitPers
		{
			get { return _unitPers; }
			set { _unitPers = value; OnPropertyChanged(); }
		}
        #endregion Unit Per Properties

        #region Set read only properties
        private bool _textReadOnly;
		public bool TextReadOnly
		{
			get { return _textReadOnly; }
			set { _textReadOnly = value; OnPropertyChanged(); }
		}

		private bool _allowVendorEdit;
		public bool AllowVendorEdit
		{
			get { return _allowVendorEdit; }
			set { _allowVendorEdit = value; OnPropertyChanged(); }
		}

		private bool _allowCategoryEdit;
		public bool AllowCategoryEdit
		{
			get { return _allowCategoryEdit; }
			set { _allowCategoryEdit = value; OnPropertyChanged(); }
		}

		private bool _allowUnitPerEdit;
		public bool AllowUnitPerEdit
		{
			get { return _allowUnitPerEdit; }
			set { _allowUnitPerEdit = value; OnPropertyChanged(); }
		}
        #endregion Set read only properties


        #region RelayCommand Properties

        public RelayCommand	CloseViewCommand { get; set; }
		public RelayCommand EditProductCommand { get; set; }
        public RelayCommand SaveProductCommand { get; set; }
        public RelayCommand CancelActionCommand { get; set; }

        #endregion RelayCommand Properties

        #region Constructor
        public ProductViewModel(IConnectionStringService connectionString,
                                ISharedDataService sharedData,
                                INavigationService navigation,
                                IVendorManager vendorManager,
								ICategoryManager categoryManager,
								IUnitPerManager unitPerManager)
        {
			SharedData = sharedData; //Will hold the Product ID selected in the products switchboard view
			ConnectionString = connectionString;
			Navigation = navigation;
			_vendorManager = vendorManager;
			_categoryManager = categoryManager;
			_unitPerManager = unitPerManager;

            //Get the Product to display
            _productManager = new ProductManager(ConnectionString.GetConnectionString(), _vendorManager, _categoryManager, _unitPerManager);
			try
			{
				int id = (int)SharedData.SharedData;
				Product = _productManager.GetByID(id);
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "Product Error", MessageBoxButton.OK, MessageBoxImage.Error);
			}

			//Retrieve all Vendors
			GetVendors();
            //Retrieve all Unit pers
            GetUnitPers();
            //Retrieve all Categories
            GetCategories();

			//Instantiate RelayCommand's
			CloseViewCommand = new RelayCommand(CloseView, CanCloseView);
			EditProductCommand = new RelayCommand(EditProduct, CanEditProduct);
			CancelActionCommand = new RelayCommand(CancelAction, CanCancelAction);
			SaveProductCommand = new RelayCommand(SaveProduct, CanSaveProduct);

			//Set the state view - means no editing allowed
			SetState("View");
			
        }
        #endregion Constructor

        private bool CanSaveProduct(object obj)
        {
			return !_state.Equals("View");
        }

        private void SaveProduct(object obj)
        {
			try
			{
				_productManager.Update();
			}
			catch (Exception ex)
			{
                MessageBox.Show(ex.Message, "Error Saving Product", MessageBoxButton.OK, MessageBoxImage.Error);
				return;
            }
			SetState("View");
        }

        private bool CanCancelAction(object obj)
        {
			return !_state.Equals("View");
        }

        private void CancelAction(object obj)
        {
            if (_state.Equals("Edit"))
			{
				//Set product to ProductUndoEdit
				Product = ProductUndoEdit;

                //Set Selected vendor to product vendor
                Product.Vendor = Vendors.First(v => v.VendorID == Product.VendorID);

                //Set Selected Unit per to Product Unit Per
                Product.Unit = UnitPers.First(u => u.UnitPerID == Product.UnitPerID);

				//Set Selected Category to Product Category
				Product.Category = Categories.First(c => c.CategoryID == Product.CategoryID);
            }
			SetState("View");
        }

        private bool CanEditProduct(object obj)
        {
			return _state.Equals("View");
        }

        private void EditProduct(object obj)
        {
			SetState("Edit");
            //prepare for cancelling any edits
            try
            {                
                ProductUndoEdit = new ProductManager(ConnectionString.GetConnectionString(), _vendorManager, _categoryManager, _unitPerManager).GetByID(Product.ProductID);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Product Undo Edit model Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool CanCloseView(object obj)
        {
			return true;
        }

        private void CloseView(object obj)
        {
			Navigation.NavigateTo<ProductsSwitchboardViewModel>();
        }


        #region Set State
        private void SetState(string state)
		{
			_state = state;

			switch (_state)
			{
				case "View":
					TextReadOnly = true;
					AllowCategoryEdit = false;
					AllowUnitPerEdit = false;
					AllowVendorEdit = false;
					break;
				default:
					TextReadOnly = false;
					AllowCategoryEdit = true;
					AllowUnitPerEdit = true;
					if (Product.InventoryTransactions!.Any<InventoryTransactionModel>())
					{
						AllowVendorEdit = false;
					}
					else
					{
						AllowVendorEdit = true;
					}
					break;
			}
		}
        #endregion Set State

        #region Get Categories
        private void GetCategories()
        {
			try
			{
				Categories = new ObservableCollection<CategoryModel>(_categoryManager.GetAll());

                //Set selected category
                Product.Category = Categories.First(c => c.CategoryID == Product.CategoryID);
            }
			catch (Exception ex)
			{
                //Error retrieving categories
                MessageBox.Show("Error retrieving the categories\r\n\r\n" + ex.Message, "Retrieval Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Navigation.NavigateTo<ProductsSwitchboardViewModel>();
            }
        }
        #endregion Get Categories

        #region Get Unit Pers
        private void GetUnitPers()
        {
			try
			{
				UnitPers = new ObservableCollection<UnitsPerModel>(_unitPerManager.GetAll());
                //Set selected unit per
                Product.Unit = UnitPers.First(u => u.UnitPerID == Product.UnitPerID);
            }
			catch (Exception ex)
			{
                //error with unitPers
                MessageBox.Show("Error retrieving unitpers\r\n\r\n" + ex.Message, "Retrieval Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Navigation.NavigateTo<ProductsSwitchboardViewModel>();
            }
        }
        #endregion Get Unit Pers

        #region Get Vendors
        private void GetVendors()
        {
			try
			{
                Vendors = new ObservableCollection<VendorModel>(_vendorManager.GetAll());

                //Set selected vendor
                Product.Vendor = Vendors.First(v => v.VendorID == Product.VendorID);
            }
			catch (Exception ex)
			{
                //Error retrieving vendors
                MessageBox.Show("Error retrieving the vendors\r\n\r\n" + ex.Message, "Retrieval Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Navigation.NavigateTo<ProductsSwitchboardViewModel>();
            }
        }
        #endregion Get Vendors
    }
}

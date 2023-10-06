using BussinessLogicLibrary.Products;
using DataAccessLibrary.CategoryRepository;
using DataAccessLibrary.UnitsPerRepository;
using DataAccessLibrary.VendorRepository;
using ModelsLibrary;
using RetailAppUI.Commands;
using RetailAppUI.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace RetailAppUI.ViewModels.Products
{
    public class ProductViewModel : BaseViewModel
    {
		private string _state;
		private ProductManager _productManager;

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
        public ProductViewModel(IConnectionStringService connectionString, ISharedDataService sharedData, INavigationService navigation)
        {
			SharedData = sharedData; //Will hold the Product ID selected in the products switchboard view
			ConnectionString = connectionString;
			Navigation = navigation;

			//Get the Product to display
			_productManager = new ProductManager(ConnectionString.GetConnectionString());
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
                ProductUndoEdit = new ProductManager(ConnectionString.GetConnectionString()).GetByID(Product.ProductID);
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
            Tuple<IEnumerable<CategoryModel>, string> categories = new CategoryRepository(ConnectionString.GetConnectionString()).GetAll().ToTuple();

            if (categories.Item2 == null)
            {
				//No errors
				Categories = new ObservableCollection<CategoryModel>(categories.Item1);

                //Set selected category
                Product.Category = Categories.First(c => c.CategoryID == Product.CategoryID);
            }
            else
            {
                //Error retrieving categories
                MessageBox.Show(categories.Item2, "Categories Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Navigation.NavigateTo<ProductsSwitchboardViewModel>();
            }
        }
        #endregion Get Categories

        #region Get Unit Pers
        private void GetUnitPers()
        {
            Tuple<IEnumerable<UnitsPerModel>, string> unitPers = new UnitsPerRepository(ConnectionString.GetConnectionString()).GetAll().ToTuple();

            if (unitPers.Item2 == null)
            {
				UnitPers = new ObservableCollection<UnitsPerModel>(unitPers.Item1);

                //Set selected unit per
                Product.Unit = UnitPers.First(u => u.UnitPerID == Product.UnitPerID);
            }
            else
            {
                //error with unitPers
                MessageBox.Show(unitPers.Item2, "UnitPer Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Navigation.NavigateTo<ProductsSwitchboardViewModel>();
            }
        }
        #endregion Get Unit Pers

        #region Get Vendors
        private void GetVendors()
        {
            Tuple<IEnumerable<VendorModel>, string> vendors = new VendorRepository(ConnectionString.GetConnectionString()).GetAll().ToTuple();

            if (vendors.Item2 == null)
            {
				Vendors = new ObservableCollection<VendorModel>(vendors.Item1);

                //Set selected vendor
                Product.Vendor = Vendors.First(v => v.VendorID == Product.VendorID);

            }
            else
            {
				//Error with vendors
				MessageBox.Show(vendors.Item2, "Vendors Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Navigation.NavigateTo<ProductsSwitchboardViewModel>();
            }
        }
        #endregion Get Vendors
    }
}

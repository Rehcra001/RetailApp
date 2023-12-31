﻿using BussinessLogicLibrary.Products;
using BussinessLogicLibrary.Purchases;
using BussinessLogicLibrary.Statuses;
using BussinessLogicLibrary.Vendors;
using ModelsLibrary;
using RetailAppUI.Commands;
using RetailAppUI.Services;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Data;

namespace RetailAppUI.ViewModels.Purchases
{
    public class AddNewPurchaseOrderViewModel : BaseViewModel
    {
        private readonly IAddNewPurchaseOrderManager _addNewPurchaseOrderManager;
		private readonly IVendorManager _vendorManager;
		private readonly IProductsManager _productsManager;
		private readonly IStatusManager _statusManager;

        private string _state;
        public ICollectionView PurchaseOrderLines { get; set; }

        private INavigationService _navigation;
		public INavigationService Navigation
		{
			get { return _navigation; }
			set { _navigation = value; OnPropertyChanged(); }
		}

		private PurchaseOrderHeaderModel _purchaseOrder;
		public PurchaseOrderHeaderModel PurchaseOrder
		{
			get { return _purchaseOrder; }
			set { _purchaseOrder = value; OnPropertyChanged(); }
		}

		private VendorModel _vendor;
		public VendorModel SelectedVendor
        {
			get { return _vendor; }
			set 
			{	
                //Check if any order lines have been added
                //These need to be removed if vendor changes
                if (VendorChanged(value.VendorID))
				{
                    _vendor = value;

                    //Change the vendor on PurchaseOrder
                    PurchaseOrder.Vendor = _vendor;
					//Set IsImported to Vendor selected
					IsImported = PurchaseOrder.Vendor.InternationalVendor;
                    //Retrieve the list of products related to this vendor
                    GetVendorProducts(value.VendorID);
                }
				OnPropertyChanged();                
            }
		}

		private bool _isImported;

		public bool IsImported
		{
			get { return _isImported; }
			set { _isImported = value; PurchaseOrder.IsImport = _isImported; OnPropertyChanged(); }
		}


		private int _selectedIndex = -1;
		public int SelectedIndex
		{
			get { return _selectedIndex; }
			set 
			{ 
				_selectedIndex = value;
				OnPropertyChanged(); 
			}
		}


		private ObservableCollection<VendorModel> _vendors;
		public ObservableCollection<VendorModel> Vendors
		{
			get { return _vendors; }
			set { _vendors = value; OnPropertyChanged(); }
		}

		private ObservableCollection<StatusModel> _orderStatuses;
		public ObservableCollection<StatusModel> OrderStatuses
		{
			get { return _orderStatuses; }
			set { _orderStatuses = value; OnPropertyChanged(); }
		}
		       

        private ObservableCollection<ProductModel> _products;        

        public ObservableCollection<ProductModel> Products
		{
			get { return _products; }
			set { _products = value; OnPropertyChanged(); }
		}

		private bool _textReadOnly;
		public bool TextReadOnly
		{
			get { return _textReadOnly; }
			set { _textReadOnly = value; OnPropertyChanged(); }
		}

		private bool _dateEnabled;
		public bool DateEnabled
		{
			get { return _dateEnabled; }
			set { _dateEnabled = value; OnPropertyChanged(); }
		}

		private bool _vendorEnabled;

		public bool VendorEnabled
		{
			get { return _vendorEnabled; }
			set { _vendorEnabled = value; OnPropertyChanged(); }
		}

		private bool _OrderStatusEnabled;

		public bool OrderStatusEnabled
		{
			get { return _OrderStatusEnabled; }
			set { _OrderStatusEnabled = value; OnPropertyChanged(); }
		}

		//Commands
		public RelayCommand	CloseViewCommand { get; set; }
        public RelayCommand AddNewLineCommand { get; set; }
        public RelayCommand RemoveLineCommand { get; set; }
		public RelayCommand SaveCommand { get; set; }

        //Constructor
        public AddNewPurchaseOrderViewModel(INavigationService navigation,
											IAddNewPurchaseOrderManager addNewPurchaseOrderManager,
                                            IVendorManager vendorManager,
											IProductsManager productsManager,
											IStatusManager statusManager)
        {
			Navigation = navigation;
			_addNewPurchaseOrderManager = addNewPurchaseOrderManager;
            _vendorManager = vendorManager;
			_productsManager = productsManager;
			_statusManager = statusManager;

            //Retrieve a list of Vendors
            GetVendors();

            //Retrieve a list of Order Statuses
            GetOrderStatuses();

            //Purchase order
			PurchaseOrder = new PurchaseOrderHeaderModel();
			//Set Order status to open
			PurchaseOrder.OrderStatus = OrderStatuses.FirstOrDefault(x => x.Status == "Open")!;
			//Set Order date
			PurchaseOrder.OrderDate = DateTime.Now;
			//Add the purchase order lines to a collection view
			PurchaseOrderLines = CollectionViewSource.GetDefaultView(PurchaseOrder.PurchaseOrderDetails);

            //Instantiate commands
            AddNewLineCommand = new RelayCommand(AddNewLine, CanAddNewLine);
			RemoveLineCommand = new RelayCommand(RemoveLine, CanRemoveLine);
			CloseViewCommand = new RelayCommand(CloseView, CanCloseView);
			SaveCommand = new RelayCommand(SavePurchaseOrder, CanSavePurchaseOrder);

			//Set the view state
            SetState("Add");
        }
        private void SetState(string state)
        {
            _state = state;

			switch (_state)
			{
				case "View":
					TextReadOnly = true;
					DateEnabled = false;
					VendorEnabled = false;
					OrderStatusEnabled = false;
                    break;
				default:
					TextReadOnly = false;
					DateEnabled = true;
                    OrderStatusEnabled = true;
                    if (_state.Equals("Edit"))
					{
						VendorEnabled = false;						
                    }
					else
					{
						VendorEnabled = true;
                    }
					break;
			}
        }     

        private bool CanSavePurchaseOrder(object obj)
        {
			return !_state.Equals("View");
        }

        private void SavePurchaseOrder(object obj)
        {
            if (_state.Equals("Add"))
			{
                try
                {
                    PurchaseOrder =_addNewPurchaseOrderManager.Insert(PurchaseOrder);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error Saving Purchase Order", MessageBoxButton.OK, MessageBoxImage.Error);
					return;
                }
            }
			SetState("View");
			//Force index change
			SelectedIndex = -1;
        }

        #region Close View
        private bool CanCloseView(object obj)
        {
            return true;
        }

        private void CloseView(object obj)
        {
            Navigation.NavigateTo<PurchaseOrdersSwitchboardViewModel>();
        }
        #endregion Close View

        private bool CanRemoveLine(object obj)
        {
			return PurchaseOrder.PurchaseOrderDetails.Count > 0 && 
				SelectedIndex >= 0 && 
				!_state.Equals("View");
        }

        private void RemoveLine(object obj)
        {
			string message = $"Line {SelectedIndex + 1} is about to be removed. \r\nAre you sure you want to remove this line?";
			MessageBoxResult result = MessageBox.Show(message, "Remove Line", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);

			if (result == MessageBoxResult.Yes)
			{
				//Check if purchase order line product is empty
				//if yes remove line
				//if not then
				//Check if purchase order line exists in Products
				//If yes then just remove line
				//if not then add purchase order line product to Products and remove the line

				ProductModel product = PurchaseOrder.PurchaseOrderDetails[SelectedIndex].Product;

                if (product.ProductID == 0)
				{
					//No product added to purchase order line - just remove the line
					PurchaseOrder.PurchaseOrderDetails.RemoveAt(SelectedIndex);
                }
				else
				{
					//A product has been added to the purchase order line
					//check if this product exists in Products property
					if (Products.Contains(product))
					{
                        //It is already in Products - just remove the line
                        PurchaseOrder.PurchaseOrderDetails.RemoveAt(SelectedIndex);
                    }
					else
					{
						//The product is not in Products property
						Products.Add(product);
                        //Remove the purchase order line
                        PurchaseOrder.PurchaseOrderDetails.RemoveAt(SelectedIndex);
                    }
				}
				//Refresh the collection view source
				PurchaseOrderLines.Refresh();
            }
			else
			{
				return;
			}
        }

        private bool CanAddNewLine(object obj)
        {
			return Products != null && Products.Count > 0 && 
				!_state.Equals("View");
        }

        private void AddNewLine(object obj)
        {
			//Check that a Vendor has been selected
			if (PurchaseOrder.Vendor.VendorID == default)
			{
				MessageBox.Show("Please select a vendor before adding purchase order lines.", "Vendor Error", MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}
			//Check if PurchaseOrderDetails is null
			else if (PurchaseOrder.PurchaseOrderDetails.Count == 0)
			{
				//Add the first purchase order line
				PurchaseOrder.PurchaseOrderDetails.Add(new PurchaseOrderDetailModel());
				//Any new lines added will only have a status of open
				PurchaseOrder.PurchaseOrderDetails[0].OrderLineStatus = OrderStatuses.First(x => x.Status!.Equals("Open"));
				PurchaseOrder.PurchaseOrderDetails[0].OrderLineStatusID = PurchaseOrder.PurchaseOrderDetails[0].OrderLineStatus.StatusID;
			}
			else
			{
				//Check data before adding a new line
				ValidateLine();
                
            }
			PurchaseOrderLines.Refresh();
            
        }

		private void ProductsLeftToOrder()
		{
            //check if any vendor products are left in Products to order
            string message = "";
            bool isValid = true;
			
            if (Products.Count == 0)
            {
                message += "All vendor products have been added to the order.\r\n";
                isValid = false;
            }
            if (isValid)
            {
                //Add another purchase order line
                PurchaseOrder.PurchaseOrderDetails.Add(new PurchaseOrderDetailModel());
				int index = PurchaseOrder.PurchaseOrderDetails.Count - 1; //find the order just added
				//Any new lines added will only have a status of open
				PurchaseOrder.PurchaseOrderDetails[index].OrderLineStatus = OrderStatuses.First(x => x.Status!.Equals("Open"));
				PurchaseOrder.PurchaseOrderDetails[index].OrderLineStatusID = PurchaseOrder.PurchaseOrderDetails[0].OrderLineStatus.StatusID;
            }
            else
            {
                //Missing data
                MessageBox.Show(message, "Missing Data", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

		private void ValidateLine()
		{
            //check that the last order line is completed before adding another
            //Product selected
            int index = PurchaseOrder.PurchaseOrderDetails.Count - 1;
            bool isValid = true;
            string message = "";

			if(	PurchaseOrder.PurchaseOrderDetails[index].Product.ProductID == 0)
            {
                message += "Please select a vendor before adding a new line.\r\n";
                isValid = false;
            }
            //Quantity added
            if (PurchaseOrder.PurchaseOrderDetails[index].QuantityOrdered <= 0)
            {
                message += "Please enter the order quantity before adding a new line.\r\n";
                isValid = false;
            }
            //Unit cost added
            if (PurchaseOrder.PurchaseOrderDetails[index].UnitCost <= 0)
            {
                message += "Please enter the unit cost before adding a new line.\r\n";
                isValid = false;
            }

            if (isValid)
            {
                //Remove the selected product from the list of products as only one product type is allowed per purchase order
                for (int i = 0; i < PurchaseOrder.PurchaseOrderDetails.Count; i++)
                {
                    if (Products.Contains(PurchaseOrder.PurchaseOrderDetails[i].Product))
                    {
                        Products.Remove(PurchaseOrder.PurchaseOrderDetails[i].Product);
                    }
                }
                ProductsLeftToOrder();
            }
            else
            {
                //Missing data
                MessageBox.Show(message, "Missing Data", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }         
        }

        private bool VendorChanged(int vendorID)
        {
			bool changeVendor = true;

			if (PurchaseOrder.PurchaseOrderDetails.Count > 0)
			{
				//check if order line product contain the vendor selected
				//Only the first line needs to be checked

				if (PurchaseOrder.PurchaseOrderDetails[0].Product.VendorID != vendorID)
				{
					//Warn that all order lines will be deleted
					MessageBoxResult result = MessageBox.Show("The purchase order lines will be deleted, \r\n as the vendor has changed. \r\n Do you want to change the vendor?",
															  "Vendor Changed",
															  MessageBoxButton.YesNo,
															  MessageBoxImage.Warning,
															  MessageBoxResult.No);
					if (result == MessageBoxResult.Yes)
					{
						//Remove any lines added
						PurchaseOrder.PurchaseOrderDetails.Clear();
						PurchaseOrderLines.Refresh();

						

						changeVendor = true;
					}
					else
					{
                        changeVendor = false;
					}
				}
			}
			return changeVendor;
        }

        /// <summary>
        /// Retrieves a list of vendors
        /// </summary>
        private void GetVendors()
		{
			try
			{
				Vendors = new ObservableCollection<VendorModel>(_vendorManager.GetAll());
			}
			catch (Exception ex)
			{
                //Error raised
                MessageBox.Show("Error retrieving the vendors\r\n\r\n" + ex.Message, "Retrieval Error.", MessageBoxButton.OK, MessageBoxImage.Error);
                Navigation.NavigateTo<PurchaseOrdersSwitchboardViewModel>();
            }
        }

		private void GetOrderStatuses()
		{
			try
			{
				OrderStatuses = new ObservableCollection<StatusModel>(_statusManager.GetAll().Where(x => x.Status!.Equals("Open")));
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
        }

        private void GetVendorProducts(int id)
		{
			try
			{
                Products = new ObservableCollection<ProductModel>(_productsManager.GetByVendorID(id));
            }
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "Vendors Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Navigation.NavigateTo<PurchaseOrdersSwitchboardViewModel>();
            }
			
		}

    }
}

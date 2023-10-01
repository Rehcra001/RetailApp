using BussinessLogicLibrary.Products;
using BussinessLogicLibrary.Purchases;
using DataAccessLibrary.OrderStatusRepository;
using DataAccessLibrary.VendorRepository;
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

namespace RetailAppUI.ViewModels.Purchases
{
    public class AddNewPurchaseOrderViewModel : BaseViewModel
    {
        private PurchaseOrderManager _purchaseOrderManager;
        public ICollectionView PurchaseOrderLines { get; set; }

        private INavigationService _navigation;
		public INavigationService Navigation
		{
			get { return _navigation; }
			set { _navigation = value; OnPropertyChanged(); }
		}

		private ICurrentViewService _currentView;
		public ICurrentViewService CurrentView
		{
			get { return _currentView; }
			set { _currentView = value; }
		}

		private IConnectionStringService _connectionString;
		public IConnectionStringService ConnectionString
		{
			get { return _connectionString; }
			set { _connectionString = value; }
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
                    
                }
				OnPropertyChanged();
                
            }
		}

		private int? _selectedIndex = null;
		public int? SelectedIndex
		{
			get { return _selectedIndex; }
			set { _selectedIndex = value; OnPropertyChanged(); }
		}


		private ObservableCollection<VendorModel> _vendors;
		public ObservableCollection<VendorModel> Vendors
		{
			get { return _vendors; }
			set { _vendors = value; OnPropertyChanged(); }
		}

		private ObservableCollection<OrderStatusModel> _orderStatuses;
		public ObservableCollection<OrderStatusModel> OrderStatuses
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

		//Commands
		public RelayCommand	CloseViewCommand { get; set; }
        public RelayCommand AddNewLineCommand { get; set; }

        //Constructor
        public AddNewPurchaseOrderViewModel(INavigationService navigation, ICurrentViewService currentView, IConnectionStringService connectionString)
        {
			Navigation = navigation;
			CurrentView = currentView;
			ConnectionString = connectionString;

            //Retrieve a list of Vendors
            GetVendors();

            //Retrieve a list of Order Statuses
            GetOrderStatuses();

            //Purchase order
            _purchaseOrderManager = new PurchaseOrderManager(ConnectionString.GetConnectionString());
			PurchaseOrder = _purchaseOrderManager.PurchaseOrder;
			//Set Order status to open
			PurchaseOrder.OrderStatus = OrderStatuses.FirstOrDefault(x => x.OrderStatus == "Open")!;
			//Set Order date
			PurchaseOrder.OrderDate = DateTime.Now.Date;

			PurchaseOrderLines = CollectionViewSource.GetDefaultView(PurchaseOrder.PurchaseOrderDetails);

            //Instantiate commands
            AddNewLineCommand = new RelayCommand(AddNewLine, CanAddNewLine);
        }

        private bool CanAddNewLine(object obj)
        {
			return true;
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
                //Get list of vendor products

                PurchaseOrder.PurchaseOrderDetails.Add(new PurchaseOrderDetailModel());
                GetVendorProducts(PurchaseOrder.Vendor.VendorID);

				
			}
			else
			{
				//check that the last order line is completed before adding another


				PurchaseOrder.PurchaseOrderDetails.Add(new PurchaseOrderDetailModel());
			}
			PurchaseOrderLines.Refresh();
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

						//Retrieve the list of products related to this vendor
						GetVendorProducts(vendorID);

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
            Tuple<IEnumerable<VendorModel>, string> vendors = new VendorRepository(ConnectionString.GetConnectionString()).GetAll().ToTuple();
			//check for errors
			if (vendors.Item2 == null)
			{
				//No error
				Vendors = new ObservableCollection<VendorModel>(vendors.Item1);
			}
			else
			{
				//Error raised
				MessageBox.Show(vendors.Item2, "Error retrieving vendors.", MessageBoxButton.OK, MessageBoxImage.Error);
				// TODO - Add return to Purchase Order Switchboard
			}
        }

		private void GetOrderStatuses()
		{
			Tuple < IEnumerable < OrderStatusModel>, string > orderStatuses = new OrderStatusRepository(ConnectionString.GetConnectionString()).GetAll().ToTuple();
			//Check for errors
			if (orderStatuses.Item2 == null)
			{
				//No errors
				OrderStatuses = new ObservableCollection<OrderStatusModel>(orderStatuses.Item1);
			}
			else
			{
                //Error raised
                MessageBox.Show(orderStatuses.Item2, "Error retrieving order statuses.", MessageBoxButton.OK, MessageBoxImage.Error);
                // TODO - Add return to Purchase Order Switchboard
            }
        }

		private void GetVendorProducts(int id)
		{
			try
			{
                Products = new ObservableCollection<ProductModel>(new ProductsManager(ConnectionString.GetConnectionString()).GetByVendorID(id));
            }
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "Vendors Error", MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}
			
		}

    }
}

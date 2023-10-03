using BussinessLogicLibrary.Purchases;
using DataAccessLibrary.OrderStatusRepository;
using DataAccessLibrary.VendorRepository;
using ModelsLibrary;
using RetailAppUI.Commands;
using RetailAppUI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace RetailAppUI.ViewModels.Purchases
{
    public class PurchaseOrdersSwitchboardViewModel : BaseViewModel
    {
        private PurchaseOrdersListManager _ordersListManager;

        private INavigationService _navigation;
        public INavigationService Navigation
        {
            get { return _navigation; }
            set { _navigation = value; OnPropertyChanged(); }
        }

        private IConnectionStringService _connectionString;
        public IConnectionStringService ConnectionString
        {
            get { return _connectionString; }
            set { _connectionString = value; }
        }

        private IEnumerable<PurchaseOrderHeaderModel> _purchaseOrders;
        public IEnumerable<PurchaseOrderHeaderModel> PurchaseOrders
        {
            get { return _purchaseOrders; }
            set { _purchaseOrders = value; OnPropertyChanged(); }
        }

        private PurchaseOrderHeaderModel _selectedPurchaseOrder;
        public PurchaseOrderHeaderModel SelectedPurchaseOrder
        {
            get { return _selectedPurchaseOrder; }
            set { _selectedPurchaseOrder = value; OnPropertyChanged(); }
        }

        private IEnumerable<OrderStatusModel> _orderStatuses;
        public IEnumerable<OrderStatusModel> OrderStatuses
        {
            get { return _orderStatuses; }
            set { _orderStatuses = value; OnPropertyChanged(); }
        }

        private IEnumerable<VendorModel> _vendors;
        public IEnumerable<VendorModel> Vendors
        {
            get { return _vendors; }
            set { _vendors = value; OnPropertyChanged(); }
        }


        //commands
        public RelayCommand NavigateToAddNewPurchaseOrdertViewCommand { get; set; }
        public RelayCommand GetAllPurchaseOrdersCommand { get; set; }
        public RelayCommand GetPurchaseOrderByOrderStatusCommand { get; set; }
        public RelayCommand GetPurchaseOrderByVendorCommand { get; set; }


        public PurchaseOrdersSwitchboardViewModel(INavigationService navigation, IConnectionStringService connectionString)
        {
            Navigation = navigation;
            ConnectionString = connectionString;

            //Instantiate orders list manager
            _ordersListManager = new PurchaseOrdersListManager(ConnectionString.GetConnectionString());

            //Retrieve Order status list
            GetOrderStatuses();

            //Retrieve Vendor list
            GetVendors();

            //Instantiate commands
            NavigateToAddNewPurchaseOrdertViewCommand = new RelayCommand(NavigateToAddNewPurchaseOrdertView, CanNavigateToAddNewPurchaseOrdertView);
            GetAllPurchaseOrdersCommand = new RelayCommand(GetAllPurchaseOrders, CanGetAllPurchaseOrders);
            GetPurchaseOrderByOrderStatusCommand = new RelayCommand(GetPurchaseOrderByOrderStatus, CanGetPurchaseOrderByOrderStatus);
            GetPurchaseOrderByVendorCommand = new RelayCommand(GetPurchaseOrderByVendor, CanGetPurchaseOrderByVendor);

        }

        private void GetVendors()
        {
            Tuple<IEnumerable<VendorModel>, string> vendors = new VendorRepository(ConnectionString.GetConnectionString()).GetAll().ToTuple();
            //Check for errors
            if (vendors.Item2 == null)
            {
                //No errors
                Vendors = vendors.Item1.OrderBy(x => x.CompanyName);
            }
            else
            {
                //Error retrieving
                MessageBox.Show(vendors.Item2, "Error Retrieving Vendors", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void GetOrderStatuses()
        {
            Tuple<IEnumerable<OrderStatusModel>, string> orderStatuses = new OrderStatusRepository(ConnectionString.GetConnectionString()).GetAll().ToTuple();
            //Check for errors
            if (orderStatuses.Item2 == null)
            {
                //No errors
                OrderStatuses = orderStatuses.Item1.OrderBy(x => x.OrderStatusID);
            }
            else
            {
                //Error retrieving
                MessageBox.Show(orderStatuses.Item2, "Error Retrieving Order Statuses", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }
        private bool CanGetPurchaseOrderByVendor(object obj)
        {
            return true;
        }

        private void GetPurchaseOrderByVendor(object obj)
        {
            if (obj == null)
            {
                MessageBox.Show("Please select a vendor.", "Vendor not Selected", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }
            else
            {
                int id = (int)obj;
                try
                {
                    _ordersListManager.GetByVendorID(id);
                    PurchaseOrders = _ordersListManager.PurchaseOrders;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error Retrieving Purchase Orders", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }
        }

        private bool CanGetPurchaseOrderByOrderStatus(object obj)
        {
            return true;
        }

        private void GetPurchaseOrderByOrderStatus(object obj)
        {
            if (obj == null)
            {
                MessageBox.Show("Please select an order status.", "Order Status not Selected", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }
            else
            {
                int id = (int)obj;
                try
                {
                    _ordersListManager.GetByOrderStatusID(id);
                    PurchaseOrders = _ordersListManager.PurchaseOrders;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error Retrieving Purchase Orders", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }
        }

        private bool CanGetAllPurchaseOrders(object obj)
        {
            return true;
        }

        private void GetAllPurchaseOrders(object obj)
        {
            try
            {
                _ordersListManager.GetAll();
                PurchaseOrders = _ordersListManager.PurchaseOrders;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error Retrieving Purchase Orders", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private bool CanNavigateToAddNewPurchaseOrdertView(object obj)
        {
            return true;
        }

        private void NavigateToAddNewPurchaseOrdertView(object obj)
        {
            Navigation.NavigateTo<AddNewPurchaseOrderViewModel>();
        }
    }
}

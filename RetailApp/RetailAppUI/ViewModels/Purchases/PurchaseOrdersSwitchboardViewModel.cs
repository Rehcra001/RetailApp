using BussinessLogicLibrary.Purchases;
using BussinessLogicLibrary.Statuses;
using BussinessLogicLibrary.Vendors;
using DataAccessLibrary.StatusRepository;
using DataAccessLibrary.VendorRepository;
using ModelsLibrary;
using ModelsLibrary.RepositoryInterfaces;
using RetailAppUI.Commands;
using RetailAppUI.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Data;

namespace RetailAppUI.ViewModels.Purchases
{
    public class PurchaseOrdersSwitchboardViewModel : BaseViewModel
    {
        private readonly PurchaseOrdersListManager _ordersListManager;
        private readonly IVendorManager _vendorManager;
        private readonly IStatusManager _statusManager;
        private string _groupByState = string.Empty;

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

        private ICurrentViewService _currentView;
        public ICurrentViewService CurrentView
        {
            get { return _currentView; }
            set { _currentView = value; }
        }

        private ISharedDataService _sharedData;
        public ISharedDataService SharedData
        {
            get { return _sharedData; }
            set { _sharedData = value; }
        }



        private ObservableCollection<PurchaseOrderHeaderModel> _purchaseOrders;
        public ObservableCollection<PurchaseOrderHeaderModel> PurchaseOrders
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

        private IEnumerable<StatusModel> _orderStatuses;
        public IEnumerable<StatusModel> OrderStatuses
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

        private ICollectionView _PurchaseOrderCollectionView;
        public ICollectionView PurchaseOrderCollectionView
        {
            get { return _PurchaseOrderCollectionView; }
            set { _PurchaseOrderCollectionView = value; }
        }

        private string _purchaseOrderIDFilter = string.Empty;     
        public string PurchaseOrderIDFilter
        {
            get { return _purchaseOrderIDFilter; }
            set { _purchaseOrderIDFilter = value; PurchaseOrderCollectionView.Refresh(); OnPropertyChanged(); }
        }


        //commands
        public RelayCommand NavigateToAddNewPurchaseOrdertViewCommand { get; set; }
        public RelayCommand GetAllPurchaseOrdersCommand { get; set; }
        public RelayCommand GetPurchaseOrderByOrderStatusCommand { get; set; }
        public RelayCommand GetPurchaseOrderByVendorCommand { get; set; }
        public RelayCommand CloseViewCommand { get; set; }
        public RelayCommand GroupByOrderStatusCommand { get; set; }
        public RelayCommand GroupByVendorCommand { get; set; }
        public RelayCommand ClearGroupByCommand { get; set; }
        public RelayCommand NavigateToPurchaseOrderViewCommand { get; set; }


        public PurchaseOrdersSwitchboardViewModel(INavigationService navigation,
                                                  IConnectionStringService connectionString,
                                                  ICurrentViewService currentView,
                                                  ISharedDataService sharedData,
                                                  IVendorManager vendorManager,
                                                  IStatusManager statusManager)
        {
            Navigation = navigation;
            ConnectionString = connectionString;
            CurrentView = currentView;
            SharedData = sharedData;
            _vendorManager = vendorManager;
            _statusManager = statusManager;

            //Instantiate orders list manager
            _ordersListManager = new PurchaseOrdersListManager(ConnectionString.GetConnectionString(), _vendorManager, _statusManager);
            //Instantiate PurchaseOrders
            PurchaseOrders = new ObservableCollection<PurchaseOrderHeaderModel>();
            //Add to collection view
            PurchaseOrderCollectionView = CollectionViewSource.GetDefaultView(PurchaseOrders);
            //Add filter on Purchase order ID
            PurchaseOrderCollectionView.Filter = FilterPurchaseOrderID;

            //Retrieve Order status list
            GetOrderStatuses();

            //Retrieve Vendor list
            GetVendors();

            //Instantiate commands
            NavigateToAddNewPurchaseOrdertViewCommand = new RelayCommand(NavigateToAddNewPurchaseOrdertView, CanNavigateToAddNewPurchaseOrdertView);
            NavigateToPurchaseOrderViewCommand = new RelayCommand(NavigateToPurchaseOrderView, CanNavigateToPurchaseOrderView);
            GetAllPurchaseOrdersCommand = new RelayCommand(GetAllPurchaseOrders, CanGetAllPurchaseOrders);
            GetPurchaseOrderByOrderStatusCommand = new RelayCommand(GetPurchaseOrderByOrderStatus, CanGetPurchaseOrderByOrderStatus);
            GetPurchaseOrderByVendorCommand = new RelayCommand(GetPurchaseOrderByVendor, CanGetPurchaseOrderByVendor);
            CloseViewCommand = new RelayCommand(CloseView, CanCloseView);
            GroupByOrderStatusCommand = new RelayCommand(GroupByOrderStatus, CanGroupByOrderStatus);
            GroupByVendorCommand = new RelayCommand(GroupByVendor, CanGroupByVendor);
            ClearGroupByCommand = new RelayCommand(ClearGroupBy, CanClearGroupBy);
        }

        private bool CanNavigateToPurchaseOrderView(object obj)
        {
            return SelectedPurchaseOrder != null;
        }

        private void NavigateToPurchaseOrderView(object obj)
        {
            //Purchase order id to be passed to PurchaseOrderView
            SharedData.SharedData = SelectedPurchaseOrder.PurchaseOrderID;
            Navigation.NavigateTo<PurchaseOrderViewModel>();
        }

        private bool CanClearGroupBy(object obj)
        {
            return !_groupByState.Equals("Clear") && PurchaseOrders.Count > 0;
        }

        private void ClearGroupBy(object obj)
        {
            SetGroupByState("Clear");
        }

        private bool CanGroupByVendor(object obj)
        {
            return !_groupByState.Equals("Vendor") && PurchaseOrders.Count > 0;
        }

        private void GroupByVendor(object obj)
        {
            SetGroupByState("Vendor");
        }

        private bool CanGroupByOrderStatus(object obj)
        {
            return !_groupByState.Equals("OrderStatus") && PurchaseOrders.Count > 0;
        }

        private void GroupByOrderStatus(object obj)
        {
            SetGroupByState("OrderStatus");
        }

        private void SetGroupByState(string state)
        {
            _groupByState = state;

            //Three states - Clear, OrderStatus, Vendor
            //Clear - clear all groupings
            //OrderStatus - group by order status
            //Vendor - group by vendor

            //Remove and groups added
            if (PurchaseOrderCollectionView.GroupDescriptions.Count > 0)
            {
                PurchaseOrderCollectionView.GroupDescriptions.Clear();
            }
            switch (_groupByState)
            {
                case "OrderStatus":
                    PurchaseOrderCollectionView.GroupDescriptions.Add(new PropertyGroupDescription("OrderStatus.Status"));
                    break;
                case "Vendor":
                    PurchaseOrderCollectionView.GroupDescriptions.Add(new PropertyGroupDescription("Vendor.CompanyName"));
                    break;
            }
        }

        private bool FilterPurchaseOrderID(object obj)
        {
            if (obj is PurchaseOrderHeaderModel purchaseOrder)
            {
                return purchaseOrder.PurchaseOrderID.ToString().Contains(PurchaseOrderIDFilter, StringComparison.InvariantCultureIgnoreCase);
            }
            return false;
        }

        private void GetVendors()
        {
            try
            {
                Vendors = _vendorManager.GetAll().OrderBy(x => x.CompanyName);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void GetOrderStatuses()
        {
            try
            {
                OrderStatuses = _statusManager.GetAll().OrderBy(x => x.StatusID);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
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
                    SetGroupByState("OrderStatus");
                    _ordersListManager.GetByVendorID(id);
                    GetOrders();
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
                    SetGroupByState("Vendor");
                    _ordersListManager.GetByOrderStatusID(id);
                    GetOrders();
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
                SetGroupByState("OrderStatus");
                _ordersListManager.GetAll();
                GetOrders();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error Retrieving Purchase Orders", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void GetOrders()
        {
            PurchaseOrders.Clear();
            foreach (PurchaseOrderHeaderModel order in _ordersListManager.PurchaseOrders)
            {
                PurchaseOrders.Add(order);
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

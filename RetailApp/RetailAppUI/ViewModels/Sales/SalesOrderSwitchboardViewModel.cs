using BussinessLogicLibrary.Customers;
using BussinessLogicLibrary.Sales;
using BussinessLogicLibrary.Statuses;
using ModelsLibrary;
using RetailAppUI.Commands;
using RetailAppUI.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Data;

namespace RetailAppUI.ViewModels.Sales
{
    public class SalesOrderSwitchboardViewModel : BaseViewModel
    {
        private ISalesManager _salesManager;
        private IStatusManager _statusManager;
        private ICustomerManager _customerManager;

        //Collection view
        public ICollectionView SalesOrderCollectionView { get; set; }

        private string _salesOrderIDFilter = string.Empty;
        public string SalesOrderIDFilter
        {
            get { return _salesOrderIDFilter; }
            set { _salesOrderIDFilter = value; SalesOrderCollectionView.Refresh(); OnPropertyChanged(); }
        }


        //Services
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

        //Data Properties
        private SalesOrderHeaderModel _selectSalesOrder;
        public SalesOrderHeaderModel SelectedSalesOrder
        {
            get { return _selectSalesOrder; }
            set { _selectSalesOrder = value; OnPropertyChanged(); }
        }

        private ObservableCollection<SalesOrderHeaderModel> _salesOrders;
        public ObservableCollection<SalesOrderHeaderModel> SalesOrders
        {
            get { return _salesOrders; }
            set { _salesOrders = value; OnPropertyChanged(); }
        }

        private IEnumerable<StatusModel> _statuses;
        public IEnumerable<StatusModel> OrderStatuses
        {
            get { return _statuses; }
            set { _statuses = value; OnPropertyChanged(); }
        }

        private IEnumerable<CustomerModel> _customers;

        public IEnumerable<CustomerModel> Customers
        {
            get { return _customers; }
            set { _customers = value; OnPropertyChanged(); }
        }




        //Commands
        public RelayCommand NavigateToAddNewSalesOrdertViewCommand { get; set; }
        public RelayCommand CloseViewCommand { get; set; }
        public RelayCommand GetAllSalesOrdersCommand { get; set; }
        public RelayCommand GetSalesOrderByOrderStatusCommand { get; set; }
        public RelayCommand GetSalesOrderByCustomerCommand { get; set; }

        public SalesOrderSwitchboardViewModel(INavigationService navigation,
                                              ICurrentViewService currentView,
                                              ISalesManager salesManager,
                                              IStatusManager statusManager,
                                              ICustomerManager customerManager)
        {
            Navigation = navigation;
            CurrentView = currentView;
            _salesManager = salesManager;
            _statusManager = statusManager;
            _customerManager = customerManager;

            //Instantiate SalesOrders
            SalesOrders = new ObservableCollection<SalesOrderHeaderModel>();
            //add to collection view
            SalesOrderCollectionView = CollectionViewSource.GetDefaultView(SalesOrders);
            //create a filter on Sales Order ID
            SalesOrderCollectionView.Filter += FilterBySalesOrderID;

            GetOrderStatuses();
            GetCustomers();

            //Instantiate commands
            NavigateToAddNewSalesOrdertViewCommand = new RelayCommand(NavigateToAddNewSalesOrdertView, CanNavigateToAddNewSalesOrdertView);
            CloseViewCommand = new RelayCommand(CloseView, CanCloseView);
            GetAllSalesOrdersCommand = new RelayCommand(GetAllSalesOrders, CanGetAllSalesOrders);
            GetSalesOrderByOrderStatusCommand = new RelayCommand(GetSalesOrderByOrderStatus, CanGetSalesOrderByOrderStatus);
            GetSalesOrderByCustomerCommand = new RelayCommand(GetSalesOrderByCustomer, CanGetSalesOrderByCustomer);
        }

        private bool CanGetSalesOrderByCustomer(object obj)
        {
            return true;
        }

        private void GetSalesOrderByCustomer(object obj)
        {
            if (obj == null)
            {
                //Nothing selected
                MessageBox.Show("Please select a customer from the list",
                                "Select Customer",
                                MessageBoxButton.OK,
                                MessageBoxImage.Exclamation);
                return;
            }
            else
            {
                try
                {
                    SalesOrders.Clear();
                    foreach (SalesOrderHeaderModel order in _salesManager.GetByCustomerID((int)obj))
                    {
                        SalesOrders.Add(order);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Unable to retrieve the list of sales orders.\r\n\r\n" + ex.Message, "Retrieval Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }
        }

        private bool CanGetSalesOrderByOrderStatus(object obj)
        {
            return true;
        }

        private void GetSalesOrderByOrderStatus(object obj)
        {
            if (obj == null)
            {
                //Nothing selected
                MessageBox.Show("Please select an order status from the list",
                                "Select Order Status",
                                MessageBoxButton.OK,
                                MessageBoxImage.Exclamation);
                return;
            }
            else
            {
                try
                {
                    SalesOrders.Clear();
                    foreach (SalesOrderHeaderModel order in _salesManager.GetByOrderStatusID((int)obj))
                    {
                        SalesOrders.Add(order);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Unable to retrieve the list of sales orders.\r\n\r\n" + ex.Message, "Retrieval Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }
        }

        private void GetCustomers()
        {
            try
            {
                Customers = _customerManager.GetAll();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to retrieve the list of customers.\r\n\r\n" + ex.Message, "Retrieval Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void GetOrderStatuses()
        {
            try
            {
                OrderStatuses = _statusManager.GetAll();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to retrieve the list of order statuses.\r\n\r\n" + ex.Message, "Retrieval Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private bool FilterBySalesOrderID(object obj)
        {
            if (obj is SalesOrderHeaderModel salesOrder)
            {
                return salesOrder.SalesOrderID.ToString().Contains(SalesOrderIDFilter, StringComparison.InvariantCultureIgnoreCase);
            }
            return false;
        }

        private bool CanGetAllSalesOrders(object obj)
        {
            return true;
        }

        private void GetAllSalesOrders(object obj)
        {
            try
            {
                SalesOrders.Clear();
                foreach (SalesOrderHeaderModel order in _salesManager.GetAllWithoutSalesOrderDetails())
                {
                    SalesOrders.Add(order);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to retrieve the list of sales orders.\r\n\r\n" + ex.Message, "Retrieval Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private bool CanCloseView(object obj)
        {
            return true;
        }

        private void CloseView(object obj)
        {
            CurrentView.CurrentView = "Homeview";
            Navigation.NavigateTo<HomeViewModel>();
        }

        private bool CanNavigateToAddNewSalesOrdertView(object obj)
        {
            return true;
        }

        private void NavigateToAddNewSalesOrdertView(object obj)
        {
            Navigation.NavigateTo<AddNewSalesOrderViewModel>();
        }
    }
}

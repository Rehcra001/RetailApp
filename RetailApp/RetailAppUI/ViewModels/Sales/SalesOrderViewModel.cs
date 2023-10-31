using BussinessLogicLibrary.Products;
using BussinessLogicLibrary.Sales;
using BussinessLogicLibrary.Statuses;
using ModelsLibrary;
using RetailAppUI.Commands;
using RetailAppUI.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Data;

namespace RetailAppUI.ViewModels.Sales
{
    public class SalesOrderViewModel : BaseViewModel
    {
        private readonly ISalesManager _salesManager;
        private readonly IStatusManager _statusManager;
        private readonly IProductsManager _productsManager;
        private string _state;


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

        private SalesOrderHeaderModel _salesOrder;
        public SalesOrderHeaderModel SalesOrder
        {
            get { return _salesOrder; }
            set { _salesOrder = value; OnPropertyChanged(); }
        }

        private StatusModel _selectedOrderStatus;
        public StatusModel SelectedOrderStatus
        {
            get { return _selectedOrderStatus; }
            set
            {
                _selectedOrderStatus = value;
                SalesOrder.OrderStatus = _selectedOrderStatus;
                OnPropertyChanged();
            }
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



        private int _selectedOrderLineIndex;
        public int SelectedOrderLineIndex
        {
            get { return _selectedOrderLineIndex; }
            set { _selectedOrderLineIndex = value; OnPropertyChanged(); }
        }


        private ObservableCollection<StatusModel> _orderLineStatuses;
        public ObservableCollection<StatusModel> OrderLineStatuses
        {
            get { return _orderLineStatuses; }
            set { _orderLineStatuses = value; OnPropertyChanged(); }
        }

        private bool _dateEnabled;

        public bool DateEnabled
        {
            get { return _dateEnabled; }
            set { _dateEnabled = value; OnPropertyChanged(); }
        }
        private bool _textReadOnly;

        public bool TextReadOnly
        {
            get { return _textReadOnly; }
            set { _textReadOnly = value; OnPropertyChanged(); }
        }
        private bool _orderStatusEnabled;

        public bool OrderStatusEnabled
        {
            get { return _orderStatusEnabled; }
            set { _orderStatusEnabled = value; OnPropertyChanged(); }
        }


        public ICollectionView SalesOrderLines { get; set; }

        //Commands
        public RelayCommand CloseViewCommand { get; set; }
        public RelayCommand EditCommand { get; set; }
        public RelayCommand InvoiceCommand { get; set; }
        public RelayCommand ReverseInvoiceCommand { get; set; }
        public RelayCommand SaveCommand { get; set; }
        public RelayCommand CancelActionCommand { get; set; }
        public RelayCommand AddNewLineCommand { get; set; }
        public RelayCommand RemoveLineCommand { get; set; }

        public SalesOrderViewModel(ISalesManager salesManager,
                                   INavigationService navigationService,
                                   ISharedDataService sharedData,
                                   IStatusManager statusManager,
                                   IProductsManager productsManager)
        {
            _salesManager = salesManager;
            Navigation = navigationService;
            SharedData = sharedData;
            _statusManager = statusManager;
            _productsManager = productsManager;

            LoadSalesOrder();

            GetStatuses();

            GetProducts();

            //Instantiate commands
            CloseViewCommand = new RelayCommand(CloseView, CanCloseView);

            SetState("view");
            
        }



        private bool CanCloseView(object obj)
        {
            return true;
        }

        private void CloseView(object obj)
        {
            if (!_state.Equals("View"))
            {
                MessageBoxResult result = MessageBox.Show("Are you sure you want to close this view?\r\nAny changes made will not be saved!",
                                                          "Close View",
                                                          MessageBoxButton.YesNo,
                                                          MessageBoxImage.Question,
                                                          MessageBoxResult.No);

                if (result == MessageBoxResult.Yes)
                {
                    Navigation.NavigateTo<SalesOrderSwitchboardViewModel>();
                }
                else
                {
                    return;
                }
            }
            else
            {
                Navigation.NavigateTo<SalesOrderSwitchboardViewModel>();
            }
            
        }
        private void SetState(string state)
        {
            _state = state;

            switch (_state)
            {
                case "View":
                    DateEnabled = false;
                    TextReadOnly = true;
                    OrderStatusEnabled = false;
                    break;
                case "Edit":
                    DateEnabled = true;
                    TextReadOnly = false;
                    OrderStatusEnabled = true;
                    break;
                case "Invoice":
                    DateEnabled = false;
                    TextReadOnly = true;
                    OrderStatusEnabled = false;
                    break;
            }
        }

        private void LoadSalesOrder()
        {
            try
            {
                long id = (long)SharedData.SharedData;
                SalesOrder = _salesManager.GetByID(id);
                SalesOrderLines = CollectionViewSource.GetDefaultView(SalesOrder.SalesOrderDetails);
    }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to retrieve sales order.\r\n\r\n" + ex.Message,
                                "Retrieval Error",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
                Navigation.NavigateTo<SalesOrderSwitchboardViewModel>();
            }
        }

        private void GetProducts()
        {
            //Retrieve all products not already in the details
            try
            {
                Products = new ObservableCollection<ProductModel>(_productsManager.GetAll().OrderBy(x => x.ProductName));
                foreach(SalesOrderDetailModel orderline in SalesOrder.SalesOrderDetails)
                {
                    Products.Remove(Products.First(x => x.ProductID == orderline.ProductID));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to retrieve the products.|r\n\r\n" + ex.Message,
                                "Retrieval Error",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
                Navigation.NavigateTo<SalesOrderSwitchboardViewModel>();
            }
        }
        private void GetStatuses()
        {
            try
            {
                IEnumerable<StatusModel> statuses = _statusManager.GetAll();
                OrderStatuses = new ObservableCollection<StatusModel>(statuses);
                OrderLineStatuses = new ObservableCollection<StatusModel>(statuses);

                //Set the selectOrderStatus to the current order status
                SelectedOrderStatus = OrderStatuses.First(x => x.StatusID == SalesOrder.OrderStatusID);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}

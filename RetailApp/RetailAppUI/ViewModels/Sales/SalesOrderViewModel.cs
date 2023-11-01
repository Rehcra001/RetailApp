using BussinessLogicLibrary.Products;
using BussinessLogicLibrary.Sales;
using BussinessLogicLibrary.Statuses;
using ModelsLibrary;
using RetailAppUI.Commands;
using RetailAppUI.Services;
using RetailAppUI.ViewModels.Purchases;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Windows;
using System.Windows.Data;

namespace RetailAppUI.ViewModels.Sales
{
    public class SalesOrderViewModel : BaseViewModel
    {
        private readonly ISalesManager _salesManager;
        private readonly IStatusManager _statusManager;
        private readonly IProductsManager _productsManager;
        private readonly IGetSalesOrderDetailsByIDManager _getSalesOrderDetailsByIDManager;
        private string _state;
        private const int OPEN = 1;


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
            set { _selectedOrderStatus = value; SalesOrder.OrderStatus = _selectedOrderStatus; OnPropertyChanged(); }
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



        private int _selectedOrderLineIndex = -1;
        public int SelectedOrderLineIndex
        {
            get { return _selectedOrderLineIndex; }
            set { _selectedOrderLineIndex = value; ProductEnabled(); OrderLineStatusEnabled(); OnPropertyChanged(); }
        }

        

        private ObservableCollection<StatusModel> _orderLineStatuses;
        public ObservableCollection<StatusModel> OrderLineStatuses
        {
            get { return _orderLineStatuses; }
            set { _orderLineStatuses = value; OnPropertyChanged(); }
        }

        public IEnumerable<SalesOrderDetailModel> OriginalOrderLines { get; set; }

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

        private bool _canChangeProduct;
        public bool CanChangeProduct
        {
            get { return _canChangeProduct; }
            set { _canChangeProduct = value; OnPropertyChanged(); }
        }

        private bool _canChangeOrderLineStatus;
        public bool CanChangeOrderLineStatus
        {
            get { return _canChangeOrderLineStatus; }
            set { _canChangeOrderLineStatus = value; OnPropertyChanged(); }
        }


        private ICollectionView _salesOrderLines;
        public ICollectionView SalesOrderLines
        {
            get { return _salesOrderLines; }
            set { _salesOrderLines = value; OnPropertyChanged(); }
        }


        //Commands
        public RelayCommand CloseViewCommand { get; set; }
        public RelayCommand EditCommand { get; set; }
        public RelayCommand InvoiceCommand { get; set; }
        public RelayCommand ReverseInvoiceCommand { get; set; }
        public RelayCommand SaveActionCommand { get; set; }
        public RelayCommand CancelActionCommand { get; set; }
        public RelayCommand AddNewLineCommand { get; set; }
        public RelayCommand RemoveLineCommand { get; set; }

        public SalesOrderViewModel(ISalesManager salesManager,
                                   INavigationService navigationService,
                                   ISharedDataService sharedData,
                                   IStatusManager statusManager,
                                   IProductsManager productsManager,
                                   IGetSalesOrderDetailsByIDManager getSalesOrderDetailsByIDManager)
        {
            _salesManager = salesManager;
            Navigation = navigationService;
            SharedData = sharedData;
            _statusManager = statusManager;
            _productsManager = productsManager;
            _getSalesOrderDetailsByIDManager = getSalesOrderDetailsByIDManager;

            LoadSalesOrder();

            GetStatuses();

            GetProducts();

            //Instantiate commands
            CloseViewCommand = new RelayCommand(CloseView, CanCloseView);
            AddNewLineCommand = new RelayCommand(AddNewLine, CanAddNewLine);
            RemoveLineCommand = new RelayCommand(RemoveLine, CanRemoveLine);
            EditCommand = new RelayCommand(EditOrder, canEditOrder);
            SaveActionCommand = new RelayCommand(SaveAction, CanSaveAction);
            CancelActionCommand = new RelayCommand(CancelAction, CanCancelAction);
            InvoiceCommand = new RelayCommand(Invoice, CanInvoice);
            ReverseInvoiceCommand = new RelayCommand(ReverseInvoice, CanReverseInvoice);

            SetState("View");

        }
        private void OrderLineStatusEnabled()
        {
            if (SelectedOrderLineIndex > OriginalOrderLines.Count() - 1)
            {
                CanChangeOrderLineStatus = false;
            }
            else
            {
                CanChangeOrderLineStatus = true;
            }
        }

        private void ProductEnabled()
        {
            if (SelectedOrderLineIndex > OriginalOrderLines.Count() - 1)
            {
                CanChangeProduct = true;
            }
            else
            {
                CanChangeProduct = false;
            }
        }

        private bool CanReverseInvoice(object obj)
        {
            return false;
        }

        private void ReverseInvoice(object obj)
        {
            throw new NotImplementedException();
        }

        private bool CanInvoice(object obj)
        {
            return false;
        }

        private void Invoice(object obj)
        {
            throw new NotImplementedException();
        }

        private bool CanCancelAction(object obj)
        {
            return !_state.Equals("View");
        }

        private void CancelAction(object obj)
        {
            throw new NotImplementedException();
        }

        private bool CanSaveAction(object obj)
        {
            return !_state.Equals("View");
        }

        private void SaveAction(object obj)
        {
            if (_state.Equals("Edit"))
            {
                try
                {
                    _salesManager.Update(SalesOrder);
                    //Get the update sales order from database

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Unable to save the sales order.\r\n\r\n" + ex.Message, "Save Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }
            

            try
            {
                //Re-loaded the sales order view
                //Make sure the shared data has the sales order ID
                SharedData.SharedData = SalesOrder.SalesOrderID;
                Navigation.NavigateTo<SalesOrderViewModel>();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\r\n Reloading Sales Order from database.", "Save Changes", MessageBoxButton.OK, MessageBoxImage.Error);
                Navigation.NavigateTo<SalesOrderSwitchboardViewModel>();
            }
        }

        private bool canEditOrder(object obj)
        {
            return _state.Equals("View");
        }

        private void EditOrder(object obj)
        {
            SetState("Edit");
        }

        private bool CanRemoveLine(object obj)
        {
            return !_state.Equals("View") && SelectedOrderLineIndex > OriginalOrderLines.Count() - 1;
        }

        private void RemoveLine(object obj)
        {
            MessageBoxResult result = MessageBox.Show($"Are you sure you want to remove line {SelectedOrderLineIndex + 1}?",
                                                      "Remove Line",
                                                      MessageBoxButton.YesNo,
                                                      MessageBoxImage.Question,
                                                      MessageBoxResult.No);
            if (result == MessageBoxResult.Yes)
            {
                SalesOrder.SalesOrderDetails.RemoveAt(SelectedOrderLineIndex);
                SalesOrderLines.Refresh();
            }
        }

        private bool CanAddNewLine(object obj)
        {
            //Find the last line
            bool canAdd = false;
            int index = SalesOrder.SalesOrderDetails.Count - 1;
            if (SalesOrder.SalesOrderDetails[index].ProductID > 0 &&
                SalesOrder.SalesOrderDetails[index].QuantityOrdered > 0 &&
                SalesOrder.SalesOrderDetails[index].OrderLineStatusID > 0)
            {
                canAdd = true;
            }

            return !_state.Equals("View") && Products.Count > 0 && canAdd;
        }

        private void AddNewLine(object obj)
        {
            SalesOrder.SalesOrderDetails.Add(new SalesOrderDetailModel());
            int index = SalesOrder.SalesOrderDetails.Count - 1;
            SalesOrder.SalesOrderDetails[index].OrderLineStatusID = OPEN;
            SalesOrder.SalesOrderDetails[index].OrderLineStatus = OrderLineStatuses.First(x => x.StatusID == OPEN);

            SalesOrderLines.Refresh();
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
                OriginalOrderLines = _getSalesOrderDetailsByIDManager.GetByID(SalesOrder.SalesOrderID);
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
                foreach (SalesOrderDetailModel orderline in SalesOrder.SalesOrderDetails)
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

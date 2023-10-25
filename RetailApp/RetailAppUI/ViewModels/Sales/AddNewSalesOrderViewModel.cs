using BussinessLogicLibrary.Customers;
using BussinessLogicLibrary.Products;
using BussinessLogicLibrary.Sales;
using BussinessLogicLibrary.Statuses;
using ModelsLibrary;
using RetailAppUI.Commands;
using RetailAppUI.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Data;

namespace RetailAppUI.ViewModels.Sales
{
    public class AddNewSalesOrderViewModel : BaseViewModel
    {
        private readonly ISalesManager _salesManager;
        private readonly ICustomerManager _customerManager;
        private readonly IProductsManager _productsManager;
        private readonly IStatusManager _statusManager;

        private string _state;
        private const int OPEN_STATUS = 1;

        private INavigationService _navigation;
        public INavigationService Navigation
        {
            get { return _navigation; }
            set { _navigation = value; OnPropertyChanged(); }
        }

        private SalesOrderHeaderModel _salesOrder;
        public SalesOrderHeaderModel SalesOrder
        {
            get { return _salesOrder; }
            set { _salesOrder = value; OnPropertyChanged(); }
        }

        private ICollectionView _salesOrderLines;
        public ICollectionView SalesOrderLines
        {
            get { return _salesOrderLines; }
            set { _salesOrderLines = value; OnPropertyChanged(); }
        }

        private int _selectedOrderLineIndex;

        public int SelectedOrderLineIndex
        {
            get { return _selectedOrderLineIndex; }
            //Change the ProductID Property to the productID of the selected sales Order Line
            set { _selectedOrderLineIndex = value; 
                  if (value > -1)
                {
                    ProductID = SalesOrder.SalesOrderDetails[value].Product.ProductID;
                }
                  OnPropertyChanged(); }
        }
        

        private ObservableCollection<CustomerModel> _customers;
        public ObservableCollection<CustomerModel> Customers
        {
            get { return _customers; }
            set { _customers = value; OnPropertyChanged(); }
        }

        private int _productID;
        public int ProductID
        {
            get { return _productID; }
            set { _productID = value;  OnPropertyChanged(); UpdateProductList(); }//Update the Products list on change UpdateProduct(value);
        }


        private IEnumerable<ProductModel> _productsAll;
        public IEnumerable<ProductModel> ProductsAll
        {
            get { return _productsAll; }
            set { _productsAll = value; }
        }


        private ObservableCollection<ProductModel> _products;
        public ObservableCollection<ProductModel> Products
        {
            get { return _products; }
            set { _products = value; OnPropertyChanged(); }
        }

        private ObservableCollection<StatusModel> _statuses;
        public ObservableCollection<StatusModel> Statuses
        {
            get { return _statuses; }
            set { _statuses = value; OnPropertyChanged(); }
        }

        private int _statusIndex;

        public int StatusIndex
        {
            get { return _statusIndex; }
            set { _statusIndex = value; OnPropertyChanged(); }
        }

        //properties to enable or disable
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

        private bool _customerEnabled;

        public bool CustomerEnabled
        {
            get { return _customerEnabled; }
            set { _customerEnabled = value; OnPropertyChanged(); }
        }

        private bool _orderStatusEnabled;

        public bool OrderStatusEnabled
        {
            get { return _orderStatusEnabled; }
            set { _orderStatusEnabled = value; OnPropertyChanged(); }
        }

        //Commands
        public RelayCommand CloseViewCommand { get; set; }
        public RelayCommand AddNewLineCommand { get; set; }
        public RelayCommand RemoveLineCommand { get; set; }
        public RelayCommand SaveCommand { get; set; }


        public AddNewSalesOrderViewModel(ISalesManager salesManager,
                                         ICustomerManager customerManager,
                                         IProductsManager productsManager,
                                         IStatusManager statusManager,
                                         INavigationService navigation)
        {
            _salesManager = salesManager;
            _customerManager = customerManager;
            _productsManager = productsManager;
            _statusManager = statusManager;
            Navigation = navigation;

            

            AddNewOrder();
            InitializeSalesOrderDetails();
            GetCustomers();
            GetProducts();
            GetStatus();

            //Instantiate commands
            AddNewLineCommand = new RelayCommand(AddNewLine, CanAddNewLine);
            RemoveLineCommand = new RelayCommand(RemoveLine, CanRemoveLine);
            CloseViewCommand = new RelayCommand(CloseView, CanCloseView);
            SaveCommand = new RelayCommand(SaveSalesOrder, CanSaveSalesOrder);

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
                    CustomerEnabled = false;
                    OrderStatusEnabled = false;
                    break;
                case "Add":
                    TextReadOnly = false;
                    DateEnabled = true;
                    OrderStatusEnabled = true;
                    CustomerEnabled = true;
                    break;
            }
        }

        private void InitializeSalesOrderDetails()
        {            
            SalesOrderLines = CollectionViewSource.GetDefaultView(SalesOrder.SalesOrderDetails);
            SelectedOrderLineIndex = -1; //nothing selected
        }

        private void AddNewOrder()
        {
            SalesOrder = new SalesOrderHeaderModel();
            SalesOrder.OrderStatusID = OPEN_STATUS;
            SalesOrder.OrderDate = DateTime.Now;
        }

        private void GetStatus()
        {
            try
            {
                Statuses = new ObservableCollection<StatusModel>
                {
                    _statusManager.GetByID(OPEN_STATUS)
                };
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to retrieve Status.\r\n\r\n" + ex.Message,
                                "Retrieval Error",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
                //Go back to the previous screen
                Navigation.NavigateTo<SalesOrderSwitchboardViewModel>();
            }
        }

        private void GetProducts()
        {
            try
            {
                Products = new ObservableCollection<ProductModel>(_productsManager.GetAll());
                ProductsAll = _productsManager.GetAll();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to retrieve Products.\r\n\r\n" + ex.Message,
                                "Retrieval Error",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
                //Go back to the previous screen
                Navigation.NavigateTo<SalesOrderSwitchboardViewModel>();
            }
        }

        private void GetCustomers()
        {
            try
            {
                Customers = new ObservableCollection<CustomerModel>(_customerManager.GetAll());
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to retrieve Customers.\r\n\r\n" + ex.Message,
                                "Retrieval Error",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
                //Go back to the previous screen
                Navigation.NavigateTo<SalesOrderSwitchboardViewModel>();
            }
        }

        private bool CanSaveSalesOrder(object obj)
        {
            return !_state.Equals("View");
        }

        private void SaveSalesOrder(object obj)
        {
            try
            {
                //Add product unit price to the sales order detail
                AddUnitPrice();

                //Reload the saved sales order
                SalesOrder = _salesManager.Insert(SalesOrder);

                //Success message
                MessageBox.Show("Sales Order saved.\r\n\r\n",
                                "Saving Sales Order",
                                MessageBoxButton.OK,
                                MessageBoxImage.Information);
                
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to save the sales Order.\r\n\r\n" + ex.Message,
                                                "Retrieval Error",
                                                MessageBoxButton.OK,
                                                MessageBoxImage.Error);
                return;
            }

            //Retrieve
            SetState("View");

            SelectedOrderLineIndex = -1;
        }

        private bool CanCloseView(object obj)
        {
            return true;
        }

        private void CloseView(object obj)
        {
            Navigation.NavigateTo<SalesOrderSwitchboardViewModel>();
        }

        private bool CanRemoveLine(object obj)
        {
            return SalesOrder.SalesOrderDetails.Count > 0
                   && _state.Equals("Add")
                   && SelectedOrderLineIndex != -1;
        }

        private void RemoveLine(object obj)
        {
            if (SelectedOrderLineIndex == -1)
            {
                MessageBox.Show("Please select a line to remove.",
                                "Remove Line",
                                MessageBoxButton.OK,
                                MessageBoxImage.Exclamation);
                return;
            }
            else
            {
                SalesOrder.SalesOrderDetails.RemoveAt(SelectedOrderLineIndex);
                UpdateProductList();
                SalesOrderLines.Refresh();
                SelectedOrderLineIndex = -1;
            }
        }

        private bool CanAddNewLine(object obj)
        {
                return _state.Equals("Add") && CheckIfCanAddNewLine();
        }

        private bool CheckIfCanAddNewLine()
        {
            bool canAdd = true;

            //Make sure the last line added has all required fields filled in
            if (SalesOrder.SalesOrderDetails.Count > 0) //Lines exist
            {
                int index = SalesOrder.SalesOrderDetails.Count - 1;

                if (SalesOrder.SalesOrderDetails[index].Product == default)
                {
                    canAdd = false; //No product selected
                }

                if (SalesOrder.SalesOrderDetails[index].QuantityOrdered == default)
                {
                    canAdd = false;//No quantity added
                }

                //No products left to add
                if (Products.Count == 0)
                {
                    canAdd = false;
                }
            }

            

            //check if any products left in product
            //each time a product is added to a line
            //it is remove from Products
            if (Products.Count == 0)
            {
                canAdd = false;
            }

            return canAdd;
        }

        private void AddNewLine(object obj)
        {
            if (Products.Count > 0)
            {
                
                SalesOrder.SalesOrderDetails.Add(new SalesOrderDetailModel());
                //Get the newly added sales order detail index
                int index = SalesOrder.SalesOrderDetails.Count - 1;
                SalesOrder.SalesOrderDetails[index].OrderLineStatus = Statuses.FirstOrDefault(x => x.StatusID == OPEN_STATUS)!;
                SalesOrder.SalesOrderDetails[index].OrderLineStatusID = OPEN_STATUS;
                SalesOrderLines.Refresh();
            }
            else
            {
                MessageBox.Show("No products left to add.",
                                "Add New Line",
                                MessageBoxButton.OK,
                                MessageBoxImage.Information);
                return;
            }
        }

        private void AddUnitPrice()
        {
            //Add the unit price to each line
            foreach (SalesOrderDetailModel orderLine in SalesOrder.SalesOrderDetails)
            {
                orderLine.UnitPrice = orderLine.Product.UnitPrice;
            }
        }


        private void UpdateProductList()
        {
            //Update products with to exclude
            //products in sales order lines
            //Remove any product in sod
           
            Products.Clear();
            Products = new ObservableCollection<ProductModel>(ProductsAll);
            foreach (SalesOrderDetailModel SOD in SalesOrder.SalesOrderDetails)
            {
                ProductModel product = SOD.Product;
                Products.Remove(product);
            }

        }
    }
}

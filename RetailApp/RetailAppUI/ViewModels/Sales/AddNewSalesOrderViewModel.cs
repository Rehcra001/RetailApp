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
            set { _salesOrderLines = value; }
        }

        private int _selectedOrderLineIndex;

        public int SelectedOrderLineIndex
        {
            get { return _selectedOrderLineIndex; }
            set { _selectedOrderLineIndex = value; OnPropertyChanged(); }
        }


        private ObservableCollection<CustomerModel> _customers;
        public ObservableCollection<CustomerModel> Customers
        {
            get { return _customers; }
            set { _customers = value; OnPropertyChanged(); }
        }

        private int _selectedProductIdex = -1;

        public int SelectedProductIndex
        {
            get { return _selectedProductIdex; }
            set { _selectedProductIdex = value; RemoveProduct(); OnPropertyChanged(); }
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
        }

        private void InitializeSalesOrderDetails()
        {            
            SalesOrderLines = CollectionViewSource.GetDefaultView(SalesOrder.SalesOrderDetails);
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

                // TODO - Add Navigation to SalesOrderSwitchboardView
            }
        }

        private void GetProducts()
        {
            try
            {
                Products = new ObservableCollection<ProductModel>(_productsManager.GetAll());
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to retrieve Products.\r\n\r\n" + ex.Message,
                                "Retrieval Error",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
                //Go back to the previous screen

                // TODO - Add Navigation to SalesOrderSwitchboardView
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

                // TODO - Add Navigation to SalesOrderSwitchboardView
            }
        }

        private bool CanSaveSalesOrder(object obj)
        {
            return true;
        }

        private void SaveSalesOrder(object obj)
        {
            throw new NotImplementedException();
        }

        private bool CanCloseView(object obj)
        {
            return true;
        }

        private void CloseView(object obj)
        {
            throw new NotImplementedException();
        }

        private bool CanRemoveLine(object obj)
        {
            return true;
        }

        private void RemoveLine(object obj)
        {
            throw new NotImplementedException();
        }

        private bool CanAddNewLine(object obj)
        {
            return Products.Count > 0;
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

        private void AddProduct()
        {

        }

        private void RemoveProduct()
        {
            //Set the unit price of the selected product
            SalesOrder.SalesOrderDetails[SelectedOrderLineIndex].UnitPrice = Products[SelectedProductIndex].UnitPrice;
            SelectedProductIndex = -1;
        }
    }
}

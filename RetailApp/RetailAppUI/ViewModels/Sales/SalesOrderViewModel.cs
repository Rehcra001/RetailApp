using BussinessLogicLibrary.Sales;
using BussinessLogicLibrary.Statuses;
using ModelsLibrary;
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

namespace RetailAppUI.ViewModels.Sales
{
    public class SalesOrderViewModel : BaseViewModel
    {
        private readonly ISalesManager _salesManager;
        private readonly IStatusManager _statusManager;


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

        private ObservableCollection<StatusModel> _orderLineStatuses;
        public ObservableCollection<StatusModel> OrderLineStatuses
        {
            get { return _orderLineStatuses; }
            set { _orderLineStatuses = value; OnPropertyChanged(); }
        }

        public ICollectionView SalesOrderLines { get; set; }

        public SalesOrderViewModel(ISalesManager salesManager,
                                   INavigationService navigationService,
                                   ISharedDataService sharedData,
                                   IStatusManager statusManager)
        {
            _salesManager = salesManager;
            Navigation = navigationService;
            SharedData = sharedData;
            _statusManager = statusManager;

            LoadSalesOrder();

            GetStatuses();
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

using BussinessLogicLibrary.Categories;
using BussinessLogicLibrary.Products;
using BussinessLogicLibrary.Purchases;
using BussinessLogicLibrary.Receipts;
using BussinessLogicLibrary.Vendors;
using DataAccessLibrary.ReceiptRepository;
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
    public class PurchaseOrderViewModel : BaseViewModel
    {
        private PurchaseOrderManager _purchaseOrderManager;
        private readonly IVendorManager _vendorManager;
        private readonly ICategoryManager _categoryManager;
        //two states view or edit
        private string _state;

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

        private ISharedDataService _sharedData;
        public ISharedDataService SharedData
        {
            get { return _sharedData; }
            set { _sharedData = value; }
        }

        private PurchaseOrderHeaderModel _purchaseOrder;

        public PurchaseOrderHeaderModel PurchaseOrder
        {
            get { return _purchaseOrder; }
            set { _purchaseOrder = value; OnPropertyChanged(); }
        }

        private StatusModel _selectedOrderStatus;
        public StatusModel SelectedOrderStatus
        {
            get { return _selectedOrderStatus; }
            set
            {
                _selectedOrderStatus = value;
                PurchaseOrder.OrderStatus = _selectedOrderStatus;
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

        private ObservableCollection<ProductModel> _products;
        public ObservableCollection<ProductModel> Products
        {
            get { return _products; }
            set { _products = value; OnPropertyChanged(); }
        }

        private ObservableCollection<ReceiptingLineModel> _receiptingLines;
        public ObservableCollection<ReceiptingLineModel> ReceiptingLines
        {
            get { return _receiptingLines; }
            set { _receiptingLines = value; OnPropertyChanged(); }
        }

        private int _selectPurchaseOrderLineIndex = -1;
        public int SelectedPurchaseOrderLineIndex
        {
            get { return _selectPurchaseOrderLineIndex; }
            set { _selectPurchaseOrderLineIndex = value; CanChangeProduct = IsNewLine(); OnPropertyChanged(); }
        }

        private ObservableCollection<ReceiptModel> _reverseReceips;

        public ObservableCollection<ReceiptModel> ReverseReceipts
        {
            get { return _reverseReceips; }
            set { _reverseReceips = value; OnPropertyChanged(); }
        }

        private int _selectedReverseReceiptIndex = -1;

        public int SelectedReverseReceiptIndex
        {
            get { return _selectedReverseReceiptIndex; }
            set { _selectedReverseReceiptIndex= value; OnPropertyChanged(); }
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

        private bool _orderStatusEnabled;
        public bool OrderStatusEnabled
        {
            get { return _orderStatusEnabled; }
            set { _orderStatusEnabled = value; OnPropertyChanged(); }
        }

        private bool _canChangeImported;
        public bool CanChangeImported
        {
            get { return _canChangeImported; }
            set { _canChangeImported = value; OnPropertyChanged(); }
        }

        private bool _canEditOderLines;
        public bool CanEditOrderLines
        {
            get { return _canEditOderLines; }
            set { _canEditOderLines = value; OnPropertyChanged(); }
        }

        private bool _canChangeProduct;
        public bool CanChangeProduct
        {
            get { return _canChangeProduct; }
            set { _canChangeProduct = value; OnPropertyChanged(); }
        }


        public ICollectionView PurchaseOrderLines { get; set; }
        public ICollectionView ReceiptingLinesCollectionView { get; set; }

        //Commands
        public RelayCommand CloseViewCommand { get; set; }
        public RelayCommand EditCommand { get; set; }
        public RelayCommand SaveCommand { get; set; }
        public RelayCommand CancelActionCommand { get; set; }
        public RelayCommand AddNewLineCommand { get; set; }
        public RelayCommand RemoveLineCommand { get; set; }
        public RelayCommand ReceiptCommand { get; set; }
        public RelayCommand ReverseReceiptCommand { get; set; }


        //constructor
        public PurchaseOrderViewModel(INavigationService navigation,
                                      IConnectionStringService connectionString,
                                      ISharedDataService sharedData,
                                      IVendorManager vendorManager,
                                      ICategoryManager categoryManager)
        {
            Navigation = navigation;
            ConnectionString = connectionString;
            SharedData = sharedData;
            _vendorManager = vendorManager;
            _categoryManager = categoryManager;

            GetPurchaseOrder();
            GetStatuses();
            GetVendorProducts(PurchaseOrder.VendorID);

            //Instantiate RelayCommand's
            CloseViewCommand = new RelayCommand(CloseView, CanCloseView);
            EditCommand = new RelayCommand(EditPurchaseOrder, CanEditPurchaseOrder);
            AddNewLineCommand = new RelayCommand(AddNewLine, CanAddNewLine);
            RemoveLineCommand = new RelayCommand(RemoveLine, CanRemoveLine);
            SaveCommand = new RelayCommand(SavePurchaseOrder, CanSavePurchaseOrder);
            CancelActionCommand = new RelayCommand(CancelAction, CanCancelAction);
            ReceiptCommand = new RelayCommand(ReceiptOrderLine, CanReceiptOrderLine);
            ReverseReceiptCommand = new RelayCommand(ReverseReceipt, CanReverseReceipt);

            ReceiptingLines = new ObservableCollection<ReceiptingLineModel>();
            ReceiptingLinesCollectionView = CollectionViewSource.GetDefaultView(ReceiptingLines);

            ReverseReceipts = new ObservableCollection<ReceiptModel>();
            
            SetState("View");
        }

        private bool CanReverseReceipt(object obj)
        {
            return _state.Equals("View") && PurchaseOrder.Receipts.Where(x => x.ReverseReferenceID == 0).Count() > 0;
        }

        private void ReverseReceipt(object obj)
        {
            //Get a list of all valid receipts that may be reversed
            SetState("ReverseReceipt");
            ReverseReceipts.Clear();
            foreach (ReceiptModel receipt in PurchaseOrder.Receipts)
            {
                if (receipt.ReverseReferenceID == 0)
                {
                    PurchaseOrderDetailModel model = PurchaseOrder.PurchaseOrderDetails.First(x => x.ProductID == receipt.ProductID);
                    receipt.ProductName = model.Product.ProductName;
                    ReverseReceipts.Add(receipt);
                }
            }
        }

        private bool CanReceiptOrderLine(object obj)
        {
            return _state.Equals("View") && PurchaseOrder.PurchaseOrderDetails.Where(x => x.OrderLineStatus.Status!.Equals("Open")).Any();
        }

        private void ReceiptOrderLine(object obj)
        {
            ReceiptingLines.Clear();

            foreach (PurchaseOrderDetailModel orderLine in PurchaseOrder.PurchaseOrderDetails)
            {
                if (orderLine.OrderLineStatus.Status!.Equals("Open"))
                {
                    ReceiptingLineModel receiptingLine = new ReceiptingLineModel
                    {
                        ProductName = orderLine.Product.ProductName!,
                        PurchaseOrderID = orderLine.PurchaseOrderID,
                        ProductID = orderLine.ProductID,
                        QuantityOrdered = orderLine.QuantityOrdered,
                        QtyToReceipt = 0,
                        QtyReceipted = orderLine.QuantityReceipted,
                        UnitCost = orderLine.UnitCost + orderLine.UnitFreightCost
                    };

                    ReceiptingLines.Add(receiptingLine);
                }
            }
            ReceiptingLinesCollectionView.Refresh();
            SetState("Receipt");
        }

        

        private bool CanSavePurchaseOrder(object obj)
        {
            if (_state.Equals("View"))
            {
                return false;
            }
            else if (_state.Equals("Edit"))
            {
                return true;
            }
            else if (_state.Equals("Receipt"))
            {
                return true;
            }
            else if (_state.Equals("ReverseReceipt") && SelectedReverseReceiptIndex != -1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void SavePurchaseOrder(object obj)
        {
            try
            {
                if (_state.Equals("Edit"))
                {
                    _purchaseOrderManager.SaveChanges();
                    
                }
                else if (_state.Equals("Receipt"))
                {
                    ReceiptManager receiptManager = new ReceiptManager(ConnectionString.GetConnectionString());
                    List<ReceiptingLineModel> receiptingLines = new List<ReceiptingLineModel>();
                    //Loop through the list of receipts
                    foreach (ReceiptingLineModel receiptingLine in ReceiptingLines)
                    {
                        if (receiptingLine.QtyToReceipt != 0)
                        {
                            receiptingLines.Add(receiptingLine);
                        }
                    }

                    try
                    {
                        receiptManager.Insert(receiptingLines);
                    }
                    catch (Exception ex)
                    {

                        MessageBox.Show(ex.Message + "\r\n Problem trying to save receipts.",
                                        "Save Receipts",
                                        MessageBoxButton.OK,
                                        MessageBoxImage.Error);
                        return;
                    }
                }
                else if (_state.Equals("ReverseReceipt"))
                {
                    MessageBoxResult result = MessageBox.Show("Are you sure that you want to reverse the selected receipt?",
                                    "Reverse Receipt",
                                    MessageBoxButton.YesNo,
                                    MessageBoxImage.Question,
                                    MessageBoxResult.No);
                    if (result == MessageBoxResult.Yes)
                    {
                        try
                        {
                            int id = ReverseReceipts[SelectedReverseReceiptIndex].ReceiptID;
                            ReceiptManager receiptManager = new ReceiptManager(ConnectionString.GetConnectionString());
                            receiptManager.Reverse(id);
                        }
                        catch (Exception ex)
                        {

                            MessageBox.Show(ex.Message + "\r\n Problem trying to reverse receipt.",
                                            "Reverse Receipt",
                                            MessageBoxButton.OK,
                                            MessageBoxImage.Error);
                            return;
                        }
                    }
                    else
                    {
                        //receipt reversal cancelled
                        return;
                    }
                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error Saving", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            //Reload the saved purchase order
            try
            {
                //Re-loaded the purchase order view
                //Make sure the shared data has the purchase order ID
                SharedData.SharedData = PurchaseOrder.PurchaseOrderID;
                Navigation.NavigateTo<PurchaseOrderViewModel>();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\r\n Reloading Purchase Order from database.", "Save Changes", MessageBoxButton.OK, MessageBoxImage.Error);
                Navigation.NavigateTo<PurchaseOrdersSwitchboardViewModel>();
            }
        }

        private bool CanCancelAction(object obj)
        {
            return !_state.Equals("View");
        }

        private void CancelAction(object obj)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to cancel these changes?\r\nThis will reloaded the view with the last saved purchase order",
                                                      "Cancel Changes", MessageBoxButton.YesNo,
                                                      MessageBoxImage.Question,
                                                      MessageBoxResult.No);
            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    //Re-loaded the purchase order view
                    //Make sure the shared data has the purchase order ID
                    SharedData.SharedData = PurchaseOrder.PurchaseOrderID;
                    Navigation.NavigateTo<PurchaseOrderViewModel>();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message + "\r\nGoing back to Purchase Order Switchboard.", "Cancel Changes", MessageBoxButton.OK, MessageBoxImage.Error);
                    Navigation.NavigateTo<PurchaseOrdersSwitchboardViewModel>();
                }
            }
        }

        private bool IsNewLine()
        {
            if (SelectedPurchaseOrderLineIndex != -1)
            {
                if (PurchaseOrder.PurchaseOrderDetails[SelectedPurchaseOrderLineIndex].ProductID == 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return false;

        }
        private bool CanRemoveLine(object obj)
        {
            return !_state.Equals("View") &&
                   SelectedPurchaseOrderLineIndex != -1 &&
                   IsNewLine();
        }

        private void RemoveLine(object obj)
        {
            string message = $"Line {SelectedPurchaseOrderLineIndex + 1} is about to be removed. \r\nAre you sure you want to remove this line?";
            MessageBoxResult result = MessageBox.Show(message, "Remove Line", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);

            if (result == MessageBoxResult.Yes)
            {
                //Check if purchase order line product is empty
                //if yes remove line
                //if not then
                //Check if purchase order line exists in Products
                //If yes then just remove line
                //if not then add purchase order line product to Products and remove the line

                ProductModel product = PurchaseOrder.PurchaseOrderDetails[SelectedPurchaseOrderLineIndex].Product;

                if (product.ProductID == 0)
                {
                    //No product added to purchase order line - just remove the line
                    PurchaseOrder.PurchaseOrderDetails.RemoveAt(SelectedPurchaseOrderLineIndex);
                }
                else
                {
                    //A product has been added to the purchase order line
                    //check if this product exists in Products property
                    if (Products.Contains(product))
                    {
                        //It is already in Products - just remove the line
                        PurchaseOrder.PurchaseOrderDetails.RemoveAt(SelectedPurchaseOrderLineIndex);
                    }
                    else
                    {
                        //The product is not in Products property
                        Products.Add(product);
                        //Remove the purchase order line
                        PurchaseOrder.PurchaseOrderDetails.RemoveAt(SelectedPurchaseOrderLineIndex);
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
            return _state.Equals("Edit") && Products != null && Products.Count > 0;
        }

        private void AddNewLine(object obj)
        {
            //Check data before adding a new line 
            ValidateLine();
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
                //check if a new line may be added
                try
                {
                    _purchaseOrderManager.AddOrderLine();
                    PurchaseOrderLines.Refresh();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Add New Line", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

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

            if (PurchaseOrder.PurchaseOrderDetails[index].Product.ProductID == 0)
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

        private bool CanEditPurchaseOrder(object obj)
        {
            return _state.Equals("View");
        }

        private void EditPurchaseOrder(object obj)
        {
            SetState("Edit");
        }

        private bool CanCloseView(object obj)
        {
            return true;
        }

        private void CloseView(object obj)
        {
            //check state
            if (_state.Equals("View"))
            {
                Navigation.NavigateTo<PurchaseOrdersSwitchboardViewModel>();
            }
            else
            {
                //State is in edit mode
                MessageBoxResult result = MessageBox.Show("Are you sure you want to close?\r\nAny changes made will not be saved",
                                                          "Close View",
                                                          MessageBoxButton.YesNo,
                                                          MessageBoxImage.Question,
                                                          MessageBoxResult.No);
                if (result == MessageBoxResult.Yes)
                {
                    Navigation.NavigateTo<PurchaseOrdersSwitchboardViewModel>();
                }
                else
                {
                    return;
                }
            }

        }

        private void SetState(string state)
        {
            _state = state;

            switch (_state)
            {
                case "View":
                    TextReadOnly = true;
                    DateEnabled = false;
                    OrderStatusEnabled = false;
                    CanChangeImported = false;
                    CanEditOrderLines = false;
                    break;
                case "Receipt":
                    TextReadOnly = true;
                    DateEnabled = false;
                    OrderStatusEnabled = false;
                    CanChangeImported = false;
                    CanEditOrderLines = false;
                    break;
                case "ReverseReceipt":
                    TextReadOnly = true;
                    DateEnabled = false;
                    OrderStatusEnabled = false;
                    CanChangeImported = false;
                    CanEditOrderLines = false;
                    break;
                default:
                    TextReadOnly = false;
                    DateEnabled = true;
                    OrderStatusEnabled = true;
                    CanChangeImported = true;
                    if (_purchaseOrderManager.CanEditOrderLines())
                    {
                        CanEditOrderLines = true;
                    }
                    else
                    {
                        CanEditOrderLines = false;
                    }
                    break;
            }
        }

        private void GetPurchaseOrder()
        {
            _purchaseOrderManager = new PurchaseOrderManager(ConnectionString.GetConnectionString());
            //fill _purchaseOrderManager PurchaseOrder property
            try
            {
                _purchaseOrderManager.GetByID((long)SharedData.SharedData);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error Retrieving Purchase Order", MessageBoxButton.OK, MessageBoxImage.Error);
                Navigation.NavigateTo<PurchaseOrdersSwitchboardViewModel>();
            }

            //Set PurchaseOrder = _purchaseOrderManager.PurchaseOrder property
            PurchaseOrder = _purchaseOrderManager.PurchaseOrder;
            //Add order lines to a collection view
            PurchaseOrderLines = CollectionViewSource.GetDefaultView(PurchaseOrder.PurchaseOrderDetails);
        }

        private void GetStatuses()
        {
            Tuple<IEnumerable<StatusModel>, string> statuses = new StatusRepository(ConnectionString.GetConnectionString()).GetAll().ToTuple();
            //check for errors
            if (statuses.Item2 == null)
            {
                //No errors
                OrderStatuses = new ObservableCollection<StatusModel>(statuses.Item1);
                OrderLineStatuses = new ObservableCollection<StatusModel>(statuses.Item1);

                //Set the selectOrderStatus to the current order status
                SelectedOrderStatus = OrderStatuses.First(x => x.StatusID == PurchaseOrder.OrderStatusID);
            }
            else
            {
                throw new Exception(statuses.Item2);
            }
        }

        private void GetVendorProducts(int id)
        {
            try
            {
                List<ProductModel> products = new ProductsManager(ConnectionString.GetConnectionString(), _vendorManager, _categoryManager).GetByVendorID(id).ToList();

                //Remove any products from the list of products if the existing purchase order lines already have that product*
                for (int i = 0; i < PurchaseOrder.PurchaseOrderDetails.Count; i++)
                {
                    int productID = PurchaseOrder.PurchaseOrderDetails[i].ProductID;
                    int index = products.FindIndex(x => x.ProductID == productID);
                    if (index != -1)
                    {
                        products.RemoveAt(index);
                    }
                }
                Products = new ObservableCollection<ProductModel>(products);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Vendors Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Navigation.NavigateTo<PurchaseOrdersSwitchboardViewModel>();
            }

        }
    }
}

using BussinessLogicLibrary.Products;
using BussinessLogicLibrary.Receipts;
using BussinessLogicLibrary.Statuses;
using BussinessLogicLibrary.Vendors;
using DataAccessLibrary.StatusRepository;
using ModelsLibrary;

namespace BussinessLogicLibrary.Purchases
{
    public class PurchaseOrderManager
    {
        public PurchaseOrderHeaderModel PurchaseOrder { get; private set; } = new PurchaseOrderHeaderModel();

        private string _connectionString;
        private readonly IVendorManager _vendorManager;
        private readonly IProductsManager _productsManager;
        private readonly IStatusManager _statusManager;
        private readonly IReceiptManager _receiptManager;

        public PurchaseOrderManager(string connectionString,
                                    IVendorManager vendorManager,
                                    IProductsManager productsManager,
                                    IStatusManager statusManager,
                                    IReceiptManager receiptManager)
        {
            _connectionString = connectionString;
            _vendorManager = vendorManager;
            _productsManager = productsManager;
            _statusManager = statusManager;
            _receiptManager = receiptManager;
        }

        /// <summary>
        /// Popluates this PurchaseOrder property
        /// </summary>
        /// <param name="id">
        /// Takes in a purchase order ID of type long
        /// </param>
        public void GetByID(long id)
        {
            PurchaseOrder = new GetPurchaseOrderManager(_connectionString, _vendorManager, _productsManager, _statusManager, _receiptManager).GetByID(id);
        }

        /// <summary>
        /// Saves any valid changes made to the purchase order
        /// </summary>
        public void SaveChanges()
        {
            new UpdatePurchaseOrderManager(_connectionString, PurchaseOrder, _vendorManager, _productsManager, _statusManager, _receiptManager);
        }

        private void GetOpenOrderLineStatus(PurchaseOrderDetailModel orderLine)
        {
            try
            {
                IEnumerable<StatusModel> lineStatuses = _statusManager.GetAll();
                orderLine.OrderLineStatus = lineStatuses.First(x => x.Status!.Equals("Open"));
                orderLine.OrderLineStatusID = orderLine.OrderLineStatus.StatusID;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void AddOrderLine()
        {
            if (CanAddOrderLine())
            {
                PurchaseOrder.PurchaseOrderDetails.Add(new PurchaseOrderDetailModel());
                int index = PurchaseOrder.PurchaseOrderDetails.Count - 1;
                //Add order line status of open
                GetOpenOrderLineStatus(PurchaseOrder.PurchaseOrderDetails[index]);
            }

        }

        private bool CanAddOrderLine()
        {
            bool CanAddLine = true;

            //Check if the order status is open
            if (!PurchaseOrder.OrderStatus.Status!.Equals("Open"))
            {
                throw new Exception("Can only add a line if the order status is open.");
            }

            return CanAddLine;
        }

        public bool CanEditOrderLines()
        {
            if (PurchaseOrder.OrderStatus.Status!.Equals("Open"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}

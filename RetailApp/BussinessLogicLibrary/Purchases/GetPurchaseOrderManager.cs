using BussinessLogicLibrary.Products;
using BussinessLogicLibrary.Receipts;
using BussinessLogicLibrary.Statuses;
using BussinessLogicLibrary.Vendors;
using DataAccessLibrary.PurchaseOrderDetailRepository;
using DataAccessLibrary.PurchaseOrderHeaderRepository;
using ModelsLibrary;

namespace BussinessLogicLibrary.Purchases
{
    public class GetPurchaseOrderManager
    {
        private string _connectionString;
        private readonly IVendorManager _vendorManager;
        private readonly IProductsManager _productsManager;
        private readonly IStatusManager _statusManager;
        private readonly IReceiptManager _receiptManager;

        private PurchaseOrderHeaderModel PurchaseOrder { get; set; } = new PurchaseOrderHeaderModel();

        public GetPurchaseOrderManager(string connectionString,
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

        public PurchaseOrderHeaderModel GetByID(long id)
        {
            GetOrder(id);
            GetOrderLines();
            GetVendor();
            GetOrderStatus();
            GetReceipts();

            return PurchaseOrder;
        }

        private void GetOrder(long id)
        {
            Tuple<PurchaseOrderHeaderModel, string> purchaseOrder = new PurchaseOrderHeaderRepository(_connectionString).GetByID(id).ToTuple();
            //check for errors
            if (purchaseOrder.Item2 == null)
            {
                //No errors
                PurchaseOrder = purchaseOrder.Item1;
            }
            else
            {
                //Error raised by database
                throw new Exception(purchaseOrder.Item2);
            }
        }

        private void GetOrderLines()
        {
            Tuple<IEnumerable<PurchaseOrderDetailModel>, string> orderLines = new PurchaseOrderDetailRepository(_connectionString).GetByPurchaseOrderID(PurchaseOrder.PurchaseOrderID).ToTuple();
            //Check for errors
            if (orderLines.Item2 == null)
            {
                //No errors
                PurchaseOrder.PurchaseOrderDetails = orderLines.Item1.ToList();
                GetOrderLinesProduct();
                GetOrderLinesStatus();
            }
            else
            {
                throw new Exception(orderLines.Item2);
            }
        }

        private void GetOrderLinesStatus()
        {
            try
            {
                //Get all statuses
                IEnumerable<StatusModel> lineStatuses = _statusManager.GetAll();

                //no errors
                foreach (PurchaseOrderDetailModel orderLine in PurchaseOrder.PurchaseOrderDetails)
                {
                    orderLine.OrderLineStatus = lineStatuses.First(x => x.StatusID == orderLine.OrderLineStatusID);
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void GetOrderLinesProduct()
        {
            try
            {
                IEnumerable<ProductModel> products = _productsManager.GetByVendorID(PurchaseOrder.VendorID);

                foreach (PurchaseOrderDetailModel orderLine in PurchaseOrder.PurchaseOrderDetails)
                {
                    orderLine.Product = products.First(x => x.ProductID == orderLine.ProductID);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void GetVendor()
        {
            try
            {
                PurchaseOrder.Vendor = _vendorManager.GetByID(PurchaseOrder.VendorID);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void GetOrderStatus()
        {
            try
            {
                PurchaseOrder.OrderStatus = _statusManager.GetByID(PurchaseOrder.OrderStatusID);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void GetReceipts()
        {
            try
            {
                PurchaseOrder.Receipts = _receiptManager.GetByPurchaseOrderID(PurchaseOrder.PurchaseOrderID);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}

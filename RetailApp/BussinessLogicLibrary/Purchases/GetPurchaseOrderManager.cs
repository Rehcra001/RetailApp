using BussinessLogicLibrary.Products;
using BussinessLogicLibrary.Vendors;
using DataAccessLibrary.ProductRepository;
using DataAccessLibrary.PurchaseOrderDetailRepository;
using DataAccessLibrary.PurchaseOrderHeaderRepository;
using DataAccessLibrary.ReceiptRepository;
using DataAccessLibrary.StatusRepository;
using ModelsLibrary;
using ModelsLibrary.RepositoryInterfaces;

namespace BussinessLogicLibrary.Purchases
{
    public class GetPurchaseOrderManager
    {
        private string _connectionString;
        private IVendorManager _vendorManager;
        private IProductsManager _productsManager;

        private PurchaseOrderHeaderModel PurchaseOrder { get; set; } = new PurchaseOrderHeaderModel();

        public GetPurchaseOrderManager(string connectionString,
                                       IVendorManager vendorManager,
                                       IProductsManager productsManager)
        {
            _connectionString = connectionString;
            _vendorManager = vendorManager;
            _productsManager = productsManager;
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
            Tuple<IEnumerable<StatusModel>, string> lineStatuses = new StatusRepository(_connectionString).GetAll().ToTuple();
            //Check for errors
            if (lineStatuses.Item2 == null)
            {
                //no errors
                foreach(PurchaseOrderDetailModel orderLine in PurchaseOrder.PurchaseOrderDetails)
                {
                    orderLine.OrderLineStatus = lineStatuses.Item1.First(x => x.StatusID == orderLine.OrderLineStatusID);
                }
            }
            else
            {
                throw new Exception(lineStatuses.Item2);
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
            Tuple<StatusModel, string> orderStatus = new StatusRepository(_connectionString).GetByID(PurchaseOrder.OrderStatusID).ToTuple();
            //check for errors
            if (orderStatus.Item2 == null)
            {
                //No errors
                PurchaseOrder.OrderStatus = orderStatus.Item1;
            }
            else
            {
                throw new Exception(orderStatus.Item2);
            }
        }

        private void GetReceipts()
        {
            Tuple<IEnumerable<ReceiptModel>, string> receipts = new ReceiptRepository(_connectionString).GetByPurchaseOrderID(PurchaseOrder.PurchaseOrderID).ToTuple();
            //check for errors
            if (receipts.Item2 == null)
            {
                //No errors
                PurchaseOrder.Receipts = receipts.Item1;
            }
            else
            {
                throw new Exception(receipts.Item2);
            }
        }

    }
}

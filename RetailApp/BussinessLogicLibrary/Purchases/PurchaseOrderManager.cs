using BussinessLogicLibrary.Products;
using BussinessLogicLibrary.Receipts;
using BussinessLogicLibrary.Statuses;
using BussinessLogicLibrary.Vendors;
using ModelsLibrary;

namespace BussinessLogicLibrary.Purchases
{
    public class PurchaseOrderManager : IPurchaseOrderManager
    {
        private readonly IGetPurchaseOrderManager _getPurchaseOrderManager;
        private readonly IUpdatePurchaseOrderManager _updatePurchaseOrderManager;
        private readonly IStatusManager _statusManager;

        public PurchaseOrderManager(IGetPurchaseOrderManager getPurchaseOrderManager,
                                    IUpdatePurchaseOrderManager updatePurchaseOrderManager,
                                    IVendorManager vendorManager,
                                    IProductsManager productsManager,
                                    IStatusManager statusManager,
                                    IReceiptManager receiptManager)
        {
            _getPurchaseOrderManager = getPurchaseOrderManager;
            _updatePurchaseOrderManager = updatePurchaseOrderManager;
            _statusManager = statusManager;
        }

        /// <summary>
        /// Popluates this PurchaseOrder property
        /// </summary>
        /// <param name="id">
        /// Takes in a purchase order ID of type long
        /// </param>
        public PurchaseOrderHeaderModel GetByID(long id)
        {
            return _getPurchaseOrderManager.GetByID(id);
        }

        /// <summary>
        /// Saves any valid changes made to the purchase order
        /// </summary>
        public void SaveChanges(PurchaseOrderHeaderModel purchase)
        {
            _updatePurchaseOrderManager.Update(purchase);
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

        public void AddOrderLine(PurchaseOrderHeaderModel purchase)
        {
            if (CanAddOrderLine(purchase))
            {
                purchase.PurchaseOrderDetails.Add(new PurchaseOrderDetailModel());
                int index = purchase.PurchaseOrderDetails.Count - 1;
                //Add order line status of open
                GetOpenOrderLineStatus(purchase.PurchaseOrderDetails[index]);
            }

        }

        private bool CanAddOrderLine(PurchaseOrderHeaderModel purchase)
        {
            bool CanAddLine = true;

            //Check if the order status is open
            if (!purchase.OrderStatus.Status!.Equals("Open"))
            {
                throw new Exception("Can only add a line if the order status is open.");
            }

            return CanAddLine;
        }

        public bool CanEditOrderLines(PurchaseOrderHeaderModel purchase)
        {
            if (purchase.OrderStatus.Status!.Equals("Open"))
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

using BussinessLogicLibrary.Statuses;
using BussinessLogicLibrary.Vendors;
using ModelsLibrary;
using ModelsLibrary.RepositoryInterfaces;

namespace BussinessLogicLibrary.Purchases
{
    public class PurchaseOrdersListManager : IPurchaseOrdersListManager
    {
        private IPurchaseOrderHeaderRepository _purchaseOrderHeaderRepository;
        private readonly IVendorManager _vendorManager;
        private readonly IStatusManager _statusManager;

        public PurchaseOrdersListManager(IPurchaseOrderHeaderRepository purchaseOrderHeaderRepository,
                                         IVendorManager vendorManager,
                                         IStatusManager statusManager)
        {
            _purchaseOrderHeaderRepository = purchaseOrderHeaderRepository;
            _vendorManager = vendorManager;
            _statusManager = statusManager;
        }

        /// <summary>
        /// Populates the PurchaseOrders List with all purchase orders in the database"
        /// </summary>
        public IEnumerable<PurchaseOrderHeaderModel> GetAll()
        {
            Tuple<IEnumerable<PurchaseOrderHeaderModel>, string> purchaseOrders = _purchaseOrderHeaderRepository.GetALL().ToTuple();

            //Check for errors
            if (purchaseOrders.Item2 == null)
            {
                //Add vendor to each order
                GetVendors(purchaseOrders.Item1);
                //Add OrderStatus to each order
                GetOrderStatus(purchaseOrders.Item1);

                return purchaseOrders.Item1;
            }
            else
            {
                //Error retrieving
                throw new Exception(purchaseOrders.Item2);
            }
        }

        /// <summary>
        /// Populates the PurchaseOrders List by order status
        /// </summary>
        /// <param name="orderStatus"></param>
        public IEnumerable<PurchaseOrderHeaderModel> GetByOrderStatusID(int id)
        {
            Tuple<IEnumerable<PurchaseOrderHeaderModel>, string> purchaseOrders = _purchaseOrderHeaderRepository.GetByOrderStatusID(id).ToTuple();

            //Check for errors
            if (purchaseOrders.Item2 == null)
            {
                //Add vendor to each order
                GetVendors(purchaseOrders.Item1);
                //Add OrderStatus to each order
                GetOrderStatus(purchaseOrders.Item1);

                return purchaseOrders.Item1;
            }
            else
            {
                //Error retrieving
                throw new Exception(purchaseOrders.Item2);
            }
        }

        /// <summary>
        /// Populates the PurchaseOrders list by vendor
        /// </summary>
        /// <param name="id"></param>
        /// <exception cref="Exception"></exception>
        public IEnumerable<PurchaseOrderHeaderModel> GetByVendorID(int id)
        {
            Tuple<IEnumerable<PurchaseOrderHeaderModel>, string> purchaseOrders = _purchaseOrderHeaderRepository.GetByVendorID(id).ToTuple();

            //Check for errors
            if (purchaseOrders.Item2 == null)
            {
                //Add vendor to each order
                GetVendors(purchaseOrders.Item1);
                //Add OrderStatus to each order
                GetOrderStatus(purchaseOrders.Item1);

                return purchaseOrders.Item1;
            }
            else
            {
                //Error retrieving
                throw new Exception(purchaseOrders.Item2);
            }
        }

        private void GetOrderStatus(IEnumerable<PurchaseOrderHeaderModel> purchases)
        {
            try
            {
                AddOrderStatus(_statusManager.GetAll(), purchases);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void AddOrderStatus(IEnumerable<StatusModel> orderStatuses, IEnumerable<PurchaseOrderHeaderModel> purchases)
        {
            foreach (PurchaseOrderHeaderModel order in purchases)
            {
                order.OrderStatus = orderStatuses.First(x => x.StatusID == order.OrderStatusID);
            }
        }

        private void GetVendors(IEnumerable<PurchaseOrderHeaderModel> purchases)
        {
            try
            {
                AddVendor(_vendorManager.GetAll(), purchases);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void AddVendor(IEnumerable<VendorModel> vendors, IEnumerable<PurchaseOrderHeaderModel> purchases)
        {
            foreach (PurchaseOrderHeaderModel order in purchases)
            {
                order.Vendor = vendors.First(x => x.VendorID == order.VendorID);
            }
        }
    }
}

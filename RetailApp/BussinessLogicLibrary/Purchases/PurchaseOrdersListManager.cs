using DataAccessLibrary.StatusRepository;
using DataAccessLibrary.PurchaseOrderHeaderRepository;
using DataAccessLibrary.VendorRepository;
using ModelsLibrary;
using ModelsLibrary.RepositoryInterfaces;
using BussinessLogicLibrary.Vendors;
using BussinessLogicLibrary.Statuses;

namespace BussinessLogicLibrary.Purchases
{
    public class PurchaseOrdersListManager
    {
        private string _connectionString;
        private readonly IVendorManager _vendorManager;
        private readonly IStatusManager _statusManager;
        private PurchaseOrderHeaderRepository _purchaseOrderHeaderRepository;

        /// <summary>
        /// Holds the list of retrieved purchase orders
        /// </summary>
        public IEnumerable<PurchaseOrderHeaderModel> PurchaseOrders { get; set; } = new List<PurchaseOrderHeaderModel>();

        public PurchaseOrdersListManager(string connectionString,
                                         IVendorManager vendorManager,
                                         IStatusManager statusManager)
        {
            _connectionString = connectionString;
            _vendorManager = vendorManager;
            _statusManager = statusManager;

            _purchaseOrderHeaderRepository = new PurchaseOrderHeaderRepository(_connectionString);
        }

        /// <summary>
        /// Populates the PurchaseOrders List with all purchase orders in the database"
        /// </summary>
        public void GetAll()
        {
            Tuple<IEnumerable<PurchaseOrderHeaderModel>, string> purchaseOrders = _purchaseOrderHeaderRepository.GetALL().ToTuple();

            //Check for errors
            if (purchaseOrders.Item2 == null)
            {
                //No error retrieving
                PurchaseOrders = purchaseOrders.Item1;
                //Add vendor to each order
                GetVendors();
                //Add OrderStatus to each order
                GetOrderStatus();
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
        public void GetByOrderStatusID(int id)
        {
            Tuple<IEnumerable<PurchaseOrderHeaderModel>, string> purchaseOrders = _purchaseOrderHeaderRepository.GetByOrderStatusID(id).ToTuple();

            //Check for errors
            if (purchaseOrders.Item2 == null)
            {
                //No error retrieving
                PurchaseOrders = purchaseOrders.Item1;
                //Add vendor to each order
                GetVendors();
                //Add OrderStatus to each order
                GetOrderStatus();
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
        public void GetByVendorID(int id)
        {
            Tuple<IEnumerable<PurchaseOrderHeaderModel>, string> purchaseOrders = _purchaseOrderHeaderRepository.GetByVendorID(id).ToTuple();

            //Check for errors
            if (purchaseOrders.Item2 == null)
            {
                //No error retrieving
                PurchaseOrders = purchaseOrders.Item1;
                //Add vendor to each order
                GetVendors();
                //Add OrderStatus to each order
                GetOrderStatus();
            }
            else
            {
                //Error retrieving
                throw new Exception(purchaseOrders.Item2);
            }
        }

        private void GetOrderStatus()
        {
            try
            {
                AddOrderStatus(_statusManager.GetAll());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void AddOrderStatus(IEnumerable<StatusModel> orderStatuses)
        {
            foreach (PurchaseOrderHeaderModel order in PurchaseOrders)
            {
                order.OrderStatus = orderStatuses.First(x => x.StatusID == order.OrderStatusID);
            }
        }

        private void GetVendors()
        {
            try
            {
                AddVendor(_vendorManager.GetAll());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void AddVendor(IEnumerable<VendorModel> vendors)
        {
            foreach (PurchaseOrderHeaderModel order in PurchaseOrders)
            {
                order.Vendor = vendors.First(x => x.VendorID == order.VendorID);
            }
        }
    }
}

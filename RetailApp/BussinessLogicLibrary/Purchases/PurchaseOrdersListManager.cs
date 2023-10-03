using DataAccessLibrary.OrderStatusRepository;
using DataAccessLibrary.PurchaseOrderHeaderRepository;
using DataAccessLibrary.VendorRepository;
using ModelsLibrary;

namespace BussinessLogicLibrary.Purchases
{
    public class PurchaseOrdersListManager
    {
        private string _connectionString;
        private PurchaseOrderHeaderRepository _purchaseOrderHeaderRepository;

        /// <summary>
        /// Holds the list of retrieved purchase orders
        /// </summary>
        public IEnumerable<PurchaseOrderHeaderModel> PurchaseOrders { get; set; } = new List<PurchaseOrderHeaderModel>();

        public PurchaseOrdersListManager(string connectionString)
        {
            _connectionString = connectionString;
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
            Tuple<IEnumerable<OrderStatusModel>, string> orderStatuses = new OrderStatusRepository(_connectionString).GetAll().ToTuple();

            //Check for errors
            if (orderStatuses.Item2 == null)
            {
                //No errors retrieving
                AddOrderStatus(orderStatuses.Item1);
            }
            else
            {
                //Error retrieving
                throw new Exception(orderStatuses.Item2);
            }
        }

        private void AddOrderStatus(IEnumerable<OrderStatusModel> orderStatuses)
        {
            foreach (PurchaseOrderHeaderModel order in PurchaseOrders)
            {
                order.OrderStatus = orderStatuses.First(x => x.OrderStatusID == order.OrderStatusID);
            }
        }

        private void GetVendors()
        {
            Tuple<IEnumerable<VendorModel>, string> vendors = new VendorRepository(_connectionString).GetAll().ToTuple();

            //Check for errors
            if (vendors.Item2 == null)
            {
                //No errors retrieving
                AddVendor(vendors.Item1);
            }
            else
            {
                //Error retrieving
                throw new Exception(vendors.Item2);
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

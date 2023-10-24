using ModelsLibrary;

namespace BussinessLogicLibrary.Sales
{
    public class SalesManager : ISalesManager
    {
        private readonly IInsertSalesOrderManager _insertSalesOrderManager;
        private readonly IGetAllSalesOrderManager _getAllSalesOrderManager;


        public SalesManager(IInsertSalesOrderManager insertSalesOrderManager, IGetAllSalesOrderManager getAllSalesOrderManager)
        {
            _insertSalesOrderManager = insertSalesOrderManager;
            _getAllSalesOrderManager = getAllSalesOrderManager;
        }

        /// <summary>
        /// Inserts a new sales order
        /// </summary>
        /// <param name="salesOrder">
        /// Takes in a SalesOrderHeaderModel for insertion
        /// </param>
        /// <returns>
        /// Returns a SalesOrderModel with Sql SalesOrderID added
        /// </returns>
        /// <exception cref="Exception">
        /// Throws an exception if any error encountered with the validation or insertion
        /// </exception>
        public SalesOrderHeaderModel Insert(SalesOrderHeaderModel salesOrder)
        {
            try
            {
                return _insertSalesOrderManager.Insert(salesOrder);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Saves the changes to an existing Sales order
        /// </summary>
        /// <param name="salesOrder">
        /// Takes in a SalesOrderHeaderModel with the changes to be saved
        /// </param>
        /// <exception cref="Exception">
        /// Throws an exception if any error encountered with validation or saving
        /// </exception>
        public void Update(SalesOrderHeaderModel salesOrder)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets all sales order headers with all details attached
        /// </summary>
        /// <returns>
        /// Returns an IEnumerable<SalesOrderHeaders>
        /// </returns>
        /// <exception cref="Exception">
        /// Throws an exception if unable to retrieve the sales orders
        /// </exception>
        public IEnumerable<SalesOrderHeaderModel> GetAllWithSalesOrderDetails()
        {
            List<SalesOrderHeaderModel> salesOrders = new List<SalesOrderHeaderModel>();
            try
            {
                salesOrders = _getAllSalesOrderManager.GetAllWithSalesOrderDetails().ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return salesOrders;
        }

        /// <summary>
        /// Gets all sales order headers with no details attached
        /// </summary>
        /// <returns>
        /// Returns an IEnumerable<SalesOrderHeaders>
        /// </returns>
        /// <exception cref="Exception">
        /// Throws an exception if unable to retrieve the sales orders
        /// </exception>
        public IEnumerable<SalesOrderHeaderModel> GetAllWithoutSalesOrderDetails()
        {
            List<SalesOrderHeaderModel> salesOrders = new List<SalesOrderHeaderModel>();
            try
            {
                salesOrders = _getAllSalesOrderManager.GetAllWithoutSalesOrderDetails().ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return salesOrders;
        }

        /// <summary>
        /// Retrieves a sales order specified by the SalesOrderID
        /// </summary>
        /// <param name="id">
        /// Takes in a long of SalesOrderID
        /// </param>
        /// <returns></returns>
        /// <exception cref="Exception">
        /// Throws an exception if unable to retrieve the sales order
        /// </exception>
        public SalesOrderHeaderModel GetByID(long id)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets all sales orders for the given customer ID 
        /// </summary>
        /// <param name="id">
        /// Takes in an integer of CustomerID
        /// </param>
        /// <returns>
        /// Returns an IEnumerable<SalesOrderHeaders> filtered by CustomerID
        /// </returns>
        /// <exception cref="Exception">
        /// Throws an exception if unable to retrieve the sales orders
        /// </exception>
        public IEnumerable<SalesOrderHeaderModel> GetByCustomerID(int id)
        {
            List<SalesOrderHeaderModel> salesOrders = new List<SalesOrderHeaderModel>();
            try
            {
                salesOrders = _getAllSalesOrderManager.GetAllWithoutSalesOrderDetails().ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return salesOrders.Where(x => x.CustomerID == id);
        }

        /// <summary>
        /// Gets all sales orders for the given Order status ID 
        /// </summary>
        /// <param name="id">
        /// Takes in an integer of OrderStatusID
        /// </param>
        /// <returns>
        /// Returns an IEnumerable<SalesOrderHeaders> filtered by OrderStatusID
        /// </returns>
        /// <exception cref="Exception">
        /// Throws an exception if unable to retrieve the sales orders
        /// </exception>
        public IEnumerable<SalesOrderHeaderModel> GetByOrderStatusID(int id)
        {
            List<SalesOrderHeaderModel> salesOrders = new List<SalesOrderHeaderModel>();
            try
            {
                salesOrders = _getAllSalesOrderManager.GetAllWithoutSalesOrderDetails().ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return salesOrders.Where(x => x.OrderStatusID == id);
        }
    }
}

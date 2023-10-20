using ModelsLibrary;

namespace BussinessLogicLibrary.Sales
{
    public class SalesManager : ISalesManager
    {
        private readonly IInsertSalesOrderManager _insertSalesOrderManager;

        public SalesManager(IInsertSalesOrderManager insertSalesOrderManager)
        {
            _insertSalesOrderManager = insertSalesOrderManager;
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
        /// Gets all sales orders
        /// </summary>
        /// <returns>
        /// Returns an IEnumerable<SalesOrderHeaders>
        /// </returns>
        /// <exception cref="Exception">
        /// Throws an exception if unable to retrieve the sales orders
        /// </exception>
        public IEnumerable<SalesOrderHeaderModel> GetAll()
        {
            throw new NotImplementedException();
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
        /// <returns>
        /// Returns an IEnumerable<SalesOrderHeaders> filtered by CustomerID
        /// </returns>
        /// <exception cref="Exception">
        /// Throws an exception if unable to retrieve the sales orders
        /// </exception>
        public IEnumerable<SalesOrderHeaderModel> GetByCustomerID(int id)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets all sales orders for the given Order status ID 
        /// </summary>
        /// <returns>
        /// Returns an IEnumerable<SalesOrderHeaders> filtered by OrderStatusID
        /// </returns>
        /// <exception cref="Exception">
        /// Throws an exception if unable to retrieve the sales orders
        /// </exception>
        public IEnumerable<SalesOrderHeaderModel> GetByOrderStatusID(int id)
        {
            throw new NotImplementedException();
        }
    }
}

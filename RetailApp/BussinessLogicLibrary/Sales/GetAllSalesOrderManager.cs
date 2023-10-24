using BussinessLogicLibrary.Customers;
using BussinessLogicLibrary.Statuses;
using ModelsLibrary;
using ModelsLibrary.RepositoryInterfaces;

namespace BussinessLogicLibrary.Sales
{
    public class GetAllSalesOrderManager : IGetAllSalesOrderManager
    {
        private readonly ISalesOrderHeaderRepository _salesOrderHeaderRepository;
        private readonly IGetAllSalesOrderDetailsManager _GetAllSalesOrderDetailManager;
        private readonly IStatusManager _statusManager;
        private readonly ICustomerManager _customerManager;

        private IEnumerable<SalesOrderHeaderModel> _salesOrderHeaders;
        private IEnumerable<SalesOrderDetailModel> _salesOrderDetails;
        private IEnumerable<CustomerModel> _customers;
        private IEnumerable<StatusModel> _statuses;

        public GetAllSalesOrderManager(ISalesOrderHeaderRepository salesOrderHeaderRepository,
                                       IGetAllSalesOrderDetailsManager getAllSalesOrderDetailsManager,
                                       IStatusManager statusManager,
                                       ICustomerManager customerManager)
        {
            _salesOrderHeaderRepository = salesOrderHeaderRepository;
            _GetAllSalesOrderDetailManager = getAllSalesOrderDetailsManager;
            _statusManager = statusManager;
            _customerManager = customerManager;
        }

        public IEnumerable<SalesOrderHeaderModel> GetAllWithSalesOrderDetails()
        {
            GetAllSalesOrderHeaders();
            GetAllSalesOrderDetails();
            GetStatuses();
            GetCustomers();
            AddSalesOrderDetails();
            AddOrderStatus();
            AddCustomer();

            return _salesOrderHeaders;
        }



        public IEnumerable<SalesOrderHeaderModel> GetAllWithoutSalesOrderDetails()
        {
            GetAllSalesOrderHeaders();
            GetStatuses();
            GetCustomers();
            AddOrderStatus();
            AddCustomer();

            return _salesOrderHeaders;
        }

        private void GetAllSalesOrderHeaders()
        {
            Tuple<IEnumerable<SalesOrderHeaderModel>, string> soh = _salesOrderHeaderRepository.GetAll().ToTuple();
            //check for errors
            if (soh.Item2 == null)
            {
                //No errors
                _salesOrderHeaders = soh.Item1;
            }
            else
            {
                //Error
                throw new Exception(soh.Item2);
            }
        }

        private void GetAllSalesOrderDetails()
        {
            try
            {
                _salesOrderDetails = _GetAllSalesOrderDetailManager.GetAll();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
        private void GetCustomers()
        {
            try
            {
                _customers = _customerManager.GetAll();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void GetStatuses()
        {
            try
            {
                _statuses = _statusManager.GetAll();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void AddSalesOrderDetails()
        {
            foreach (SalesOrderHeaderModel sod in _salesOrderHeaders)
            {
                sod.SalesOrderDetails = _salesOrderDetails.Where(x => x.SalesOrderID == sod.SalesOrderID).ToList();
            }
        }

        private void AddOrderStatus()
        {
            foreach (SalesOrderHeaderModel soh in _salesOrderHeaders)
            {
                soh.OrderStatus = _statuses.FirstOrDefault(x => x.StatusID == soh.OrderStatusID)!;
            }
        }

        private void AddCustomer()
        {
            foreach (SalesOrderHeaderModel soh in _salesOrderHeaders)
            {
                soh.Customer = _customers.FirstOrDefault(x => x.CustomerID == soh.CustomerID)!;
            }
        }
    }
}

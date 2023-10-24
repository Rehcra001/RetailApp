using BussinessLogicLibrary.Products;
using BussinessLogicLibrary.Statuses;
using ModelsLibrary;
using ModelsLibrary.RepositoryInterfaces;

namespace BussinessLogicLibrary.Sales
{
    public class GetAllSalesOrderDetailsManager : IGetAllSalesOrderDetailsManager
    {
        private readonly ISalesOrderDetailRepository _salesOrderDetailRepository;
        private readonly IProductsManager _productsManager;
        private readonly IStatusManager _statusManager;

        private IEnumerable<SalesOrderDetailModel> _salesOrderDetails;
        private IEnumerable<ProductModel> _products;
        private IEnumerable<StatusModel> _statuses;

        public GetAllSalesOrderDetailsManager(ISalesOrderDetailRepository salesOrderDetailRepository,
                                              IProductsManager productsManager,
                                              IStatusManager statusManager)
        {
            _salesOrderDetailRepository = salesOrderDetailRepository;
            _productsManager = productsManager;
            _statusManager = statusManager;
        }

        public IEnumerable<SalesOrderDetailModel> GetAll()
        {
            GetAllSalesOrderDetails();
            GetAllProducts();
            GetAllStatuses();
            AddProducts();
            AddOrderLinesStatuses();

            return _salesOrderDetails;
        }

        private void AddOrderLinesStatuses()
        {
            foreach (SalesOrderDetailModel sod in _salesOrderDetails)
            {
                sod.OrderLineStatus = _statuses.FirstOrDefault(x => x.StatusID == sod.OrderLineStatusID)!;
            }
        }

        private void AddProducts()
        {
            foreach (SalesOrderDetailModel sod in _salesOrderDetails)
            {
                sod.Product = _products.FirstOrDefault(x => x.ProductID == sod.ProductID)!;
            }
        }

        private void GetAllStatuses()
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

        private void GetAllProducts()
        {
            try
            {
                _products = _productsManager.GetAll();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void GetAllSalesOrderDetails()
        {
            Tuple<IEnumerable<SalesOrderDetailModel>, string> sod = _salesOrderDetailRepository.GetAll().ToTuple();
            //check for errors
            if (sod.Item2 == null)
            {
                //No errors
                _salesOrderDetails = sod.Item1;
            }
            else
            {
                //Error
                throw new Exception(sod.Item2);
            }

        }
    }
}

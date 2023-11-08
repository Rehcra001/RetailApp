using ModelsLibrary;
using ModelsLibrary.RepositoryInterfaces;

namespace BussinessLogicLibrary.Sales
{
    public class GetSalesOrderDetailsByIDManager : IGetSalesOrderDetailsByIDManager
    {
        private readonly ISalesOrderDetailRepository _salesOrderDetailRepository;

        public GetSalesOrderDetailsByIDManager(ISalesOrderDetailRepository salesOrderDetailRepository)
        {
            _salesOrderDetailRepository = salesOrderDetailRepository;
        }

        public IEnumerable<SalesOrderDetailModel> GetByID(long id)
        {
            Tuple<IEnumerable<SalesOrderDetailModel>, string> orderLines = _salesOrderDetailRepository.GetBySaleOrderID(id).ToTuple();
            //Check for errors
            if (orderLines.Item2 == null)
            {
                //No errors
                return orderLines.Item1;
            }
            else
            {
                //Error
                throw new Exception(orderLines.Item2);
            }
        }
    }
}

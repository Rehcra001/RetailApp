using ModelsLibrary;

namespace BussinessLogicLibrary.Sales
{
    public interface IGetSalesOrderDetailsByIDManager
    {
        IEnumerable<SalesOrderDetailModel> GetByID(long id);
    }
}
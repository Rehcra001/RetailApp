using ModelsLibrary;

namespace BussinessLogicLibrary.Sales
{
    public interface IGetAllSalesOrderDetailsManager
    {
        IEnumerable<SalesOrderDetailModel> GetAll();
    }
}
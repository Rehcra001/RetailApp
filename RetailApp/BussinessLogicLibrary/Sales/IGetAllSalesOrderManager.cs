using ModelsLibrary;

namespace BussinessLogicLibrary.Sales
{
    public interface IGetAllSalesOrderManager
    {
        IEnumerable<SalesOrderHeaderModel> GetAllWithoutSalesOrderDetails();
        IEnumerable<SalesOrderHeaderModel> GetAllWithSalesOrderDetails();
    }
}
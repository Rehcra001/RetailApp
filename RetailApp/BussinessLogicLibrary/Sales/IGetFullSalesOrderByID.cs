using ModelsLibrary;

namespace BussinessLogicLibrary.Sales
{
    public interface IGetFullSalesOrderByID
    {
        SalesOrderHeaderModel GetByID(long id);
    }
}
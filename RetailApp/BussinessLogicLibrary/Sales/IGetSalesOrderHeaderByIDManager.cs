using ModelsLibrary;

namespace BussinessLogicLibrary.Sales
{
    public interface IGetSalesOrderHeaderByIDManager
    {
        SalesOrderHeaderModel GetByID(long id);
    }
}
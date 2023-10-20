using ModelsLibrary;

namespace BussinessLogicLibrary.Sales
{
    public interface IInsertSalesOrderManager
    {
        SalesOrderHeaderModel Insert(SalesOrderHeaderModel salesOrder);
    }
}
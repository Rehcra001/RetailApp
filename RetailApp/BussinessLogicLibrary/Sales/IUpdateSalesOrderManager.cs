using ModelsLibrary;

namespace BussinessLogicLibrary.Sales
{
    public interface IUpdateSalesOrderManager
    {
        void Update(SalesOrderHeaderModel salesOrderHeader);
    }
}
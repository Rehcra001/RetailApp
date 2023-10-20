using ModelsLibrary;

namespace BussinessLogicLibrary.Sales
{
    public interface ISalesManager
    {
        IEnumerable<SalesOrderHeaderModel> GetAll();
        IEnumerable<SalesOrderHeaderModel> GetByCustomerID(int id);
        SalesOrderHeaderModel GetByID(long id);
        IEnumerable<SalesOrderHeaderModel> GetByOrderStatusID(int id);
        SalesOrderHeaderModel Insert(SalesOrderHeaderModel salesOrder);
        void Update(SalesOrderHeaderModel salesOrder);
    }
}
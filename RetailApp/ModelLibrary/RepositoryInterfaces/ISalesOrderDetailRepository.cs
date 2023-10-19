namespace ModelsLibrary.RepositoryInterfaces
{
    public interface ISalesOrderDetailRepository
    {
        string Insert(SalesOrderDetailModel salesOrderDetail);
        string Update(SalesOrderDetailModel salesOrderDetail);
        (IEnumerable<SalesOrderDetailModel>, string) GetAll();
        (IEnumerable<SalesOrderDetailModel>, string) GetBySaleOrderID(long id);
    }
}

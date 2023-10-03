namespace ModelsLibrary.RepositoryInterfaces
{
    public interface IPurchaseOrderHeaderRepository
    {
        (PurchaseOrderHeaderModel, string) Insert(PurchaseOrderHeaderModel purchaseOrderHeader);
        string Update(PurchaseOrderHeaderModel purchaseOrderHeader);
        (IEnumerable<PurchaseOrderHeaderModel>, string) GetALL();
        (IEnumerable<PurchaseOrderHeaderModel>, string) GetByOrderStatus(string orderStatus);
        (PurchaseOrderHeaderModel, string) GetByID(long id);
    }
}

namespace ModelsLibrary.RepositoryInterfaces
{
    public interface IPurchaseOrderHeaderRepository
    {
        (PurchaseOrderHeaderModel, string) Insert(PurchaseOrderHeaderModel purchaseOrderHeader);
        string Update(PurchaseOrderHeaderModel purchaseOrderHeader);
        (IEnumerable<PurchaseOrderHeaderModel>, string) GetALL();
        (PurchaseOrderHeaderModel, string) GetByID(long id);
    }
}

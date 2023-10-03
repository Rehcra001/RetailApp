namespace ModelsLibrary.RepositoryInterfaces
{
    public interface IPurchaseOrderHeaderRepository
    {
        (PurchaseOrderHeaderModel, string) Insert(PurchaseOrderHeaderModel purchaseOrderHeader);
        string Update(PurchaseOrderHeaderModel purchaseOrderHeader);
        (IEnumerable<PurchaseOrderHeaderModel>, string) GetALL();
        (IEnumerable<PurchaseOrderHeaderModel>, string) GetByOrderStatusID(int id);
        (IEnumerable<PurchaseOrderHeaderModel>, string) GetByVendorID(int id);
        (PurchaseOrderHeaderModel, string) GetByID(long id);
    }
}

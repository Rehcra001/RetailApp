using ModelsLibrary;

namespace BussinessLogicLibrary.Purchases
{
    public interface IPurchaseOrderManager
    {
        void AddOrderLine(PurchaseOrderHeaderModel purchase);
        bool CanEditOrderLines(PurchaseOrderHeaderModel purchase);
        PurchaseOrderHeaderModel GetByID(long id);
        void SaveChanges(PurchaseOrderHeaderModel purchase);
    }
}
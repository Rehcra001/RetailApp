using ModelsLibrary;

namespace BussinessLogicLibrary.Purchases
{
    public interface IUpdatePurchaseOrderManager
    {
        void Update(PurchaseOrderHeaderModel purchase);
    }
}
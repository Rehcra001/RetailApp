using ModelsLibrary;

namespace BussinessLogicLibrary.Purchases
{
    public interface IAddNewPurchaseOrderManager
    {
        PurchaseOrderHeaderModel Insert(PurchaseOrderHeaderModel purchaseOrder);
    }
}
using ModelsLibrary;

namespace BussinessLogicLibrary.Purchases
{
    public interface IGetPurchaseOrderManager
    {
        PurchaseOrderHeaderModel GetByID(long id);
    }
}
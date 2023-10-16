using ModelsLibrary;

namespace BussinessLogicLibrary.Purchases
{
    public interface IPurchaseOrdersListManager
    {
        IEnumerable<PurchaseOrderHeaderModel> GetAll();
        IEnumerable<PurchaseOrderHeaderModel> GetByOrderStatusID(int id);
        IEnumerable<PurchaseOrderHeaderModel> GetByVendorID(int id);
    }
}
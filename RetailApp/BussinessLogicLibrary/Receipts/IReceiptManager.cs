using ModelsLibrary;

namespace BussinessLogicLibrary.Receipts
{
    public interface IReceiptManager
    {
        IEnumerable<ReceiptModel> GetByPurchaseOrderID(long id);
        void Insert(IEnumerable<ReceiptingLineModel> receiptLines);
        void Reverse(int id);
    }
}
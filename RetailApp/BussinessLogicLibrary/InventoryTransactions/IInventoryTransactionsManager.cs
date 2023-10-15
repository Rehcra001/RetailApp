using ModelsLibrary;

namespace BussinessLogicLibrary.InventoryTransactions
{
    public interface IInventoryTransactionsManager
    {
        IEnumerable<InventoryTransactionModel> GetByProductID(int id);
    }
}
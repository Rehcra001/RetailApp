using ModelsLibrary;
using ModelsLibrary.RepositoryInterfaces;

namespace BussinessLogicLibrary.InventoryTransactions
{
    public class InventoryTransactionsManager : IInventoryTransactionsManager
    {
        private readonly IInventoryTransactionRepository _inventoryTransactionRepository;

        public InventoryTransactionsManager(IInventoryTransactionRepository inventoryTransactionRepository)
        {
            _inventoryTransactionRepository = inventoryTransactionRepository;
        }

        public IEnumerable<InventoryTransactionModel> GetByProductID(int id)
        {
            Tuple<IEnumerable<InventoryTransactionModel>, string> transactions = _inventoryTransactionRepository.GetByProductID(id).ToTuple();

            //check if error raised by datasource
            if (transactions.Item2 == null)
            {
                //No errors
                return transactions.Item1;
            }
            else
            {
                //error raised
                throw new Exception(transactions.Item2);
            }
        }
    }
}

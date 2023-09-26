using DataAccessLibrary.InventoryTransactionRepository;
using ModelsLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLogicLibrary.InventoryTransactions
{
    public class InventoryTransactionsManager
    {
        private readonly string _connectionString;
        private IEnumerable<InventoryTransactionModel> _inventoryTransactions;

        public InventoryTransactionsManager(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IEnumerable<InventoryTransactionModel> GetByProductID(int id)
        {
            Tuple<IEnumerable<InventoryTransactionModel>, string> transactions = new InventoryTransactionRepository(_connectionString).GetByProductID(id).ToTuple();

            //check if error raised by datasource
            if (transactions.Item2 == null)
            {
                //No errors
                _inventoryTransactions = transactions.Item1;
            }
            else
            {
                //error raised
                throw new Exception(transactions.Item2);
            }

            return _inventoryTransactions;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelsLibrary.RepositoryInterfaces
{
    public interface IInventoryTransactionRepository
    {
        (IEnumerable<InventoryTransactionModel>, string) GetByProductID(int id);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelsLibrary.RepositoryInterfaces
{
    public interface ISalesOrderHeaderRepository
    {
        (SalesOrderHeaderModel, string) Insert(SalesOrderHeaderModel salesOrderHeader);
        string Update(SalesOrderHeaderModel salesOrderHeader);
        (IEnumerable<SalesOrderHeaderModel>, string) GetAll();
        (SalesOrderHeaderModel, string) GetBySalesOrderID(long id);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelsLibrary.RepositoryInterfaces
{
    public interface IPurchaseOrderDetailRepository
    {
        string Insert(PurchaseOrderDetailModel purchaseOrderDetail);
        string Update(PurchaseOrderDetailModel purchaseOrderDetail);
        (IEnumerable<PurchaseOrderDetailModel>, string) GetAll();
        (IEnumerable<PurchaseOrderDetailModel>, string) GetByPurchaseOrderID(long id);
    }
}

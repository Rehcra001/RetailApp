using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelsLibrary.RepositoryInterfaces
{
    public interface IOrderStatusRepository
    {
        (IEnumerable<OrderStatusModel>, string) GetAll();
        (OrderStatusModel, string) GetByID(int id);
    }
}

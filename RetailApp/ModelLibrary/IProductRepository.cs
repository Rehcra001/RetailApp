using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelsLibrary
{
    public interface IProductRepository
    {
        (ProductModel, string) Insert(ProductModel product);
        string Update(ProductModel product);
        (IList<ProductModel>, string) GetAll();
        (ProductModel, string) GetByID(int id);
    }
}

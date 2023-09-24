using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelsLibrary.RepositoryInterfaces
{
    public interface ICategoryRepository
    {
        (CategoryModel, string) Insert(CategoryModel category);
        string Update(CategoryModel category);
        (IEnumerable<CategoryModel>, string) GetAll();
        (CategoryModel, string) GetByID(int id);
    }
}

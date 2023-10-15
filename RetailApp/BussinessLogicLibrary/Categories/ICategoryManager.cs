using ModelsLibrary;

namespace BussinessLogicLibrary.Categories
{
    public interface ICategoryManager
    {
        IEnumerable<CategoryModel> GetAll();
        CategoryModel GetByID(int id);
        CategoryModel Insert(CategoryModel category);
        void Update(CategoryModel category);
    }
}
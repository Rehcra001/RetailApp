using ModelsLibrary;

namespace BussinessLogicLibrary.Products
{
    public interface IProductManager
    {
        ProductModel GetByID(int id);
        ProductModel Insert(ProductModel product);
        void Update(ProductModel product);
    }
}
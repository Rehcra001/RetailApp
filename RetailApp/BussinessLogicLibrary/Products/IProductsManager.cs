using ModelsLibrary;

namespace BussinessLogicLibrary.Products
{
    public interface IProductsManager
    {
        IEnumerable<ProductModel> GetAll();
        IEnumerable<ProductModel> GetByVendorID(int id);
    }
}
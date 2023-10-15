using BussinessLogicLibrary.Categories;
using BussinessLogicLibrary.UnitPers;
using BussinessLogicLibrary.Vendors;
using ModelsLibrary;
using ModelsLibrary.RepositoryInterfaces;

namespace BussinessLogicLibrary.Products
{
    public class ProductsManager : IProductsManager
    {
        private readonly IProductRepository _productRepository;
        private readonly IVendorManager _vendorManager;
        private readonly ICategoryManager _categoryManager;
        private readonly IUnitPerManager _unitPerManager;


        public ProductsManager(IProductRepository productRepository,
                               IVendorManager vendorManager,
                               ICategoryManager categoryManager,
                               IUnitPerManager unitPerManager)
        {
            _productRepository = productRepository;
            _vendorManager = vendorManager;
            _categoryManager = categoryManager;
            _unitPerManager = unitPerManager;
        }

        /// <summary>
        /// Gets a list of Products and their related 
        /// Vendor and unitPer
        /// </summary>
        /// <returns>
        /// Returns an IEnumerable</ProductModel>
        /// </returns>
        public IEnumerable<ProductModel> GetAll()
        {
            List<ProductModel> products = new List<ProductModel>();
            //Fills the _products with products, vendors and units
            products = GetProducts(products).ToList();
            products = GetVendors(products).ToList();
            products = GetUnitsPer(products).ToList();
            products = GetCategories(products).ToList();

            return products;
        }

        public IEnumerable<ProductModel> GetByVendorID(int id)
        {
            Tuple<IEnumerable<ProductModel>, string> products = _productRepository.GetByVendorID(id).ToTuple();


            //Check for errors
            if (products.Item2 == null)
            {
                //No errors
                return products.Item1;
            }
            else
            {
                //Error with Products
                throw new Exception(products.Item2);
            }
        }

        private IEnumerable<ProductModel> GetCategories(List<ProductModel> products)
        {
            try
            {
                IEnumerable<CategoryModel> categories = _categoryManager.GetAll();

                foreach (ProductModel product in products!)
                {
                    product.Category = categories.First(c => c.CategoryID == product.CategoryID);
                }
                return products;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private IEnumerable<ProductModel> GetUnitsPer(List<ProductModel> products)
        {
            try
            {
                IEnumerable<UnitsPerModel> unitPers = _unitPerManager.GetAll();

                foreach (ProductModel product in products!)
                {
                    product.Unit = unitPers.First(u => u.UnitPerID == product.UnitPerID);
                }
                return products;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private IEnumerable<ProductModel> GetVendors(List<ProductModel> products)
        {
            try
            {
                IEnumerable<VendorModel> vendors = _vendorManager.GetAll();

                foreach (ProductModel product in products!)
                {
                    product.Vendor = vendors.First(v => v.VendorID == product.VendorID);
                }
                return products;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private IEnumerable<ProductModel> GetProducts(List<ProductModel> products)
        {
            Tuple<IEnumerable<ProductModel>, string> getProducts = _productRepository.GetAll().ToTuple();


            //Check for errors
            if (getProducts.Item2 == null)
            {
                //No errors
                return getProducts.Item1;
            }
            else
            {
                //Error with Products
                throw new Exception(getProducts.Item2);
            }
        }
    }
}

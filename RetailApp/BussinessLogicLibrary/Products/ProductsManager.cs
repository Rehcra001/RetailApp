using BussinessLogicLibrary.Categories;
using BussinessLogicLibrary.UnitPers;
using BussinessLogicLibrary.Vendors;
using DataAccessLibrary.CategoryRepository;
using DataAccessLibrary.ProductRepository;
using DataAccessLibrary.UnitsPerRepository;
using DataAccessLibrary.VendorRepository;
using ModelsLibrary;
using ModelsLibrary.RepositoryInterfaces;

namespace BussinessLogicLibrary.Products
{
    public class ProductsManager
    {
        private IEnumerable<ProductModel>? _products;
        private readonly string _connectionString;
        private readonly IVendorManager _vendorManager;
        private readonly ICategoryManager _categoryManager;
        private readonly IUnitPerManager _unitPerManager;


        public ProductsManager(string connectionString,
                               IVendorManager vendorManager,
                               ICategoryManager categoryManager,
                               IUnitPerManager unitPerManager)
        {
            _connectionString = connectionString;
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
            //Fills the _products with products, vendors and units
            GetProducts();
            GetVendors();
            GetUnitsPer();
            GetCategories();

            return _products;
        }

        public IEnumerable<ProductModel> GetByVendorID(int id)
        {
            Tuple<IEnumerable<ProductModel>, string> products = new ProductRepository(_connectionString).GetByVendorID(id).ToTuple();


            //Check for errors
            if (products.Item2 == null)
            {
                //No errors
                _products = products.Item1;
            }
            else
            {
                //Error with Products
                throw new Exception(products.Item2);
            }

            return _products;
        }

        private void GetCategories()
        {
            try
            {
                IEnumerable<CategoryModel> categories = _categoryManager.GetAll();

                foreach (ProductModel product in _products!)
                {
                    product.Category = categories.First(c => c.CategoryID == product.CategoryID);
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void GetUnitsPer()
        {
            try
            {
                IEnumerable<UnitsPerModel> unitPers = _unitPerManager.GetAll();

                foreach (ProductModel product in _products!)
                {
                    product.Unit = unitPers.First(u => u.UnitPerID == product.UnitPerID);
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void GetVendors()
        {
            try
            {
                IEnumerable<VendorModel> vendors = _vendorManager.GetAll();

                foreach (ProductModel product in _products!)
                {
                    product.Vendor = vendors.First(v => v.VendorID == product.VendorID);
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void GetProducts()
        {
            Tuple<IEnumerable<ProductModel>, string> products = new ProductRepository(_connectionString).GetAll().ToTuple();


            //Check for errors
            if (products.Item2 == null)
            {
                //No errors
                _products = products.Item1;
            }
            else
            {
                //Error with Products
                throw new Exception(products.Item2);
            }
        }
    }
}

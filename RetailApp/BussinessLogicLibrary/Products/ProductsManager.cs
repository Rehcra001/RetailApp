using DataAccessLibrary.CategoryRepository;
using DataAccessLibrary.ProductRepository;
using DataAccessLibrary.UnitsPerRepository;
using DataAccessLibrary.VendorRepository;
using ModelsLibrary;

namespace BussinessLogicLibrary.Products
{
    public class ProductsManager
    {
        private IEnumerable<ProductModel>? _products;
        private readonly string _connectionString;


        public ProductsManager(string connectionString)
        {
            _connectionString = connectionString;
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

        private void GetCategories()
        {
            Tuple<IEnumerable<CategoryModel>, string> categories = new CategoryRepository(_connectionString).GetAll().ToTuple();

            if (categories.Item2 == null)
            {
                //No errors
                foreach (ProductModel product in _products)
                {
                    product.Category = categories.Item1.First(c => c.CategoryID == product.CategoryID);
                }
            }
            else
            {
                //Error retrieving categories
                throw new Exception(categories.Item2);
            }
        }

        private void GetUnitsPer()
        {
            Tuple<IEnumerable<UnitsPerModel>, string> unitPers = new UnitsPerRepository(_connectionString).GetAll().ToTuple();

            if (unitPers.Item2 == null)
            {
                //No Error
                foreach (ProductModel product in _products)
                {
                    product.Unit = unitPers.Item1.First(u => u.UnitPerID == product.UnitPerID);
                }
            }
            else
            {
                //error with unitPers
                throw new Exception(unitPers.Item2);
            }
        }

        private void GetVendors()
        {
            Tuple<IEnumerable<VendorModel>, string> vendors = new VendorRepository(_connectionString).GetAll().ToTuple();

            if (vendors.Item2 == null)
            {
                foreach (ProductModel product in _products)
                {
                    //Add vendor detail to each product in products
                    product.Vendor = vendors.Item1.First(v => v.VendorID == product.VendorID);
                }
            }
            else
            {
                //Error with vendors
                throw new Exception(vendors.Item2);
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

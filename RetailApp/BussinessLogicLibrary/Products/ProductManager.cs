using DataAccessLibrary.ProductRepository;
using DataAccessLibrary.VendorRepository;
using DataAccessLibrary.UnitsPerRepository;
using ModelsLibrary;

namespace BussinessLogicLibrary.Products
{
    public class ProductManager
    {
        private IEnumerable<ProductModel>? _products;
        private ProductModel? _product;
        private string _connectionString;



        public ProductManager(string connectionString)
        {
            _connectionString = connectionString;
        }


        public IEnumerable<ProductModel> GetAll()
        {
            Tuple<IEnumerable<ProductModel>, string> products = new ProductRepository(_connectionString).GetAll().ToTuple();
            Tuple<IEnumerable<VendorModel>, string> vendors = new VendorRepository(_connectionString).GetAll().ToTuple();
            Tuple<IEnumerable<UnitsPerModel>, string> unitPers = new UnitsPerRepository(_connectionString).GetAll().ToTuple();

            //Check for errors
            if (products.Item2 == null)
            {
                //No errors
                _products = products.Item1;

                if (vendors.Item2 == null)
                {
                    foreach (ProductModel product in _products)
                    {
                        //Add vendor detail to each product in products
                        product.Vendor = vendors.Item1.First(v => v.VendorID == product.VendorID);
                    }

                    if(unitPers.Item2 == null)
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
                else
                {
                    //Error with vendors
                    throw new Exception(vendors.Item2);
                }
                
            }
            else
            {
                //Error with Products
                throw new Exception(products.Item2);
            }
            return _products;
        }
    }
}

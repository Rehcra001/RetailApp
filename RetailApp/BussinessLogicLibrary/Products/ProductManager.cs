using DataAccessLibrary.ProductRepository;
using ModelsLibrary;

namespace BussinessLogicLibrary.Products
{
    /// <summary>
    /// Managers a single product model
    /// </summary>
    public class ProductManager
    {
		private string _connectionString;
		public ProductModel Product { get; set; } = new ProductModel();

        public ProductManager(string connectionString)
        {
			_connectionString = connectionString;
        }

        /// <summary>
        /// Retrieve's an existing product with Vendor, unit and transactions detail using the product id
        /// </summary>
        /// <param name="id">
        /// The unique product id
        /// </param>
        public void GetByID(int id)
		{
            throw new NotImplementedException();

			// TODO - ProductManage GetByID method still needs to be implemented

			// TODO - Transactions list still to be implemented
        }

		public void Update(VendorModel vendor, UnitsPerModel unitsPer)
		{
            throw new NotImplementedException();

			// TODO - Update method still needs to be implemented
        }

		public void Insert(VendorModel vendor, UnitsPerModel unitsPer, CategoryModel category)
		{
			//Add the vendor and unitsPer id
			Product.VendorID = vendor.VendorID;
			Product.UnitPerID = unitsPer.UnitPerID;
			Product.CategoryID = category.CategoryID;

			//Validate the model
			if (Product.Validate())
			{
				Tuple<ProductModel, string> product = new ProductRepository(_connectionString).Insert(Product).ToTuple();

				//Check for insert errors
				if (product.Item2 == null) //null if no error inserting the new product
				{
					Product = product.Item1;
				}
				else
				{
					//Error inserting the new product
					throw new Exception(product.Item2);
				}
			}
			else
			{
				throw new Exception(Product.ValidationMessage);
			}
        }
    }
}

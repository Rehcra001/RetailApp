using DataAccessLibrary.CategoryRepository;
using DataAccessLibrary.InventoryTransactionRepository;
using DataAccessLibrary.ProductRepository;
using DataAccessLibrary.UnitsPerRepository;
using DataAccessLibrary.VendorRepository;
using ModelsLibrary;
using ModelsLibrary.RepositoryInterfaces;
using System.ComponentModel.DataAnnotations;

namespace BussinessLogicLibrary.Products
{
    /// <summary>
    /// Managers a single product model
    /// </summary>
    public class ProductManager
    {
		private string _connectionString;
        private IVendorRepository _vendorRepository;
		public ProductModel Product { get; set; } = new ProductModel();

        public ProductManager(string connectionString, IVendorRepository vendorRepository)
        {
			_connectionString = connectionString;
            _vendorRepository = vendorRepository;
        }

        #region Retrieve a product
        /// <summary>
        /// Retrieve's an existing product with Vendor, unit and transactions detail using the product id
        /// </summary>
        /// <param name="id">
        /// The unique product id
        /// </param>
        public ProductModel GetByID(int id)
		{
			Tuple<ProductModel, string> product = new ProductRepository(_connectionString).GetByID(id).ToTuple();

			//check for errors
			if (product.Item2 == null)
			{
				Product = product.Item1;
				GetVendor();
				GetCategory();
                GetUnitPer();
				GetTransactions();
			}
			else
			{
                //Error retrieving the product
                throw new Exception(product.Item2);
            }

            return Product;
        }

        /// <summary>
        /// Retrieves the transactions for this product
        /// </summary>
        /// <exception cref="Exception"></exception>
        private void GetTransactions()
        {
            Tuple<IEnumerable<InventoryTransactionModel>, string> transactions = new InventoryTransactionRepository(_connectionString).GetByProductID(Product.ProductID).ToTuple();

            //check for errors
            if (transactions.Item2 == null)
            {
                //No Error
                Product.InventoryTransactions = transactions.Item1;
            }
            else
            {
                //error
                throw new Exception(transactions.Item2);
            }
        }

        /// <summary>
        /// Retrieves the category for this product
        /// </summary>
        /// <exception cref="Exception"></exception>
        private void GetCategory()
        {
            Tuple<CategoryModel, string> category = new CategoryRepository(_connectionString).GetByID(Product.CategoryID).ToTuple();

            if (category.Item2 == null)
            {
                //No errors
                Product.Category = category.Item1;
            }
            else
            {
                //Error retrieving categories
                throw new Exception(category.Item2);
            }
        }

        /// <summary>
        /// Retrieves the unit per for this product
        /// </summary>
        /// <exception cref="Exception"></exception>
        private void GetUnitPer()
        {
            Tuple<UnitsPerModel, string> unitPer = new UnitsPerRepository(_connectionString).GetByID(Product.UnitPerID).ToTuple();

            if (unitPer.Item2 == null)
            {
                //No Error
                Product.Unit = unitPer.Item1;
            }
            else
            {
                //error with unitPers
                throw new Exception(unitPer.Item2);
            }
        }

        /// <summary>
        /// Retrieves the Vendor for this product
        /// </summary>
        /// <exception cref="Exception"></exception>
        private void GetVendor()
        {
            Tuple<VendorModel, string> vendor = _vendorRepository.GetByID(Product.VendorID).ToTuple();

            if (vendor.Item2 == null)
            {
                Product.Vendor = vendor.Item1;
            }
            else
            {
                //Error with vendors
                throw new Exception(vendor.Item2);
            }
        }
        #endregion Retrieve a product


        public void Update()
		{
            //Update Product
            if (Product.VendorID != Product.Vendor.VendorID)
            {
                Product.VendorID = Product.Vendor.VendorID;
            }

            if (Product.UnitPerID != Product.Unit.UnitPerID)
            {
                Product.UnitPerID = Product.Unit.UnitPerID;
            }

            if (Product.CategoryID != Product.Category.CategoryID)
            {
                Product.CategoryID = Product.Category.CategoryID;
            }

            //Validate the model
            if (Product.Validate())
            {
                string errorMessage = new ProductRepository(_connectionString).Update(Product);

                //Check for insert errors
                if (errorMessage != null) //null if no error inserting the new product
                {
                    //Error inserting the new product
                    throw new Exception(errorMessage);
                }
            }
            else
            {
                throw new Exception(Product.ValidationMessage);
            }
        }


        /// <summary>
        /// Inserts a new Product into the database
        /// </summary>
        /// <param name="vendor">
        /// Requires a vendor model
        /// </param>
        /// <param name="unitsPer">
        /// Requires a UnitsPer model
        /// </param>
        /// <param name="category">
        /// Requires a Category model
        /// </param>
        /// <exception cref="Exception"></exception>
		public void Insert()
		{
			//Add the vendor and unitsPer id
			Product.VendorID = Product.Vendor.VendorID;
			Product.UnitPerID = Product.Unit.UnitPerID;
			Product.CategoryID = Product.Category.CategoryID;

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

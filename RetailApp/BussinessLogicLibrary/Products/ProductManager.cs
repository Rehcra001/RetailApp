using BussinessLogicLibrary.Categories;
using BussinessLogicLibrary.InventoryTransactions;
using BussinessLogicLibrary.UnitPers;
using BussinessLogicLibrary.Vendors;
using DataAccessLibrary.InventoryTransactionRepository;
using DataAccessLibrary.ProductRepository;
using ModelsLibrary;
using ModelsLibrary.RepositoryInterfaces;

namespace BussinessLogicLibrary.Products
{
    /// <summary>
    /// Managers a single product model
    /// </summary>
    public class ProductManager
    {
		private string _connectionString;
        private readonly IVendorManager _vendorManager;
        private readonly ICategoryManager _categoryManager;
        private readonly IUnitPerManager _unitPerManager;
        private readonly IInventoryTransactionsManager _inventoryTransactionManager;

		public ProductModel Product { get; set; } = new ProductModel();

        public ProductManager(string connectionString,
                              IVendorManager vendorManager,
                              ICategoryManager categoryManager,
                              IUnitPerManager unitPerManager,
                              IInventoryTransactionsManager inventoryTransactionsManager)
        {
			_connectionString = connectionString;
            _vendorManager = vendorManager;
            _categoryManager = categoryManager;
            _unitPerManager = unitPerManager;
            _inventoryTransactionManager = inventoryTransactionsManager;
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
            try
            {
                Product.InventoryTransactions = _inventoryTransactionManager.GetByProductID(Product.ProductID);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Retrieves the category for this product
        /// </summary>
        /// <exception cref="Exception"></exception>
        private void GetCategory()
        {
            try
            {
                Product.Category = _categoryManager.GetByID(Product.CategoryID);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Retrieves the unit per for this product
        /// </summary>
        /// <exception cref="Exception"></exception>
        private void GetUnitPer()
        {
            try
            {
                Product.Unit = _unitPerManager.GetByID(Product.UnitPerID);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Retrieves the Vendor for this product
        /// </summary>
        /// <exception cref="Exception"></exception>
        private void GetVendor()
        {
            try
            {
                Product.Vendor = _vendorManager.GetByID(Product.VendorID);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
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

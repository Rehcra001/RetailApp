using BussinessLogicLibrary.Categories;
using BussinessLogicLibrary.InventoryTransactions;
using BussinessLogicLibrary.UnitPers;
using BussinessLogicLibrary.Vendors;
using ModelsLibrary;
using ModelsLibrary.RepositoryInterfaces;

namespace BussinessLogicLibrary.Products
{
    /// <summary>
    /// Managers a single product model
    /// </summary>
    public class ProductManager : IProductManager
    {
        private readonly IProductRepository _productRepository;
        private readonly IVendorManager _vendorManager;
        private readonly ICategoryManager _categoryManager;
        private readonly IUnitPerManager _unitPerManager;
        private readonly IInventoryTransactionsManager _inventoryTransactionManager;

        public ProductManager(IProductRepository productRepository,
                              IVendorManager vendorManager,
                              ICategoryManager categoryManager,
                              IUnitPerManager unitPerManager,
                              IInventoryTransactionsManager inventoryTransactionsManager)
        {
            _productRepository = productRepository;
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
            Tuple<ProductModel, string> product = _productRepository.GetByID(id).ToTuple();

            //check for errors
            if (product.Item2 == null)
            {
                GetVendor(product.Item1);
                GetCategory(product.Item1);
                GetUnitPer(product.Item1);
                GetTransactions(product.Item1);

                return product.Item1;
            }
            else
            {
                //Error retrieving the product
                throw new Exception(product.Item2);
            }
        }

        /// <summary>
        /// Retrieves the transactions for this product
        /// </summary>
        /// <exception cref="Exception"></exception>
        private void GetTransactions(ProductModel product)
        {
            try
            {
                product.InventoryTransactions = _inventoryTransactionManager.GetByProductID(product.ProductID);
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
        private void GetCategory(ProductModel product)
        {
            try
            {
                product.Category = _categoryManager.GetByID(product.CategoryID);
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
        private void GetUnitPer(ProductModel product)
        {
            try
            {
                product.Unit = _unitPerManager.GetByID(product.UnitPerID);
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
        private void GetVendor(ProductModel product)
        {
            try
            {
                product.Vendor = _vendorManager.GetByID(product.VendorID);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion Retrieve a product


        public void Update(ProductModel product)
        {
            //Update Product
            if (product.VendorID != product.Vendor.VendorID)
            {
                product.VendorID = product.Vendor.VendorID;
            }

            if (product.UnitPerID != product.Unit.UnitPerID)
            {
                product.UnitPerID = product.Unit.UnitPerID;
            }

            if (product.CategoryID != product.Category.CategoryID)
            {
                product.CategoryID = product.Category.CategoryID;
            }

            //Check for validation errors
            if (product.Validate())
            {
                //No Validation errors
                string errorMessage = _productRepository.Update(product);

                //Check for insert errors
                if (errorMessage != null) //null if no error inserting the new product
                {
                    //Error inserting the new product
                    throw new Exception(errorMessage);
                }
            }
            else
            {
                //Validation errors
                throw new Exception(product.ValidationMessage);
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
		public ProductModel Insert(ProductModel product)
        {
            //Add the vendor and unitsPer id
            product.VendorID = product.Vendor.VendorID;
            product.UnitPerID = product.Unit.UnitPerID;
            product.CategoryID = product.Category.CategoryID;

            //Validate the model
            if (product.Validate())
            {
                Tuple<ProductModel, string> insertedProduct = _productRepository.Insert(product).ToTuple();

                //Check for insert errors
                if (insertedProduct.Item2 == null) //null if no error inserting the new product
                {
                    return insertedProduct.Item1;
                }
                else
                {
                    //Error inserting the new product
                    throw new Exception(insertedProduct.Item2);
                }
            }
            else
            {
                throw new Exception(product.ValidationMessage);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelsLibrary
{
    public class ProductModel
    {
        /// <summary>
        /// Unique product id key
        /// </summary>
        public int ProductID { get; set; }

        /// <summary>
        /// Holds the name of this product
        /// </summary>
        public string? ProductName { get; set; }

        /// <summary>
        /// Holds the product description
        /// </summary>
        public string? ProductDescription { get; set; }

        /// <summary>
        /// Holds the unique vendor id
        /// </summary>
        public int VendorID { get; set; }

        /// <summary>
        /// Holds the vendors product name beign purchased
        /// </summary>
        public string? VendorProductName { get; set; }

        /// <summary>
        /// Holds the current selling price of this product
        /// </summary>
        public decimal UnitPrice { get; set; }

        /// <summary>
        /// Holds the weighted average cost of this prod
        /// </summary>
        public decimal UnitCost { get; set; }

        /// <summary>
        /// Holds the total quantity of this product that is on hand
        /// </summary>
        public int OnHand { get; set; }

        /// <summary>
        /// Holds the total quantity of this product that is on order
        /// </summary>
        public int OnOrder { get; set; }

        /// <summary>
        /// Amount required based on generated sales orders
        /// </summary>
        public int SalesDemand { get; set; }

        /// <summary>
        /// Holds the reorder point of this product (quantity)
        /// </summary>
        public int ReorderPoint { get; set; }

        /// <summary>
        /// Unique unitPer id key
        /// </summary>
        public int UnitPerID { get; set; }

        /// <summary>
        /// Holds the unit weight of this product
        /// </summary>
        public double UnitWeight { get; set; }

        /// <summary>
        /// Holds if this product should be reordered when reorder point is reached
        /// </summary>
        public bool Obsolete { get; set; }

        /// <summary>
        /// Holds the vendor detail for this product
        /// </summary>
        public VendorModel? Vendor { get; set; }

        /// <summary>
        /// Holds the unit per for this product
        /// </summary>
        public UnitsModel? Unit { get; set; }

        // TODO - Add a list of product transactions

        public string? ValidationMessage { get; private set; }

        public bool Validate()
        {
            ValidationMessage = "";
            bool isValid = true;

            //Product Name
            if (string.IsNullOrWhiteSpace(ProductName))
            {
                ValidationMessage += "Product name is required.\r\n";
                isValid = false;
            }
            else if (ProductName.Length > 100)
            {
                ValidationMessage += "Product name cannot have more than 100 characters.\r\n";
                isValid = false;
            }

            //Product description
            if (string.IsNullOrWhiteSpace(ProductDescription))
            {
                ValidationMessage += "Product description is required.\r\n";
                isValid = false;
            }
            else if (ProductDescription.Length > 255)
            {
                ValidationMessage += "Product description cannot have more than 255 characters.\r\n";
                isValid = false;
            }

            //Vendor ID
            if (VendorID == 0)
            {
                ValidationMessage += "Vendor is required.\r\n";
                isValid = false;
            }

            //Vendor Product Name
            if (string.IsNullOrWhiteSpace(VendorProductName))
            {
                ValidationMessage += "Vendor Product Name is required.\r\n";
                isValid = false;
            }
            else if (VendorProductName.Length > 100)
            {
                ValidationMessage += "Vendor Product Name cannot have more than 100 characters.\r\n";
                isValid = false;
            }

            //Unit Price
            if (UnitPrice < 0)
            {
                ValidationMessage += "Unit price must be a positive value.\r\n";
                isValid = false;
            }

            //Unit Per ID
            if (UnitPerID == 0)
            {
                ValidationMessage += "Unit Per is required.\r\n";
                isValid = false;
            }

            //Unit Weight
            if (UnitWeight < 0)
            {
                ValidationMessage += "Unit Weight must be a positive value.\r\n";
                isValid = false;
            }


            return isValid;
        }
    }
}

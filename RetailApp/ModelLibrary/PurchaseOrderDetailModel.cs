using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelsLibrary
{
    public class PurchaseOrderDetailModel
    {
        /// <summary>
        /// purchase order ID links to the purchase order header
        /// </summary>
        public long PurchaseOrderID { get; set; }

        /// <summary>
        /// prouct ID links to the product ordered
        /// </summary>
        public int ProductID { get; set; }

        /// <summary>
        /// Quantity of the product ordered
        /// </summary>
        public int QuantityOrdered { get; set; }

        /// <summary>
        /// Cost per unit of product (kg, meter etc)
        /// </summary>
        public decimal UnitCost { get; set; }

        /// <summary>
        /// Freight cost per unit
        /// </summary>
        public decimal UnitFreightCost { get; set; }

        /// <summary>
        /// Holds the quantity receipted for this purchase order line
        /// </summary>
        public int QuantityReceipted { get; set; }

        /// <summary>
        /// Holds the id of the order line status
        /// </summary>
        public int OrderLineStatusID { get; set; }

        /// <summary>
        /// Holds the product detail for this purchase order line
        /// </summary>
        public ProductModel Product { get; set; } = new ProductModel();

        /// <summary>
        /// Holds the status detail for this purchase order line
        /// </summary>
        public StatusModel OrderLineStatus { get; set; } = new StatusModel();

        /// <summary>
        /// Holds the Validation errors if any for this purchase order detail
        /// </summary>
        public string? ValidationMessage { get; private set; }

        /// <summary>
        /// Validates the data entered into this purchase order detail.
        /// Populates ValidationMessage if any validation errors found
        /// </summary>
        /// <returns>
        /// Returns if all validations pass
        /// </returns>
        public bool ValidateAll()
        {
            ValidationMessage = "";
            bool isValid = true;

            if (PurchaseOrderID == 0)
            {
                ValidationMessage += "Purchase Order Header ID is required.\r\n";
                isValid = false;
            }
            else if (PurchaseOrderID < 0)
            {
                ValidationMessage += "Purchase Order ID must be positive\r\n";
                isValid = false;
            }

            if (ProductID == 0)
            {
                ValidationMessage += "Product ID is required.\r\n";
                isValid = false;
            }
            else if (ProductID < 0)
            {
                ValidationMessage += "Product ID must be positive\r\n";
                isValid = false;
            }

            if (QuantityOrdered == 0)
            {
                ValidationMessage += "Quantity is required\r\n";
                isValid = false;
            }
            else if (QuantityOrdered < 0)
            {
                ValidationMessage += "Quantity must be positive\r\n";
                isValid = false;
            }

            if (UnitCost == 0)
            {
                ValidationMessage += "Unit cost is required.\r\n";
                isValid = false;
            }
            else if (UnitCost < 0)
            {
                ValidationMessage += "Unit cost must be positive.\r\n";
                isValid = false;
            }

            if (UnitFreightCost < 0)
            {
                ValidationMessage += "Unit freight cost must be positive.\r\n";
                isValid = false;
            }

            if (OrderLineStatusID == 0)
            {
                ValidationMessage += "Purchase order line must have a status.\r\n";
                isValid = false;
            }

            return isValid;
        }

        /// <summary>
        /// Excludes the Purchase order ID when validating, 
        /// Populates the ValidationMessage property with any validation errors
        /// </summary>
        /// <returns>
        /// Returns true is all validations passed
        /// </returns>
        public bool ValidateExcludePurchaseOrderID()
        {
            ValidationMessage = "";
            bool isValid = true;

            if (ProductID == 0)
            {
                ValidationMessage += "Product ID is required.\r\n";
                isValid = false;
            }
            else if (ProductID < 0)
            {
                ValidationMessage += "Product ID must be positive\r\n";
                isValid = false;
            }

            if (QuantityOrdered == 0)
            {
                ValidationMessage += "Quantity is required\r\n";
                isValid = false;
            }
            else if (QuantityOrdered < 0)
            {
                ValidationMessage += "Quantity must be positive\r\n";
                isValid = false;
            }

            if (UnitCost == 0)
            {
                ValidationMessage += "Unit cost is required.\r\n";
                isValid = false;
            }
            else if (UnitCost < 0)
            {
                ValidationMessage += "Unit cost must be positive.\r\n";
                isValid = false;
            }

            if (UnitFreightCost < 0)
            {
                ValidationMessage += "Unit freight cost must be positive.\r\n";
                isValid = false;
            }

            if (OrderLineStatusID == 0)
            {
                ValidationMessage += "Purchase order line must have a status.\r\n";
                isValid = false;
            }

            return isValid;
        }
    }
}

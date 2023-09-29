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
        public int PurchaseOrderID { get; set; }

        /// <summary>
        /// prouct ID links to the product ordered
        /// </summary>
        public int ProductID { get; set; }

        /// <summary>
        /// Quantity of the product ordered
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// Cost per unit of product (kg, meter etc)
        /// </summary>
        public decimal UnitCost { get; set; }

        /// <summary>
        /// Freight cost per unit
        /// </summary>
        public decimal UnitFreightcost { get; set; }

        /// <summary>
        /// Indicates if this line is complete
        /// </summary>
        public bool LineFilled { get; set; }

        /// <summary>
        /// Holds the product detail for this purchase order detail
        /// </summary>
        public ProductModel Product { get; set; } = new ProductModel();

        /// <summary>
        /// Holds the Validation errors if any for this purchase order detail
        /// </summary>
        public string? ValidationMessage { get; set; }

        /// <summary>
        /// Validates the data entered into this purchase order detail.
        /// Populates ValidationMessage if any validation errors found
        /// </summary>
        /// <returns>
        /// Returns false if any validation errors found
        /// </returns>
        public bool Validate()
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

            if (Quantity == 0)
            {
                ValidationMessage += "Quantity is required\r\n";
                isValid = false;
            }
            else if (Quantity < 0)
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

            if (UnitFreightcost < 0)
            {
                ValidationMessage += "Unit freight cost must be positive.\r\n";
                isValid = false;
            }

            return isValid;
        }
    }
}

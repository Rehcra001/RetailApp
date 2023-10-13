using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ModelsLibrary
{
    public class ReceiptModel
    {
        /// <summary>
        /// Holds the unique receipt id
        /// </summary>
        public int ReceiptID { get; set; }

        /// <summary>
        /// Holds the purchase id related to this receipt
        /// </summary>
        public long PurchaseOrderID { get; set; }

        /// <summary>
        /// Holds the product id related to this receipt
        /// </summary>
        public int ProductID { get; set; }

        /// <summary>
        /// Holds the date the receipt was created
        /// </summary>
        public DateTime ReceiptDate { get; set; }

        /// <summary>
        /// Holds the quantity receipted
        /// </summary>
        public int QuantityReceipted { get; set; }

        /// <summary>
        /// holds the cost per unit receipted
        /// </summary>
        public decimal UnitCost { get; set; }

        /// <summary>
        /// Holds a reference to a reversal or reversed receipt
        /// will be zero if no reversal done
        /// </summary>
        public int ReverseReferenceID { get; set; }

        /// <summary>
        /// Holds the name of the product for this productID if needed
        /// </summary>
        public string? ProductName { get; set; }

        /// <summary>
        /// Holds validation error messages
        /// </summary>
        public string? ValidationMessage { get; set; }

        public bool Validate()
        {
            bool isValid = true;
            ValidationMessage = "";

            if (PurchaseOrderID == default)
            {
                ValidationMessage += "Purchase Order ID is required.";
                isValid = false;
            }

            if (ProductID == default)
            {
                ValidationMessage += "ProductID is required.";
                isValid = false;
            }

            if (QuantityReceipted == default)
            {
                ValidationMessage += "Quantity receipted is required.";
                isValid = true;
            }

            if (UnitCost == default)
            {
                ValidationMessage += "Unit cost is required.";
                isValid = false;
            }

            return isValid;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelsLibrary
{
    public class ReceiptingLineModel
    {
        public string? ProductName { get; set; }
        public long PurchaseOrderID { get; set; }
        public int ProductID { get; set; }
        public int QuantityOrdered { get; set; }
        public int QtyToReceipt { get; set; }
        public int QtyReceipted { get; set; }
        public decimal UnitCost { get; set; }

        public string? ValidationMessage { get; private set; }

        public bool Validate()
        {
            bool isValid = true;
            ValidationMessage = "";

            if (PurchaseOrderID == default)
            {
                ValidationMessage += "PurchaseOrderID is required.";
                isValid = false;
            }

            if (ProductID == default)
            {
                ValidationMessage += "ProductID is required.";
                isValid = false;
            }

            if (QtyToReceipt == default)
            {
                ValidationMessage += "Quantity to receipt is required.";
                isValid = false;
            }

            if (QtyToReceipt + QtyReceipted > QuantityOrdered)
            {
                ValidationMessage += "Cannot receipt more than quantity order.";
                isValid = false;
            }

            if (UnitCost == default)
            {
                ValidationMessage += "Unit Cost is required.";
                isValid = false;
            }

            return isValid;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelsLibrary
{
    public class InvoicingLineModel
    {
        public string? ProductName { get; set; }
        public long SalesOrderID { get; set; }
        public int ProductID { get; set; }
        public int QuantityOrdered { get; set; }
        public int QtyToInvoice { get; set; }
        public int QtyInvoiced { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Discount { get; set; }

        public string? ValidationMessage { get; private set; }

        public bool Validate()
        {
            bool isValid = true;
            ValidationMessage = "";

            if (SalesOrderID == default)
            {
                ValidationMessage += "PurchaseOrderID is required.";
                isValid = false;
            }

            if (ProductID == default)
            {
                ValidationMessage += "ProductID is required.";
                isValid = false;
            }

            if (QtyToInvoice == default)
            {
                ValidationMessage += "Quantity to receipt is required.";
                isValid = false;
            }

            if (QtyToInvoice + QtyInvoiced > QuantityOrdered)
            {
                ValidationMessage += "Cannot receipt more than quantity order.";
                isValid = false;
            }

            return isValid;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
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
        /// Hold the date the receipt was created
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
    }
}

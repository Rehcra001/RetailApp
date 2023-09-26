using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelsLibrary
{
    public class InventoryTransactionModel
    {
        /// <summary>
        /// Holds the unique transaction id
        /// </summary>
        public int TransactionID { get; set; }

        /// <summary>
        /// Holds the transaction type:
        ///     R for a product receipted
        ///     I for an product issued / invoiced
        /// </summary>
        public string? TransactionType { get; set; }

        /// <summary>
        /// Holds the date and time of the transaction
        /// </summary>
        public DateTime TransactionDate { get; set; }

        /// <summary>
        /// Holds the product id transacted on
        /// </summary>
        public int ProductID { get; set; }

        /// <summary>
        /// Holds the receipt id or goods issued id
        /// </summary>
        public int OrderID { get; set; }

        /// <summary>
        /// Holds the number of product transacted on
        /// </summary>
        public int Quantity { get; set; }
    }
}

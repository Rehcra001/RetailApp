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
    }
}

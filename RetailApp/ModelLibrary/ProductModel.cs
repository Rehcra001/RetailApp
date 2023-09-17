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
    }
}

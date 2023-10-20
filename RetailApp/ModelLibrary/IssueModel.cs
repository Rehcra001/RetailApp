﻿namespace ModelsLibrary
{
    public class IssueModel
    {
        /// <summary>
        /// Holds the unique reference ID of this goods issued line
        /// </summary>
        public int IssueID { get; set; }

        /// <summary>
        /// Holds the reference ID of the sales order line being issued
        /// </summary>
        public long SalesOrderID { get; set; }

        /// <summary>
        /// Holds the reference ID of the product issued
        /// </summary>
        public int ProductID { get; set; }

        /// <summary>
        /// Holds the date the goods issue was created
        /// </summary>
        public DateTime IssueDate { get; set; }

        /// <summary>
        /// Holds the quantity issued from stock of this product
        /// </summary>
        public int QuantityIssued { get; set; }

        /// <summary>
        /// Holds the weighted unit cost of the product issued
        /// </summary>
        public decimal UnitCost { get; set; }

        /// <summary>
        /// Holds the reference to the reversed issue ID
        /// </summary>
        public int ReverseReferenceID { get; set; }

        /// <summary>
        /// Holds any validation errors generated by Validate()
        /// </summary>
        public string ValidationMessage { get; private set; } = string.Empty;


        public bool Validate()
        {
            ValidationMessage = "";
            bool isValid = true;

            if (SalesOrderID == default)
            {
                ValidationMessage += "Sales Order ID is required.\r\n";
                isValid = false;
            }

            if (ProductID == default)
            {
                ValidationMessage += "Product ID is required.\r\n";
                isValid = false;
            }

            if (QuantityIssued == default)
            {
                ValidationMessage += "Quantity issued is required.\r\n";
            }

            return isValid;
        }
    }
}

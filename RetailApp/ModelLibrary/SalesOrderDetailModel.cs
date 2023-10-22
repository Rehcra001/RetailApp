namespace ModelsLibrary
{
    public class SalesOrderDetailModel
    {
        /// <summary>
        /// Holds the reference to the sales order
        /// </summary>
        public long SalesOrderID { get; set; }

        /// <summary>
        /// Hold the reference to the product 
        /// </summary>
        public int ProductID { get; set; }

        /// <summary>
        /// Holds the quantity of product to be sold
        /// </summary>
        public int QuantityOrdered { get; set; }

        /// <summary>
        /// Holds the unit price of the product to be sold
        /// </summary>
        public decimal UnitPrice { get; set; }

        /// <summary>
        /// Holds the weighted unit cost of the product to be sold
        /// </summary>
        public decimal UnitCost { get; set; }

        /// <summary>
        /// Holds the discount percentage applicable to this line
        /// </summary>
        public decimal Discount { get; set; }

        /// <summary>
        /// Holds the quantity invoiced
        /// </summary>
        public int QuantityInvoiced { get; set; }

        /// <summary>
        /// Holds the reference to the status of this line
        /// </summary>
        public int OrderLineStatusID { get; set; }

        /// <summary>
        /// Holds the product detail for this sales order line
        /// </summary>
        public ProductModel Product { get; set; } = new ProductModel();

        /// <summary>
        /// Holds the status detail for this sales order line
        /// </summary>
        public StatusModel OrderLineStatus { get; set; } = new StatusModel();

        /// <summary>
        /// Holds the validation error messages
        /// </summary>
        public string ValidationMessage { get; private set; } = string.Empty;

        /// <summary>
        /// Validates this model and fills the ValidationMessage property if any validation errors encountered
        /// </summary>
        /// <param name="excludeSalesOrderID">
        /// Takes in a bool: 
        /// True will exclude validation of the SalesOrderID. 
        /// False will include validation of the SalesOrderID
        /// </param>
        /// <returns>
        /// Returns true if valdation successful - false otherwise
        /// </returns>
        public bool Validate(bool excludeSalesOrderID = false)
        {
            ValidationMessage = "";
            bool isValid = true;

            if (!excludeSalesOrderID)
            {
                if (SalesOrderID == default)
                {
                    ValidationMessage += "Sales order ID is required.\r\n";
                    isValid = false;
                }
            }

            if (ProductID == default)
            {
                ValidationMessage += "Product ID is required.\r\n";
                isValid = false;
            }

            if (QuantityOrdered == default)
            {
                ValidationMessage += "Quantity is required.\r\n";
                isValid = false;
            }
            else if (QuantityOrdered < 0)
            {
                ValidationMessage += "Quantity must be a positive value\r\b";
                isValid = false;
            }

            if (UnitPrice == default)
            {
                ValidationMessage += "Unit Price is required.\r\n";
                isValid = false;
            }
            else if (UnitPrice < 0)
            {
                ValidationMessage += "Unit Price must be a positive value.\r\n";
                isValid = false;
            }

            if (UnitCost < 0)
            {
                ValidationMessage += "Unit Cost must be a positive value.\r\n";
                isValid = false;
            }

            if (Discount < 0 || Discount > 1)
            {
                ValidationMessage += "Discount may only be between 0 and 1.\r\n";
                isValid = false;
            }

            if (QuantityInvoiced < 0)
            {
                ValidationMessage += "Quantity Invoiced cannot be negative.\r\n";
                isValid = false;
            }

            if (OrderLineStatusID == default)
            {
                ValidationMessage += "Order line status ID is required.\r\n";
                isValid = false;
            }

            return isValid;
        }
    }
}

namespace ModelsLibrary
{
    public class SalesOrderHeaderModel
    {
        /// <summary>
        /// Holds the unique sales order ID
        /// </summary>
        public long SalesOrderID { get; set; }

        /// <summary>
        /// Holds the customer id
        /// </summary>
        public int CustomerID { get; set; }

        /// <summary>
        /// Holds the customers purchase order reference
        /// </summary>
        public string CustomerPurchaseOrder { get; set; } = string.Empty;

        /// <summary>
        /// Holds the date the order is created
        /// </summary>
        public DateTime OrderDate { get; set; }

        /// <summary>
        /// Holds the value of the order excl. VAT
        /// </summary>
        public decimal OrderAmount { get; set; }

        /// <summary>
        /// Holds the VAT percentage
        /// </summary>
        public decimal VATPercentage { get; set; }

        /// <summary>
        /// Holds the VAT value of the order
        /// </summary>
        public decimal VATAmount { get; set; }

        /// <summary>
        /// Holds the total value of the order incl. VAT
        /// </summary>
        public decimal TotalAmount { get; set; }

        /// <summary>
        /// Holds the delivery date to the customer
        /// </summary>
        public DateTime DeliveryDate { get; set; }

        /// <summary>
        /// Holds the status id of the order
        /// </summary>
        public int OrderStatusID { get; set; }

        /// <summary>
        /// Holds the customer details associated with this order
        /// </summary>
        public CustomerModel Customer { get; set; } = new CustomerModel();

        /// <summary>
        /// Holds a list of Sales order lines associated with this order
        /// </summary>
        public List<SalesOrderDetailModel> SalesOrderDetails { get; set; } = new List<SalesOrderDetailModel>();

        /// <summary>
        /// Holds the Order status of this order
        /// </summary>
        public StatusModel OrderStatus { get; set; } = new StatusModel();

        /// <summary>
        /// Holds any validation errors raised by Validate()
        /// </summary>
        public string ValidationMessage { get; private set; } = string.Empty;

        /// <summary>
        /// Validates this model and fills the ValidationMessage property if any validation errors encountered
        /// </summary>
        /// <returns>
        /// Returns true if valdation successful - false otherwise
        /// </returns>
        public bool Validate()
        {
            ValidationMessage = "";
            bool isValid = true;

            if (CustomerID == default)
            {
                ValidationMessage += "Customer ID is required.\r\n";
                isValid = false;
            }

            if (string.IsNullOrWhiteSpace(CustomerPurchaseOrder))
            {
                ValidationMessage += "Customer reference is required.\r\n";
                isValid = false;
            }

            if (OrderDate == default)
            {
                ValidationMessage += "Order date is required.\r\n";
                isValid = false;
            }

            if (OrderAmount == default)
            {
                ValidationMessage += "Order Amount is required.\r\n";
                isValid = false;
            }
            else if (OrderAmount < 0)
            {
                ValidationMessage += "Order amount must be positive.\r\n";
                isValid = false;
            }

            if (VATPercentage == default)
            {
                ValidationMessage += "VAT percentage is required.\r\n";
                isValid = false;
            }
            else if (VATPercentage < 0)
            {
                ValidationMessage += "VAT percentage may not be negative.\r\n";
                isValid = false;
            }
            else if (VATPercentage > 1)
            {
                ValidationMessage += "VAT percentage must be between 0 and 1.\r\n";
                isValid = false;
            }

            if (VATAmount == default)
            {
                ValidationMessage += "VAT Amount is required.\r\n";
                isValid = false;
            }
            else if (VATAmount < 0)
            {
                ValidationMessage += "VAT amount may not be negative.\r\n";
                isValid = false;
            }

            if (TotalAmount == default)
            {
                ValidationMessage += "Total amount is required.\r\n";
                isValid = false;
            }
            else if (TotalAmount < 0)
            {
                ValidationMessage += "Total amount may not be negative.\r\n";
                isValid = false;
            }

            if (DeliveryDate == default)
            {
                ValidationMessage += "Delivery date is required.\r\n";
                isValid = false;
            }
            else if (DeliveryDate < OrderDate)
            {
                ValidationMessage += "Delivery date may not be earlier than order date.\r\n";
                isValid = false;
            }

            if (OrderStatusID == default)
            {
                ValidationMessage += "Order Status ID is required.\r\n";
                isValid = false;
            }

            return isValid;
        }
    }
}

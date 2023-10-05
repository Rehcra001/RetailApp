﻿namespace ModelsLibrary
{
    public class PurchaseOrderHeaderModel
    {
        /// <summary>
        /// Holds the unique purchase order number generated by database
        /// </summary>
        public long PurchaseOrderID { get; set; }

        /// <summary>
        /// Holds the vendor ID (Key)
        /// </summary>
        public int VendorID { get; set; }

        /// <summary>
        /// The vendors sales id
        /// </summary>
        public string? VendorReference { get; set; }

        /// <summary>
        /// The date the order is created
        /// </summary>
        public DateTime OrderDate { get; set; }

        /// <summary>
        /// The total value of the products purchased
        /// </summary>
        public decimal OrderAmount { get; set; }

        /// <summary>
        /// VAT percentage
        /// </summary>
        public decimal VATPercentage { get; set; }

        /// <summary>
        /// The VAT on the order amount
        /// </summary>
        public decimal VATAmount { get; set; }

        /// <summary>
        /// The sum of the OrderAmount and VATAmount
        /// </summary>
        public decimal TotalAmount { get; set; }

        /// <summary>
        /// The date the products purchased are expected
        /// </summary>
        public DateTime? RequiredDate { get; set; } = null;

        /// <summary>
        /// Holds the status ID of this order
        /// </summary>
        public int OrderStatusID { get; set; }

        /// <summary>
        /// Flag to indicated if this purchase order is an import
        /// </summary>
        public bool IsImport { get; set; }

        /// <summary>
        /// Holds the Vendor detail associated with this purchase order
        /// </summary>
        public VendorModel Vendor { get; set; } = new VendorModel();

        /// <summary>
        /// Holds a list of purchase order details associated with this purchase order
        /// </summary>
        public List<PurchaseOrderDetailModel> PurchaseOrderDetails { get; set; } = new List<PurchaseOrderDetailModel>();

        /// <summary>
        /// Holds the Order status of this purchase order
        /// </summary>
        public OrderStatusModel OrderStatus { get; set; } = new OrderStatusModel();

        /// <summary>
        /// Holds the list of Receipts associated with this purchase order
        /// </summary>
        public IEnumerable<ReceiptModel> Receipts { get; set; } = new List<ReceiptModel>();

        /// <summary>
        /// Holds Validation error strings
        /// </summary>
        public string? ValidationMessage { get; set; }

        public bool Validate()
        {
            bool isValid = true;
            ValidationMessage = "";

            if (VendorID == default)
            {
                ValidationMessage += "Vendor ID is required.\r\n";
                isValid = false;
            }

            if (string.IsNullOrWhiteSpace(VendorReference))
            {
                ValidationMessage += "Vendor Reference is required.\r\n";
                isValid = false;
            }
            else if (VendorReference.Length > 20)
            {
                ValidationMessage += "Vendor Reference cannot contain more than 20 charcters.\r\n";
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
                ValidationMessage += "Order Amount must be positive.\r\n";
                isValid = false;
            }

            if (VATPercentage == default && !IsImport)
            {
                ValidationMessage += "VAT Percentage is required.\r\n";
                isValid = false;
            }

            if (VATPercentage > 0 && IsImport)
            {
                ValidationMessage += "VAT Percentage not required with import.\r\n";
                isValid = false;
            }

            if (VATPercentage < 0)
            {
                ValidationMessage += "VAT Percentage must be positive\r\n";
                isValid = false;
            }

            if (VATAmount == default && !IsImport)
            {
                ValidationMessage += "VAT Amount is required.\r\n";
                isValid = false;
            }

            if (VATAmount > 0 && IsImport)
            {
                ValidationMessage += "VAT Amount is not required with import.\r\n";
                isValid = false;
            }

            if (VATAmount < 0)
            {
                ValidationMessage += "VAT Amount must be positive.\r\n";
                isValid = false;
            }

            if (TotalAmount == default)
            {
                ValidationMessage += "Total Amount is required.\r\n";
                isValid = false;
            }

            if (RequiredDate == default)
            {
                ValidationMessage += "Required date is required.\r\n";
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

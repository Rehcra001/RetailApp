using DataAccessLibrary.PurchaseOrderDetailRepository;
using DataAccessLibrary.PurchaseOrderHeaderRepository;
using DataAccessLibrary.VATRepository;
using ModelsLibrary;

namespace BussinessLogicLibrary.Purchases
{
    public class PurchaseOrderManager
    {
        private string _connectionString;

        public PurchaseOrderHeaderModel PurchaseOrder { get; set; } = new PurchaseOrderHeaderModel();

        public PurchaseOrderManager(string connectionString)
        {
            _connectionString = connectionString;
        }

        /// <summary>
        /// Inserts this PurchaseOrder property into the database
        /// </summary>
        public void Insert()
        {
            //Check each purchase order detail in the list of purchase order details
            if (PurchaseOrder.PurchaseOrderDetails.Count == 0)
            {
                //no order details exist
                throw new Exception("A purchase order must contain at least one order line.");
            }
            else
            {
                //Does contain at least one order line
                //Validate each order line
                foreach (PurchaseOrderDetailModel orderLine in PurchaseOrder.PurchaseOrderDetails)
                {
                    //check if the product in the order is not null
                    if (orderLine.Product != default)
                    {
                        //not null
                        //Set productID of OrderLine
                        orderLine.ProductID = orderLine.Product.ProductID;
                    }
                    else
                    {
                        //Throw error
                        throw new Exception("A product is missing from the order line");
                    }

                    //No purchase order ID yet
                    //Validation to exclude purchase order ID                    
                    if (!orderLine.ValidateExcludePurchaseOrderID())
                    {
                        //Validation error
                        throw new Exception(orderLine.ValidationMessage);
                    }
                }

                //no validation errors on order lines
                //update purchase order header

                //Check Vendor
                if (PurchaseOrder.Vendor != default)
                {
                    //Vendor not null
                    PurchaseOrder.VendorID = PurchaseOrder.Vendor.VendorID;
                }
                else
                {
                    throw new Exception("Vendor is missing. Please add a vendor.");
                }

                //Check Order status
                if (PurchaseOrder.OrderStatus != default)
                {
                    //Order status not null
                    PurchaseOrder.OrderStatusID = PurchaseOrder.OrderStatus.OrderStatusID;
                }
                else
                {
                    //throw error
                    throw new Exception("Order Status is missing. Please add an order status.");
                }

                //Add total order amount excluding VAT
                PurchaseOrder.OrderAmount = PurchaseOrder.PurchaseOrderDetails.Sum(x => x.Quantity * (x.UnitCost + x.UnitFreightCost));

                //Check if import
                if (!PurchaseOrder.IsImport)
                {
                    //Calculate Vat amount
                    if (PurchaseOrder.VATPercentage != default)
                    {
                        PurchaseOrder.VATAmount = PurchaseOrder.OrderAmount * PurchaseOrder.VATPercentage;
                    }
                    else
                    {
                        //Add VAT Percentage
                        Tuple<VatModel, string> vat = new VATRepository(_connectionString).Get().ToTuple();
                        if (vat.Item2 == null)
                        {
                            //No errors
                            PurchaseOrder.VATPercentage = vat.Item1.VatDecimal;
                            PurchaseOrder.VATAmount = PurchaseOrder.OrderAmount * PurchaseOrder.VATPercentage;
                        }
                        else
                        {
                            throw new Exception(vat.Item2);
                        }
                    }

                    //Add Total Amount
                    PurchaseOrder.TotalAmount = PurchaseOrder.OrderAmount + PurchaseOrder.VATAmount; 
                }
                else
                {
                    //No vat if imported
                    PurchaseOrder.TotalAmount = PurchaseOrder.OrderAmount;
                }

                //Validate purchase order header before saving to database
                if (PurchaseOrder.Validate())
                {
                    Tuple<PurchaseOrderHeaderModel, string> purchaseHeader = new PurchaseOrderHeaderRepository(_connectionString).Insert(PurchaseOrder).ToTuple();
                    //Check for errors
                    if (purchaseHeader.Item2 == null)
                    {
                        //no errors
                        //add purchase order id
                        PurchaseOrder.PurchaseOrderID = purchaseHeader.Item1.PurchaseOrderID;
                    }
                    else
                    {
                        //error saving purchase order header
                        throw new Exception(purchaseHeader.Item2);
                    }
                }
                else
                {
                    //Validation errors
                    throw new Exception(PurchaseOrder.ValidationMessage);
                }

                //Update each order line with the purchase order ID and save
                foreach (PurchaseOrderDetailModel orderLine in PurchaseOrder.PurchaseOrderDetails)
                {
                    orderLine.PurchaseOrderID = PurchaseOrder.PurchaseOrderID;
                    
                    //Validate and save order line
                    if (orderLine.ValidateAll())
                    {
                        string errorMessage = new PurchaseOrderDetailRepository(_connectionString).Insert(orderLine);
                        //Check for errors
                        if (errorMessage != null)
                        {
                            //error raised
                            throw new Exception(errorMessage);
                        }
                        //Set CanProductChange to false as once saved it cannot be changed
                        orderLine.CanChangeProduct = false;
                    }
                    else
                    {
                        //Validation errors
                        throw new Exception(orderLine.ValidationMessage);
                    }
                }
            }
        }

        /// <summary>
        /// Will use this PurchaseOrder property to update the 
        /// corresponding data in the data base
        /// </summary>
        public void Update()
        {

        }

        /// <summary>
        /// Will set this PurchaseOrder property to the one 
        /// retrieved from the database with the id passed in
        /// </summary>
        /// <param name="id">
        /// The purchase order ID of an existing purchase order
        /// </param>
        public void GetByID(long id)
        {

        }
    }
}

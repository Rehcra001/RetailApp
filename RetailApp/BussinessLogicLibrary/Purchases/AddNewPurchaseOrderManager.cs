using BussinessLogicLibrary.VAT;
using ModelsLibrary;
using ModelsLibrary.RepositoryInterfaces;

namespace BussinessLogicLibrary.Purchases
{
    public class AddNewPurchaseOrderManager : IAddNewPurchaseOrderManager
    {
        private readonly IPurchaseOrderHeaderRepository _purchaseOrderHeaderRepository;
        private readonly IPurchaseOrderDetailRepository _purchaseOrderDetailRepository;
        private readonly IVATManager _vatManager;

        //public PurchaseOrderHeaderModel PurchaseOrder { get; set; } = new PurchaseOrderHeaderModel();

        public AddNewPurchaseOrderManager(IPurchaseOrderHeaderRepository purchaseOrderHeaderRepository,
                                          IPurchaseOrderDetailRepository purchaseOrderDetailRepository,
                                          IVATManager vatManager)
        {
            _purchaseOrderHeaderRepository = purchaseOrderHeaderRepository;
            _purchaseOrderDetailRepository = purchaseOrderDetailRepository;
            _vatManager = vatManager;
        }

        /// <summary>
        /// Inserts this PurchaseOrder property into the database
        /// </summary>
        public PurchaseOrderHeaderModel Insert(PurchaseOrderHeaderModel purchaseOrder)
        {
            //Check each purchase order detail in the list of purchase order details
            if (purchaseOrder.PurchaseOrderDetails.Count == 0)
            {
                //no order details exist
                throw new Exception("A purchase order must contain at least one order line.");
            }
            else
            {
                //Does contain at least one order line
                //Validate each order line
                foreach (PurchaseOrderDetailModel orderLine in purchaseOrder.PurchaseOrderDetails)
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
                if (purchaseOrder.Vendor != default)
                {
                    //Vendor not null
                    purchaseOrder.VendorID = purchaseOrder.Vendor.VendorID;
                }
                else
                {
                    throw new Exception("Vendor is missing. Please add a vendor.");
                }

                //Check Order status
                if (purchaseOrder.OrderStatus != default)
                {
                    //Order status not null
                    purchaseOrder.OrderStatusID = purchaseOrder.OrderStatus.StatusID;
                }
                else
                {
                    //throw error
                    throw new Exception("Order Status is missing. Please add an order status.");
                }

                //Add total order amount excluding VAT
                purchaseOrder.OrderAmount = purchaseOrder.PurchaseOrderDetails.Sum(x => x.QuantityOrdered * (x.UnitCost + x.UnitFreightCost));

                //Check if import
                if (!purchaseOrder.IsImport)
                {
                    //Calculate Vat amount
                    if (purchaseOrder.VATPercentage != default)
                    {
                        purchaseOrder.VATAmount = purchaseOrder.OrderAmount * purchaseOrder.VATPercentage;
                    }
                    else
                    {
                        try
                        {
                            purchaseOrder.VATPercentage = _vatManager.Get().VatDecimal;
                            purchaseOrder.VATAmount = purchaseOrder.OrderAmount * purchaseOrder.VATPercentage;
                        }
                        catch (Exception ex)
                        {
                            throw new Exception(ex.Message);
                        }
                    }

                    //Add Total Amount
                    purchaseOrder.TotalAmount = purchaseOrder.OrderAmount + purchaseOrder.VATAmount;
                }
                else
                {
                    //No vat if imported
                    purchaseOrder.TotalAmount = purchaseOrder.OrderAmount;
                }

                //Validate purchase order header before saving to database
                if (purchaseOrder.Validate())
                {
                    Tuple<PurchaseOrderHeaderModel, string> purchaseHeader = _purchaseOrderHeaderRepository.Insert(purchaseOrder).ToTuple();
                    //Check for errors
                    if (purchaseHeader.Item2 == null)
                    {
                        //no errors
                        //add purchase order id
                        purchaseOrder.PurchaseOrderID = purchaseHeader.Item1.PurchaseOrderID;
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
                    throw new Exception(purchaseOrder.ValidationMessage);
                }

                //Update each order line with the purchase order ID and save
                foreach (PurchaseOrderDetailModel orderLine in purchaseOrder.PurchaseOrderDetails)
                {
                    orderLine.PurchaseOrderID = purchaseOrder.PurchaseOrderID;

                    //Validate and save order line
                    if (orderLine.ValidateAll())
                    {
                        string errorMessage = _purchaseOrderDetailRepository.Insert(orderLine);
                        //Check for errors
                        if (errorMessage != null)
                        {
                            //error raised
                            throw new Exception(errorMessage);
                        }
                    }
                    else
                    {
                        //Validation errors
                        throw new Exception(orderLine.ValidationMessage);
                    }
                }
            }
            return purchaseOrder;
        }
    }
}

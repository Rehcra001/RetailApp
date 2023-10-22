using BussinessLogicLibrary.VAT;
using ModelsLibrary;
using ModelsLibrary.RepositoryInterfaces;

namespace BussinessLogicLibrary.Sales
{
    public class InsertSalesOrderManager : IInsertSalesOrderManager
    {
        private readonly ISalesOrderHeaderRepository _salesOrderHeaderRepository;
        private readonly ISalesOrderDetailRepository _salesOrderDetailRepository;
        private readonly IVATManager _vatManager;

        private SalesOrderHeaderModel SalesOrder { get; set; }
        private const int OPEN_STATUS = 1;

        public InsertSalesOrderManager(ISalesOrderHeaderRepository salesOrderHeaderRepository,
                                       ISalesOrderDetailRepository salesOrderDetailRepository,
                                       IVATManager vatManager)
        {
            _salesOrderHeaderRepository = salesOrderHeaderRepository;
            _salesOrderDetailRepository = salesOrderDetailRepository;
            _vatManager = vatManager;
        }

        public SalesOrderHeaderModel Insert(SalesOrderHeaderModel salesOrder)
        {
            SalesOrder = salesOrder;

            //Validate the sales order details
            ValidateSalesOrderDetails();

            //Validate the sales order header
            ValidateSalesOrderHeader();

            //Insert new sales order header and get the sales order id
            InsertHeader();

            //Insert the sales order details
            InsertDetails();

            return SalesOrder;
        }

        private void InsertDetails()
        {
            //Add sales order id to each line and insert
            foreach (SalesOrderDetailModel orderLine in SalesOrder.SalesOrderDetails)
            {
                orderLine.SalesOrderID = SalesOrder.SalesOrderID;
                string errorMessage = _salesOrderDetailRepository.Insert(orderLine);
                //Check for errors
                if (errorMessage != null)
                {
                    throw new Exception(errorMessage);
                }
            }
        }

        private void InsertHeader()
        {
            //Insert Header and get back sales order ID
            Tuple<SalesOrderHeaderModel, string> salesOrder = _salesOrderHeaderRepository.Insert(SalesOrder).ToTuple();
            //Check for errors
            if (salesOrder.Item2 == null)
            {
                //No errors
                SalesOrder.SalesOrderID = salesOrder.Item1.SalesOrderID;
            }
            else
            {
                //error
                throw new Exception(salesOrder.Item2);
            }
        }

        private void ValidateSalesOrderHeader()
        {
            CheckCustomer();

            CheckOrderStatus();

            CheckOrderAmount();

            CheckVatPercentage();

            CheckVatAmount();

            CheckTotalAmount();

            //Final validation
            if (!SalesOrder.Validate())
            {
                throw new Exception(SalesOrder.ValidationMessage);
            }
        }

        private void CheckTotalAmount()
        {
            //Add total amount
            SalesOrder.TotalAmount = SalesOrder.OrderAmount + SalesOrder.VATAmount;
        }

        private void CheckVatAmount()
        {
            //Add VAT amount
            SalesOrder.VATAmount = SalesOrder.OrderAmount * SalesOrder.VATPercentage;
        }

        private void CheckVatPercentage()
        {
            //Add the vat percentage
            SalesOrder.VATPercentage = _vatManager.Get().VatDecimal;
        }

        private void CheckOrderAmount()
        {
            //Calculate the order amount including any discount
            decimal amount = SalesOrder.SalesOrderDetails.Sum(x => (x.QuantityOrdered * x.UnitPrice) - (x.QuantityOrdered * x.UnitPrice * x.Discount));
            //Add to the order amount
            SalesOrder.OrderAmount = amount;
        }

        private void CheckOrderStatus()
        {
            //check if OrderStatusID added
            if (SalesOrder.OrderStatusID != OPEN_STATUS)
            {
                //New order so needs to be open
                SalesOrder.OrderStatusID = OPEN_STATUS;
            }
        }

        private void CheckCustomer()
        {
            //Check if CustomerID added
            if (SalesOrder.CustomerID == default)
            {
                //Check if customer model attached
                if (SalesOrder.Customer != default)
                {
                    SalesOrder.CustomerID = SalesOrder.Customer.CustomerID;
                }
            }
        }

        /// <summary>
        /// Validates the sales order details
        /// </summary>
        /// <exception cref="Exception"></exception>
        private void ValidateSalesOrderDetails()
        {
            //Check if any details exist
            if (SalesOrder.SalesOrderDetails.Count() == 0)
            {
                string message = "A sales order must have at least one order line.";
                throw new Exception(message);
            }

            //lines exist
            foreach (SalesOrderDetailModel orderLine in SalesOrder.SalesOrderDetails)
            {
                //check if product id added
                if (orderLine.ProductID == default)
                {
                    //check if a product model was attached
                    if (orderLine.Product != default)
                    {
                        //Add product id
                        orderLine.ProductID = orderLine.Product.ProductID;
                    }
                }

                //check if the order line status id was added
                if (orderLine.OrderLineStatusID != OPEN_STATUS)
                {
                    //New line so order line status needs to be open
                    orderLine.OrderLineStatusID = OPEN_STATUS;
                }

                //Validate
                //exclude sales order id validation as header must still be inserted
                bool excludeSalesOrderIDValidation = true;
                if (!orderLine.Validate(excludeSalesOrderIDValidation))
                {
                    throw new Exception(orderLine.ValidationMessage);
                }
            }
        }
    }
}

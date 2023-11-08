using ModelsLibrary;

namespace BussinessLogicLibrary.Sales
{
    public class ValidateExistingSalesOrderHeader : IValidateExistingSalesOrderHeader
    {
        private readonly IValidateExistingSalesOrderDetails _validateExistingSalesOrderDetails;
        private readonly IGetSalesOrderHeaderByIDManager _getSalesOrderHeaderByIDManager;

        private SalesOrderHeaderModel OriginalSalesOrderHeader { get; set; }
        private SalesOrderHeaderModel SalesOrderHeader { get; set; }

        private string message = "";
        private bool newLinesAdded = false;
        private bool existingLinesAltered = false;

        //Order statuses
        private const int OPEN = 1;
        private const int COMPLETED = 2;
        private const int FILLED = 3;
        private const int CANCELLED = 4;

        public ValidateExistingSalesOrderHeader(IValidateExistingSalesOrderDetails validateExistingSalesOrderDetails,
                                                IGetSalesOrderHeaderByIDManager getSalesOrderHeaderByIDManager)
        {
            _validateExistingSalesOrderDetails = validateExistingSalesOrderDetails;
            _getSalesOrderHeaderByIDManager = getSalesOrderHeaderByIDManager;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="salesOrder"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public (bool, bool) Validate(SalesOrderHeaderModel salesOrder)
        {
            SalesOrderHeader = salesOrder;
            //Get original for comparison
            OriginalSalesOrderHeader = _getSalesOrderHeaderByIDManager.GetByID(SalesOrderHeader.SalesOrderID);

            //First validate sales order details
            Tuple<bool, bool> orderLines = _validateExistingSalesOrderDetails.Validate(salesOrder).ToTuple();

            //check if new lines added
            newLinesAdded = orderLines.Item1;
            //check if existing lines altered
            existingLinesAltered = orderLines.Item2;

            //Do the simple model validation first
            if (!SalesOrderHeader.Validate())
            {
                //Validation error
                throw new Exception(SalesOrderHeader.ValidationMessage);
            }

            CheckCustomerID();

            CheckAmounts();

            CheckOrderStatus();

            //if this point reached no validation exceptions encountered
            return (newLinesAdded, existingLinesAltered);
        }

        private void CheckOrderStatus()
        {
            DetermineValidOrderStatus();
        }

        private void DetermineValidOrderStatus()
        {
            Dictionary<int, int> orderLineStatuses = LoadOrderLineStatuses();
            int numLines = SalesOrderHeader.SalesOrderDetails.Count();
            //int validStatus = 0;
            if (orderLineStatuses.ContainsKey(OPEN))
            {
                //SO status must be open
                if (SalesOrderHeader.OrderStatusID != OPEN)
                {
                    SalesOrderHeader.OrderStatusID = OPEN;
                }
            }
            else if (orderLineStatuses.ContainsKey(COMPLETED))
            {
                //SO status must be completed as no lines open
                if (SalesOrderHeader.OrderStatusID != COMPLETED)
                {
                    SalesOrderHeader.OrderStatusID = COMPLETED;
                }
            }
            else if (orderLineStatuses.ContainsKey(FILLED) &&
                     orderLineStatuses[FILLED] == numLines)
            {
                //SO status must be Filled as all lines are marked as filled
                if (SalesOrderHeader.OrderStatusID != FILLED)
                {
                    SalesOrderHeader.OrderStatusID = FILLED;
                }
            }
            else if (orderLineStatuses.ContainsKey(CANCELLED) &&
                     orderLineStatuses[CANCELLED] == numLines)
            {
                //SO status must be Cancelled as all lines have been cancelled
                if (SalesOrderHeader.OrderStatusID != CANCELLED)
                {
                    SalesOrderHeader.OrderStatusID = CANCELLED;
                }
            }
        }

        private Dictionary<int, int> LoadOrderLineStatuses()
        {
            Dictionary<int, int> orderLineStatuses = new Dictionary<int, int>();

            foreach (SalesOrderDetailModel orderLine in SalesOrderHeader.SalesOrderDetails)
            {
                if (orderLineStatuses.ContainsKey(orderLine.OrderLineStatusID))
                {
                    orderLineStatuses[orderLine.OrderLineStatusID]++;
                }
                else
                {
                    orderLineStatuses.Add(orderLine.OrderLineStatusID, 1);
                }
            }

            return orderLineStatuses;
        }

        private void CheckAmounts()
        {
            CheckOrderAmount();
        }

        private void CheckOrderAmount()
        {
            decimal orderAmount = SalesOrderHeader.SalesOrderDetails.Where(x => x.OrderLineStatusID != CANCELLED)
                                                                    .Sum(x => x.UnitPrice * x.QuantityOrdered);
            if (SalesOrderHeader.OrderAmount != orderAmount)
            {
                //update
                SalesOrderHeader.OrderAmount = orderAmount;
            }
            CheckVATPercentage();
        }
        private void CheckVATPercentage()
        {
            //Vat percentage may not be changed on an existing sales order
            if (SalesOrderHeader.VATPercentage != OriginalSalesOrderHeader.VATPercentage)
            {
                message = "VAT Percentage may not be altered on an existing sales order.";
                throw new Exception(message);
            }
            CheckVATAmount();
        }
        private void CheckVATAmount()
        {
            decimal vatAmount = SalesOrderHeader.OrderAmount * SalesOrderHeader.VATPercentage;
            if (SalesOrderHeader.VATAmount != vatAmount)
            {
                //update
                SalesOrderHeader.VATAmount = vatAmount;
            }
            CheckTotalAmount();
        }
        private void CheckTotalAmount()
        {
            decimal totalAmount = SalesOrderHeader.OrderAmount + SalesOrderHeader.VATAmount;
            if (SalesOrderHeader.TotalAmount != totalAmount)
            {
                SalesOrderHeader.TotalAmount = totalAmount;
            }
        }

        private void CheckCustomerID()
        {
            //Customer may not be altered on existing sales orders
            if (SalesOrderHeader.SalesOrderID != OriginalSalesOrderHeader.SalesOrderID)
            {
                message = "The customer may not be altered on an existing sales order.";
                throw new Exception(message);
            }
        }
    }
}

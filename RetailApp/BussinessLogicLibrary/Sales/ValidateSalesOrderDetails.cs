using ModelsLibrary;

namespace BussinessLogicLibrary.Sales
{
    public class ValidateSalesOrderDetails : IValidateSalesOrderDetails
    {
        private readonly IGetSalesOrderDetailsByIDManager _salesOrderDetailByIDManager;
        private bool newLinesAdded;
        private bool existingLinesAltered;

        private SalesOrderHeaderModel SalesOrder { get; set; }
        private IEnumerable<SalesOrderDetailModel> OriginalSalesOrderDetails { get; set; }

        //Order line statuses
        private const int OPEN = 1;
        private const int COMPLETED = 2;
        private const int FILLED = 3;
        private const int CANCELLED = 4;

        public ValidateSalesOrderDetails(IGetSalesOrderDetailsByIDManager getSalesOrderDetailsByIDManager)
        {
            _salesOrderDetailByIDManager = getSalesOrderDetailsByIDManager;
        }

        /// <summary>
        /// Validates a list of sales order details for an existing sales order.
        /// </summary>
        /// <param name="salesOrder">
        /// Takes in an existing SalesOrderHeaderModel. ie it already has a sales order id allocated by sql
        /// </param>
        /// <returns>
        /// Returns a tuple of bools
        /// Item1 is true if any new line have been added
        /// Item2 is true if any existing lines were changed
        /// </returns>
        public (bool, bool) Validate(SalesOrderHeaderModel salesOrder)
        {
            SalesOrder = salesOrder;
            GetOriginalSalesOrderDetails();

            //Check if existing lines removed - not allowed
            CheckIfLinesRemoved();

            //Check for duplicate line
            //A line is a duplicate if the 
            //same product appears on more than
            //one line
            CheckForDuplicateLines();

            //Check if new lines were added
            CheckIfNewLinesAdded();

            //Check if existing lines altered
            CheckIfExistingLinesAltered();

            return (newLinesAdded, existingLinesAltered);
        }
        private void GetOriginalSalesOrderDetails()
        {
            OriginalSalesOrderDetails = _salesOrderDetailByIDManager.GetByID(SalesOrder.SalesOrderID);
        }

        private void CheckIfNewLinesAdded()
        {
            foreach (SalesOrderDetailModel orderLine in SalesOrder.SalesOrderDetails)
            {
                if (OriginalSalesOrderDetails.FirstOrDefault(x => x.ProductID == orderLine.ProductID) == default)
                {
                    ValidateNewLine(orderLine);
                    newLinesAdded = true;
                }
            }
        }

        private void CheckIfExistingLinesAltered()
        {
            foreach (SalesOrderDetailModel orderLine in OriginalSalesOrderDetails)
            {
                //Find the corresponding line in SalesOrder.SalesOrderDetails
                int index = SalesOrder.SalesOrderDetails.FindIndex(x => x.SalesOrderID == orderLine.SalesOrderID && x.ProductID == orderLine.ProductID);
                if (index != -1) // existing
                {
                    if (ExistingLineChanged(orderLine))
                    {
                        //Validate existing line
                        ValidateExistingLine(orderLine);
                        existingLinesAltered = true;
                    }
                }
            }
        }

        private bool ExistingLineChanged(SalesOrderDetailModel orderLine)
        {
            bool changed = true;
            SalesOrderDetailModel original = OriginalSalesOrderDetails.First(x => x.ProductID == orderLine.ProductID);

            //Compare each property
            if (orderLine.ProductID == orderLine.ProductID
                && orderLine.QuantityOrdered == original.QuantityOrdered
                && orderLine.OrderLineStatusID == original.OrderLineStatusID
                && orderLine.UnitPrice == original.UnitPrice)
            {
                return false;
            }
            return changed;
        }

        private void ValidateNewLine(SalesOrderDetailModel orderLine)
        {
            //Check if sales order id added
            if (orderLine.SalesOrderID == default)
            {
                orderLine.SalesOrderID = SalesOrder.SalesOrderID;
            }
            CheckProductID(orderLine);
            CheckOrderLineStatus(orderLine);

            //Do model validation
            if (!orderLine.Validate())
            {
                throw new Exception(orderLine.ValidationMessage);
            }

            //Make sure status is OPEN
            if (orderLine.OrderLineStatusID != OPEN)
            {
                string message = "A new line may only have a status of open.";
                throw new Exception(message);
            }
        }

        private void ValidateExistingLine(SalesOrderDetailModel orderLine)
        {
            //Check if sales order id added
            if (orderLine.SalesOrderID == default)
            {
                orderLine.SalesOrderID = SalesOrder.SalesOrderID;
            }
            CheckProductID(orderLine);
            CheckOrderLineStatus(orderLine);
            CheckUnitPrice(orderLine);
            //Do model validation
            if (!orderLine.Validate())
            {
                throw new Exception(orderLine.ValidationMessage);
            }
            ValidateExistingOrderLineStatus(orderLine);
        }


        private void ValidateExistingOrderLineStatus(SalesOrderDetailModel orderLine)
        {
            switch (orderLine.OrderLineStatusID)
            {
                case OPEN:
                    VerifyOpenStatus(orderLine);
                    break;
                case COMPLETED:
                    VerifyCompletedStatus(orderLine);
                    break;
                case FILLED:
                    VerifyFilledStatus(orderLine);
                    break;
                case CANCELLED:
                    VerifyCancelledStatus(orderLine);
                    break;
            }
        }

        private void VerifyCancelledStatus(SalesOrderDetailModel orderLine)
        {
            //Status may only be cancelled if nothing has been invoiced yet
            if (orderLine.QuantityInvoiced > 0)
            {
                string message = "Status may only be cancelled if nothing has been invoiced yet.";
                throw new Exception(message);
            }
        }

        private void VerifyFilledStatus(SalesOrderDetailModel orderLine)
        {
            //Status may only be filled if both Quantity invoice and ordered
            //Are the same
            if (orderLine.QuantityOrdered > orderLine.QuantityInvoiced)
            {
                string message = "Line may not be marked as filled as\r\n";
                message += "quantity ordered is greater than qty invoiced";
                throw new Exception(message);
            }
        }

        private void VerifyCompletedStatus(SalesOrderDetailModel orderLine)
        {
            //Status can only be completed if quantity invoiced is less 
            //than quantity ordered
            if (!(orderLine.QuantityOrdered > orderLine.QuantityInvoiced) || orderLine.QuantityInvoiced == 0)
            {
                string message = "Status may only be completed if quantity ordered\r\n";
                message += "is greater than quantity invoiced and quantity invoiced is\r\n";
                message += "greater than zeor.";
                throw new Exception(message);
            }
        }

        private void VerifyOpenStatus(SalesOrderDetailModel orderLine)
        {
            //Status can only be open if quantity invoiced is less than 
            //quantity ordered 
            if (orderLine.QuantityOrdered == orderLine.QuantityInvoiced)
            {
                string message = "Status may only be open if quantity ordered is > quantity invoiced.";
                throw new Exception(message);
            }

        }
        private void CheckUnitPrice(SalesOrderDetailModel orderLine)
        {
            if (orderLine.UnitPrice >= 0)
            {
                if (orderLine.Product != default)
                {
                    //make it the same as the product unit price
                    //bussiness rule
                    orderLine.UnitPrice = orderLine.Product.UnitPrice;
                }
            }
        }

        private void CheckOrderLineStatus(SalesOrderDetailModel orderLine)
        {
            if (orderLine.OrderLineStatusID == default)
            {
                //Check if order line status attached
                if (orderLine.OrderLineStatus != default)
                {
                    orderLine.OrderLineStatusID = orderLine.OrderLineStatus.StatusID;
                }
            }
        }

        private void CheckProductID(SalesOrderDetailModel orderLine)
        {
            if (orderLine.ProductID == default)
            {
                //Check if product attached
                if (orderLine.Product != default)
                {
                    orderLine.ProductID = orderLine.Product.ProductID;
                }
            }
        }

        private void CheckForDuplicateLines()
        {
            Dictionary<int, int> dupes = new Dictionary<int, int>();
            foreach (SalesOrderDetailModel orderLine in SalesOrder.SalesOrderDetails)
            {
                if (dupes.ContainsKey(orderLine.ProductID))
                {
                    string message = "A product may only appear once per Sales Order.";
                    throw new Exception(message);
                }
                else
                {
                    dupes.Add(orderLine.ProductID, 1);
                }
            }
        }

        private void CheckIfLinesRemoved()
        {
            foreach (SalesOrderDetailModel orderLine in OriginalSalesOrderDetails)
            {
                //Find the corresponding line in SalesOrder.SalesOrderDetails
                int index = SalesOrder.SalesOrderDetails.FindIndex(x => x.SalesOrderID == orderLine.SalesOrderID && x.ProductID == orderLine.ProductID);
                if (index == -1)
                {
                    string message = "Sales order line with: \r\n";
                    message += $"Sales Order ID: {orderLine.SalesOrderID}\r\n";
                    message += $"Product ID: {orderLine.ProductID}\r\n";
                    message += "was removed!\r\n\r\n";
                    message += "Consider marking the line as Complete or Cancelled.";
                    throw new Exception(message);
                }
            }
        }
    }
}

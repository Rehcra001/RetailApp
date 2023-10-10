using DataAccessLibrary.PurchaseOrderDetailRepository;
using DataAccessLibrary.PurchaseOrderHeaderRepository;
using DataAccessLibrary.StatusRepository;
using DataAccessLibrary.VATRepository;
using ModelsLibrary;

namespace BussinessLogicLibrary.Purchases
{
    public class UpdatePurchaseOrderManager
    {
        private readonly PurchaseOrderHeaderModel _originalPurchaseOrder;
        private readonly PurchaseOrderHeaderModel _editedPurchaseOrder;
        private bool existingLinesAltered = false;
        private bool newLinesAdded = false;
        private bool headerDetailsAltered = false;

        private const int OPEN = 1;
        private const int COMPLETED = 2;
        private const int FILLED = 3;
        private const int CANCELLED = 4;

        private readonly string _connectionString;

        /// <summary>
        /// Creates a new instance of UpdatePurchaseOrderManager, 
        /// It will validate and save any changes made to the 
        /// purchase order passed in
        /// </summary>
        /// <param name="connectionString">
        /// Takes in a connection string to the datasource
        /// </param>
        /// <param name="purchaseOrder">
        /// Takes in an existing purchase order model that has been edited
        /// </param>
        public UpdatePurchaseOrderManager(string connectionString, PurchaseOrderHeaderModel editedPurchaseOrder)
        {
            _connectionString = connectionString;
            _editedPurchaseOrder = editedPurchaseOrder;
            _originalPurchaseOrder = new GetPurchaseOrderManager(_connectionString).GetByID(_editedPurchaseOrder.PurchaseOrderID);
            Update();
        }

        private void Update()
        {
            //validate order lines
            //Will throw execption as needed
            ValidateLines();
            //validate purchase order header detail
            //Will throw execption as needed
            ValidateOrderHeader();

            //If lines have been altered or added then update the order amount, Vat and Total Amount
            if (existingLinesAltered || newLinesAdded)
            {
                UpdateHeaderAmounts();
                headerDetailsAltered = true;
            }

            if (headerDetailsAltered)
            {
                //Update header
                UpdateHeader();
            }

            if (existingLinesAltered)
            {
                //Update existing lines
                UpdateLines();
            }

            if (newLinesAdded)
            {
                //Add new lines
                AddLines();
            }
        }

        private void AddLines()
        {
            foreach (PurchaseOrderDetailModel orderLine in _editedPurchaseOrder.PurchaseOrderDetails)
            {
                int index = _originalPurchaseOrder.PurchaseOrderDetails.FindIndex(x => x.ProductID == orderLine.ProductID);
                if (index == -1)
                {
                    string errorMessage = new PurchaseOrderDetailRepository(_connectionString).Insert(orderLine);
                    if (errorMessage != null)
                    {
                        string message = $"Error saving line {index + 1} to the database.\r\n";
                        throw new Exception(message + errorMessage);
                    }
                }
            }
        }

        private void UpdateLines()
        {
            foreach (PurchaseOrderDetailModel orderLine in _editedPurchaseOrder.PurchaseOrderDetails)
            {
                int index = _originalPurchaseOrder.PurchaseOrderDetails.FindIndex(x => x.ProductID == orderLine.ProductID);
                if (index != -1)
                {
                    string errorMessage = new PurchaseOrderDetailRepository(_connectionString).Update(orderLine);
                    if (errorMessage != null)
                    {
                        string message = $"Error saving line {index + 1} to the database.\r\n";
                        throw new Exception(message + errorMessage);
                    }
                }
            }
        }

        private void UpdateHeader()
        {
            string errorMessage = new PurchaseOrderHeaderRepository(_connectionString).Update(_editedPurchaseOrder);
            if (errorMessage != null)
            {
                throw new Exception(errorMessage);
            }
        }

        private void ValidateOrderHeader()
        {
            //Check all required fields have been completed
            _editedPurchaseOrder.Validate();
            if (!string.IsNullOrWhiteSpace(_editedPurchaseOrder.ValidationMessage))
            {
                throw new Exception(_editedPurchaseOrder.ValidationMessage);
            }

            CheckOrderDate();
            CheckRequiredDate();
            CheckVendor();
            CheckVendorReference();
            CheckImported();
            CheckOrderAmount();
            CheckVatAmount();
            CheckTotalAmount();
            CheckOrderStatus();
        }
        private void UpdateHeaderAmounts()
        {
            //Add total order amount excluding VAT
            _editedPurchaseOrder.OrderAmount = _editedPurchaseOrder.PurchaseOrderDetails.Sum(x => x.QuantityOrdered * (x.UnitCost + x.UnitFreightCost));

            //Check if import
            if (!_editedPurchaseOrder.IsImport)
            {
                //Calculate Vat amount
                if (_editedPurchaseOrder.VATPercentage != default)
                {
                    _editedPurchaseOrder.VATAmount = _editedPurchaseOrder.OrderAmount * _editedPurchaseOrder.VATPercentage;
                }
                else
                {
                    //Add VAT Percentage
                    Tuple<VatModel, string> vat = new VATRepository(_connectionString).Get().ToTuple();
                    if (vat.Item2 == null)
                    {
                        //No errors
                        _editedPurchaseOrder.VATPercentage = vat.Item1.VatDecimal;
                        _editedPurchaseOrder.VATAmount = _editedPurchaseOrder.OrderAmount * _editedPurchaseOrder.VATPercentage;
                    }
                    else
                    {
                        throw new Exception(vat.Item2);
                    }
                }

                //Add Total Amount
                _editedPurchaseOrder.TotalAmount = _editedPurchaseOrder.OrderAmount + _editedPurchaseOrder.VATAmount;
            }
            else
            {
                //No vat if imported
                _editedPurchaseOrder.TotalAmount = _editedPurchaseOrder.OrderAmount;
            }
        }

        private void CheckTotalAmount()
        {
            if (_editedPurchaseOrder.TotalAmount != _originalPurchaseOrder.TotalAmount)
            {
                string message = "Total amount may not be altered.\r\n";
                message += "This will be automatically altered if necessary.";
                throw new Exception(message);
            }
        }

        private void CheckVatAmount()
        {
            if (_editedPurchaseOrder.VATAmount != _originalPurchaseOrder.VATAmount)
            {
                string message = "VAT amount may not be altered.\r\n";
                message += "This will be automatically adjusted if necessary.";
                throw new Exception(message);
            }
        }

        private void CheckOrderAmount()
        {
            if (_editedPurchaseOrder.OrderAmount != _originalPurchaseOrder.OrderAmount)
            {
                string message = "Order amount may not be altered.\r\n";
                message += "This will be automatically adjusted if necessary.";
                throw new Exception(message);
            }
        }

        private void CheckImported()
        {
            if (_editedPurchaseOrder.IsImport != _originalPurchaseOrder.IsImport)
            {
                string message = "Imported may not be altered, this is vendor dependent.";
                throw new Exception(message);
            }
        }

        private void CheckOrderStatus()
        {
            string status = _editedPurchaseOrder.OrderStatus.Status!;
            //Order status must match all line states
            switch (status)
            {
                case "Open":
                    ValidateOpenOrderStatus();
                    break;
                case "Completed":
                    ValidateCompletedOrderStatus();
                    break;
                case "Filled":
                    ValidateFilledOrderStatus();
                    break;
                case "Cancelled":
                    ValidateCancelledOrderStatus();
                    break;
            }
        }

        private void ValidateCancelledOrderStatus()
        {
            //An order can only be marked as cancelled if:
            //No lines have been receipted
            //or all lines have been cancelled
            bool canCancel = true;
            foreach (PurchaseOrderDetailModel orderLine in _editedPurchaseOrder.PurchaseOrderDetails)
            {
                if (!orderLine.OrderLineStatus.Status!.Equals("Cancelled"))
                {
                    if (!orderLine.OrderLineStatus.Status!.Equals("Open"))
                    {
                        canCancel = false;
                    }
                    else
                    {
                        if (orderLine.QuantityReceipted != 0)
                        {
                            canCancel = false;
                        }
                    }
                }
            }

            if (canCancel)
            {
                //Change order status and orderline status
                _editedPurchaseOrder.OrderStatusID = _editedPurchaseOrder.OrderStatus.StatusID;
                foreach (PurchaseOrderDetailModel orderLine in _editedPurchaseOrder.PurchaseOrderDetails)
                {
                    if (orderLine.OrderLineStatus.Status!.Equals("Open"))
                    {
                        orderLine.OrderLineStatus = GetStatus(CANCELLED);
                        orderLine.OrderLineStatusID = orderLine.OrderLineStatus.StatusID;
                        existingLinesAltered = true;
                    }
                }
            }
            else
            {
                string message = "This order may not be cancelled.";
                throw new Exception(message);
            }
        }

        private void ValidateFilledOrderStatus()
        {
            //can only be filled if all lines are fully receipted
            bool linesFilled = true;
            foreach (PurchaseOrderDetailModel orderLine in _editedPurchaseOrder.PurchaseOrderDetails)
            {
                if (orderLine.QuantityReceipted < orderLine.QuantityOrdered)
                {
                    linesFilled = false;
                }
                else
                {
                    //line is fully receipted check line status must be filled if not change to filled
                    if (!orderLine.OrderLineStatus.Status!.Equals("Filled"))
                    {
                        orderLine.OrderLineStatus = GetStatus(FILLED);
                        orderLine.OrderLineStatusID = orderLine.OrderLineStatus.StatusID;
                        existingLinesAltered = true;
                    }
                }
            }

            if (!linesFilled)
            {
                string message = "All lines must be fully receipted before order can be marked as filled";
                throw new Exception(message);
            }
            else
            {
                headerDetailsAltered = true;
                _editedPurchaseOrder.OrderStatusID = _editedPurchaseOrder.OrderStatus.StatusID;
            }

        }

        private void ValidateCompletedOrderStatus()
        {
            //Order Status is Completed here
            //At least one line should be completed or open with partial receipt

            //if no lines have any thing receipted it cannot be marked as completed
            bool partialReceipt = false;
            foreach (PurchaseOrderDetailModel orderLine in _editedPurchaseOrder.PurchaseOrderDetails)
            {
                if (orderLine.QuantityReceipted > 0 && orderLine.QuantityReceipted < orderLine.QuantityOrdered)
                {
                    //Partial receipted
                    partialReceipt = true;
                }
            }

            if (partialReceipt)
            {
                //Changed all open partially receipted lines to completed
                //and all open lines with no receipts to cancelled
                foreach (PurchaseOrderDetailModel orderLine in _editedPurchaseOrder.PurchaseOrderDetails)
                {
                    if (orderLine.OrderLineStatus.Status!.Equals("Open"))
                    {
                        if (orderLine.QuantityReceipted > 0 && orderLine.QuantityReceipted < orderLine.QuantityOrdered)
                        {
                            //paritial
                            orderLine.OrderLineStatus = GetStatus(COMPLETED);
                            orderLine.OrderLineStatusID = orderLine.OrderLineStatus.StatusID;
                            existingLinesAltered = true;
                        }
                        else
                        {
                            orderLine.OrderLineStatus = GetStatus(CANCELLED);
                            orderLine.OrderLineStatusID = orderLine.OrderLineStatus.StatusID;
                            existingLinesAltered = true;
                        }
                    }
                }
                _editedPurchaseOrder.OrderStatusID = _editedPurchaseOrder.OrderStatus.StatusID;
                headerDetailsAltered = true;
            }
            else
            {
                string message = "At least one line must be partially receipted to change the order status to completed";
                throw new Exception(message);
            }

        }

        private void ValidateOpenOrderStatus()
        {
            //Order status is open here
            //At least one line must be open if not mark as complete or cancelled
            if (!newLinesAdded || !existingLinesAltered)
            {
                bool lineOpen = false;
                bool lineCompleted = false;
                bool lineCancelled = false;
                bool lineFilled = false;
                foreach (PurchaseOrderDetailModel orderLine in _editedPurchaseOrder.PurchaseOrderDetails)
                {
                    if (orderLine.OrderLineStatus.Status!.Equals("Open"))
                    {
                        lineOpen = true;
                    }
                    else if (orderLine.OrderLineStatus.Status!.Equals("Completed"))
                    {
                        lineCompleted = true;
                    }
                    else if (orderLine.OrderLineStatus.Status!.Equals("Cancelled"))
                    {
                        lineCancelled = true;
                    }
                    else if (orderLine.OrderLineStatus.Status!.Equals("Filled"))
                    {
                        lineFilled = true;
                    }
                }
                //no lines open 
                if (!lineOpen)
                {
                    if (lineCompleted)
                    {
                        //Check if current status different to the original
                        //if yes then throw an exception that at least one line must opened as well
                        //Before saving order status
                        if (_editedPurchaseOrder.OrderStatus.StatusID != _originalPurchaseOrder.OrderStatus.StatusID)
                        {
                            string message = "At least one line must be altered to open\r\n in order to change the order status to Open";
                            throw new Exception(message);
                        }
                        else
                        {
                            //Mark order as completed
                            _editedPurchaseOrder.OrderStatus = GetStatus(COMPLETED);
                            _editedPurchaseOrder.OrderStatusID = _editedPurchaseOrder.OrderStatus.StatusID;
                            headerDetailsAltered = true;
                        }
                        
                    }
                    else if (!lineCompleted && !lineFilled && lineCancelled)
                    {
                        //check if current order status is different from original
                        //change to open and open all lines
                        if (_editedPurchaseOrder.OrderStatus.StatusID != _originalPurchaseOrder.OrderStatus.StatusID)
                        {
                            _editedPurchaseOrder.OrderStatusID = _editedPurchaseOrder.OrderStatus.StatusID;
                            //All lines are currently cancelled - change to open
                            foreach (PurchaseOrderDetailModel orderLine in _editedPurchaseOrder.PurchaseOrderDetails)
                            {
                                //change line to open
                                orderLine.OrderLineStatus = GetStatus(OPEN);
                                orderLine.OrderLineStatusID = orderLine.OrderLineStatus.StatusID;
                                existingLinesAltered = true;
                            }
                        }
                        else
                        {
                            //Mark order as Cancelled - undo change to open
                            _editedPurchaseOrder.OrderStatus = GetStatus(CANCELLED);
                            _editedPurchaseOrder.OrderStatusID = _editedPurchaseOrder.OrderStatus.StatusID;
                            headerDetailsAltered = true;
                        }

                        
                    }
                }
                else
                {
                    //check if order status was changed from Completed
                    //if yes then allow change
                    if (_editedPurchaseOrder.OrderStatus.StatusID != _originalPurchaseOrder.OrderStatus.StatusID)
                    {
                        //Set order status id and line status id to open
                        if (_originalPurchaseOrder.OrderStatus.Status!.Equals("Completed"))
                        {
                            _editedPurchaseOrder.OrderStatusID = _editedPurchaseOrder.OrderStatus.StatusID;
                            headerDetailsAltered = true;
                            foreach (PurchaseOrderDetailModel orderLine in _editedPurchaseOrder.PurchaseOrderDetails)
                            {
                                if (orderLine.OrderLineStatus.Status!.Equals("Open"))
                                {
                                    orderLine.OrderLineStatusID = orderLine.OrderLineStatus.StatusID;
                                    existingLinesAltered = true;
                                }
                            }
                        }
                    }

                    //check if the order quantity was altered to suit a partial receipt
                    foreach (PurchaseOrderDetailModel orderLine in _editedPurchaseOrder.PurchaseOrderDetails)
                    {
                        if (orderLine.OrderLineStatus.Status!.Equals("Open") && orderLine.QuantityOrdered == orderLine.QuantityReceipted)
                        {
                            //Change line to filled
                            orderLine.OrderLineStatus = GetStatus(FILLED);
                            orderLine.OrderLineStatusID = orderLine.OrderLineStatus.StatusID;
                            existingLinesAltered = true;
                        }
                    }
                    //Check if all lines are now filled
                    //if this is the case then the order status of open must be altered to Filled
                    bool allLineFilled = true;
                    foreach (PurchaseOrderDetailModel orderLine in _editedPurchaseOrder.PurchaseOrderDetails)
                    {
                        if (!orderLine.OrderLineStatus.Status!.Equals("Filled"))
                        {
                            allLineFilled = false;
                        }
                    }
                    if (allLineFilled)
                    {
                        //Change order status to filled
                        _editedPurchaseOrder.OrderStatus = GetStatus(FILLED);
                        _editedPurchaseOrder.OrderStatusID = _editedPurchaseOrder.OrderStatus.StatusID;
                        headerDetailsAltered = true;
                    }
                }
            }
        }

        private StatusModel GetStatus(int id)
        {
            StatusModel status = new StatusModel();
            Tuple<StatusModel, string> statusData = new StatusRepository(_connectionString).GetByID(id).ToTuple();
            //check for errors
            if (statusData.Item2 == null)
            {
                //No error
                status = statusData.Item1;
            }
            else
            {
                throw new Exception(statusData.Item2);
            }
            return status;
        }

        private void CheckVendorReference()
        {
            if (_editedPurchaseOrder.VendorReference != _originalPurchaseOrder.VendorReference)
            {
                headerDetailsAltered = true;
            }
        }

        private void CheckVendor()
        {
            //The vendor may not be changed as this is an existing order
            if (_editedPurchaseOrder.Vendor.VendorID != _originalPurchaseOrder.Vendor.VendorID)
            {
                string message = "The vendor may not be changed on an existing order.";
                throw new Exception(message);
            }
        }

        private void CheckRequiredDate()
        {
            //check if required date was altered
            if (_editedPurchaseOrder.RequiredDate != _originalPurchaseOrder.RequiredDate)
            {
                headerDetailsAltered = true;
            }
            //Required date may not be less than the order date
            if (_editedPurchaseOrder.RequiredDate < _editedPurchaseOrder.OrderDate)
            {
                string message = "The required date may not be earlier than the order date";
                throw new Exception(message);
            }
        }

        private void CheckOrderDate()
        {
            //check if the order date was altered
            if (_editedPurchaseOrder.OrderDate != _originalPurchaseOrder.OrderDate)
            {
                headerDetailsAltered = true;
            }
        }

        private void ValidateLines()
        {
            //Exisiting lines
            foreach (PurchaseOrderDetailModel orderLine in _originalPurchaseOrder.PurchaseOrderDetails)
            {
                int index = _editedPurchaseOrder.PurchaseOrderDetails.FindIndex(x => x.ProductID == orderLine.ProductID);
                //Make sure no line has been removed
                if (index == -1)
                {
                    string message = $"Order line with {orderLine.Product.ProductName} has been removed. \r\n";
                    message += "Existing purchase order lines may not be removed.\r\n";
                    message += "Saving these changes is not possible at this stage.\r\n";
                    throw new Exception(message);
                }
                else
                {
                    //Line not removed check if the line has changed
                    if (HasLineChanged(index, orderLine))
                    {
                        //Set existingLinesAltered
                        existingLinesAltered = true;
                        //Validate any changes
                        _editedPurchaseOrder.PurchaseOrderDetails[index].ValidateAll();
                    }
                }
            }

            //New Lines
            //Check if new lines added
            foreach (PurchaseOrderDetailModel orderLine in _editedPurchaseOrder.PurchaseOrderDetails)
            {
                if (_originalPurchaseOrder.PurchaseOrderDetails.FindIndex(x => x.ProductID == orderLine.ProductID) == -1)
                {
                    ValidateNewLine(orderLine);
                    newLinesAdded = true;
                }
            }
        }

        private void ValidateNewLine(PurchaseOrderDetailModel orderLine)
        {
            //Make sure the order line product is not in any other lines
            //Only one product type per purchase order allowed
            int index = _originalPurchaseOrder.PurchaseOrderDetails.FindIndex(x => x.ProductID == orderLine.ProductID);
            if (index != -1)
            {
                //Product already in another purchase order line
                string message = "Only one product type is allowed per purchase order.\r\n";
                throw new Exception(message);
            }

            //Add purchase order id and product id
            orderLine.PurchaseOrderID = _editedPurchaseOrder.PurchaseOrderID;
            orderLine.ProductID = orderLine.Product.ProductID;
            orderLine.ValidateAll();

            //Check if validation message is empty
            if (!string.IsNullOrWhiteSpace(orderLine.ValidationMessage))
            {
                throw new Exception(orderLine.ValidationMessage);
            }

            //New line may only have a status of open
            if (!orderLine.OrderLineStatus.Status!.Equals("Open"))
            {
                string message = "New lines may only have a status of Open.\r\n";
                throw new Exception(message);
            }
        }

        /// <summary>
        /// Checks if the existing order line has altered
        /// </summary>
        /// <param name="index">
        /// The index of _editedPurchaseOrder.PurchaseOrderDetails to be checked
        /// </param>
        /// <param name="originalLine">
        /// The purchase order line from _originalPurchaseOrder to be used in the comparison
        /// </param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        private bool HasLineChanged(int index, PurchaseOrderDetailModel originalLine)
        {
            bool lineChanged = false;

            //Compare edited purchase order lines against orderLine
            PurchaseOrderDetailModel editedLine = _editedPurchaseOrder.PurchaseOrderDetails[index];

            //Product not allowed to change
            if (editedLine.ProductID != editedLine.Product.ProductID)
            {
                string message = $"Order line {index + 1} has the product changed \r\n";
                message += $"from {originalLine.Product.ProductName} to {editedLine.Product.ProductName}.\r\n";
                message += "Existing order line product item may not be altered.\r\n";
                message += $"Please change the product back to {originalLine.Product.ProductName}.\r\n";
                throw new Exception(message);
            }




            //Quantity on order cannot be less than amount receipted
            int quantityReceipted = editedLine.QuantityReceipted;

            if (editedLine.QuantityOrdered < quantityReceipted)
            {
                string message = $"Order line {index + 1} has {quantityReceipted} units receipted. \r\n";
                message += $"The ordered quantity for this line may not be less than that.\r\n";
                message += "Please correct the quantity ordered.\r\n";
                throw new Exception(message);
            }
            else
            {
                //check if quantity changed
                if (editedLine.QuantityOrdered != originalLine.QuantityOrdered)
                {
                    lineChanged = true;
                }
            }

            //Check if unit cost altered
            if (editedLine.UnitCost != originalLine.UnitCost)
            {
                lineChanged = true;
            }

            //check if unit freight cost altered
            if (editedLine.UnitFreightCost != originalLine.UnitFreightCost)
            {
                lineChanged = true;
            }

            //Check if the order line status was changed and validate the change
            if (editedLine.OrderLineStatus.Status != originalLine.OrderLineStatus.Status)
            {
                if (lineChanged && !editedLine.OrderLineStatus.Status!.Equals("Open"))
                {
                    //status of line must be open
                    string message = "Altered lines may only have a status of open.";
                    throw new Exception(message);
                }
                else
                {
                    ValidateOrderLineStatus(editedLine, originalLine);
                    lineChanged = true;
                }                
            }
            else if (lineChanged)
            {
                ValidateOrderLineStatus(editedLine, originalLine);
            }

            return lineChanged;
        }

        private void ValidateOrderLineStatus(PurchaseOrderDetailModel editedLine, PurchaseOrderDetailModel originalLine)
        {
            string originalStatus = originalLine.OrderLineStatus.Status!;
            string editedStatus = editedLine.OrderLineStatus.Status!;
            int qtyReceipted = editedLine.QuantityReceipted;
            string message = "";
            //switch on the original status
            switch (originalStatus)
            {
                case "Open":
                    switch (editedStatus)
                    {
                        case "Completed":
                            //May only be changed to Completed if a partial quantity of the order has been receipted                            
                            if (qtyReceipted == 0 || qtyReceipted == editedLine.QuantityOrdered)
                            {
                                message += "Completed order status line may only be added to lines with partial receipts";
                                throw new Exception(message);
                            }
                            else
                            {
                                editedLine.OrderLineStatusID = editedLine.OrderLineStatus.StatusID;
                                existingLinesAltered = true;
                            }
                            break;
                        case "Filled":
                            if (qtyReceipted < editedLine.QuantityOrdered)
                            {
                                message += "Filled order status line may only be added to lines where quantiy receipted = quantity ordered";
                                throw new Exception(message);
                            }
                            break;
                        case "Cancelled":
                            if (qtyReceipted > 0)
                            {
                                message += "Cancelled line status is only allowed if nothing has been receipted on this line";
                                throw new Exception(message);
                            }
                            else
                            {
                                editedLine.OrderLineStatusID = editedLine.OrderLineStatus.StatusID;
                                existingLinesAltered = true;
                            }
                            break;
                    }
                    break;
                case "Completed":
                    if (!editedStatus.Equals("Open"))
                    {
                        message += "Completed status may only be altered to Open";
                        throw new Exception(message);
                    }
                    break;
                case "Filled":
                    message += "Filled line status may not be altered";
                    throw new Exception(message);
                case "Cancelled":
                    if (!editedStatus.Equals("Open"))
                    {
                        message += "Cancelled status may only be altered to Open";
                        throw new Exception(message);
                    }
                    break;
            }
        }
    }
}

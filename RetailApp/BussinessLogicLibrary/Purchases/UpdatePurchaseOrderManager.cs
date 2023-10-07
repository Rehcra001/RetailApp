using DataAccessLibrary.ReceiptRepository;
using ModelsLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLogicLibrary.Purchases
{
    public class UpdatePurchaseOrderManager
    {
        private readonly PurchaseOrderHeaderModel _originalPurchaseOrder;
        private readonly PurchaseOrderHeaderModel _editedPurchaseOrder;
        private bool existingLinesAltered = false;
        private bool newLinesAdded = false;
        private bool headerDetailsAltered = false;

        private string _connectionString;

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

        }

        private void ValidateOrderHeader()
        {


            //Check if the order status has changed
            //If existing lines have changed or new lines added
            //then order status must be open
            if (existingLinesAltered || newLinesAdded)
            {
                //Check header status - must be open
                if (!_editedPurchaseOrder.OrderStatus.Status!.Equals("Open"))
                {

                }
            }

            //validate the data entered into header
            _editedPurchaseOrder.Validate();
        }

        private void ValidateLines()
        {            
            //Make sure no line has been removed
            foreach (PurchaseOrderDetailModel orderLine in _originalPurchaseOrder.PurchaseOrderDetails)
            {
                int index = _editedPurchaseOrder.PurchaseOrderDetails.FindIndex(x => x.ProductID == orderLine.ProductID);

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
                    if(HasLineChanged(index, orderLine))
                    {
                        //Set existingLinesAltered
                        existingLinesAltered = true;
                        //Validate any changes
                        _editedPurchaseOrder.PurchaseOrderDetails[index].ValidateAll();
                    }
                }
            }

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
                string message = $"Order line {index +1} has the product changed \r\n";
                message += $"from {originalLine.Product.ProductName} to {editedLine.Product.ProductName}.\r\n";
                message += "Existing order line product item may not be altered.\r\n";
                message += $"Please change the product back to {originalLine.Product.ProductName}.\r\n";
                throw new Exception(message);
            }

            //Check if the order line status was changed and validate the change
            if (editedLine.OrderLineStatus.Status != originalLine.OrderLineStatus.Status)
            {
                ValidateOrderLineStatus(editedLine, originalLine);
            }

            
            //Quantity on order cannot be less than amount receipted
            int quantityReceipted = GetLineReceiptQuantity(editedLine); 

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

            return lineChanged;
        }

        private int GetLineReceiptQuantity(PurchaseOrderDetailModel orderLine)
        {
            int quantityReceipted = 0;

            foreach (ReceiptModel receipt in _editedPurchaseOrder.Receipts)
            {
                if (receipt.ProductID == orderLine.ProductID)
                {
                    quantityReceipted += receipt.QuantityReceipted;
                }
            }

            return quantityReceipted;
        }

        private void ValidateOrderLineStatus(PurchaseOrderDetailModel editedLine, PurchaseOrderDetailModel originalLine)
        {
            string originalStatus = originalLine.OrderLineStatus.Status!;
            string editedStatus = editedLine.OrderLineStatus.Status!;
            int qtyReceipted = GetLineReceiptQuantity(editedLine);
            string message = "";
            //switch on the original status
            switch (originalStatus)
            {
                case "Open":
                    switch (editedStatus)
                    {
                        case "Completed":
                            //May only be changed to Completed if a partial quantity of the order has been receipted                            
                            if ( qtyReceipted == 0 || qtyReceipted == editedLine.QuantityOrdered)
                            {
                                message += "Completed order status line may only be added to lines with partial receipts";
                                throw new Exception(message); 
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

        private bool HeaderChanged()
        {
            bool hasChanged = false;

            //Compare each property
            if (!_editedPurchaseOrder.OrderDate.Equals(_originalPurchaseOrder.OrderDate))
            {
                hasChanged = true;
            }
            //if (!_editedPurchaseOrder.)

            return hasChanged;

        }
        private void UpdateHeader()
        {
            
        }

        private void UpdateLine()
        {

        }

        private void AddLine()
        {

        }

    }
}

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
                if (!_editedPurchaseOrder.OrderStatus.OrderStatus!.Equals("Open"))
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
                if (orderLine.ProductID == 0)
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
                string message = "Only on product type is allowed per purchase order.\r\n";
                throw new Exception(message);
            }

            //Add purchase order id and product id
            orderLine.PurchaseOrderID = _editedPurchaseOrder.PurchaseOrderID;
            orderLine.ProductID = orderLine.Product.ProductID;
            orderLine.ValidateAll();
        }

        private bool HasLineChanged(int index, PurchaseOrderDetailModel orderLine)
        {
            bool lineChanged = false;

            //Compare edited purchase order lines against orderLine
            PurchaseOrderDetailModel line = _editedPurchaseOrder.PurchaseOrderDetails[index];

            //Product not allowed to change
            if (line.ProductID != line.Product.ProductID)
            {
                string message = $"Order line {index +1} has the product changed \r\n";
                message += $"from {orderLine.Product.ProductName} to {line.Product.ProductName}.\r\n";
                message += "Existing order line product item may not be altered.\r\n";
                message += $"Please change the product back to {orderLine.Product.ProductName}.\r\n";
                throw new Exception(message);
            }

            //Quantity on order cannot be less than amount receipted
            int quantityReceipted = 0;
            foreach (ReceiptModel receipt in _editedPurchaseOrder.Receipts)
            {
                if (receipt.ProductID == line.ProductID)
                {
                    quantityReceipted += receipt.QuantityReceipted;
                }
            }

            if (line.Quantity < quantityReceipted)
            {
                string message = $"Order line {index + 1} has {quantityReceipted} units receipted. \r\n";
                message += $"The ordered quantity for this line may not be less than that.\r\n";
                message += "Please correct the quantity ordered.\r\n";
                throw new Exception(message);
            }
            else
            {
                //check if quantity changed
                if (line.Quantity != orderLine.Quantity)
                {
                    lineChanged = true;
                }
            }

            //Check if unit cost altered
            if (line.UnitCost != orderLine.UnitCost)
            {
                lineChanged = true;
            }

            //check if unit freight cost altered
            if (line.UnitFreightCost != orderLine.UnitFreightCost)
            {
                lineChanged = true;
            }

            return lineChanged;
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

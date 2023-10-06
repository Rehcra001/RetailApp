using ModelsLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLogicLibrary.Purchases
{
    public class PurchaseOrderManager
    {
        public PurchaseOrderHeaderModel PurchaseOrder { get; private set; } = new PurchaseOrderHeaderModel();

        private string _connectionString;

        public PurchaseOrderManager(string connectionString)
        {
            _connectionString = connectionString;
        }

        /// <summary>
        /// Popluates this PurchaseOrder property
        /// </summary>
        /// <param name="id">
        /// Takes in a purchase order ID of type long
        /// </param>
        public void GetByID(long id)
        {
            PurchaseOrder = new GetPurchaseOrderManager(_connectionString).GetByID(id);
        }

        public void SaveChanges()
        {
            // TODO - Add a UpdatePurchaseOrderManager
        }

        private void ChangeOrderStatus(OrderStatusModel that)
        {
            PurchaseOrder.OrderStatus = that;
            PurchaseOrder.OrderStatusID = that.OrderStatusID;
        }

        /// <summary>
        /// Checks if the current purcase order status may be changed
        /// </summary>
        /// <param name="that">
        /// The order status to be changed to
        /// </param>
        /// <returns>
        /// True: if status may change
        /// False: if status may not change
        /// </returns>
        /// <exception cref="Exception">
        /// Throws an exception of type string if change is not allowed
        /// </exception>
        public bool CanChangeOrderStatus(OrderStatusModel that)
        {
            bool CanChange = false;
            string message = "";

            switch (that.OrderStatus)
            {
                case "Open": //Changing to open
                    if (PurchaseOrder.OrderStatus.OrderStatus!.Equals("Filled"))
                    {
                        message += "All order lines have been filled.\r\nIf a new order line is required,\r\n then please create a new purchase order";
                        throw new Exception(message);
                    }
                    else
                    {
                        CanChange = true;
                    }
                    break;
                case "Filled": //Changing to filled

                    if (PurchaseOrder.OrderStatus.OrderStatus!.Equals("Completed") || PurchaseOrder.OrderStatus.OrderStatus!.Equals("Cancelled"))
                    {
                        message += "The order status must first be altered to open and fully receipted before changing the status to Filled";
                        throw new Exception(message);
                    }
                    else if (PurchaseOrder.OrderStatus.OrderStatus!.Equals("Open"))
                    {
                        //Check that all order lines have been fully receipted
                        foreach (PurchaseOrderDetailModel orderLine in PurchaseOrder.PurchaseOrderDetails)
                        {
                            //Try to find the corresponding receipt
                            //if found then compare the quantities between each
                            if (PurchaseOrder.Receipts.FirstOrDefault(x => x.ProductID == orderLine.ProductID) != default)
                            {
                                //Compare quantities
                                ReceiptModel receipt = PurchaseOrder.Receipts.FirstOrDefault(x => x.ProductID == orderLine.ProductID)!;
                                if (receipt.QuantityReceipted != orderLine.Quantity)
                                {
                                    message += "All order lines must be fully receipted before the order status can be changed to Filled";
                                    throw new Exception(message);
                                }
                            }
                            else
                            {
                                message += "All order lines must be fully receipted before the order status can be marked as Filled";
                                throw new Exception(message);
                            }
                        }
                    }
                    else
                    {
                        //Already marked as filled if this stage is reached
                        //So no need to change
                        CanChange = false;
                    }
                    break;
                case "Completed": //Changing to completed
                    //Only open orders can be changed to completed
                    if (!PurchaseOrder.OrderStatus.OrderStatus!.Equals("Open"))
                    {
                        message += "Only open purchase orders may be marked as completed";
                        throw new Exception(message);
                    }
                    else
                    {
                        CanChange = true;
                    }
                        break;
                case "Cancelled": //Changing to Cancelled
                    //Only open orders can be changed to cancelled
                    if (!PurchaseOrder.OrderStatus.OrderStatus!.Equals("Open"))
                    {
                        message += "Only open purchase orders may be marked as completed";
                        throw new Exception(message);
                    }
                    else
                    {
                        CanChange = true;
                    }
                    break;
            }

            if (CanChange)
            {
                //Update the order status
                ChangeOrderStatus(that);
            }

            return CanChange;
        }

        public void AddOrderLine()
        {
            if (CanAddOrderLine())
            {
                PurchaseOrder.PurchaseOrderDetails.Add(new PurchaseOrderDetailModel());
            }
            
        }

        private bool CanAddOrderLine()
        {
            bool CanAddLine = true;

            //Check if the order status is open
            if (!PurchaseOrder.OrderStatus.OrderStatus!.Equals("Open"))
            {
                throw new Exception("Can only add a line if the order status is open.");
            }

            return CanAddLine;
        }

        public bool CanEditOrderLines()
        {
            if (PurchaseOrder.OrderStatus.OrderStatus!.Equals("Open"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}

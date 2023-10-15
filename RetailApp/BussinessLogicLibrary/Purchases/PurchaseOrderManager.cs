﻿using BussinessLogicLibrary.Products;
using BussinessLogicLibrary.Vendors;
using DataAccessLibrary.StatusRepository;
using ModelsLibrary;

namespace BussinessLogicLibrary.Purchases
{
    public class PurchaseOrderManager
    {
        public PurchaseOrderHeaderModel PurchaseOrder { get; private set; } = new PurchaseOrderHeaderModel();

        private string _connectionString;
        private readonly IVendorManager _vendorManager;
        private readonly IProductsManager _productsManager;

        public PurchaseOrderManager(string connectionString, IVendorManager vendorManager, IProductsManager productsManager)
        {
            _connectionString = connectionString;
            _vendorManager = vendorManager;
            _productsManager = productsManager;
        }

        /// <summary>
        /// Popluates this PurchaseOrder property
        /// </summary>
        /// <param name="id">
        /// Takes in a purchase order ID of type long
        /// </param>
        public void GetByID(long id)
        {
            PurchaseOrder = new GetPurchaseOrderManager(_connectionString, _vendorManager, _productsManager).GetByID(id);
        }

        /// <summary>
        /// Saves any valid changes made to the purchase order
        /// </summary>
        public void SaveChanges()
        {
            new UpdatePurchaseOrderManager(_connectionString, PurchaseOrder, _vendorManager, _productsManager);
        }

        private void GetOrderLineStatus(PurchaseOrderDetailModel orderLine)
        {
            Tuple<IEnumerable<StatusModel>, string> lineStatuses = new StatusRepository(_connectionString).GetAll().ToTuple();
            //Check for errors
            if (lineStatuses.Item2 == null)
            {
                //no errors
                orderLine.OrderLineStatus = lineStatuses.Item1.First(x => x.Status!.Equals("Open"));
                orderLine.OrderLineStatusID = orderLine.OrderLineStatus.StatusID;
            }
            else
            {
                throw new Exception(lineStatuses.Item2);
            }
        }

        public void AddOrderLine()
        {
            if (CanAddOrderLine())
            {
                PurchaseOrder.PurchaseOrderDetails.Add(new PurchaseOrderDetailModel());
                int index = PurchaseOrder.PurchaseOrderDetails.Count - 1;
                //Add order line status of open
                GetOrderLineStatus(PurchaseOrder.PurchaseOrderDetails[index]);
            }

        }

        private bool CanAddOrderLine()
        {
            bool CanAddLine = true;

            //Check if the order status is open
            if (!PurchaseOrder.OrderStatus.Status!.Equals("Open"))
            {
                throw new Exception("Can only add a line if the order status is open.");
            }

            return CanAddLine;
        }

        public bool CanEditOrderLines()
        {
            if (PurchaseOrder.OrderStatus.Status!.Equals("Open"))
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

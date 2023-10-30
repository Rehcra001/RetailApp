using BussinessLogicLibrary.Customers;
using BussinessLogicLibrary.Issues;
using BussinessLogicLibrary.Products;
using BussinessLogicLibrary.Statuses;
using ModelsLibrary;
using ModelsLibrary.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLogicLibrary.Sales
{
    public class GetFullSalesOrderByID : IGetFullSalesOrderByID
    {
        private SalesOrderHeaderModel SalesOrder { get; set; }

        private readonly IGetSalesOrderDetailsByIDManager _getSalesOrderDetailsByIDManager;
        private readonly IGetSalesOrderHeaderByIDManager _getSalesOrderHeaderByIDManager;
        private readonly ICustomerManager _customerManager;
        private readonly IProductsManager _productsManager;
        private readonly IIssuesManager _issueManager;
        private readonly IStatusManager _statusManager;

        public GetFullSalesOrderByID(IGetSalesOrderDetailsByIDManager getSalesOrderDetailsByIDManager,
                                     IGetSalesOrderHeaderByIDManager getSalesOrderHeaderByIDManager,
                                     ICustomerManager customerManager,
                                     IProductsManager productsManager,
                                     IIssuesManager issueManager,
                                     IStatusManager statusManager)
        {
            _getSalesOrderDetailsByIDManager = getSalesOrderDetailsByIDManager;
            _getSalesOrderHeaderByIDManager = getSalesOrderHeaderByIDManager;
            _customerManager = customerManager;
            _productsManager = productsManager;
            _issueManager = issueManager;
            _statusManager = statusManager;
        }

        public SalesOrderHeaderModel GetByID(long id)
        {
            GetHeader(id);
            GetDetails(id);
            FillHeader();
            FillDetails();

            return SalesOrder;
        }

        private void GetHeader(long id)
        {
            SalesOrder = _getSalesOrderHeaderByIDManager.GetByID(id);
        }

        private void GetDetails(long id)
        {
            SalesOrder.SalesOrderDetails = _getSalesOrderDetailsByIDManager.GetByID(id).ToList();
        }

        private void FillHeader()
        {
            // Add customer object
            SalesOrder.Customer = _customerManager.GetByID(SalesOrder.CustomerID);
            // Add status objec
            SalesOrder.OrderStatus = _statusManager.GetByID(SalesOrder.OrderStatusID);
            // Add issues list object
            SalesOrder.Issues = _issueManager.GetBySalesOrderID(SalesOrder.SalesOrderID);
        }

        private void FillDetails()
        {
            //Get products
            LoadProducts();
            //Get statuses
            LoadLineStatus();
        }

        private void LoadProducts()
        {
            IEnumerable<ProductModel> products = _productsManager.GetAll();

            foreach (SalesOrderDetailModel orderLine in SalesOrder.SalesOrderDetails)
            {
                orderLine.Product = products.First(x => x.ProductID == orderLine.ProductID);
            }
        }

        private void LoadLineStatus()
        {
            IEnumerable<StatusModel> statuses = _statusManager.GetAll();

            foreach (SalesOrderDetailModel orderLine in SalesOrder.SalesOrderDetails)
            {
                orderLine.OrderLineStatus = statuses.First(x => x.StatusID == orderLine.OrderLineStatusID);
            }
        }
    }
}

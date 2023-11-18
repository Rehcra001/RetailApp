using BussinessLogicLibrary.Categories;
using BussinessLogicLibrary.CompanyDetail;
using BussinessLogicLibrary.Customers;
using BussinessLogicLibrary.InventoryTransactions;
using BussinessLogicLibrary.Issues;
using BussinessLogicLibrary.Products;
using BussinessLogicLibrary.Purchases;
using BussinessLogicLibrary.Receipts;
using BussinessLogicLibrary.Sales;
using BussinessLogicLibrary.SalesMetrics;
using BussinessLogicLibrary.Statuses;
using BussinessLogicLibrary.UnitPers;
using BussinessLogicLibrary.VAT;
using BussinessLogicLibrary.Vendors;
using DataAccessLibrary;
using DataAccessLibrary.CategoryRepository;
using DataAccessLibrary.CompanyDetailRepository;
using DataAccessLibrary.CustomerRepository;
using DataAccessLibrary.InventoryTransactionRepository;
using DataAccessLibrary.IssuesRepository;
using DataAccessLibrary.ProductRepository;
using DataAccessLibrary.PurchaseOrderDetailRepository;
using DataAccessLibrary.PurchaseOrderHeaderRepository;
using DataAccessLibrary.ReceiptRepository;
using DataAccessLibrary.SalesMetricsRepository;
using DataAccessLibrary.SalesOrderDetailRepository;
using DataAccessLibrary.SalesOrderHeaderRepository;
using DataAccessLibrary.StatusRepository;
using DataAccessLibrary.UnitsPerRepository;
using DataAccessLibrary.VATRepository;
using DataAccessLibrary.VendorRepository;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ModelsLibrary.RepositoryInterfaces;
using RetailAppUI.Services;
using RetailAppUI.ViewModels;
using RetailAppUI.ViewModels.Adminstration;
using RetailAppUI.ViewModels.Products;
using RetailAppUI.ViewModels.Purchases;
using RetailAppUI.ViewModels.Reports;
using RetailAppUI.ViewModels.Reports.SalesMetrics;
using RetailAppUI.ViewModels.Sales;
using RetailAppUI.Views;
using System;
using System.IO;
using System.Windows;

namespace RetailAppUI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private ServiceProvider _serviceProvider;

        /// <summary>
        /// Adds access to appsettings.json
        /// </summary>
        /// <returns>
        /// Returns an IConfiguration
        /// </returns>
        private IConfiguration AddConfiguration()
        {
            IConfigurationBuilder builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");
            return builder.Build();
        }

        public App()
        {

            IServiceCollection services = new ServiceCollection();

            //Views and viewModels
            services.AddSingleton<MainView>(provider => new MainView
            {
                DataContext = provider.GetRequiredService<MainViewModel>()
            });
            services.AddSingleton<MainViewModel>();
            services.AddSingleton<HomeViewModel>();
            services.AddTransient<VendorViewModel>();
            services.AddTransient<CustomerViewModel>();
            services.AddTransient<CompanyDetailViewModel>();
            services.AddTransient<ProductsSwitchboardViewModel>();
            services.AddTransient<AddNewProductViewModel>();
            services.AddTransient<ProductViewModel>();
            services.AddTransient<AdministrativeSwitchboardViewModel>();
            services.AddTransient<ProductCategoryViewModel>();
            services.AddTransient<ProductUnitPerViewModel>();
            services.AddTransient<AddNewPurchaseOrderViewModel>();
            services.AddTransient<PurchaseOrdersSwitchboardViewModel>();
            services.AddTransient<PurchaseOrderViewModel>();
            services.AddTransient<AddNewSalesOrderViewModel>();
            services.AddTransient<SalesOrderSwitchboardViewModel>();
            services.AddTransient<SalesOrderViewModel>();
            services.AddTransient<ReportsSwitchboardViewModel>();
            services.AddTransient<SalesMetricsYTDViewModel>();

            //Add appsettings.json Configuration
            services.AddSingleton(AddConfiguration());

            //Sql connection
            services.AddSingleton<IRelationalDataAccess, RelationalDataAccess>();

            //Repositories
            services.AddTransient<ICompanyDetailRepository, CompanyDetailRepository>();
            services.AddTransient<IVendorRepository, VendorRepository>();
            services.AddTransient<ICustomerRepository, CustomerRepository>();
            services.AddTransient<ICategoryRepository, CategoryRepository>();
            services.AddTransient<IUnitsPerRepository, UnitsPerRepository>();
            services.AddTransient<IInventoryTransactionRepository, InventoryTransactionRepository>();
            services.AddTransient<IProductRepository, ProductRepository>();
            services.AddTransient<IStatusRepository, StatusRepository>();
            services.AddTransient<IReceiptsRepository, ReceiptRepository>();
            services.AddTransient<IPurchaseOrderDetailRepository, PurchaseOrderDetailRepository>();
            services.AddTransient<IPurchaseOrderHeaderRepository, PurchaseOrderHeaderRepository>();
            services.AddTransient<IVATRepository, VATRepository>();
            services.AddTransient<ISalesOrderHeaderRepository, SalesOrderHeaderRepository>();
            services.AddTransient<ISalesOrderDetailRepository, SalesOrderDetailRepository>();
            services.AddTransient<IIssueRepository, IssuesRepository>();
            services.AddTransient<ISalesMetricsYTDRepository, SalesMetricsYTDRepository>();

            //Managers
            services.AddTransient<ICompanyDetailManager, CompanyDetailManager>();
            services.AddTransient<IVendorManager, VendorManager>();
            services.AddTransient<ICustomerManager, CustomerManager>();
            services.AddTransient<ICategoryManager, CategoryManager>();
            services.AddTransient<IUnitPerManager, UnitPerManager>();
            services.AddTransient<IInventoryTransactionsManager, InventoryTransactionsManager>();
            services.AddTransient<IStatusManager, StatusManager>();
            services.AddTransient<IReceiptManager, ReceiptManager>();
            services.AddTransient<IVATManager, VATManager>();
            services.AddTransient<IIssuesManager, IssuesManager>();

            //Products
            services.AddTransient<IProductsManager, ProductsManager>();
            services.AddTransient<IProductManager, ProductManager>();
            services.AddTransient<IProductsManager, ProductsManager>();

            
            //Purchase 
            services.AddTransient<IAddNewPurchaseOrderManager, AddNewPurchaseOrderManager>();
            services.AddTransient<IGetPurchaseOrderManager, GetPurchaseOrderManager>();
            services.AddTransient<IPurchaseOrdersListManager, PurchaseOrdersListManager>();
            services.AddTransient<IUpdatePurchaseOrderManager, UpdatePurchaseOrderManager>();
            services.AddTransient<IPurchaseOrderManager, PurchaseOrderManager>();

            //Sales
            services.AddTransient<ISalesManager, SalesManager>();
            services.AddTransient<IInsertSalesOrderManager, InsertSalesOrderManager>();
            services.AddTransient<IGetAllSalesOrderDetailsManager, GetAllSalesOrderDetailsManager>();
            services.AddTransient<IGetAllSalesOrderManager, GetAllSalesOrderManager>();
            services.AddTransient<IGetSalesOrderDetailsByIDManager, GetSalesOrderDetailsByIDManager>();
            services.AddTransient<IGetSalesOrderHeaderByIDManager, GetSalesOrderHeaderByIDManager>();
            services.AddTransient<IValidateExistingSalesOrderDetails, ValidateExistingSalesOrderDetails>();
            services.AddTransient<IValidateExistingSalesOrderHeader, ValidateExistingSalesOrderHeader>();
            services.AddTransient<IUpdateSalesOrderManager, UpdateSalesOrderManager>();
            services.AddTransient<IGetFullSalesOrderByID, GetFullSalesOrderByID>();

            //Sales Metrics
            services.AddTransient<ISalesMetricsManager, SalesMetricsManager>();
            services.AddTransient<ITop10ProductsByRevenueYTDChart, Top10ProductsByRevenueYTDChart>();
            services.AddTransient<IMonthlyRevenueYTDChart, MonthlyRevenueYTDChart>();
            services.AddTransient<ISalesRevenueYTD, SalesRevenueYTD>();


            //Services
            services.AddSingleton<INavigationService, NavigationService>();
            services.AddSingleton<IConnectionStringService, ConnectionStringService>();
            services.AddSingleton<ICurrentViewService, CurrentViewService>();
            services.AddSingleton<ISharedDataService, SharedDataService>();

            services.AddSingleton<Func<Type, BaseViewModel>>(ServiceProvider => viewModelType => (BaseViewModel)ServiceProvider.GetRequiredService(viewModelType));



            _serviceProvider = services.BuildServiceProvider();

        }

        protected override void OnStartup(StartupEventArgs e)
        {
            var mainView = _serviceProvider.GetRequiredService<MainView>();
            mainView.Show();
            base.OnStartup(e);
        }
    }
}

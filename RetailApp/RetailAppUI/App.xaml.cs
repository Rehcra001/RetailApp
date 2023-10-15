using BussinessLogicLibrary.Categories;
using BussinessLogicLibrary.CompanyDetail;
using BussinessLogicLibrary.Customers;
using BussinessLogicLibrary.InventoryTransactions;
using BussinessLogicLibrary.UnitPers;
using BussinessLogicLibrary.Vendors;
using DataAccessLibrary;
using DataAccessLibrary.CategoryRepository;
using DataAccessLibrary.CompanyDetailRepository;
using DataAccessLibrary.CustomerRepository;
using DataAccessLibrary.InventoryTransactionRepository;
using DataAccessLibrary.UnitsPerRepository;
using DataAccessLibrary.VendorRepository;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ModelsLibrary.RepositoryInterfaces;
using RetailAppUI.Services;
using RetailAppUI.ViewModels;
using RetailAppUI.ViewModels.Adminstration;
using RetailAppUI.ViewModels.Products;
using RetailAppUI.ViewModels.Purchases;
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

            //Managers
            services.AddTransient<ICompanyDetailManager, CompanyDetailManager>();
            services.AddTransient<IVendorManager, VendorManager>();
            services.AddTransient<ICustomerManager, CustomerManager>();
            services.AddTransient<ICategoryManager, CategoryManager>();
            services.AddTransient<IUnitPerManager, UnitPerManager>();
            services.AddTransient<IInventoryTransactionsManager, InventoryTransactionsManager>();

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

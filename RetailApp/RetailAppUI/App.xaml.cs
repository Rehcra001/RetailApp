using Microsoft.Extensions.DependencyInjection;
using RetailAppUI.Services;
using RetailAppUI.ViewModels;
using RetailAppUI.ViewModels.Adminstration;
using RetailAppUI.ViewModels.Products;
using RetailAppUI.Views;
using System;
using System.Windows;

namespace RetailAppUI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private ServiceProvider __serviceProvider;

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

            //Services
            services.AddSingleton<INavigationService, NavigationService>();
            services.AddSingleton<IConnectionStringService, ConnectionStringService>();
            services.AddSingleton<ICurrentViewService, CurrentViewService>();
            services.AddSingleton<ISharedDataService, SharedDataService>();

            services.AddSingleton<Func<Type, BaseViewModel>>(ServiceProvider => viewModelType => (BaseViewModel)ServiceProvider.GetRequiredService(viewModelType));

            __serviceProvider = services.BuildServiceProvider();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            var mainView = __serviceProvider.GetRequiredService<MainView>();
            mainView.Show();
            base.OnStartup(e);
        }
    }
}

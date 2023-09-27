using RetailAppUI.Commands;
using RetailAppUI.Services;
using RetailAppUI.ViewModels.Products;
using RetailAppUI.ViewModels.Adminstration;

namespace RetailAppUI.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
		private INavigationService _navigation;

		public INavigationService Navigation
		{
			get { return _navigation; }
			set { _navigation = value; OnPropertyChanged(); }
		}

        private ICurrentViewService _currentView;

        public ICurrentViewService CurrentView
        {
            get { return _currentView; }
            set { _currentView = value; OnPropertyChanged(); }
        }



        public RelayCommand NavigateToVendorViewCommand { get; set; }
        public RelayCommand NavigateToCustomerViewCommand { get; set; }
        public RelayCommand NavigateToCompanyDetailViewCommand { get; set; }
        public RelayCommand NavigateToProductsSwithboardViewCommand { get; set; }

        public MainViewModel(INavigationService navigation, ICurrentViewService currentView)
        {
            Navigation = navigation;
            CurrentView = currentView;
            CurrentView.CurrentView = "MainView";

            NavigateToVendorViewCommand = new RelayCommand(NavigateToVendorView, CanNavigateToVendorView);
            NavigateToCustomerViewCommand = new RelayCommand(NavigateToCustomerView, CanNavigateToCustomerView);
            NavigateToCompanyDetailViewCommand = new RelayCommand(NavigateToCompanyDetailView, CanNavigateToCompanyDetailView);
            NavigateToProductsSwithboardViewCommand = new RelayCommand(NavigateToProductsSwithboardView, CanNavigateToProductsSwithboardView);
        }

        private bool CanNavigateToProductsSwithboardView(object obj)
        {
            return !CurrentView.CurrentView!.Equals("ProductViews");
        }

        private void NavigateToProductsSwithboardView(object obj)
        {
            CurrentView.CurrentView = "ProductViews";
            Navigation.NavigateTo<ProductsSwitchboardViewModel>();
        }

        private bool CanNavigateToCompanyDetailView(object obj)
        {
            return !CurrentView.CurrentView!.Equals("CompanyDetailView");
        }

        private void NavigateToCompanyDetailView(object obj)
        {
            CurrentView.CurrentView = "CompanyDetailView";
            Navigation.NavigateTo<CompanyDetailViewModel>();
        }

        private bool CanNavigateToCustomerView(object obj)
        {
            return !CurrentView.CurrentView!.Equals("CustomerView");
        }

        private void NavigateToCustomerView(object obj)
        {
            CurrentView.CurrentView = "CustomerView";
            Navigation.NavigateTo<CustomerViewModel>();
        }

        private bool CanNavigateToVendorView(object obj)
        {
            return !CurrentView.CurrentView!.Equals("VendorView");
        }

        private void NavigateToVendorView(object obj)
        {
            CurrentView.CurrentView = "VendorView";
            Navigation.NavigateTo<VendorViewModel>();
        }
    }
}

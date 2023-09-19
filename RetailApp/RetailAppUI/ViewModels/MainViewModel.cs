using RetailAppUI.Commands;
using RetailAppUI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public RelayCommand NavigateToCustomerViewModel { get; set; }
        public MainViewModel(INavigationService navigation, ICurrentViewService currentView)
        {
            Navigation = navigation;
            CurrentView = currentView;
            CurrentView.CurrentView = "MainView";

            NavigateToVendorViewCommand = new RelayCommand(NavigateToVendorView, CanNavigateToVendorView);
            NavigateToCustomerViewModel = new RelayCommand(NavigateToCustomerView, CanNavigateToCustomerView);
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

using RetailAppUI.Commands;
using RetailAppUI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetailAppUI.ViewModels.Adminstration
{
    public class AdministrativeSwitchboardViewModel : BaseViewModel
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
            set { _currentView = value; }
        }


        public RelayCommand CloseViewCommand { get; set; }
        public RelayCommand NavigateToVendorViewCommand { get; set; }
        public RelayCommand NavigateToCustomerViewCommand { get; set; }
        public RelayCommand NavigateToCompanyDetailViewCommand { get; set; }

        public AdministrativeSwitchboardViewModel(INavigationService navigation, ICurrentViewService currentView)
        {
            Navigation = navigation;
            CurrentView = currentView;

            CloseViewCommand = new RelayCommand(CloseView, CanCloseView);
            NavigateToVendorViewCommand = new RelayCommand(NavigateToVendorView, CanNavigateToVendorView);
            NavigateToCustomerViewCommand = new RelayCommand(NavigateToCustomerView, CanNavigateToCustomerView);
            NavigateToCompanyDetailViewCommand = new RelayCommand(NavigateToCompanyDetailView, CanNavigateToCompanyDetailView);
        }



        private bool CanNavigateToCompanyDetailView(object obj)
        {
            return true;
        }

        private void NavigateToCompanyDetailView(object obj)
        {
            Navigation.NavigateTo<CompanyDetailViewModel>();
        }

        private bool CanNavigateToCustomerView(object obj)
        {
            return true;
        }

        private void NavigateToCustomerView(object obj)
        {
            Navigation.NavigateTo<CustomerViewModel>();
        }

        private bool CanNavigateToVendorView(object obj)
        {
            return true;
        }

        private void NavigateToVendorView(object obj)
        {
            Navigation.NavigateTo<VendorViewModel>();
        }
        private bool CanCloseView(object obj)
        {
            return true;
        }

        private void CloseView(object obj)
        {
            CurrentView.CurrentView = "HomeView";
            Navigation.NavigateTo<HomeViewModel>();
        }
    }
}

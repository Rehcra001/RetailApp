﻿using RetailAppUI.Commands;
using RetailAppUI.Services;
using RetailAppUI.ViewModels.Products;
using RetailAppUI.ViewModels.Adminstration;
using System;
using RetailAppUI.ViewModels.Purchases;

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
        
        public RelayCommand NavigateToProductsSwithboardViewCommand { get; set; }
        public RelayCommand NavigateToAdministrationSwitchboardViewCommand { get; set; }
        public RelayCommand NavigateToPurchaseOrderSwitchboardViewCommand { get; set; }

        public MainViewModel(INavigationService navigation, ICurrentViewService currentView)
        {
            Navigation = navigation;
            CurrentView = currentView;
            CurrentView.CurrentView = "MainView";

            
            NavigateToProductsSwithboardViewCommand = new RelayCommand(NavigateToProductsSwithboardView, CanNavigateToProductsSwithboardView);
            NavigateToAdministrationSwitchboardViewCommand = new RelayCommand(NavigateToAdministrationSwitchboardView, CanNavigateToAdministrationSwitchboardView);
            NavigateToPurchaseOrderSwitchboardViewCommand = new RelayCommand(NavigateToPurchaseOrderSwitchboardView, CanNavigateToPurchaseOrderSwitchboardView);
        }

        private bool CanNavigateToPurchaseOrderSwitchboardView(object obj)
        {
            return !CurrentView.CurrentView!.Equals("PurchaseOrders");
        }

        private void NavigateToPurchaseOrderSwitchboardView(object obj)
        {
            CurrentView.CurrentView = "PurchaseOrders";
            Navigation.NavigateTo<PurchaseOrdersSwitchboardViewModel>();
        }

        private bool CanNavigateToAdministrationSwitchboardView(object obj)
        {
            return !CurrentView.CurrentView!.Equals("Administration");
        }

        private void NavigateToAdministrationSwitchboardView(object obj)
        {
            CurrentView.CurrentView = "Administration";
            Navigation.NavigateTo<AdministrativeSwitchboardViewModel>();
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
    }
}

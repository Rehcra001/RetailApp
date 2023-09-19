﻿using RetailAppUI.Commands;
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


        public RelayCommand NavigateToVendorViewCommand { get; set; }
        public RelayCommand NavigateToCustomerViewModel { get; set; }
        public MainViewModel(INavigationService navigation)
        {
            Navigation = navigation;

            NavigateToVendorViewCommand = new RelayCommand(NavigateToVendorView, CanNavigateToVendorView);
            NavigateToCustomerViewModel = new RelayCommand(NavigateToCustomerView, CanNavigateToCustomerView);
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
    }
}

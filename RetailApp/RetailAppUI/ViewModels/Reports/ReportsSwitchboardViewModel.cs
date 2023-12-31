﻿using BussinessLogicLibrary.SalesMetrics;
using ChartModelsLibrary.ChartModels;
using RetailAppUI.Commands;
using RetailAppUI.Services;
using RetailAppUI.ViewModels.Reports.SalesMetrics;
using RetailAppUI.ViewModels.Reports.VendorMetrics;
using System;
using System.Collections.Generic;

namespace RetailAppUI.ViewModels.Reports
{
    public class ReportsSwitchboardViewModel : BaseViewModel
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


        //commands
        public RelayCommand CloseViewCommand { get; set; }
        public RelayCommand NavigateToSalesDashboardYTDCommand { get; set; }
        public RelayCommand NavigateToSalesDashboardMTDCommand { get; set; }
        public RelayCommand NavigateToVendorDashboardYTDCommand { get; set; }

        public ReportsSwitchboardViewModel(INavigationService navigation,
                                           ICurrentViewService currentView)
        {
            Navigation = navigation;
            CurrentView = currentView;

            //Instantiate commands
            CloseViewCommand = new RelayCommand(CloseView, CanCloseView);
            NavigateToSalesDashboardYTDCommand = new RelayCommand(NavigateToSalesDashboardYTD, CanNavigateToSalesDashboardYTD);
            NavigateToSalesDashboardMTDCommand = new RelayCommand(NavigateToSalesDashboardMTD, CanNavigateToSalesDashboardMTDC);
            NavigateToVendorDashboardYTDCommand = new RelayCommand(NavigateToVendorDashboardYTD, CanNavigateToVendorDashboardYTD);
        }

        private bool CanNavigateToVendorDashboardYTD(object obj)
        {
            return true;
        }

        private void NavigateToVendorDashboardYTD(object obj)
        {
            Navigation.NavigateTo<VendorMetricsYTDViewModel>();
        }

        private bool CanNavigateToSalesDashboardMTDC(object obj)
        {
            return true;
        }

        private void NavigateToSalesDashboardMTD(object obj)
        {
            Navigation.NavigateTo<SalesMetricsMTDViewModel>();
        }

        private bool CanNavigateToSalesDashboardYTD(object obj)
        {
            return true;
        }

        private void NavigateToSalesDashboardYTD(object obj)
        {
            
            Navigation.NavigateTo<SalesMetricsYTDViewModel>();
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

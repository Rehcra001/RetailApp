using RetailAppUI.Commands;
using RetailAppUI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetailAppUI.ViewModels.Products
{
    public class ProductsSwitchboardViewModel : BaseViewModel
    {
		private ICurrentViewService	_currentView;

		public ICurrentViewService CurrentView
		{
			get { return _currentView; }
			set { _currentView = value; OnPropertyChanged(); }
		}

		private INavigationService _navigation;

		public INavigationService Navigation
		{
			get { return _navigation; }
			set { _navigation = value; OnPropertyChanged(); }
		}

		private ISharedDataService _sharedData;

		public ISharedDataService SharedData
		{
			get { return _sharedData; }
			set { _sharedData = value; }
		}


		public RelayCommand CloseViewCommand { get; set; }

        public ProductsSwitchboardViewModel(INavigationService navigation, ICurrentViewService currentView, ISharedDataService sharedData)
        {
			Navigation = navigation;
			CurrentView = currentView;
			SharedData = sharedData;

			CloseViewCommand = new RelayCommand(CloseView, CanCloseView);
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

using DataAccessLibrary.VendorRepository;
using ModelsLibrary;
using RetailAppUI.Commands;
using RetailAppUI.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace RetailAppUI.ViewModels
{
    public class CompanyDetailViewModel : BaseViewModel
    {
        private CompanyDetailRepository _companyDetailRepository;
        private CompanyDetailModel? _companyDetail;
        private string _connectionString;
        private INavigationService _navigation;
        private string _state;
        private CompanyDetailModel undoCompanyDetailEdit;
        private bool _textReadOnly;
        private ICurrentViewService _currentView;
        private string _addIsVisible;

        public CompanyDetailModel CompanyDetail
        {
            get { return _companyDetail; }
            set { _companyDetail = value; OnPropertyChanged(); }
        }

        public bool TextReadOnly { get => _textReadOnly; set { _textReadOnly = value; OnPropertyChanged(); } }

        public string AddIsVisible { get => _addIsVisible; set { _addIsVisible = value; OnPropertyChanged(); } }

        public INavigationService Navigation { get => _navigation; set { _navigation = value; OnPropertyChanged(); } }

        public ICurrentViewService CurrentView { get => _currentView; set { _currentView = value; OnPropertyChanged(); } }

        public RelayCommand CloseViewCommand { get; set; }
        public RelayCommand AddNewCompanyDetailCommand { get; set; }
        public RelayCommand EditCompanyDetailCommand { get; set; }
        public RelayCommand SaveCompanyDetailCommand { get; set; }
        public RelayCommand CancelActionCommand { get; set; }

        public CompanyDetailViewModel(IConnectionStringService connectionString, INavigationService navigation, ICurrentViewService currentView)
        {
            Navigation = navigation;
            CurrentView = currentView;


            _connectionString = connectionString.GetConnectionString();
            _companyDetailRepository = new CompanyDetailRepository(_connectionString);
            Tuple<CompanyDetailModel, string> companyDetail = _companyDetailRepository.Get().ToTuple();
            if (companyDetail.Item2 == null)
            {
                //No error retrieving company detail
                CompanyDetail = companyDetail.Item1;
                if (CompanyDetail.CompanyID == 0)
                {
                    //No record returned allow adding
                    AddIsVisible = "Visible";
                }
                else
                {
                    //Valid record returned disallow adding a new one
                    AddIsVisible = "Hidden";
                }
            }
            else
            {
                MessageBox.Show(companyDetail.Item2, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            CloseViewCommand = new RelayCommand(CloseView, CanCloseView);
            AddNewCompanyDetailCommand = new RelayCommand(AddNewCompanyDetail, CanAddNewCompanyDetail);
            SaveCompanyDetailCommand = new RelayCommand(SaveCompanyDetail, CanSaveCompanyDetail);
            CancelActionCommand = new RelayCommand(CancelAction, CanCancelAction);
            EditCompanyDetailCommand = new RelayCommand(EditCompanyDetail, CanEditCompanyDetail);


            SetState("View");
        }

        private bool CanEditCompanyDetail(object obj)
        {
            return _state.Equals("View") && CompanyDetail != null && CompanyDetail.CompanyID != 0;
        }

        private void EditCompanyDetail(object obj)
        {
            //Save a copy of the existing Company detail prior to editing
            int id = CompanyDetail.CompanyID;
            Tuple<CompanyDetailModel, string> companyDetail = _companyDetailRepository.Get().ToTuple();
            //check if there was an error retrieving
            if (companyDetail.Item2 == null)
            {
                //No error
                undoCompanyDetailEdit = companyDetail.Item1;
                SetState("Edit");
            }
            else
            {
                //error copying existing company detail
                MessageBox.Show(companyDetail.Item2, "Error Editing Company Detail", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private bool CanCancelAction(object obj)
        {
            return !_state.Equals("View");
        }

        private void CancelAction(object obj)
        {
            if (_state.Equals("Add"))
            {
                CompanyDetail = new CompanyDetailModel();
            }
            else if (_state.Equals("Edit"))
            {
                //Undo any editing
                CompanyDetail = undoCompanyDetailEdit;
            }
            SetState("View");
        }

        private bool CanSaveCompanyDetail(object obj)
        {
            return !_state.Equals("View");
        }

        private void SaveCompanyDetail(object obj)
        {
            if (CompanyDetail.Validate())
            {
                if (_state.Equals("Add"))
                {
                    Tuple<CompanyDetailModel, string> companyDetail = _companyDetailRepository.Insert(CompanyDetail).ToTuple();
                    //check if insert was successful
                    if (companyDetail.Item2 == null)
                    {
                        CompanyDetail = companyDetail.Item1;
                        //Hide the Add new button as only one record is allowed
                        AddIsVisible = "Hidden";
                    }
                    else
                    {
                        //Problem adding the new company detail
                        MessageBox.Show(companyDetail.Item2, "Error Adding Company Detail", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                }
                else if (_state.Equals("Edit"))
                {
                    string errorMessage = _companyDetailRepository.Update(CompanyDetail);
                    //Check if no error saving
                    if (errorMessage != null)
                    {
                        //Error saving edit
                        MessageBox.Show(errorMessage, "Error Saving Company Detail", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                }
            }
            else
            {
                MessageBox.Show(CompanyDetail.ValidationMessage, "Data Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }


            SetState("View");
        }

        private bool CanAddNewCompanyDetail(object obj)
        {
            return _state.Equals("View");
        }

        private void AddNewCompanyDetail(object obj)
        {
            CompanyDetail = new CompanyDetailModel();
            SetState("Add");
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

        private void SetState(string state)
        {
            _state = state;

            switch (_state)
            {
                case "View":
                    TextReadOnly = true;
                    break;
                default:
                    TextReadOnly = false;
                    break;
            }
        }
    }
}

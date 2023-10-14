using BussinessLogicLibrary.CompanyDetail;
using DataAccessLibrary.CompanyDetailRepository;
using ModelsLibrary;
using RetailAppUI.Commands;
using RetailAppUI.Services;
using System;
using System.Windows;

namespace RetailAppUI.ViewModels.Adminstration
{
    public class CompanyDetailViewModel : BaseViewModel
    {
        private readonly ICompanyDetailManager _companyDetailManager;
        private CompanyDetailModel? _companyDetail;
        private INavigationService _navigation;
        private IConnectionStringService _connection;
        private string _state;
        private CompanyDetailModel undoCompanyDetailEdit;
        private bool _textReadOnly;
        private string _addIsVisible;


        public CompanyDetailModel? CompanyDetail { get => _companyDetail; set { _companyDetail = value; OnPropertyChanged(); } }

        public bool TextReadOnly { get => _textReadOnly; set { _textReadOnly = value; OnPropertyChanged(); } }

        public string AddIsVisible { get => _addIsVisible; set { _addIsVisible = value; OnPropertyChanged(); } }

        public INavigationService Navigation { get => _navigation; set { _navigation = value; OnPropertyChanged(); } }


        public RelayCommand CloseViewCommand { get; set; }
        public RelayCommand AddNewCompanyDetailCommand { get; set; }
        public RelayCommand EditCompanyDetailCommand { get; set; }
        public RelayCommand SaveCompanyDetailCommand { get; set; }
        public RelayCommand CancelActionCommand { get; set; }

        public CompanyDetailViewModel(INavigationService navigation, ICompanyDetailManager companyDetailManager)
        {
            Navigation = navigation;

            _companyDetailManager = companyDetailManager;

            //retrieve the company detail
            try
            {
                CompanyDetail = _companyDetailManager.Get();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to retrieve the Company Detail.\r\n\r\n" + ex.Message, "Retrieval Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Navigation.NavigateTo<AdministrativeSwitchboardViewModel>();
            }

            //Set the visibility level of AddIsVisible
            if (CompanyDetail!.CompanyID == 0)
            {
                AddIsVisible = "Visible";
            }
            else
            {
                AddIsVisible = "Hidden";
            }

            //Instantiate the commands
            CloseViewCommand = new RelayCommand(CloseView, CanCloseView);
            AddNewCompanyDetailCommand = new RelayCommand(AddNewCompanyDetail, CanAddNewCompanyDetail);
            SaveCompanyDetailCommand = new RelayCommand(SaveCompanyDetail, CanSaveCompanyDetail);
            CancelActionCommand = new RelayCommand(CancelAction, CanCancelAction);
            EditCompanyDetailCommand = new RelayCommand(EditCompanyDetail, CanEditCompanyDetail);

            //Set the state
            SetState("View");
        }

        private bool CanEditCompanyDetail(object obj)
        {
            return _state.Equals("View") && CompanyDetail != null && CompanyDetail.CompanyID != 0;
        }

        private void EditCompanyDetail(object obj)
        {
            //Save a copy of the existing Company detail prior to editing            
            //retrieve the company detail
            try
            {
                undoCompanyDetailEdit = _companyDetailManager.Get();
                SetState("Edit");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error retrieving model for undo action.\r\n\r\n" + ex.Message, "Retrieval Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Navigation.NavigateTo<AdministrativeSwitchboardViewModel>();
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
            if (_state.Equals("Add"))
            {
                try
                {
                    CompanyDetail = _companyDetailManager.Insert(CompanyDetail);
                    //Hide the Add new button as only one record is allowed
                    AddIsVisible = "Hidden";
                    //Give user acknowledgement of the successful save
                    MessageBox.Show("Company Detail Saved", "Save Company Detail", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error saving the model.\r\n\r\n" + ex.Message, "Retrieval Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }
            else if (_state.Equals("Edit"))
            {
                try
                {
                    _companyDetailManager.Update(CompanyDetail);

                    //Give user acknowledgement of the successful save
                    MessageBox.Show("Company Detail Saved", "Save Company Detail", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error saving the model.\r\n\r\n" + ex.Message, "Update Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
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
            Navigation.NavigateTo<AdministrativeSwitchboardViewModel>();
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

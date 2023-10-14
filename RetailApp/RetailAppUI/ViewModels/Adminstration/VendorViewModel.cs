using BussinessLogicLibrary.Vendors;
using DataAccessLibrary.VendorRepository;
using ModelsLibrary;
using RetailAppUI.Commands;
using RetailAppUI.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;

namespace RetailAppUI.ViewModels.Adminstration
{
    public class VendorViewModel : BaseViewModel
    {
        private IVendorManager _vendorManager;
        private VendorModel? _vendor;
        private ObservableCollection<VendorModel> _vendors;
        private INavigationService _navigation;
        private int _vendorIndex;
        private string _state;
        private VendorModel _undoVendorEdit;
        private bool _textReadOnly;

        public VendorModel Vendor
        {
            get { return _vendor; }
            set { _vendor = value; OnPropertyChanged(); }
        }

        public ObservableCollection<VendorModel> Vendors
        {
            get { return _vendors; }
            set { _vendors = value; OnPropertyChanged(); }
        }

        public int VendorIndex
        {
            get { return _vendorIndex; }
            set { _vendorIndex = value; OnPropertyChanged(); }
        }

        private bool _checkEnabled;

        public bool CheckEnabled
        {
            get { return _checkEnabled; }
            set { _checkEnabled = value; OnPropertyChanged(); }
        }


        public bool TextReadOnly { get => _textReadOnly; set { _textReadOnly = value; OnPropertyChanged(); } }

        public INavigationService Navigation { get => _navigation; set { _navigation = value; OnPropertyChanged(); } }


        public RelayCommand CloseViewCommand { get; set; }
        public RelayCommand AddNewVendorCommand { get; set; }
        public RelayCommand EditVendorCommand { get; set; }

        //public RelayCommand DeleteVendorCommand { get; set; }
        public RelayCommand SaveVendorCommand { get; set; }
        public RelayCommand CancelActionCommand { get; set; }

        public VendorViewModel(INavigationService navigation, IVendorManager vendorManager)
        {
            Navigation = navigation;
            _vendorManager = vendorManager;

            //Populate Vendors Property
            try
            {
                Vendors = new ObservableCollection<VendorModel>(_vendorManager.GetAll());
                VendorIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error retrieving Vendors.\r\n\r\n" + ex.Message, "Retrieval Errro", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            CloseViewCommand = new RelayCommand(CloseView, CanCloseView);
            AddNewVendorCommand = new RelayCommand(AddNewVendor, CanAddNewVendor);
            SaveVendorCommand = new RelayCommand(SaveVendor, CanSaveVendor);
            CancelActionCommand = new RelayCommand(CancelAction, CanCancelAction);
            EditVendorCommand = new RelayCommand(EditVendor, CanEditVendor);
            //DeleteVendorCommand = new RelayCommand(DeleteVendor, CanDeleteVendor);


            SetState("View");
        }

        //private bool CanDeleteVendor(object obj)
        //{
        //    return _state.Equals("View") && Vendors.Count > 0;
        //}

        //private void DeleteVendor(object obj)
        //{
        //    MessageBoxResult result = MessageBox.Show("Are you sure you want to delete this vendor?", 
        //                                              "Delete Vendor", 
        //                                              MessageBoxButton.YesNo, 
        //                                              MessageBoxImage.Question, 
        //                                              MessageBoxResult.No);
        //    if (result == MessageBoxResult.Yes)
        //    {
        //        string errorMessage = _vendorRepository.Delete(Vendor);
        //        //check for errors
        //        if (errorMessage != null)
        //        {
        //            MessageBox.Show("Unable to delete this vendor. \r\n" + errorMessage, 
        //                            "Error Deleting Vendor", 
        //                            MessageBoxButton.OK, 
        //                            MessageBoxImage.Error);
        //            return;
        //        }
        //        else
        //        {
        //            Vendors.RemoveAt(VendorIndex);
        //            VendorIndex = 0;
        //        }
        //    }
        //}

        private bool CanEditVendor(object obj)
        {
            return _state.Equals("View") && Vendors.Count > 0;
        }

        private void EditVendor(object obj)
        {
            SetState("Edit");
            //Save a copy of the existing vendor prior to editing
            int id = Vendors[VendorIndex].VendorID;

            try
            {
                _undoVendorEdit = _vendorManager.GetByID(id);
            }
            catch (Exception ex)
            {
                //error copying existing vendor
                MessageBox.Show("Error retrieve the undo vendor model.\r\n\r\n" + ex.Message, "Retrieval Error", MessageBoxButton.OK, MessageBoxImage.Error);
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
                //Remove the vendor model added
                //Index is already on the new record
                Vendors.RemoveAt(VendorIndex);
                //Change the index to the first record
                VendorIndex = 0;
            }
            else if (_state.Equals("Edit"))
            {
                //Undo any editing
                Vendor = _undoVendorEdit;
            }
            SetState("View");
        }

        private bool CanSaveVendor(object obj)
        {
            return !_state.Equals("View");
        }

        private void SaveVendor(object obj)
        {
            if (_state.Equals("Add"))
            {
                try
                {
                    Vendors[VendorIndex] = _vendorManager.Insert(Vendors[VendorIndex]);
                }
                catch (Exception ex)
                {
                    //Problem adding the new vendor
                    MessageBox.Show("Error Saving the new vendor.\r\n\r\n" + ex.Message, "Error Adding Vendor", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }
            else if (_state.Equals("Edit"))
            {
                try
                {
                    _vendorManager.Update(Vendors[VendorIndex]);
                }
                catch (Exception ex)
                {
                    //Error saving edit
                    MessageBox.Show("Error saving the vendor.\r\n\r\n" + ex.Message, "Error Saving Vendor", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }


            SetState("View");
        }

        private bool CanAddNewVendor(object obj)
        {
            return _state.Equals("View");
        }

        private void AddNewVendor(object obj)
        {
            //Add and move to new vendor model
            Vendors.Add(new VendorModel());
            VendorIndex = Vendors.Count - 1;
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
                    CheckEnabled = false;
                    break;
                default:
                    TextReadOnly = false;
                    CheckEnabled = true;
                    break;
            }
        }
    }
}

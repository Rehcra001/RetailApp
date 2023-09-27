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
        private VendorRepository _vendorRepository;
        private VendorModel? _vendor;
        private ObservableCollection<VendorModel> _vendors;
        private string _connectionString;
        private INavigationService _navigation;
        private int _vendorIndex;
        private string _state;
        private VendorModel undoVendorEdit;
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

        public bool TextReadOnly { get => _textReadOnly; set { _textReadOnly = value; OnPropertyChanged(); }  }

        public INavigationService Navigation { get => _navigation; set { _navigation = value; OnPropertyChanged(); } }


        public RelayCommand CloseViewCommand { get; set; }
        public RelayCommand AddNewVendorCommand { get; set; }
        public RelayCommand EditVendorCommand { get; set; }
        //public RelayCommand DeleteVendorCommand { get; set; }
        public RelayCommand SaveVendorCommand { get; set; }
        public RelayCommand CancelActionCommand { get; set; }

        public VendorViewModel(IConnectionStringService connectionString, INavigationService navigation)
        {
            Navigation = navigation;


            _connectionString = connectionString.GetConnectionString();
            _vendorRepository = new VendorRepository(_connectionString);
            Tuple<IEnumerable<VendorModel>, string> vendors = _vendorRepository.GetAll().ToTuple();
            if (vendors.Item2 == null)
            {
                //No error retrieving vendors
                Vendors = new ObservableCollection<VendorModel>(vendors.Item1);
                VendorIndex = 0;
            }
            else
            {
                MessageBox.Show(vendors.Item2, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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
            //Save a copy of the existing vendor prior to editing
            int id = Vendors[VendorIndex].VendorID;
            Tuple<VendorModel, string> vendor = _vendorRepository.GetByID(id).ToTuple();
            //check if there was an error retrieving
            if (vendor.Item2 == null)
            {
                //No error
                undoVendorEdit = vendor.Item1;
                SetState("Edit");
            }
            else
            {
                //error copying existing vendor
                MessageBox.Show(vendor.Item2, "Error Editing Vendor", MessageBoxButton.OK, MessageBoxImage.Error);
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
                Vendor = undoVendorEdit;
            }
            SetState("View");
        }

        private bool CanSaveVendor(object obj)
        {
            return !_state.Equals("View");
        }

        private void SaveVendor(object obj)
        {
            if (Vendor.Validate())
            {
                if (_state.Equals("Add"))
                {
                    Tuple<VendorModel, string> vendor = _vendorRepository.Insert(Vendors[VendorIndex]).ToTuple();
                    //check if insert was successful
                    if (vendor.Item2 == null)
                    {
                        Vendors[VendorIndex] = vendor.Item1;

                        VendorIndex = Vendors.Count - 1;
                    }
                    else
                    {
                        //Problem adding the new vendor
                        MessageBox.Show(vendor.Item2, "Error Adding Vendor", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                }
                else if (_state.Equals("Edit"))
                {
                    string errorMessage = _vendorRepository.Update(Vendors[VendorIndex]);
                    //Check if no error saving
                    if (errorMessage != null)
                    {
                        //Error saving edit
                        MessageBox.Show(errorMessage, "Error Saving Vendor", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                }
            }
            else
            {
                MessageBox.Show(Vendor.ValidationMessage, "Data Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
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
                    break;
                default:
                    TextReadOnly = false;
                    break;
            }
        }
    }
}

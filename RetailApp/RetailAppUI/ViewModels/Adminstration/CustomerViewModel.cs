using BussinessLogicLibrary.Customers;
using DataAccessLibrary.CustomerRepository;
using ModelsLibrary;
using RetailAppUI.Commands;
using RetailAppUI.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;

namespace RetailAppUI.ViewModels.Adminstration
{
    public class CustomerViewModel : BaseViewModel
    {
        private ICustomerManager _customerManager;
        private CustomerModel? _customer;
        private ObservableCollection<CustomerModel> _customers;
        private INavigationService _navigation;
        private int _customerIndex;
        private string _state;
        private CustomerModel _undoCustomerEdit;
        private bool _textReadOnly;

        public CustomerModel Customer
        {
            get { return _customer; }
            set { _customer = value; OnPropertyChanged(); }
        }

        public ObservableCollection<CustomerModel> Customers
        {
            get { return _customers; }
            set { _customers = value; OnPropertyChanged(); }
        }

        public int CustomerIndex
        {
            get { return _customerIndex; }
            set { _customerIndex = value; OnPropertyChanged(); }
        }

        public bool TextReadOnly { get => _textReadOnly; set { _textReadOnly = value; OnPropertyChanged(); } }

        public INavigationService Navigation { get => _navigation; set { _navigation = value; OnPropertyChanged(); } }


        public RelayCommand CloseViewCommand { get; set; }
        public RelayCommand AddNewCustomerCommand { get; set; }
        public RelayCommand EditCustomerCommand { get; set; }
        //public RelayCommand DeleteCustomerCommand { get; set; }
        public RelayCommand SaveCustomerCommand { get; set; }
        public RelayCommand CancelActionCommand { get; set; }

        public CustomerViewModel(INavigationService navigation, ICustomerManager customerManager)
        {
            Navigation = navigation;
            _customerManager = customerManager;

            //Retriev a list of customers and populate Customers Property
            try
            {
                Customers = new ObservableCollection<CustomerModel>(_customerManager.GetAll());
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error retrieving the customers.\r\n\r\n." + ex.Message, "Retrieval Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            //Instantiate commands
            CloseViewCommand = new RelayCommand(CloseView, CanCloseView);
            AddNewCustomerCommand = new RelayCommand(AddNewCustomer, CanAddNewCustomer);
            SaveCustomerCommand = new RelayCommand(SaveCustomer, CanSaveCustomer);
            CancelActionCommand = new RelayCommand(CancelAction, CanCancelAction);
            EditCustomerCommand = new RelayCommand(EditCustomer, CanEditCustomer);
            //DeleteCustomerCommand = new RelayCommand(DeleteCustomer, CanDeleteCustomer);


            SetState("View");
        }

        //private bool CanDeleteCustomer(object obj)
        //{
        //    return _state.Equals("View") && Customers.Count > 0;
        //}

        //private void DeleteCustomer(object obj)
        //{
        //    MessageBoxResult result = MessageBox.Show("Are you sure you want to delete this customer?",
        //                                              "Delete Customer",
        //                                              MessageBoxButton.YesNo,
        //                                              MessageBoxImage.Question,
        //                                              MessageBoxResult.No);
        //    if (result == MessageBoxResult.Yes)
        //    {
        //        string errorMessage = _customerRepository.Delete(Customer);
        //        //check for errors
        //        if (errorMessage != null)
        //        {
        //            MessageBox.Show("Unable to delete this customer. \r\n" + errorMessage,
        //                            "Error Deleting Customer",
        //                            MessageBoxButton.OK,
        //                            MessageBoxImage.Error);
        //            return;
        //        }
        //        else
        //        {
        //            Customers.RemoveAt(CustomerIndex);
        //            CustomerIndex = 0;
        //        }
        //    }
        //}

        private bool CanEditCustomer(object obj)
        {
            return _state.Equals("View") && Customers.Count > 0;
        }

        private void EditCustomer(object obj)
        {
            //Save a copy of the existing vendor prior to editing
            int id = Customers[CustomerIndex].CustomerID;
            try
            {
                _undoCustomerEdit = _customerManager.GetByID(id);
                SetState("Edit");
            }
            catch (Exception ex)
            {
                //error copying existing customer
                MessageBox.Show("Error retrieving the undo customer model.\r\n\r\n" + ex.Message, "Retrieval Error", MessageBoxButton.OK, MessageBoxImage.Error);
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
                Customers.RemoveAt(CustomerIndex);
                //Change the index to the first record
                CustomerIndex = 0;
            }
            else if (_state.Equals("Edit"))
            {
                //Undo any editing
                Customer = _undoCustomerEdit;
            }
            SetState("View");
        }

        private bool CanSaveCustomer(object obj)
        {
            return !_state.Equals("View");
        }

        private void SaveCustomer(object obj)
        {
            if (_state.Equals("Add"))
            {
                try
                {
                    Customers[CustomerIndex] = _customerManager.Insert(Customers[CustomerIndex]);

                    CustomerIndex = Customers.Count - 1;
                }
                catch (Exception ex)
                {
                    //Problem adding the new vendor
                    MessageBox.Show("Error saving the customer.\r\n\r\n" + ex.Message, "Saving Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }
            else if (_state.Equals("Edit"))
            {
                try
                {
                    _customerManager.Update(Customers[CustomerIndex]);
                }
                catch (Exception ex)
                {
                    //Problem adding the new vendor
                    MessageBox.Show("Error saving the customer.\r\n\r\n" + ex.Message, "Saving Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }

            SetState("View");
        }

        private bool CanAddNewCustomer(object obj)
        {
            return _state.Equals("View");
        }

        private void AddNewCustomer(object obj)
        {
            //Add and move to new vendor model
            Customers.Add(new CustomerModel());
            CustomerIndex = Customers.Count - 1;
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

using DataAccessLibrary.CustomerRepository;
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
    public class CustomerViewModel : BaseViewModel
    {
        private CustomerRepository _customerRepository;
        private CustomerModel? _customer;
        private ObservableCollection<CustomerModel> _customers;
        private string _connectionString;
        private INavigationService _navigation;
        private int _customerIndex;
        private string _state;
        private CustomerModel undoCustomerEdit;
        private bool _textReadOnly;
        private ICurrentViewService _currentView;

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

        public ICurrentViewService CurrentView { get => _currentView; set { _currentView = value; OnPropertyChanged(); } }

        public RelayCommand CloseViewCommand { get; set; }
        public RelayCommand AddNewCustomerCommand { get; set; }
        public RelayCommand EditCustomerCommand { get; set; }
        public RelayCommand DeleteCustomerCommand { get; set; }
        public RelayCommand SaveCustomerCommand { get; set; }
        public RelayCommand CancelActionCommand { get; set; }

        public CustomerViewModel(IConnectionStringService connectionString, INavigationService navigation, ICurrentViewService currentView)
        {
            Navigation = navigation;
            CurrentView = currentView;


            _connectionString = connectionString.GetConnectionString();
            _customerRepository = new CustomerRepository(_connectionString);
            Tuple<IEnumerable<CustomerModel>, string> customers = _customerRepository.GetAll().ToTuple();
            if (customers.Item2 == null)
            {
                //No error retrieving vendors
                Customers = new ObservableCollection<CustomerModel>(customers.Item1);
                CustomerIndex = 0;
            }
            else
            {
                MessageBox.Show(customers.Item2, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            CloseViewCommand = new RelayCommand(CloseView, CanCloseView);
            AddNewCustomerCommand = new RelayCommand(AddNewCustomer, CanAddNewCustomer);
            SaveCustomerCommand = new RelayCommand(SaveCustomer, CanSaveCustomer);
            CancelActionCommand = new RelayCommand(CancelAction, CanCancelAction);
            EditCustomerCommand = new RelayCommand(EditCustomer, CanEditCustomer);
            DeleteCustomerCommand = new RelayCommand(DeleteCustomer, CanDeleteCustomer);


            SetState("View");
        }

        private bool CanDeleteCustomer(object obj)
        {
            return _state.Equals("View") && Customers.Count > 0;
        }

        private void DeleteCustomer(object obj)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to delete this customer?",
                                                      "Delete Customer",
                                                      MessageBoxButton.YesNo,
                                                      MessageBoxImage.Question,
                                                      MessageBoxResult.No);
            if (result == MessageBoxResult.Yes)
            {
                string errorMessage = _customerRepository.Delete(Customer);
                //check for errors
                if (errorMessage != null)
                {
                    MessageBox.Show("Unable to delete this customer. \r\n" + errorMessage,
                                    "Error Deleting Customer",
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Error);
                    return;
                }
                else
                {
                    Customers.RemoveAt(CustomerIndex);
                    CustomerIndex = 0;
                }
            }
        }

        private bool CanEditCustomer(object obj)
        {
            return _state.Equals("View") && Customers.Count > 0;
        }

        private void EditCustomer(object obj)
        {
            //Save a copy of the existing vendor prior to editing
            int id = Customers[CustomerIndex].CustomerID;
            Tuple<CustomerModel, string> customer = _customerRepository.GetByID(id).ToTuple();
            //check if there was an error retrieving
            if (customer.Item2 == null)
            {
                //No error
                undoCustomerEdit = customer.Item1;
                SetState("Edit");
            }
            else
            {
                //error copying existing vendor
                MessageBox.Show(customer.Item2, "Error Editing Customer", MessageBoxButton.OK, MessageBoxImage.Error);
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
                Customer = undoCustomerEdit;
            }
            SetState("View");
        }

        private bool CanSaveCustomer(object obj)
        {
            return !_state.Equals("View");
        }

        private void SaveCustomer(object obj)
        {
            if (Customer.Validate())
            {
                if (_state.Equals("Add"))
                {
                    Tuple<CustomerModel, string> customer = _customerRepository.Insert(Customers[CustomerIndex]).ToTuple();
                    //check if insert was successful
                    if (customer.Item2 == null)
                    {
                        Customers[CustomerIndex] = customer.Item1;

                        CustomerIndex = Customers.Count - 1;
                    }
                    else
                    {
                        //Problem adding the new vendor
                        MessageBox.Show(customer.Item2, "Error Adding Customer", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                }
                else if (_state.Equals("Edit"))
                {
                    string errorMessage = _customerRepository.Update(Customers[CustomerIndex]);
                    //Check if no error saving
                    if (errorMessage != null)
                    {
                        //Error saving edit
                        MessageBox.Show(errorMessage, "Error Saving Customer", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                }
            }
            else
            {
                MessageBox.Show(Customer.ValidationMessage, "Data Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
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

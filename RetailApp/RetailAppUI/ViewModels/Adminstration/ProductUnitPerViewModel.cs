using DataAccessLibrary.CategoryRepository;
using ModelsLibrary;
using RetailAppUI.Commands;
using RetailAppUI.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows;
using DataAccessLibrary.UnitsPerRepository;

namespace RetailAppUI.ViewModels.Adminstration
{
    public class ProductUnitPerViewModel : BaseViewModel
    {
        private UnitsPerRepository _unitPerRepository;
        private UnitsPerModel _undoEdit;

        private ICollectionView _unitPerCollectionView;
        public ICollectionView UnitPerCollectionView
        {
            get { return _unitPerCollectionView; }
            set { _unitPerCollectionView = value; OnPropertyChanged(); }
        }

        private UnitsPerModel _selectedUnitPer;
        public UnitsPerModel SelectedUnitPer
        {
            get { return _selectedUnitPer; }
            set { _selectedUnitPer = value; OnPropertyChanged(); }
        }

        private ObservableCollection<UnitsPerModel> _unitPers;
        public ObservableCollection<UnitsPerModel> UnitPers
        {
            get { return _unitPers; }
            set { _unitPers = value; OnPropertyChanged(); }
        }



        private IConnectionStringService _connectionString;
        private string _state;

        public IConnectionStringService ConnectionString
        {
            get { return _connectionString; }
            set { _connectionString = value; }
        }

        private INavigationService _navigation;

        public INavigationService Navigation
        {
            get { return _navigation; }
            set { _navigation = value; OnPropertyChanged(); }
        }

        private bool _textReadOnly;
        public bool TextReadOnly
        {
            get { return _textReadOnly; }
            set { _textReadOnly = value; OnPropertyChanged(); }
        }



        //Commands
        public RelayCommand CloseViewCommand { get; set; }
        public RelayCommand AddNewCommand { get; set; }
        public RelayCommand EditCommand { get; set; }
        public RelayCommand SaveCommand { get; set; }
        public RelayCommand CancelActionCommand { get; set; }

        public ProductUnitPerViewModel(IConnectionStringService connectionString, INavigationService navigation)
        {
            ConnectionString = connectionString;
            Navigation = navigation;

            //Unit Per repository
            _unitPerRepository = new UnitsPerRepository(ConnectionString.GetConnectionString());

            //Load Unit Pers
            Tuple<IEnumerable<UnitsPerModel>, string> unitPers = _unitPerRepository.GetAll().ToTuple();
            //check for errors
            if (unitPers.Item2 == null)
            {
                //No Errors
                UnitPers = new ObservableCollection<UnitsPerModel>(unitPers.Item1);
                //Add to collection View
                UnitPerCollectionView = CollectionViewSource.GetDefaultView(UnitPers);

                //set SelectedUnitPer
                if (UnitPers.Any<UnitsPerModel>())
                {
                    SelectedUnitPer = UnitPers.First();
                }
            }
            else
            {
                //Error raised by database
                MessageBox.Show("Error retrieving the unit pers.\r\n" + unitPers.Item2, "Unit Per Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            //Instantiate commands
            CloseViewCommand = new RelayCommand(CloseView, CanCloseView);
            AddNewCommand = new RelayCommand(AddNewUnitPer, CanAddNewUnitPer);
            EditCommand = new RelayCommand(EditUnitPer, CanEditUnitPer);
            SaveCommand = new RelayCommand(SaveUnitPer, CanSaveUnitPer);
            CancelActionCommand = new RelayCommand(CancelAction, CanCancelAction);


            SetState("View");
        }

        private bool CanCancelAction(object obj)
        {
            return !_state.Equals("View");
        }

        private void CancelAction(object obj)
        {
            //check if state is add or edit
            if (_state.Equals("Add"))
            {
                //Remove the added UnitPerModel
                int index = UnitPers.Count - 1;
                UnitPers.RemoveAt(index);
                //set SelectedUnitPer 
                if (UnitPers.Any<UnitsPerModel>())
                {
                    SelectedUnitPer = UnitPers.First();
                }
            }
            else if (_state.Equals("Edit"))
            {
                SelectedUnitPer.UnitPer = _undoEdit.UnitPer;
                SelectedUnitPer.UnitPerDescription = _undoEdit.UnitPerDescription;
                SelectedUnitPer = (UnitsPerModel)UnitPerCollectionView.CurrentItem;
                UnitPerCollectionView.Refresh();
            }
            SetState("View");
        }

        private bool CanSaveUnitPer(object obj)
        {
            return !_state.Equals("View");
        }

        private void SaveUnitPer(object obj)
        {
            //Check if state is add or edit
            if (_state.Equals("Add"))
            {
                if (!string.IsNullOrWhiteSpace(SelectedUnitPer.UnitPer))
                {
                    //Check that the new unit per does not exist yet
                    foreach (UnitsPerModel model in UnitPers)
                    {
                        if (SelectedUnitPer.UnitPerID != 0 && SelectedUnitPer.UnitPer.Equals(model.UnitPer, StringComparison.InvariantCultureIgnoreCase))
                        {
                            MessageBox.Show("This Unit Per already extis.", "Unit Per Exists", MessageBoxButton.OK, MessageBoxImage.Stop);
                            return;
                        }
                    }
                }

                //Validate data
                if (SelectedUnitPer.Validate())
                {
                    //Add to database
                    Tuple<UnitsPerModel, string> unitPer = _unitPerRepository.Insert(SelectedUnitPer).ToTuple();
                    //Check for errors
                    if (unitPer.Item2 == null)//Will be null if no error raised
                    {
                        //Add the unique id returned from the database
                        SelectedUnitPer.UnitPerID = unitPer.Item1.UnitPerID;
                        UnitPerCollectionView.Refresh();
                    }
                    else
                    {
                        MessageBox.Show(unitPer.Item2, "Error Saving Unit Per", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                }
                else
                {
                    //Shows data validation errors
                    MessageBox.Show(SelectedUnitPer.ValidationMessage, "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }
            else if (_state.Equals("Edit"))
            {
                if (!string.IsNullOrWhiteSpace(SelectedUnitPer.UnitPer))
                {
                    //Check that the edited category does not exist yet
                    foreach (UnitsPerModel model in UnitPers)
                    {
                        if (SelectedUnitPer.UnitPerID != model.UnitPerID && SelectedUnitPer.UnitPer.Equals(model.UnitPer, StringComparison.InvariantCultureIgnoreCase))
                        {
                            MessageBox.Show("This unit per already extis.", "Unit Per Exists", MessageBoxButton.OK, MessageBoxImage.Stop);
                            return;
                        }
                    }
                }

                if (SelectedUnitPer.Validate())
                {
                    //No validation errors
                    //try and save edited unit per
                    string errorMessage = _unitPerRepository.Update(SelectedUnitPer);
                    //Check for error
                    if (errorMessage != null)
                    {
                        //Error raised by database
                        MessageBox.Show(errorMessage, "Error Saving Unit Per", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                }
                else
                {
                    //Validation error
                    //Shows data validation errors
                    MessageBox.Show(SelectedUnitPer.ValidationMessage, "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }
            SetState("View");
        }

        private bool CanEditUnitPer(object obj)
        {
            return _state.Equals("View");
        }

        private void EditUnitPer(object obj)
        {
            //Save the current UnitPer
            _undoEdit = new UnitsPerModel();
            _undoEdit.UnitPerID = SelectedUnitPer.UnitPerID;
            _undoEdit.UnitPer = SelectedUnitPer.UnitPer;
            _undoEdit.UnitPerDescription = SelectedUnitPer.UnitPerDescription;
            SetState("Edit");
        }

        private bool CanAddNewUnitPer(object obj)
        {
            return _state.Equals("View");
        }

        private void AddNewUnitPer(object obj)
        {
            //Add new UnitPerModel to UnitPers
            UnitPers.Add(new UnitsPerModel());
            //Move to the newly created objec
            UnitPerCollectionView.MoveCurrentToLast();
            SelectedUnitPer = (UnitsPerModel)UnitPerCollectionView.CurrentItem;
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

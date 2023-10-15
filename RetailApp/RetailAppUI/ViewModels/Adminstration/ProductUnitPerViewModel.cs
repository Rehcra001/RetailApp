using BussinessLogicLibrary.UnitPers;
using ModelsLibrary;
using RetailAppUI.Commands;
using RetailAppUI.Services;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Data;

namespace RetailAppUI.ViewModels.Adminstration
{
    public class ProductUnitPerViewModel : BaseViewModel
    {
        private readonly IUnitPerManager _unitPerManager;
        private UnitsPerModel _undoEdit;
        private string _state;

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

        public ProductUnitPerViewModel(INavigationService navigation, IUnitPerManager unitPerManager)
        {
            Navigation = navigation;
            _unitPerManager = unitPerManager;


            //Load Unit Pers
            try
            {
                UnitPers = new ObservableCollection<UnitsPerModel>(_unitPerManager.GetAll());
                //Add to collection View
                UnitPerCollectionView = CollectionViewSource.GetDefaultView(UnitPers);
            }
            catch (Exception ex)
            {
                // Error raised by database
                MessageBox.Show("Error retrieving the unit pers.\r\n\r\n" + ex.Message, "Retrieval Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Navigation.NavigateTo<AdministrativeSwitchboardViewModel>();
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

                try
                {
                    //Add the unique id returned from the database
                    SelectedUnitPer.UnitPerID = _unitPerManager.Insert(SelectedUnitPer).UnitPerID;
                    UnitPerCollectionView.Refresh();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error Saving Unit Per" + ex.Message, "Saving Error", MessageBoxButton.OK, MessageBoxImage.Error);
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

                try
                {
                    _unitPerManager.Update(SelectedUnitPer);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error Saving Unit Per" + ex.Message, "Saving Error", MessageBoxButton.OK, MessageBoxImage.Error);
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

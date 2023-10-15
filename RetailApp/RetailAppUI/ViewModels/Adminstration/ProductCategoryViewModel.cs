using BussinessLogicLibrary.Categories;
using DataAccessLibrary.CategoryRepository;
using ModelsLibrary;
using RetailAppUI.Commands;
using RetailAppUI.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Data;

namespace RetailAppUI.ViewModels.Adminstration
{
    public class ProductCategoryViewModel : BaseViewModel
    {
        private ICategoryManager _categoryManager;
        private string _state;
        private CategoryModel _undoEdit;

        private ICollectionView _categoryCollectionView;
        public ICollectionView CategoryCollectionView
        {
            get { return _categoryCollectionView; }
            set { _categoryCollectionView = value; OnPropertyChanged(); }
        }

        private CategoryModel _selectedCategory;
        public CategoryModel SelectedCategory
        {
            get { return _selectedCategory; }
            set { _selectedCategory = value; OnPropertyChanged(); }
        }

        private ObservableCollection<CategoryModel> _categories;
        public ObservableCollection<CategoryModel> Categories
        {
            get { return _categories; }
            set { _categories = value; OnPropertyChanged(); }
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

        public ProductCategoryViewModel(INavigationService navigation, ICategoryManager categoryManager)
        {
            Navigation = navigation;
            _categoryManager = categoryManager;

            //Load Categories
            try
            {
                Categories = new ObservableCollection<CategoryModel>(_categoryManager.GetAll());
                CategoryCollectionView = CollectionViewSource.GetDefaultView(Categories);

                //set SelectedCategory 
                if (Categories.Any<CategoryModel>())
                {
                    SelectedCategory = Categories.First();
                }
            }
            catch (Exception ex)
            {
                //Unable to retrieve categories
                MessageBox.Show("Error retrieving the categories.\r\n\r\n" + ex.Message, "Retrieval Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Navigation.NavigateTo<AdministrativeSwitchboardViewModel>();
            }

            //Instantiate commands
            CloseViewCommand = new RelayCommand(CloseView, CanCloseView);
            AddNewCommand = new RelayCommand(AddNewCategory, CanAddNewCategory);
            EditCommand = new RelayCommand(EditCategory, CanEditCategory);
            SaveCommand = new RelayCommand(SaveCategory, CanSaveCategory);
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
                //Remove the added CategoryModel
                int index = Categories.Count - 1;
                Categories.RemoveAt(index);
                //set SelectedCategory 
                if (Categories.Any<CategoryModel>())
                {
                    SelectedCategory = Categories.First();
                }
            }
            else if (_state.Equals("Edit"))
            {
                SelectedCategory.CategoryName = _undoEdit.CategoryName;
                SelectedCategory = (CategoryModel)CategoryCollectionView.CurrentItem;
                CategoryCollectionView.Refresh();
            }
            SetState("View");
        }

        private bool CanSaveCategory(object obj)
        {
            return !_state.Equals("View");
        }

        private void SaveCategory(object obj)
        {
            //Check if state is add or edit
            if (_state.Equals("Add"))
            {
                if (!string.IsNullOrWhiteSpace(SelectedCategory.CategoryName))
                {
                    //Check that the new category does not exist yet
                    foreach (CategoryModel model in Categories)
                    {
                        if (SelectedCategory.CategoryID != 0 && SelectedCategory.CategoryName.Equals(model.CategoryName, StringComparison.InvariantCultureIgnoreCase))
                        {
                            MessageBox.Show("This category already extis.", "Category Exists", MessageBoxButton.OK, MessageBoxImage.Stop);
                            return;
                        }
                    }
                }

                //Insert new category
                try
                {
                    //Add the unique id returned from the database
                    SelectedCategory.CategoryID = _categoryManager.Insert(SelectedCategory).CategoryID;
                    CategoryCollectionView.Refresh();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error saving the category.\r\n\r\n" + ex.Message, "Saving Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }
            else if (_state.Equals("Edit"))
            {
                if (!string.IsNullOrWhiteSpace(SelectedCategory.CategoryName))
                {
                    //Check that the edited category does not exist yet
                    foreach (CategoryModel model in Categories)
                    {
                        if (SelectedCategory.CategoryID != model.CategoryID && SelectedCategory.CategoryName.Equals(model.CategoryName, StringComparison.InvariantCultureIgnoreCase))
                        {
                            MessageBox.Show("This category already extis.", "Category Exists", MessageBoxButton.OK, MessageBoxImage.Stop);
                            return;
                        }
                    }
                }

                try
                {
                    _categoryManager.Update(SelectedCategory);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error saving the category.\r\n\r\n" + ex.Message, "Saving Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }
            SetState("View");
        }

        private bool CanEditCategory(object obj)
        {
            return _state.Equals("View");
        }

        private void EditCategory(object obj)
        {
            //Save the current category
            _undoEdit = new CategoryModel();
            _undoEdit.CategoryID = SelectedCategory.CategoryID;
            _undoEdit.CategoryName = SelectedCategory.CategoryName;
            SetState("Edit");
        }

        private bool CanAddNewCategory(object obj)
        {
            return _state.Equals("View");
        }

        private void AddNewCategory(object obj)
        {
            //Add new CategoryModel to Categories
            Categories.Add(new CategoryModel());
            //Move to the newly created objec
            CategoryCollectionView.MoveCurrentToLast();
            SelectedCategory = (CategoryModel)CategoryCollectionView.CurrentItem;
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

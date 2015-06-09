using RecEpee.DataAccess;
using RecEpee.Framework;
using RecEpee.Models;
using RecEpee.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;
using System.Windows.Input;

namespace RecEpee.ViewModels
{
    class AllRecipesViewModel : ViewModelBase
    {
        public AllRecipesViewModel()
        {
            _recipeRepository = Ioc.GetInstance<IDataRepository<Recipe>>();
            _dialogService = Ioc.GetInstance<IDialogService>();

            tryLoadRecipes();            

            Recipes = new ObservableCollection<RecipeViewModel>(_recipes.Select(r => new RecipeViewModel(r)));

            _selectedRecipeIndex = 0;
            _selectedRecipe = null;
            _searchText = "";
            _editing = false;

            SetUpCommands();

            SetUpRecipesCollectionView();
        }

        private void SetUpCommands()
        {
            AddRecipe = new RelayCommand((p) => addRecipe());
            RemoveRecipe = new RelayCommand((p) => removeRecipe());
            EditRecipe = new RelayCommand((p) => Editing = true);
            ShowRecipe = new RelayCommand((p) => Editing = false);
            Close = new RelayCommand((p) => close());
            About = new RelayCommand((p) => showAboutDialog());
            ClearSearch = new RelayCommand((p) => clearSearch());
        }

        private void SetUpRecipesCollectionView()
        {
            _recipesCollectionView = CollectionViewSource.GetDefaultView(Recipes);

            _recipesCollectionView.SetGrouping("Category");
            _recipesCollectionView.SetSorting("Category");
            _recipesCollectionView.SetSorting("Title");
            _recipesCollectionView.SetFiltering("Title", OnFilterRecipes);
        }

        private ICollectionView _recipesCollectionView;
        public ICollectionView RecipesCollectionView
        {
            get { return _recipesCollectionView; }
        }

        private bool OnFilterRecipes(object obj)
        {
            var recipe = (RecipeViewModel)obj;

            return recipe.Title.ToLowerInvariant().Contains(SearchText.ToLowerInvariant());
        }

        private void tryLoadRecipes()
        {
            try
            {
                _recipes = _recipeRepository.Load();
            }
            catch (Exception)
            {
                _recipes = new List<Recipe>();
            }
        }

        private IList<Recipe> _recipes;
        public ObservableCollection<RecipeViewModel> Recipes { get; private set; }

        private RecipeViewModel _selectedRecipe;
        public RecipeViewModel SelectedRecipe
        {
            get { return _selectedRecipe; }

            set
            {
                if (value == null) // when live sorting, item is unselected. Reassign it to preserve current selection
                {
                    value = _selectedRecipe;
                }

                SetProperty(value);
            }
        }

        private int _selectedRecipeIndex;
        public int SelectedRecipeIndex
        {
            get { return _selectedRecipeIndex; }
            set { SetProperty(value); }
        }

        private string _searchText;
        public string SearchText
        {
            get { return _searchText; }
            set { SetProperty(value); _recipesCollectionView.Refresh(); }
        }

        private bool _editing;
        public bool Editing
        {
            get { return _editing; }
            set { SetProperty(value); }
        }

        public ICommand AddRecipe { get; private set; }
        public ICommand RemoveRecipe { get; private set; }
        public ICommand EditRecipe { get; private set; }
        public ICommand ShowRecipe { get; private set; }
        public ICommand Close { get; private set; }
        public ICommand About { get; private set; }
        public ICommand ClearSearch { get; private set; }

        private void addRecipe()
        {
            var newRecipe = new RecipeViewModel(new Recipe { Title = "New recipe" });

            Recipes.Add(newRecipe);
            SelectedRecipe = newRecipe;
        }

        private void removeRecipe()
        {
            if (_dialogService.ShowConfirmationDialog() == true)
            {
                var nextSelectedIndex = GetNextSelectedIndex();

                Recipes.Remove(SelectedRecipe);
                SelectedRecipeIndex = nextSelectedIndex;
            }            
        }

        private int GetNextSelectedIndex()
        {
            return _selectedRecipeIndex > 0 ? _selectedRecipeIndex - 1 :
                _selectedRecipeIndex < Recipes.Count ? _selectedRecipeIndex : -1;
        }
        
        private void close()
        {
            _recipeRepository.Save(GetRecipes());
        }

        private List<Recipe> GetRecipes()
        {
            return Recipes.Select(vm => vm.Model).ToList();
        }

        private void showAboutDialog()
        {
            _dialogService.ShowAboutDialog();
        }        

        private void clearSearch()
        {
            SearchText = "";
        }   

        private IDataRepository<Recipe> _recipeRepository;
        private IDialogService _dialogService;
    }
}

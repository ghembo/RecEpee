using RecEpee.DataAccess;
using RecEpee.Framework;
using RecEpee.Models;
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

            tryLoadRecipes();            

            Recipes = new ObservableCollection<RecipeViewModel>(_recipes.Select(r => new RecipeViewModel(r)));

            _selectedRecipeIndex = 0;
            _selectedRecipe = null;
            _searchText = "";

            _addRecipe = new RelayCommand((p) => addRecipe());
            _removeRecipe = new RelayCommand((p) => removeRecipe());
            _close = new RelayCommand((p) => close());
            _about = new RelayCommand((p) => showAboutDialog());

            _collectionView = CollectionViewSource.GetDefaultView(Recipes);
            _collectionView.GroupDescriptions.Add(new PropertyGroupDescription("Category"));
            _collectionView.SortDescriptions.Add(new SortDescription("Title", ListSortDirection.Ascending));
            _collectionView.Filter = OnFilterRecipes;
            var cvls = _collectionView as ICollectionViewLiveShaping;
            cvls.IsLiveSorting = true;
            cvls.LiveSortingProperties.Add("Title");
            cvls.IsLiveGrouping = true;
            cvls.LiveGroupingProperties.Add("Category");
            cvls.IsLiveFiltering = true;
            cvls.LiveFilteringProperties.Add("Title");
        }

        private ICollectionView _collectionView;
        public ICollectionView CollectionView
        {
            get { return _collectionView; }
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
            set { SetProperty(value); _collectionView.Refresh(); }
        }

        private ICommand _addRecipe;
        public ICommand AddRecipe
        {
            get { return _addRecipe; }
        }

        private void addRecipe()
        {
            var newRecipe = new RecipeViewModel(new Recipe { Title = "New recipe" });

            Recipes.Add(newRecipe);
            SelectedRecipe = newRecipe;
        }

        private ICommand _removeRecipe;
        public ICommand RemoveRecipe
        {
            get { return _removeRecipe; }
        }

        private void removeRecipe()
        {
            var nextSelectedIndex = GetNextSelectedIndex();

            Recipes.Remove(SelectedRecipe);
            SelectedRecipeIndex = nextSelectedIndex;
        }

        private int GetNextSelectedIndex()
        {
            return _selectedRecipeIndex > 0 ? _selectedRecipeIndex - 1 :
                _selectedRecipeIndex < Recipes.Count ? _selectedRecipeIndex : -1;
        }

        private ICommand _close;
        public ICommand Close
        {
            get { return _close; }
        }

        private void close()
        {
            _recipeRepository.Save(GetRecipes());
        }

        private List<Recipe> GetRecipes()
        {
            return Recipes.Select(vm => vm.Model).ToList();
        }

        private ICommand _about;
        public ICommand About
        {
            get { return _about; }
        }

        private void showAboutDialog()
        {
            VvmBinder.GetView<AboutViewModel>().ShowDialog();
        }        

        private IDataRepository<Recipe> _recipeRepository;
    }
}

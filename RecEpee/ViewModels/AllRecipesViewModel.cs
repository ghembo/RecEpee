using RecEpee.DataAccess;
using RecEpee.Framework;
using RecEpee.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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

            _addRecipe = new RelayCommand((p) => addRecipe());
            _removeRecipe = new RelayCommand((p) => removeRecipe());
            _close = new RelayCommand((p) => close());           
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

        private ICommand _addRecipe;
        public ICommand AddRecipe
        {
            get { return _addRecipe; }
            set { SetProperty(value); }
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
            set { SetProperty(value); }
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
            set { SetProperty(value); }
        }

        private void close()
        {
            _recipeRepository.Save(GetRecipes());
        }

        private List<Recipe> GetRecipes()
        {
            return Recipes.Select(vm => vm.Model).ToList();
        }

        private IDataRepository<Recipe> _recipeRepository;
    }
}

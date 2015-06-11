using RecEpee.Framework;
using RecEpee.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace RecEpee.ViewModels
{
    class RecipeViewModel: ViewModel<Recipe>
    {
        public RecipeViewModel() : base()
        {
            Initialize();
        }

        public RecipeViewModel(Recipe recipe) : base(recipe)
        {
            Initialize();
        }

        protected override void OnBindModel()
        {
            _ingredients = ColLectionSyncher.GetSynchedCollection(Model.Ingredients);
        }

        private void Initialize()
        {
            RemoveIngredient = new RelayCommand((p) => removeIngredient(p));
            AddIngredient = new RelayCommand("Add ingredient", (p) => addIngredient(), (p) => canAddIngredient());
            _newIngredient = null;
        }

        public string Title
        {
            get { return Model.Title; }
            set { SetModelProperty(value); }
        }

        public string Description
        {
            get { return Model.Description; }
            set { SetModelProperty(value); }
        }

        public string Category
        {
            get { return Model.Category; }
            set { SetModelProperty(value); }
        }

        public static IReadOnlyCollection<string> Categories { get { return Recipe.Categories; } }

        public int Portions
        {
            get { return Model.Portions; }
            set { SetModelProperty(value); }
        }

        private ObservableCollection<Ingredient> _ingredients;
        public ObservableCollection<Ingredient> Ingredients
        {
            get { return _ingredients; }
            set { SetProperty(value); }
        }

        private int? _newPortions;
        public int? NewPortions
        {
            get { return _newPortions; }
            set { SetProperty(value); CalculateNewIngredients(); }
        }

        private ObservableCollection<Ingredient> _newIngredients;
        public ObservableCollection<Ingredient> NewIngredients
        {
            get { return _newIngredients; }
            set { SetProperty(value); }
        }

        private void CalculateNewIngredients()
        {
            if (NewPortions.HasValue)
            {
                NewIngredients = new ObservableCollection<Ingredient>(Model.Ingredients.Select(ing => ing.GetWithDifferentQuantity(GetNewQuantity(ing.Quantity))));
            }
            else
            {
                NewIngredients = null;
            }
            
        }

        private double? GetNewQuantity(double? quantity)
        {
            if (quantity.HasValue)
            {
                return (NewPortions.Value * quantity.Value) / Portions;
            }
            else
            {
                return null;
            }
            
        }

        private string _newIngredient;
        public string NewIngredient
        {
            get { return _newIngredient; }
            set { SetProperty(value); }
        }

        public ICommand AddIngredient { get; private set; }
        public ICommand RemoveIngredient { get; private set; }

        private bool canAddIngredient()
        {
            return string.IsNullOrWhiteSpace(NewIngredient) == false;
        }

        private void addIngredient()
        {
            var ingredient = IngredientParser.Parse(NewIngredient);
            Ingredients.Add(ingredient);

            NewIngredient = null;
        }

        private void removeIngredient(object item)
        {
            Ingredients.Remove((Ingredient)item);
        }
    }
}

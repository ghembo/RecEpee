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
        private const string TempExportPath = @"C:\temprecipes.html";
        private const string DefaultExportPath = @"C:\recipes.html";

        public AllRecipesViewModel()
        {
            _recipeRepository = Ioc.GetInstance<IDataRepository<Recipe>>();
            _exporter= Ioc.GetInstance<IExporter<Recipe>>();
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
            AddRecipe = new RelayCommand("Add recipe", (p) => { addRecipe(); Editing = true; });
            RemoveRecipe = new RelayCommand("Remove", (p) => removeRecipe());
            EditRecipe = new RelayCommand("Edit", (p) => Editing = true);
            ShowRecipe = new RelayCommand("Done", (p) => Editing = false);
            Close = new RelayCommand((p) => close());
            About = new RelayCommand("_About...", (p) => _dialogService.ShowAboutDialog());
            ClearSearch = new RelayCommand((p) => SearchText = "");
            Export = new RelayCommand("_Export", new KeyGesture(Key.E, ModifierKeys.Control), (p) => export());
            Print = new RelayCommand("_Print", new KeyGesture(Key.P, ModifierKeys.Control), (p) => print());
            PrintPreview = new RelayCommand("P_rint preview", new KeyGesture(Key.R, ModifierKeys.Control), (p) => showPrintPreview());
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
        public ICommand Export { get; private set; }
        public ICommand Print { get; private set; }
        public ICommand PrintPreview { get; private set; }

        private void addRecipe()
        {
            var newRecipe = new RecipeViewModel(Recipe.GetNewRecipe());

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

        private void print()
        {
            _exporter.Export(GetRecipes(), TempExportPath);

            Printer.PrintHtmlFile(TempExportPath);
        }

        private void showPrintPreview()
        {
            _exporter.Export(GetRecipes(), TempExportPath);

            Printer.ShowPrintPreviewForHtmlFile(TempExportPath);
        }

        private void export()
        {
            _exporter.Export(GetRecipes(), DefaultExportPath);
            Osal.ShowFileWithDefaultProgram(DefaultExportPath);
        }

        private IDataRepository<Recipe> _recipeRepository;
        private IExporter<Recipe> _exporter;
        private IDialogService _dialogService;
    }
}

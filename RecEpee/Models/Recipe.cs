using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace RecEpee.Models
{
    public class Recipe
    {
        public const string Uncategorized = "Nessuna categoria";
        public static readonly IReadOnlyCollection<string> Categories = new ReadOnlyCollection<string>(new string[] { "Antipasto", "Primo", "Secondo", "Contorno", "Dolce", Uncategorized });

        public Recipe()
        {
            Ingredients = new List<Ingredient>();
        }

        public static Recipe GetNewRecipe()
        {
            return new Recipe { Title = "New recipe", Portions = 4, Category = Uncategorized };
        }

        public string Title { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public int Portions { get; set; }

        // Must be like this to enable serialization
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists")]
        public List<Ingredient> Ingredients { get; set; }
    }
}

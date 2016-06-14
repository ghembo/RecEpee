using RecEpee.Models;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;

namespace RecEpee.DataAccess
{
    class SqLiteRecipeRepository : IDataRepository<Recipe>
    {
        private const string DefaultDbName = @"RecEpee.sqlite";
        private const string RecipeTableName = @"RECIPE";
        private const string IngredientTableName = @"INGREDIENT";

        public List<Recipe> Load()
        {
            var recipes = new List<Recipe>();

            if (File.Exists(DefaultDbName))
            {
                using (var connection = new SQLiteConnection("Data Source=" + DefaultDbName + ";Version=3;"))
                {
                    connection.Open();

                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = "select * from " + RecipeTableName;

                        var reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            long recipeId = (long)reader["id"];

                            using (var ingredientCommand = connection.CreateCommand())
                            {
                                ingredientCommand.CommandText = "select * from " + IngredientTableName + " where recipeId = " + recipeId;

                                var ingredientReader = ingredientCommand.ExecuteReader();

                                var ingredients = new List<Ingredient>();

                                while (ingredientReader.Read())
                                {
                                    ingredients.Add(new Ingredient()
                                    {
                                        Name = (string)ingredientReader["name"],
                                        Quantity = (double?)ingredientReader["quantity"],
                                        Unit = (string)ingredientReader["unit"]
                                    });
                                }

                                recipes.Add(new Recipe()
                                {
                                    Title = (string)reader["title"],
                                    Category = (string)reader["category"],
                                    Description = (string)reader["description"],
                                    Portions = (int)reader["portions"],
                                    Ingredients = ingredients
                                });
                            }                            
                        }
                    }
                }
            }

            return recipes;
        }

        public List<Recipe> Load(string path)
        {
            return Load();
        }

        public void Save(List<Recipe> recipes)
        {
            if (!File.Exists(DefaultDbName))
            {
                createDatabase();
            }            

            using (var connection = new SQLiteConnection("Data Source=" + DefaultDbName + ";Version=3;"))
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    var transaction = connection.BeginTransaction();

                    command.CommandText = "delete from " + RecipeTableName;
                    command.ExecuteNonQuery();

                    command.CommandText = "delete from " + IngredientTableName;
                    command.ExecuteNonQuery();

                    foreach (var recipe in recipes)
                    {
                        command.CommandText = string.Format("insert into {0} (title, category, description, portions) values ('{1}', '{2}', '{3}', {4})",
                            RecipeTableName, recipe.Title, recipe.Category, recipe.Description, recipe.Portions);

                        var inserted = command.ExecuteNonQuery();
                        var recipeId = connection.LastInsertRowId;

                        foreach (var ingredient in recipe.Ingredients)
                        {
                            command.CommandText = string.Format("insert into {0} (name, quantity, unit, recipeId) values ('{1}', {2}, '{3}', {4})",
                            IngredientTableName, ingredient.Name, ingredient.Quantity, ingredient.Unit, recipeId);

                            inserted = command.ExecuteNonQuery();
                        }
                    }

                    transaction.Commit();
                }
            }
        }

        private static void createDatabase()
        {
            SQLiteConnection.CreateFile(DefaultDbName);

            using (var connection = new SQLiteConnection("Data Source=" + DefaultDbName + ";Version=3;"))
            {
                connection.Open();
                
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "CREATE TABLE " + RecipeTableName + " (id INTEGER PRIMARY KEY AUTOINCREMENT, title TEXT, category TEXT, description TEXT, portions INT)";
                    command.ExecuteNonQuery();

                    command.CommandText = "CREATE TABLE " + IngredientTableName + " (id INTEGER PRIMARY KEY AUTOINCREMENT, name TEXT, quantity REAL, unit TEXT, recipeId INT)";                    
                    command.ExecuteNonQuery();
                }                
            }
        }

        public void Save(List<Recipe> recipes, string path)
        {
            Save(recipes);
        }
    }
}

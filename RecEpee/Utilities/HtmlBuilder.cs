using RecEpee.Models;
using System.Collections.Generic;
using System.IO;
using System.Web.UI;
using System.Windows;
using System.Linq;

namespace RecEpee.Utilities
{
    class HtmlBuilder
    {
        // TODO: link alle varie categorie
        public static void RenderHtml(List<Recipe> recipes, TextWriter textWriter)
        {
            recipes = recipes.OrderBy((r) => r.Category).ThenBy((r) => r.Title).ToList();

            using (HtmlTextWriter writer = new HtmlTextWriter(textWriter))
            {                
                writer.RenderBeginTag(HtmlTextWriterTag.Html);

                RenderHead(writer);

                writer.AddAttribute(HtmlTextWriterAttribute.Class, "container");
                writer.RenderBeginTag(HtmlTextWriterTag.Body);

                writer.AddAttribute(HtmlTextWriterAttribute.Class, "page-header");
                writer.RenderBeginTag(HtmlTextWriterTag.Div);

                writer.RenderBeginTag(HtmlTextWriterTag.H1);
                writer.Write("Recipes list");
                writer.RenderEndTag();

                writer.RenderEndTag();

                RenderRecipes(recipes, writer);

                writer.RenderEndTag();

                writer.RenderEndTag();
            }
        }

        public static void RenderHtml(Recipe recipe, TextWriter textWriter)
        {
            using (HtmlTextWriter writer = new HtmlTextWriter(textWriter))
            {
                writer.RenderBeginTag(HtmlTextWriterTag.Html);

                RenderHead(writer);

                writer.AddAttribute(HtmlTextWriterAttribute.Class, "container");
                writer.RenderBeginTag(HtmlTextWriterTag.Body);

                writer.AddAttribute(HtmlTextWriterAttribute.Class, "page-header");
                writer.RenderBeginTag(HtmlTextWriterTag.Div);

                writer.RenderBeginTag(HtmlTextWriterTag.H1);
                writer.Write(recipe.Title);
                writer.RenderEndTag();

                writer.RenderEndTag();

                RenderRecipe(recipe, writer);

                writer.RenderEndTag();

                writer.RenderEndTag();
            }
        }

        private static void RenderHead(HtmlTextWriter writer)
        {
            writer.RenderBeginTag(HtmlTextWriterTag.Head);

            writer.AddAttribute(HtmlTextWriterAttribute.Type, "text/css");
            writer.RenderBeginTag(HtmlTextWriterTag.Style);
            writer.Write(GetCss());
            writer.RenderEndTag();

            writer.RenderBeginTag(HtmlTextWriterTag.Title);
            writer.Write("Recipes list");
            writer.RenderEndTag();

            writer.RenderEndTag();
        }

        private static string GetCss()
        {
            StreamReader reader = new StreamReader(Application.GetResourceStream(new System.Uri(@"pack://application:,,,/bootstrap.min.css")).Stream);

            return reader.ReadToEnd();
        }

        private static void RenderRecipes(List<Recipe> dataList, HtmlTextWriter writer)
        {
            string category = null;

            foreach (var recipe in dataList)
            {
                if (recipe.Category != category)
                {
                    category = recipe.Category;

                    RenderCategory(category, writer);
                }

                RenderRecipe(recipe, writer);
            }
        }

        private static void RenderCategory(string category, HtmlTextWriter writer)
        {
            writer.RenderBeginTag(HtmlTextWriterTag.H2);
            writer.Write(category);
            writer.RenderEndTag();
        }

        private static void RenderRecipe(Recipe recipe, HtmlTextWriter writer)
        {
            writer.AddAttribute(HtmlTextWriterAttribute.Class, "panel panel-default");
            writer.RenderBeginTag(HtmlTextWriterTag.Div);

            writer.AddAttribute(HtmlTextWriterAttribute.Class, "panel-heading");
            writer.RenderBeginTag(HtmlTextWriterTag.Div);

            writer.AddAttribute(HtmlTextWriterAttribute.Class, "panel-title");
            writer.RenderBeginTag(HtmlTextWriterTag.H3);
            writer.RenderBeginTag(HtmlTextWriterTag.Strong);
            writer.Write(recipe.Title);

//             writer.RenderBeginTag(HtmlTextWriterTag.Small);
//             writer.Write(recipe.Category);
//             writer.RenderEndTag();

            writer.RenderEndTag();
            writer.RenderEndTag();

            writer.RenderEndTag();

            writer.AddAttribute(HtmlTextWriterAttribute.Class, "panel-body");
            writer.RenderBeginTag(HtmlTextWriterTag.Div);

            writer.RenderBeginTag(HtmlTextWriterTag.H4);
            writer.Write("Ingredients for " + recipe.Portions + " people:");
            writer.RenderEndTag();

            RenderIngredients(recipe.Ingredients, writer);

            writer.RenderBeginTag(HtmlTextWriterTag.H4);
            writer.Write("Description:");
            writer.RenderEndTag();

            writer.RenderBeginTag(HtmlTextWriterTag.P);
            writer.Write(recipe.Description);
            writer.RenderEndTag();

            writer.RenderEndTag();

            writer.RenderEndTag();
        }

        private static void RenderIngredients(List<Ingredient> ingredients, HtmlTextWriter writer)
        {
            writer.RenderBeginTag(HtmlTextWriterTag.Ul);

            foreach (var ingredient in ingredients)
            {
                RenderIngredient(ingredient, writer);
            }

            writer.RenderEndTag();
        }

        private static void RenderIngredient(Ingredient ingredient, HtmlTextWriter writer)
        {
            writer.RenderBeginTag(HtmlTextWriterTag.Li);
            writer.Write(ingredient.Name);
            writer.RenderEndTag();
        }
    }
}

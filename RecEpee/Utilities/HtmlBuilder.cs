using RecEpee.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.UI;
using System.Windows;

namespace RecEpee.Utilities
{
    class HtmlBuilder
    {
        public static void RenderHtml(List<Recipe> recipes, TextWriter textWriter)
        {
            recipes = recipes.OrderBy((r) => r.Category).ThenBy((r) => r.Title).ToList();

            using (HtmlTextWriter writer = new HtmlTextWriter(textWriter))
            {                
                writer.RenderBeginTag(HtmlTextWriterTag.Html);

                RenderHead(writer);

                writer.AddAttribute(HtmlTextWriterAttribute.Class, "container");
                writer.RenderBeginTag(HtmlTextWriterTag.Body);

                RenderHeader(writer);

                writer.AddAttribute(HtmlTextWriterAttribute.Class, "row");
                writer.RenderBeginTag(HtmlTextWriterTag.Div);

                writer.AddAttribute(HtmlTextWriterAttribute.Class, "col-md-10");
                writer.RenderBeginTag(HtmlTextWriterTag.Div);

                RenderRecipes(recipes, writer);

                writer.RenderEndTag(); // div

                writer.AddAttribute(HtmlTextWriterAttribute.Class, "col-md-2");
                writer.RenderBeginTag(HtmlTextWriterTag.Div);

                RenderNavigation(recipes, writer);

                writer.RenderEndTag(); // div

                writer.RenderEndTag(); // row

                writer.RenderEndTag(); // container

                writer.RenderEndTag(); // html
            }
        }

        private static void RenderHeader(HtmlTextWriter writer)
        {
            writer.AddAttribute(HtmlTextWriterAttribute.Class, "row");
            writer.RenderBeginTag(HtmlTextWriterTag.Div);

            writer.AddAttribute(HtmlTextWriterAttribute.Class, "page-header col-md-12");
            writer.RenderBeginTag(HtmlTextWriterTag.Div);

            writer.RenderBeginTag(HtmlTextWriterTag.H1);
            writer.Write("Recipes list");
            writer.RenderEndTag();

            writer.RenderEndTag(); // div

            writer.RenderEndTag(); // row
        }

        private static void RenderNavigation(List<Recipe> recipes, HtmlTextWriter writer)
        {
            writer.AddAttribute(HtmlTextWriterAttribute.Class, "List-group");
            writer.RenderBeginTag(HtmlTextWriterTag.Ul);

            var categories = recipes.GroupBy((r) => r.Category).Select((c) => new {name = c.Key, count = c.Count()});

            foreach (var category in categories)
            {
                RenderNavigationEntry(writer, category.name, category.count);
            }           

            writer.RenderEndTag(); // ul
        }

        private static void RenderNavigationEntry(HtmlTextWriter writer, string text, int count)
        {
            writer.AddAttribute(HtmlTextWriterAttribute.Class, "List-group-item");
            writer.RenderBeginTag(HtmlTextWriterTag.Li);

            RenderBadge(writer, count);

            RenderInternalLink(writer, text);

            writer.RenderEndTag(); // li
        }

        private static void RenderInternalLink(HtmlTextWriter writer, string text)
        {
            writer.AddAttribute(HtmlTextWriterAttribute.Href, "#" + text);
            writer.RenderBeginTag(HtmlTextWriterTag.A);
            writer.Write(text);
            writer.RenderEndTag();
        }

        private static void RenderBadge(HtmlTextWriter writer, int count)
        {
            writer.AddAttribute(HtmlTextWriterAttribute.Class, "badge");
            writer.RenderBeginTag(HtmlTextWriterTag.Span);
            writer.Write(count);
            writer.RenderEndTag();
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

            RenderCss(writer);

            RenderTitle(writer);

            writer.RenderEndTag();
        }

        private static void RenderTitle(HtmlTextWriter writer)
        {
            writer.RenderBeginTag(HtmlTextWriterTag.Title);
            writer.Write("Recipes list");
            writer.RenderEndTag();
        }

        private static void RenderCss(HtmlTextWriter writer)
        {
            writer.AddAttribute(HtmlTextWriterAttribute.Type, "text/css");
            writer.RenderBeginTag(HtmlTextWriterTag.Style);
            writer.Write(GetCss());
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
            writer.AddAttribute(HtmlTextWriterAttribute.Id, category);
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

using RecEpee.Models;
using System.Collections.Generic;
using System.IO;
using System.Web.UI;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace RecEpee.DataAccess
{
    class XmlRecipeRepository : IDataRepository<Recipe>
    {
        public const string DefaultSavePath = @"C:\recipes.xml";
        public const string DefaultExportPath = @"C:\recipes.html";
        private const string TempExportPath = @"C:\temprecipes.html";

        public List<Recipe> Load()
        {
            return Load(DefaultSavePath);
        }

        public List<Recipe> Load(string path)
        {
            XmlSerializer deserializer = new XmlSerializer(typeof(List<Recipe>));

            List<Recipe> recipes = new List<Recipe>();

            using (TextReader reader = new StreamReader(path))
            {
                recipes = (List<Recipe>)deserializer.Deserialize(reader);
            }

            return recipes;
        }

        public void Save(List<Recipe> recipes)
        {
            Save(recipes, DefaultSavePath);
        }

        public void Save(List<Recipe> recipes, string path)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<Recipe>));

            using (TextWriter writer = new StreamWriter(path))
            {
                serializer.Serialize(writer, recipes);
            }
        }

        public void Export(List<Recipe> dataList)
        {
            Export(dataList, DefaultExportPath);
        }

        public void Export(List<Recipe> dataList, string path)
        {            
            using (TextWriter textWriter = new StreamWriter(path))
            {
                using (HtmlTextWriter writer = new HtmlTextWriter(textWriter))
                {
                    writer.RenderBeginTag(HtmlTextWriterTag.Html);

                    writer.RenderBeginTag(HtmlTextWriterTag.Body);

                    writer.RenderBeginTag(HtmlTextWriterTag.H1);
                    writer.Write("Recipes list");
                    writer.RenderEndTag();

                    foreach (var recipe in dataList)
                    {
                        const string recipeClass = "recipe";

                        writer.AddAttribute(HtmlTextWriterAttribute.Class, recipeClass);
                        writer.RenderBeginTag(HtmlTextWriterTag.Div);

                        writer.RenderBeginTag(HtmlTextWriterTag.H2);
                        writer.Write(recipe.Title);
                        writer.RenderEndTag();

                        writer.RenderBeginTag(HtmlTextWriterTag.H3);
                        writer.Write("Ingredients for " + recipe.Portions + " people:");
                        writer.RenderEndTag();

                        writer.RenderBeginTag(HtmlTextWriterTag.Ul);

                        foreach (var ingredient in recipe.Ingredients)
                        {
                            writer.RenderBeginTag(HtmlTextWriterTag.Li);
                            writer.Write(ingredient.Name);
                            writer.RenderEndTag();
                        }

                        writer.RenderEndTag();

                        writer.RenderBeginTag(HtmlTextWriterTag.H3);
                        writer.Write("Description:");
                        writer.RenderEndTag();

                        writer.RenderBeginTag(HtmlTextWriterTag.P);
                        writer.Write(recipe.Description);
                        writer.RenderEndTag();

                        writer.RenderEndTag();
                    }

                    writer.RenderEndTag();

                    writer.RenderEndTag();
                }
            }      
        }

        public void Print(List<Recipe> dataList)
        {
            Export(dataList, TempExportPath);

            WebBrowser webBrowser = new WebBrowser();
            webBrowser.DocumentCompleted += (a, b) => webBrowser.ShowPrintDialog();
            webBrowser.Url = new System.Uri(TempExportPath);  
        }
    }
}

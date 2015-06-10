using RecEpee.Models;
using System.Collections.Generic;
using System.IO;
using System.Web.UI;
using System.Windows.Forms;
using System.Xml.Serialization;
using RecEpee.Utilities;

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
                HtmlBuilder.RenderHtml(dataList, textWriter);
            }      
        }

        public void Print(List<Recipe> dataList)
        {
            Export(dataList, TempExportPath);

            WebBrowser webBrowser = new WebBrowser();
            webBrowser.DocumentCompleted += (a, b) => webBrowser.ShowPrintPreviewDialog();
            webBrowser.Url = new System.Uri(TempExportPath);  
        }
    }
}

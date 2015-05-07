using RecEpee.Models;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace RecEpee.DataAccess
{
    class XmlRecipeRepository : IDataRepository<Recipe>
    {
        public List<Recipe> Load()
        {
            return Load(@"C:\recipes.xml");
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
            Save(recipes, @"C:\recipes.xml");
        }

        public void Save(List<Recipe> recipes, string path)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<Recipe>));

            using (TextWriter writer = new StreamWriter(path))
            {
                serializer.Serialize(writer, recipes);
            }
        }
    }
}

using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace Ricettario.Models
{
    class Persistence
    {
        public static List<Recipe> Load()
        {
            List<Recipe> recipes;

            XmlSerializer deserializer = new XmlSerializer(typeof(List<Recipe>));
            using (TextReader reader = new StreamReader(@"C:\recipes.xml"))
            {
                recipes = (List<Recipe>)deserializer.Deserialize(reader);
            }

            return recipes;
        }

        public static void Save(List<Recipe> recipes)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<Recipe>));
            using (TextWriter writer = new StreamWriter(@"C:\recipes.xml"))
            {
                serializer.Serialize(writer, recipes);
            }
        }
    }
}

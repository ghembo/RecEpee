using RecEpee.Models;
using RecEpee.Utilities;
using System.Collections.Generic;
using System.IO;

namespace RecEpee.DataAccess
{
    class HtmlRecipeExporter : IExporter<Recipe>
    {
        public void Export(List<Recipe> dataList, string path)
        {
            using (TextWriter textWriter = new StreamWriter(path))
            {
                HtmlBuilder.RenderHtml(dataList, textWriter);
            }      
        }
    }
}

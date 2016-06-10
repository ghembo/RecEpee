using System.Collections.Generic;

namespace RecEpee.DataAccess
{
    interface IExporter<T> 
    {
        void Export(List<T> dataList, string path);
    }    
}

using System.Collections.Generic;

namespace RecEpee.DataAccess
{
    interface IDataRepository<T> 
    {
        List<T> Load();
        List<T> Load(string path);
        void Save(List<T> dataList);
        void Save(List<T> dataList, string path);
    }    
}

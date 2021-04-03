using System.Collections.Generic;

namespace DataIngestion.TestAssignment.DataStores
{
    public interface IDataProvider<TKey, TValue>
    {
        TValue GetById(TKey id);

        IEnumerable<TValue> GetAll();
    }
}
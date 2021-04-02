using DataIngestion.TestAssignment.InputModels;
using System.Collections.Generic;

namespace DataIngestion.TestAssignment.DataStores
{
    public interface IDataStore<TValue>
    {
        void Add(TValue item);

        void AddMany(IEnumerable<TValue> items);
    }
}
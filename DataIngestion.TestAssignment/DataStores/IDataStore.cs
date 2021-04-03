using DataIngestion.TestAssignment.InputModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataIngestion.TestAssignment.DataStores
{
    public interface IDataStore<TValue>
    {
        void Add(TValue item);

        Task AddManyAsync(IAsyncEnumerable<TValue> items);
    }
}
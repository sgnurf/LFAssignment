using DataIngestion.TestAssignment.InputModels;

namespace DataIngestion.TestAssignment.DataStores
{
    public class CollectionStore : DataStore<long, Collection>, ICollectionProvider
    {
        public CollectionStore()
            : base(c => c.Id)
        {
        }
    }
}
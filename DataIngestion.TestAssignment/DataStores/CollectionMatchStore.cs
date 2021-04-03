using DataIngestion.TestAssignment.InputModels;

namespace DataIngestion.TestAssignment.DataStores
{
    public class CollectionMatchStore : DataStore<long, CollectionMatch>, ICollectionMatchProvider
    {
        public CollectionMatchStore()
            : base(c => c.CollectionId)
        {
        }
    }
}
using DataIngestion.TestAssignment.InputModels;

namespace DataIngestion.TestAssignment.DataStores
{
    public interface ICollectionMatchProvider : IDataProvider<long, CollectionMatch>
    {
    }
}
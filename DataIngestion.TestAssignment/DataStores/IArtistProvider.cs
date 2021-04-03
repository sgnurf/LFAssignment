using DataIngestion.TestAssignment.InputModels;

namespace DataIngestion.TestAssignment.DataStores
{
    public interface IArtistProvider : IDataProvider<long, Artist> { }
}
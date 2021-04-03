using DataIngestion.TestAssignment.InputModels;

namespace DataIngestion.TestAssignment.DataStores
{
    public class ArtistStore : DataStore<long, Artist>, IArtistProvider
    {
        public ArtistStore()
            : base(a => a.Id)
        {
        }
    }
}
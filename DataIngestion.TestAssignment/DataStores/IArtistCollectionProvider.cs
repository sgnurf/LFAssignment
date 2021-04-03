using DataIngestion.TestAssignment.InputModels;
using System.Collections.Generic;

namespace DataIngestion.TestAssignment.DataStores
{
    public interface IArtistCollectionProvider : IDataProvider<(long, long, int), ArtistCollection>
    {
        IEnumerable<ArtistCollection> GetForCollection(long collectionId);
    }
}
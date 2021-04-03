using DataIngestion.TestAssignment.InputModels;
using System.Collections.Generic;
using System.Linq;

namespace DataIngestion.TestAssignment.DataStores
{
    internal class ArtistCollectionStore : DataStore<(long, long, int?), ArtistCollection>, IArtistCollectionProvider
    {
        private readonly Dictionary<long, IList<ArtistCollection>> artistCollectionsByCollection = new Dictionary<long, IList<ArtistCollection>>();

        public ArtistCollectionStore()
            :base((ac)=>(ac.ArtistId, ac.CollectionId, ac.RoleId))
        {
        }

        public override void Add(ArtistCollection item)
        {
            base.Add(item);

            if (!artistCollectionsByCollection.ContainsKey(item.CollectionId))
            {
                artistCollectionsByCollection[item.CollectionId] = new List<ArtistCollection> { item };
            }
            else
            {
                artistCollectionsByCollection[item.CollectionId].Add(item);
            }
        }

        public IEnumerable<ArtistCollection> GetForCollection(long collectionId)
        {
            return artistCollectionsByCollection.TryGetValue(collectionId, out var artistCollections)
                ? artistCollections
                : Enumerable.Empty<ArtistCollection>();
        }
    }
}
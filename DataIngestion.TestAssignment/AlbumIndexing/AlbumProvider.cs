using DataIngestion.TestAssignment.DataStores;
using DataIngestion.TestAssignment.InputModels;
using DataIngestion.TestAssignment.TargetModels;
using System.Collections.Generic;
using System.Linq;

namespace DataIngestion.TestAssignment.AlbumIndexing
{
    public class AlbumProvider : IAlbumProvider
    {
        private readonly ICollectionProvider collectionProvider;
        private readonly IAlbumFactory albumFactory;

        public AlbumProvider(ICollectionProvider collectionProvider, IAlbumFactory albumFactory)
        {
            this.collectionProvider = collectionProvider;
            this.albumFactory = albumFactory;
        }

        public IEnumerable<Album> GetAlbums()
        {
            IEnumerable<Collection> collections = collectionProvider.GetAll();
            
            if(collections == null)
            {
                yield break;
            }

            foreach (Collection collection in collectionProvider.GetAll())
            {
                yield return albumFactory.CreateAlbum(collection);
            }
        }
    }
}
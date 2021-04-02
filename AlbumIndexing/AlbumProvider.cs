using DataIngestion.TestAssignment.DataStores;
using DataIngestion.TestAssignment.InputModels;
using DataIngestion.TestAssignment.TargetModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataIngestion.TestAssignment.AlbumIndexing
{
    public class AlbumProvider : IAlbumProvider
    {
        private readonly IDataProvider<long,Artist> artistProvider;
        private readonly IArtistCollectionProvider artistCollectionProvider;
        private readonly IDataProvider<long, Collection> collectionProvider;
        private readonly IDataProvider<long, CollectionMatch> collectionMatchProvider;

        public AlbumProvider(
            IDataProvider<long, Artist> artistProvider,
            IArtistCollectionProvider artistCollectionProvider,
            IDataProvider<long, Collection> collectionProvider,
            IDataProvider<long, CollectionMatch> collectionMatchProvider)
        {
            this.artistProvider = artistProvider;
            this.artistCollectionProvider = artistCollectionProvider;
            this.collectionProvider = collectionProvider;
            this.collectionMatchProvider = collectionMatchProvider;
        }

        public IEnumerable<Album> GetAlbums()
        {
            foreach(Collection collection in collectionProvider.GetAll())
            {
                try
                {
                    yield return CreateAlbumRecord(collection);
                }
                finally
                {

                }
            }
        }

        private Album CreateAlbumRecord(Collection collection)
        {
            IEnumerable<ArtistCollection> artistCollections = artistCollectionProvider.GetForCollection(collection.Id);

            List<AlbumArtist> artists = new List<AlbumArtist>();
            foreach (ArtistCollection artistCollection in artistCollections)
            {
                Artist artist = artistProvider.GetById(artistCollection.ArtistId);
                if (artist != null)
                {
                    artists.Add(new AlbumArtist(artist.Id, artist.Name));
                }
            }

            CollectionMatch collectionMatch = collectionMatchProvider.GetById(collection.Id);

            return new Album(
                collection.Id,
                collection.Name,
                collection.ViewUrl,
                collectionMatch?.Upc,
                collection.OriginalReleaseDate,
                collection.IsCompilation,
                collection.LabelStudio,
                collection.ArtworkUrl,
                artists.ToArray());
        }
    }
}
using DataIngestion.TestAssignment.DataStores;
using DataIngestion.TestAssignment.InputModels;
using DataIngestion.TestAssignment.TargetModels;
using System.Collections.Generic;

namespace DataIngestion.TestAssignment.AlbumIndexing
{
    public class AlbumFactory : IAlbumFactory
    {
        private readonly IArtistProvider artistProvider;
        private readonly IArtistCollectionProvider artistCollectionProvider;
        private readonly ICollectionMatchProvider collectionMatchProvider;

        public AlbumFactory(
            IArtistProvider artistProvider,
            IArtistCollectionProvider artistCollectionProvider,
            ICollectionMatchProvider collectionMatchProvider)
        {
            this.artistProvider = artistProvider;
            this.artistCollectionProvider = artistCollectionProvider;
            this.collectionMatchProvider = collectionMatchProvider;
        }

        public Album CreateAlbum(Collection collection)
        {
            AlbumArtist[] artists = CreateAlbumArtists(collection);

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
                artists);
        }

        private AlbumArtist[] CreateAlbumArtists(Collection collection)
        {
            IEnumerable<ArtistCollection> artistCollections = artistCollectionProvider.GetForCollection(collection.Id);

            if (artistCollections == null)
            {
                return new AlbumArtist[0];
            }

            List<AlbumArtist> artists = new List<AlbumArtist>();
            foreach (ArtistCollection artistCollection in artistCollections)
            {
                Artist artist = artistProvider.GetById(artistCollection.ArtistId);
                if (artist != null)
                {
                    artists.Add(new AlbumArtist(artist.Id, artist.Name));
                }
            }

            return artists.ToArray();
        }
    }
}
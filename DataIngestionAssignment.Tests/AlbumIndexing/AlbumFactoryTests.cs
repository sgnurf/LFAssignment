using DataIngestion.TestAssignment.AlbumIndexing;
using DataIngestion.TestAssignment.DataStores;
using DataIngestion.TestAssignment.InputModels;
using DataIngestion.TestAssignment.TargetModels;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace DataIngestionAssignment.Tests.AlbumIndexing
{
    public class AlbumFactoryTests
    {
        [Fact]
        public void CreateAlbum_CollectionFieldAreSet()
        {
            //Arrange
            AlbumFactory albumFactory = GetAlbumFactory();
            Collection collection = GetMockCollection();

            //Act
            Album album = albumFactory.CreateAlbum(collection);

            //Assert
            Assert.Equal(collection.Id, album.Id);
            Assert.Equal(collection.ArtworkUrl, album.ImageUrl);
            Assert.Equal(collection.IsCompilation, album.IsCompilation);
            Assert.Equal(collection.LabelStudio, album.Label);
            Assert.Equal(collection.Name, album.Name);
            Assert.Equal(collection.OriginalReleaseDate, album.releaseDate);
            Assert.Equal(collection.ViewUrl, album.url);
        }

        [Fact]
        public void CreateAlbum_NoCollectionMatch_UpcIsNull()
        {
            //Arrange
            Collection collection = GetMockCollection();
            AlbumFactory albumFactory = GetAlbumFactory(
                CollectionId: collection.Id,
                collectionMatch: null);

            //Act
            Album album = albumFactory.CreateAlbum(collection);

            //Assert
            Assert.Null(album.upc);
        }

        [Fact]
        public void CreateAlbum_WithCollectionMatch_UpcIsSet()
        {
            //Arrange
            Collection collection = GetMockCollection();
            CollectionMatch collectionMatch = new CollectionMatch(collection.Id, "upc", null, null);
            AlbumFactory albumFactory = GetAlbumFactory(
                CollectionId: collection.Id,
                collectionMatch: collectionMatch);

            //Act
            Album album = albumFactory.CreateAlbum(collection);

            //Assert
            Assert.Equal(collectionMatch.Upc, album.upc);
        }

        [Fact]
        public void CreateAlbum_NoArtistCollection_ArtistFieldIsEmpty()
        {
            //Arrange
            Collection collection = GetMockCollection();
            AlbumFactory albumFactory = GetAlbumFactory(
                CollectionId: collection.Id,
                artistCollections: null);

            //Act
            Album album = albumFactory.CreateAlbum(collection);

            //Assert
            Assert.Empty(album.Artists);
        }

        [Fact]
        public void CreateAlbum_FoundArtistCollection_ArtistFieldIsSet()
        {
            //Arrange
            Collection collection = GetMockCollection();
            long[] artistIds = { 1, 2 };
            IEnumerable<Artist> artists = artistIds.Select(id => GetMockArtist(id));
            IEnumerable<ArtistCollection> artistCollections = artistIds.Select(id => GetMockArtistCollection(collection.Id, id));

            AlbumFactory albumFactory = GetAlbumFactory(
                CollectionId: collection.Id,
                artistCollections: artistCollections,
                artists: artists);

            //Act
            Album album = albumFactory.CreateAlbum(collection);

            //Assert
            Assert.Equal(artistIds.Length, album.Artists.Length);
            foreach(Artist artist in artists)
                Assert.Contains(album.Artists, a => a.Id == artist.Id && a.Name == artist.Name) ;
        }

        [Fact]
        public void CreateAlbum_ArtistMissingInStore_ArtistNotInArtistList()
        {
            //Arrange
            Collection collection = GetMockCollection();
            long[] artistIdsInArtistCollectionStore = { 1, 2 };
            long[] artistIdsInArtistStore = { 1 };

            IEnumerable<Artist> artists = artistIdsInArtistStore.Select(id => GetMockArtist(id));
            IEnumerable<ArtistCollection> artistCollections = artistIdsInArtistCollectionStore.Select(id => GetMockArtistCollection(collection.Id, id));

            AlbumFactory albumFactory = GetAlbumFactory(
                CollectionId: collection.Id,
                artistCollections: artistCollections,
                artists: artists);

            //Act
            Album album = albumFactory.CreateAlbum(collection);

            //Assert
            Assert.Equal(artistIdsInArtistStore.Length, album.Artists.Length);
            foreach (Artist artist in artists)
                Assert.Contains(album.Artists, a => a.Id == artist.Id && a.Name == artist.Name);
        }

        private static AlbumFactory GetAlbumFactory(
            long CollectionId = 0,
            CollectionMatch collectionMatch = null,
            IEnumerable<ArtistCollection> artistCollections = null,
            IEnumerable<Artist> artists = null)

        {
            Mock<IArtistProvider> artistProvider = new Mock<IArtistProvider>();
            artistProvider.Setup(p => p.GetById(It.IsAny<long>())).Returns((long id) => artists?.FirstOrDefault(a => a.Id == id));

            Mock<IArtistCollectionProvider> artistCollectionProvider = new Mock<IArtistCollectionProvider>();
            artistCollectionProvider.Setup(p => p.GetForCollection(CollectionId)).Returns(artistCollections);

            Mock<ICollectionMatchProvider> collectionMatchProvider = new Mock<ICollectionMatchProvider>();
            collectionMatchProvider.Setup(p => p.GetById(CollectionId)).Returns(collectionMatch);

            AlbumFactory albumFactory = new AlbumFactory(artistProvider.Object, artistCollectionProvider.Object, collectionMatchProvider.Object);
            return albumFactory;
        }

        private static Collection GetMockCollection() => new Collection(1, "Name", "Title", "SearchTerm", 1, "ArtistName", "ViewUrl", "Artworkurl", new DateTime(2021, 4, 3), new DateTime(2021, 4, 4), "Label", "ContentProvider", "Copyright", "PLine", 1, true, 1);

        private static Artist GetMockArtist(long artistId) => new Artist(artistId, $"Artist Name {artistId}", false, null, 0);

        private static ArtistCollection GetMockArtistCollection(long collectionId, long artistId) => new ArtistCollection(artistId, collectionId, false, 0);
    }
}
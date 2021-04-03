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
    public class AlbumProviderTests
    {
        [Fact]
        public void GetAlbums_StoreHasNoCollection_ReturnsEmptyCollection()
        {
            Mock<ICollectionProvider> collectionStore = new Mock<ICollectionProvider>();
            collectionStore
                .Setup(s => s.GetAll())
                .Returns<IEnumerable<Collection>>(null);
            AlbumProvider provider = new AlbumProvider(collectionStore.Object, null);

            IEnumerable<Album> albums = provider.GetAlbums();

            Assert.Empty(albums);
        }

        [Fact]
        public void GetAlbums_StoreHasCollections_ReturnsAlbumForEachCollection()
        {
            //Arrange
            long[] collectionIds = { 1, 2, 3 };
            IEnumerable<Collection> collections = CreateMockCollectionFromId(collectionIds);

            Mock<ICollectionProvider> collectionStore = new Mock<ICollectionProvider>();
            collectionStore.Setup(s => s.GetAll()).Returns(collections);

            Mock<IAlbumFactory> albumFactory = new Mock<IAlbumFactory>();
            albumFactory
                .Setup(f => f.CreateAlbum(It.IsAny<Collection>()))
                .Returns((Func<Collection, Album>)CreateMockAlbumFromCollection);

            AlbumProvider provider = new AlbumProvider(collectionStore.Object, albumFactory.Object);

            //Act
            IEnumerable<Album> albums = provider.GetAlbums();

            //Assert
            Assert.Equal(collectionIds.Length, albums.Count());
            foreach (long id in collectionIds)
            {
                Assert.Contains(albums, a => a.Id == id);
            }
        }

        private IEnumerable<Collection> CreateMockCollectionFromId(long[] ids)
        {
            foreach (long id in ids)
            {
                yield return new Collection(id, null, null, null, null, null, null, null, DateTime.Now, DateTime.Now, null, null, null, null, null, false, null);
            }
        }

        private Album CreateMockAlbumFromCollection(Collection collection)
        {
            return new Album(collection.Id, null, null, null, DateTime.Now, false, null, null, null);
        }
    }
}
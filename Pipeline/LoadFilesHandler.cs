using DataIngestion.TestAssignment.DataStores;
using DataIngestion.TestAssignment.FileParsing;
using DataIngestion.TestAssignment.InputModels;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DataIngestion.TestAssignment.Pipeline
{
    public class LoadFilesHandler : IRequestHandler<LoadFilesRequest>
    {
        private readonly IFileParser fileParser;
        private readonly IDataStore<Artist> artistStore;
        private readonly IDataStore<ArtistCollection> artistCollectionStore;
        private readonly IDataStore<Collection> collectionStore;
        private readonly IDataStore<CollectionMatch> collectionMatchStore;

        public LoadFilesHandler(
            IFileParser fileParser,
            IDataStore<Artist> artistStore,
            IDataStore<ArtistCollection> artistCollectionStore,
            IDataStore<Collection> collectionStore,
            IDataStore<CollectionMatch> collectionMatchStore
            )
        {
            this.fileParser = fileParser;
            this.artistStore = artistStore;
            this.artistCollectionStore = artistCollectionStore;
            this.collectionStore = collectionStore;
            this.collectionMatchStore = collectionMatchStore;
        }

        public Task<Unit> Handle(LoadFilesRequest request, CancellationToken cancellationToken)
        {

            IEnumerable<Artist> artists = fileParser.Parse<Artist>(request.files.Artist);
            artistStore.AddMany(artists);

            IEnumerable<ArtistCollection> artistCollections = fileParser.Parse<ArtistCollection>(request.files.ArtistCollection);
            artistCollectionStore.AddMany(artistCollections);

            IEnumerable<CollectionMatch> collectionMatches = fileParser.Parse<CollectionMatch>(request.files.CollectionMatch);
            collectionMatchStore.AddMany(collectionMatches);

            IEnumerable<Collection> collections =  fileParser.Parse<Collection>(request.files.Collection);
            collectionStore.AddMany(collections);

            return Unit.Task;
        }
    }
}
using DataIngestion.TestAssignment.AlbumIndexing;
using DataIngestion.TestAssignment.TargetModels;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DataIngestion.TestAssignment.Pipeline
{
    internal class IndexAlbumRequesthandler : IRequestHandler<IndexAlbumRequest>
    {
        private readonly IAlbumProvider albumProvider;
        private readonly IAlbumIndexer albumIndexer;

        public IndexAlbumRequesthandler(IAlbumProvider albumProvider, IAlbumIndexer albumIndexer)
        {
            this.albumProvider = albumProvider;
            this.albumIndexer = albumIndexer;
        }

        public Task<Unit> Handle(IndexAlbumRequest request, CancellationToken cancellationToken)
        {
            IEnumerable<Album> albums = albumProvider.GetAlbums();
            albumIndexer.Index(albums);

            return Unit.Task;
        }
    }
}
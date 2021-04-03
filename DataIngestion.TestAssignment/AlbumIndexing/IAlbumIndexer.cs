using DataIngestion.TestAssignment.TargetModels;
using System.Collections.Generic;
using System.Threading;

namespace DataIngestion.TestAssignment.AlbumIndexing
{
    public interface IAlbumIndexer
    {
        void Index(IEnumerable<Album> albums, CancellationToken cancellationToken);
    }
}
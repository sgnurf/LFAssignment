using DataIngestion.TestAssignment.TargetModels;
using System.Collections.Generic;

namespace DataIngestion.TestAssignment.AlbumIndexing
{
    public interface IAlbumIndexer
    {
        void Index(IEnumerable<Album> albums);
    }
}
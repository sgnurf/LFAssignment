using DataIngestion.TestAssignment.TargetModels;
using System.Collections.Generic;

namespace DataIngestion.TestAssignment.AlbumIndexing
{
    public interface IAlbumProvider
    {
        IEnumerable<Album> GetAlbums();
    }
}
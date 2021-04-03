using DataIngestion.TestAssignment.InputModels;
using DataIngestion.TestAssignment.TargetModels;

namespace DataIngestion.TestAssignment.AlbumIndexing
{
    public interface IAlbumFactory
    {
        Album CreateAlbum(Collection collection);
    }
}
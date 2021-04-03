using System.Threading.Tasks;

namespace DataIngestion.TestAssignment.FileExtraction
{
    public interface IFileExtractor
    {
        Task Extract(string archiveFile, string destinationFolder);
    }
}
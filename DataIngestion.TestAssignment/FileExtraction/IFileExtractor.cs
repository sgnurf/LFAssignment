using System.Threading;
using System.Threading.Tasks;

namespace DataIngestion.TestAssignment.FileExtraction
{
    public interface IFileExtractor
    {
        Task Extract(string archiveFile, string destinationFolder, CancellationToken cancellationToken);
    }
}
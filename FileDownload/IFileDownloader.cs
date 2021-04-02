using System.Threading.Tasks;

namespace DataIngestion.TestAssignment.FileDownload
{
    public interface IFileDownloader
    {
        Task DownloadAsync(string fileId, string destinationPath);
    }
}
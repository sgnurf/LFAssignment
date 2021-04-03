using Google.Apis.Download;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace DataIngestion.TestAssignment.FileDownload
{
    public interface IGoogleDriveService
    {
        Task<IDownloadProgress> GetFile(string fileId, Stream downloadStream, CancellationToken cancellationToken);
    }
}
using Google.Apis.Download;
using Storage.Net.Blobs;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace DataIngestion.TestAssignment.FileDownload
{
    public class GoogleDriveFileDownloader : IFileDownloader
    {
        private readonly IGoogleDriveService googleDriveService;
        private readonly IBlobStorage blobStorage;

        public GoogleDriveFileDownloader(IGoogleDriveService googleDriveService, IBlobStorage blobStorage)
        {
            this.googleDriveService = googleDriveService;
            this.blobStorage = blobStorage;
        }

        public async Task DownloadAsync(string fileId, CancellationToken cancellationToken)
        {
            IDownloadProgress downloadStatus;

            using (MemoryStream memoryStream = new MemoryStream())
            {
                downloadStatus = await googleDriveService.GetFile(fileId, memoryStream, cancellationToken);

                if (downloadStatus.Status != DownloadStatus.Completed)
                {
                    throw new Exception($"Failed to download file {fileId}", downloadStatus.Exception);
                }

                memoryStream.Position = 0;
                await blobStorage.WriteAsync(fileId, memoryStream, append: false, cancellationToken: cancellationToken);
            }
        }
    }
}
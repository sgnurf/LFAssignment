using Google.Apis.Download;
using Google.Apis.Drive.v3;
using Storage.Net.Blobs;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace DataIngestion.TestAssignment.FileDownload
{
    public class GoogleDriveFileDownloader : IFileDownloader
    {
        private readonly IGoogleServiceInitialiseProvider googleServiceInitialiseProvider;
        private readonly IBlobStorage blobStorage;

        public GoogleDriveFileDownloader(IGoogleServiceInitialiseProvider googleServiceInitialiseProvider, IBlobStorage blobStorage)
        {
            this.googleServiceInitialiseProvider = googleServiceInitialiseProvider;
            this.blobStorage = blobStorage;
        }

        public async Task DownloadAsync(string fileId, CancellationToken cancellationToken)
        {
            var initializer = googleServiceInitialiseProvider.GetInitialiser();
            DriveService driveService = new DriveService(initializer);

            IDownloadProgress downloadStatus;

            using (MemoryStream memoryStream = new MemoryStream())
            {
                downloadStatus = await driveService.Files.Get(fileId).DownloadAsync(memoryStream, cancellationToken);
                
                if (downloadStatus.Status != DownloadStatus.Completed)
                {
                    throw new Exception($"Failed to download file {fileId}", downloadStatus.Exception);
                }

                memoryStream.Position = 0;
                await blobStorage.WriteAsync(fileId, memoryStream);
            }
        }
    }
}
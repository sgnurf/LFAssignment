using Google.Apis.Download;
using Google.Apis.Drive.v3;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace DataIngestion.TestAssignment.FileDownload
{
    public class GoogleDriveFileDownloader : IFileDownloader
    {
        private readonly IGoogleServiceInitialiseProvider googleServiceInitialiseProvider;

        public GoogleDriveFileDownloader(IGoogleServiceInitialiseProvider googleServiceInitialiseProvider)
        {
            this.googleServiceInitialiseProvider = googleServiceInitialiseProvider;
        }

        public async Task DownloadAsync(string fileId, string destinationFile, CancellationToken cancellationToken)
        {
            var initializer = googleServiceInitialiseProvider.GetInitialiser();
            DriveService driveService = new DriveService(initializer);

            IDownloadProgress downloadStatus;

            using (FileStream fileStream = File.Create(destinationFile))
            {
                downloadStatus = await driveService.Files.Get(fileId).DownloadAsync(fileStream, cancellationToken);
            }

            if (downloadStatus.Status != DownloadStatus.Completed)
            {
                throw new Exception($"Failed to download file {fileId}", downloadStatus.Exception);
            }
        }
    }
}
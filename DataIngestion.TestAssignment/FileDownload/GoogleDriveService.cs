using DataIngestion.TestAssignment.Configuration;
using Google.Apis.Download;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace DataIngestion.TestAssignment.FileDownload
{
    internal class GoogleDriveService : IGoogleDriveService
    {
        private readonly GoogleApiConfiguration googleApiConfiguration;
        private Lazy<DriveService> driveService;

        public GoogleDriveService(IOptions<GoogleApiConfiguration> googleApiConfiguration)
        {
            this.googleApiConfiguration = googleApiConfiguration.Value;
            driveService = new Lazy<DriveService>(CreateDriveService);
        }

        public Task<IDownloadProgress> GetFile(string fileId, Stream downloadStream, CancellationToken cancellationToken)
        {
            return driveService.Value.Files.Get(fileId).DownloadAsync(downloadStream, cancellationToken);
        }

        private DriveService CreateDriveService()
        {
            var initializer = GetInitialiser();
            return new DriveService(initializer);
        }

        private BaseClientService.Initializer GetInitialiser()
        {
            return new BaseClientService.Initializer()
            {
                ApiKey = googleApiConfiguration.ApiKey,
                ApplicationName = googleApiConfiguration.ApplicationName
            };
        }
    }
}
using DataIngestion.TestAssignment.Configuration;
using DataIngestion.TestAssignment.FileDownload;
using MediatR;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace DataIngestion.TestAssignment.Pipeline
{
    internal class DownloadFilesHandler : IRequestHandler<DownloadFilesRequest, InputFiles>
    {
        private readonly IFileDownloader fileDownloader;

        public DownloadFilesHandler(IFileDownloader fileDownloader)
        {
            this.fileDownloader = fileDownloader;
        }

        public async Task<InputFiles> Handle(DownloadFilesRequest request, CancellationToken cancellationToken)
        {
            InputFiles filesToDownload = request.Files;

            //TODO: Inject instead of making the decision here
            InputFiles targetFiles = GetTargetFilePath();

            Task[] downloadTasks = new Task[] {
                fileDownloader.DownloadAsync(filesToDownload.Artist, targetFiles.Artist, cancellationToken),
                fileDownloader.DownloadAsync(filesToDownload.ArtistCollection, targetFiles.ArtistCollection, cancellationToken),
                fileDownloader.DownloadAsync(filesToDownload.Collection, targetFiles.Collection, cancellationToken),
                fileDownloader.DownloadAsync(filesToDownload.CollectionMatch, targetFiles.CollectionMatch, cancellationToken)
            };

            await Task.WhenAll(downloadTasks);

            return targetFiles;
        }

        private static InputFiles GetTargetFilePath() =>
            new InputFiles
            {
                Artist = Path.Combine(Path.GetTempPath(), nameof(InputFiles.Artist)),
                ArtistCollection = Path.Combine(Path.GetTempPath(), nameof(InputFiles.ArtistCollection)),
                Collection = Path.Combine(Path.GetTempPath(), nameof(InputFiles.Collection)),
                CollectionMatch = Path.Combine(Path.GetTempPath(), nameof(InputFiles.CollectionMatch))
            };
    }
}
using DataIngestion.TestAssignment.Configuration;
using DataIngestion.TestAssignment.FileDownload;
using MediatR;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DataIngestion.TestAssignment.Pipeline
{
    internal class DownloadFilesHandler : IRequestHandler<DownloadFilesRequest, string[]>
    {
        private readonly IFileDownloader fileDownloader;

        public DownloadFilesHandler(IFileDownloader fileDownloader)
        {
            this.fileDownloader = fileDownloader;
        }

        public async Task<string[]> Handle(DownloadFilesRequest request, CancellationToken cancellationToken)
        {
            string[] filesToDownload = request.Files;


            IEnumerable<Task<string>> downloadTasks = filesToDownload.Select(
                file => DownloadFile(file,cancellationToken)
                );

            return await Task.WhenAll<string>(downloadTasks);
        }

        private async Task<string> DownloadFile(string file, CancellationToken cancellationToken)
        {
            string targetPath = GetDestinationFilePath(file);
            await fileDownloader.DownloadAsync(file, GetDestinationFilePath(file), cancellationToken);
            return targetPath;
        }

        //TODO: Make destination folder Configurable
        private static string GetDestinationFilePath(string fileId) => Path.Combine(Path.GetTempPath(), fileId);
    }
}
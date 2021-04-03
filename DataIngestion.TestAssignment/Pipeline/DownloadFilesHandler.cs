using DataIngestion.TestAssignment.FileDownload;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DataIngestion.TestAssignment.Pipeline
{
    internal class DownloadFilesHandler : IRequestHandler<DownloadFilesRequest>
    {
        private readonly IFileDownloader fileDownloader;

        public DownloadFilesHandler(IFileDownloader fileDownloader)
        {
            this.fileDownloader = fileDownloader;
        }

        public async Task<Unit> Handle(DownloadFilesRequest request, CancellationToken cancellationToken)
        {
            string[] filesToDownload = request.Files;

            IEnumerable<Task> downloadTasks = filesToDownload.Select(
                file => DownloadFile(file, cancellationToken)
                );

            await Task.WhenAll(downloadTasks);

            return Unit.Value;
        }

        private async Task DownloadFile(string file, CancellationToken cancellationToken)
        {
            await fileDownloader.DownloadAsync(file, cancellationToken);
        }
    }
}
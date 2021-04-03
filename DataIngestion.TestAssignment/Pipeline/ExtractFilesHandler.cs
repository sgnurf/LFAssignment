using DataIngestion.TestAssignment.FileExtraction;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace DataIngestion.TestAssignment.Pipeline
{
    internal class ExtractFilesHandler : IRequestHandler<ExtractFilesRequest>
    {
        private readonly IFileExtractor fileExtractor;

        public ExtractFilesHandler(IFileExtractor fileExtractor)
        {
            this.fileExtractor = fileExtractor;
        }

        public async Task<Unit> Handle(ExtractFilesRequest request, CancellationToken cancellationToken)
        {
            foreach(string file in request.Files)
            {
                await fileExtractor.Extract(file, request.DestinationPath);
            }

            return Unit.Value;
        }
    }
}
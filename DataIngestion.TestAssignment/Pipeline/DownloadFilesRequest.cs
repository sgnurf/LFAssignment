using MediatR;

namespace DataIngestion.TestAssignment.Pipeline
{
    internal class DownloadFilesRequest : IRequest<string[]>
    {
        public string[] Files { get; set; }
    }
}
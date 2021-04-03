using MediatR;

namespace DataIngestion.TestAssignment.Pipeline
{
    internal class DownloadFilesRequest : IRequest
    {
        public string[] Files { get; set; }
    }
}
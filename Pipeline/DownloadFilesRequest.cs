using DataIngestion.TestAssignment.Configuration;
using MediatR;
using System.Collections.Generic;

namespace DataIngestion.TestAssignment.Pipeline
{
    internal class DownloadFilesRequest : IRequest<InputFiles>
    {
        public InputFiles Files { get; set; }
    }
}
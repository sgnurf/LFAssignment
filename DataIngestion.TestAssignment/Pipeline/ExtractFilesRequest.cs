using MediatR;
using System.Collections.Generic;

namespace DataIngestion.TestAssignment.Pipeline
{
    public class ExtractFilesRequest : IRequest
    {
        public IEnumerable<string> Files { get; set; }
        public string DestinationPath { get; set; }
    }
}
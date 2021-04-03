using DataIngestion.TestAssignment.Configuration;
using MediatR;

namespace DataIngestion.TestAssignment.Pipeline
{
    public class LoadFilesRequest : IRequest
    {
        public InputFiles files { get; set; }
    }
}
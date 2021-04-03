using DataIngestion.TestAssignment.FileParsing.LineParsing;
using System;

namespace DataIngestion.TestAssignment.FileParsing
{
    internal class LineParserFactory : ILineParserFactory
    {
        private readonly IServiceProvider serviceProvider;

        public LineParserFactory(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public ILineParser<T> GetParserForType<T>()
        {
            return (ILineParser<T>)serviceProvider.GetService(typeof(ILineParser<T>));
        }
    }
}
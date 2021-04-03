using DataIngestion.TestAssignment.FileParsing.LineParsing;

namespace DataIngestion.TestAssignment.FileParsing
{
    internal interface ILineParserFactory
    {
        ILineParser<T> GetParserForType<T>();
    }
}
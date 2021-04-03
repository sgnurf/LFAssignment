using System.Collections.Generic;

namespace DataIngestion.TestAssignment.FileParsing
{
    public interface IFileParser
    {
        IEnumerable<T> Parse<T>(string file);
    }
}
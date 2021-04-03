using System.Collections.Generic;

namespace DataIngestion.TestAssignment.FileParsing
{
    public interface IDataBaseTableFileParser<T>
    {
        IEnumerable<T> Parse(string filePath);
    }
}
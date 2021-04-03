using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataIngestion.TestAssignment.FileParsing
{
    public interface IFileParser
    {
        IAsyncEnumerable<T> Parse<T>(string file);
    }
}
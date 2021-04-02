using System.Collections.Generic;

namespace DataIngestion.TestAssignment.FileParsing
{
    internal class FileParser : IFileParser
    {
        private readonly IDataBaseTableFileParserFactory dataBaseTableFileParserFactory;

        public FileParser(IDataBaseTableFileParserFactory dataBaseTableFileParserFactory)
        {
            this.dataBaseTableFileParserFactory = dataBaseTableFileParserFactory;
        }

        public IEnumerable<T> Parse<T>(string file)
        {
            IDataBaseTableFileParser<T> parser = dataBaseTableFileParserFactory.GetParserForType<T>();
            return parser.Parse(file);
        }
    }
}
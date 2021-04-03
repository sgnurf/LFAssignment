namespace DataIngestion.TestAssignment.FileParsing
{
    internal interface IDataBaseTableFileParserFactory
    {
        IDataBaseTableFileParser<T> GetParserForType<T>();
    }
}
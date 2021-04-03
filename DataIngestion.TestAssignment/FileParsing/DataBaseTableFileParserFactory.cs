using System;

namespace DataIngestion.TestAssignment.FileParsing
{
    internal class DataBaseTableFileParserFactory : IDataBaseTableFileParserFactory
    {
        private readonly IServiceProvider serviceProvider;

        public DataBaseTableFileParserFactory(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public IDataBaseTableFileParser<T> GetParserForType<T>()
        {
            return (IDataBaseTableFileParser<T>)serviceProvider.GetService(typeof(IDataBaseTableFileParser<T>));
        }
    }
}
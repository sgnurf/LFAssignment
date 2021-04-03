using DataIngestion.TestAssignment.FileParsing.LineParsing;
using Microsoft.Extensions.Logging;
using Storage.Net.Blobs;
using System.Collections.Generic;
using System.IO;

namespace DataIngestion.TestAssignment.FileParsing
{
    internal class FileParser : IFileParser
    {
        private const char EndOfText = '\u0002';
        private readonly ILineParserFactory lineParserFactory;
        private readonly IBlobStorage blobStorage;
        private readonly ILogger<FileParser> logger;

        public FileParser(ILineParserFactory lineParserFactory, IBlobStorage blobStorage, ILogger<FileParser> logger)
        {
            this.lineParserFactory = lineParserFactory;
            this.blobStorage = blobStorage;
            this.logger = logger;
        }

        public async IAsyncEnumerable<T> Parse<T>(string file)
        {
            ILineParser<T> lineParser = lineParserFactory.GetParserForType<T>();

            using (Stream fileStream = await blobStorage.OpenReadAsync(file))
            using (StreamReader streamReader = new StreamReader(fileStream))
            {
                string line;
                while ((line = streamReader.ReadLine()) != null)
                {
                    T record = lineParser.ParseLine(line.Trim(EndOfText));

                    if (record != null)
                        yield return record;
                }
            }
        }
    }
}
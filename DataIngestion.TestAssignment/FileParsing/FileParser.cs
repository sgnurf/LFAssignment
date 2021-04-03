using DataIngestion.TestAssignment.FileParsing.LineParsing;
using Microsoft.Extensions.Logging;
using Storage.Net.Blobs;
using System;
using System.Collections.Generic;
using System.IO;

namespace DataIngestion.TestAssignment.FileParsing
{
    internal class FileParser : IFileParser
    {
        private const char StartOfHeading = '\u0001';
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
                    T record = ParseLine(lineParser, line);

                    //TODO: Add Reporting on failed parsing
                    if (record != null)
                        yield return record;
                }
            }
        }

        private T ParseLine<T>(ILineParser<T> lineParser, string line)
        {
            if (IsHeaderLine(line))
            {
                return default(T);
            }

            try
            {
                string[] values = GetLineValues(line);
                T record = lineParser.ParseLine(values);
                return record;
            }
            catch(Exception e)
            {
                logger.LogError(e, "Failed to Parse line {0}", line);
                return default(T);
            }
        }

        private static bool IsHeaderLine(string line)
        {
            return line[0] == '#';
        }

        private static string[] GetLineValues(string line)
        {
            return line.Trim(EndOfText).Split(StartOfHeading);
        }
    }
}
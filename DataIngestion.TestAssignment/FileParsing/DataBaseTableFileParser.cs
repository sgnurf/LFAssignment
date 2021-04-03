using DataIngestion.TestAssignment.FileParsing.LineParsing;
using System;
using System.Collections.Generic;
using System.IO;

namespace DataIngestion.TestAssignment.FileParsing
{
    public class DataBaseTableFileParser<T> : IDataBaseTableFileParser<T>
    {
        private const char StartOfHeading = '\u0001';
        private const char EndOfText = '\u0002';

        private readonly ILineParser<T> recordGenerator;

        public DataBaseTableFileParser(ILineParser<T> recordGenerator)
        {
            this.recordGenerator = recordGenerator;
        }

        public IEnumerable<T> Parse(string filePath)
        {
            IEnumerable<string> lines = File.ReadLines(filePath);
            foreach (string line in lines)
            {
                if (line.Length == 0 || line[0] == '#')
                {
                    continue;
                }

                string[] values = ParseLine(line);
                T record = default(T);

                try
                {
                    record = recordGenerator.ParseLine(values);
                }
                catch(Exception e)
                {
                    Console.WriteLine($"Failed to Process line: {line}. Exception{e}.");
                }

                if(record != null)
                    yield return recordGenerator.ParseLine(values);
            }
        }

        private static string[] ParseLine(string line)
        {
            return line.Trim(EndOfText).Split(StartOfHeading);
        }
    }
}
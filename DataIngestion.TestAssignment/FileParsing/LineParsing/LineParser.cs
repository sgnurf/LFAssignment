namespace DataIngestion.TestAssignment.FileParsing.LineParsing
{
    public class LineParser<T> : ILineParser<T>
    {
        private const char StartOfHeading = '\u0001';
        private readonly ILineValuesProcessor<T> lineValuesProcessor;

        public LineParser(ILineValuesProcessor<T> lineValuesProcessor)
        {
            this.lineValuesProcessor = lineValuesProcessor;
        }

        public T ParseLine(string line)
        {
            if (IsHeaderLine(line))
            {
                return default(T);
            }

            string[] values = GetLineValues(line);
            return lineValuesProcessor.ProcessValues(values);
        }

        private static bool IsHeaderLine(string line)
        {
            return line[0] == '#';
        }

        private static string[] GetLineValues(string line)
        {
            return line.Split(StartOfHeading);
        }
    }
}
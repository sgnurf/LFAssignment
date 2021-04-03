namespace DataIngestion.TestAssignment.FileParsing.LineParsing
{
    public interface ILineParser<T>
    {
        T ParseLine(string line);
    }
}
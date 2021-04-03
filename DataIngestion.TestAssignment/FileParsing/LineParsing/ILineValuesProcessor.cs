namespace DataIngestion.TestAssignment.FileParsing.LineParsing
{
    public interface ILineValuesProcessor<T>
    {
        T ProcessValues(string[] values);
    }
}
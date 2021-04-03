namespace DataIngestion.TestAssignment.FileParsing.LineParsing
{
    public abstract class AbstractLineValuesProcessor<T> : ILineValuesProcessor<T>
    {
        protected abstract int ExpectedValueCount { get; }

        public T ProcessValues(string[] values)
        {
            if (values == null
                || values.Length != ExpectedValueCount)
            {
                //TODO: Report error
                return default(T);
            }

            return ProcessValuesInternal(values);
        }

        protected abstract T ProcessValuesInternal(string[] values);
    }
}
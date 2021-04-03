using DataIngestion.TestAssignment.Extensions;
using DataIngestion.TestAssignment.InputModels;

namespace DataIngestion.TestAssignment.FileParsing.LineParsing
{
    public class CollectionMatchLineValuesProcessor : AbstractLineValuesProcessor<CollectionMatch>
    {
        protected override int ExpectedValueCount => 5;

        protected override CollectionMatch ProcessValuesInternal(string[] values)
        {
            return new CollectionMatch(
                CollectionId: long.Parse(values[1]),
                Upc: values[2],
                Grid: values[3],
                AmgAlbumId: values[4].ParseIntegerOrNull()
            );
        }
    }
}
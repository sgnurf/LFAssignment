using DataIngestion.TestAssignment.Extensions;
using DataIngestion.TestAssignment.InputModels;

namespace DataIngestion.TestAssignment.FileParsing.LineParsing
{
    public class ArtistLineValuesProcessor : AbstractLineValuesProcessor<Artist>
    {
        protected override int ExpectedValueCount => 6;

        protected override Artist ProcessValuesInternal(string[] values)
        {
            return new Artist(
                Id: long.Parse(values[1]),
                Name: values[2],
                IsActualArtist: values[3] == "1",
                ViewUrl: values[4],
                artistType: values[5].ParseIntegerOrNull()
                );
        }
    }
}
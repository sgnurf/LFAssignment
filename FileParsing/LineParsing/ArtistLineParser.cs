using DataIngestion.TestAssignment.InputModels;

namespace DataIngestion.TestAssignment.FileParsing.LineParsing
{
    public class ArtistLineParser : ILineParser<Artist>
    {
        public Artist ParseLine(string[] values)
        {
            return new Artist(
                Id: long.Parse(values[1]),
                Name: values[2],
                IsActualArtist: values[3] == "1",
                ViewUrl: values[4],
                artistType: int.Parse(values[5])
                );
        }
    }
}
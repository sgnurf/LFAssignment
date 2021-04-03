using DataIngestion.TestAssignment.InputModels;
using System;

namespace DataIngestion.TestAssignment.FileParsing.LineParsing
{
    public class ArtistCollectionLineParser : ILineParser<ArtistCollection>
    {
        public ArtistCollection ParseLine(string[] values)
        {
            return new ArtistCollection(
                ArtistId: long.Parse(values[1]),
                CollectionId: long.Parse(values[2]),
                IsPrimaryArtist: values[3] == "1",
                RoleId: int.Parse(values[4])
            );
        }
    }
}
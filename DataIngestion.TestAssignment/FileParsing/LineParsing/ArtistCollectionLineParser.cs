using DataIngestion.TestAssignment.Extensions;
using DataIngestion.TestAssignment.InputModels;
using System;

namespace DataIngestion.TestAssignment.FileParsing.LineParsing
{
    public class ArtistCollectionLineParser : AbstractLineValuesProcessor<ArtistCollection>
    {
        protected override int ExpectedValueCount => 5;

        protected override ArtistCollection ProcessValuesInternal(string[] values)
        {
            return new ArtistCollection(
                ArtistId: long.Parse(values[1]),
                CollectionId: long.Parse(values[2]),
                IsPrimaryArtist: values[3] == "1",
                RoleId: values[4].ParseIntegerOrNull()
            );
        }
    }
}
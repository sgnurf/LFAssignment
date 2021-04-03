using DataIngestion.TestAssignment.Extensions;
using DataIngestion.TestAssignment.InputModels;
using System;

namespace DataIngestion.TestAssignment.FileParsing.LineParsing
{
    public class CollectionLineParser : ILineParser<Collection>
    {
        public Collection ParseLine(string[] values)
        {
            return new Collection(
                Id: long.Parse(values[1]),
                Name: values[2],
                TitleVersion: values[3],
                SearchTerms: values[4],
                ParentalAdvisoryId: values[5].ParseIntegerOrNull(),
                ArtistDisplayName: values[6],
                ViewUrl: values[7],
                ArtworkUrl: values[8],
                OriginalReleaseDate: DateTime.Parse(values[9]),
                ITunesReleaseDate: DateTime.Parse(values[10]),
                LabelStudio: values[11],
                ContentProviderName: values[12],
                Copyright: values[13],
                PLine: values[14],
                MediaTypeId: values[15].ParseIntegerOrNull(),
                IsCompilation: values[16] == "1",
                CollectionTypeId: values[17].ParseIntegerOrNull()
            );
        }
    }
}
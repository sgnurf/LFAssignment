using DataIngestion.TestAssignment.FileParsing.LineParsing;
using DataIngestion.TestAssignment.InputModels;
using System;
using Xunit;

namespace DataIngestionAssignment.Tests.FileParsing
{
    public class ArtistLineValuesProcessorTests
    {
        [Theory]
        [InlineData("1", "2")]
        [InlineData("1", "2", "3", "4", "5", "6", "7", "8")]
        public void ProcessValues_ValueCountIncorrect_ReturnsNull(params string[] values)
        {
            //Arrange
            ArtistLineValuesProcessor artistLineValuesProcessor = new ArtistLineValuesProcessor();

            //Act
            Artist artist = artistLineValuesProcessor.ProcessValues(values);

            //Assert
            Assert.Null(artist);
        }

        [Fact]
        public void ProcessValues_ValidValues_ReturnsArtist()
        {
            //Arrange
            ArtistLineValuesProcessor artistLineValuesProcessor = new ArtistLineValuesProcessor();
            string[] values = { "ImportDate", "1", "ArtistName", "1", "ViewUrl", "1" };
            Artist expectedArtist = new Artist(1, "ArtistName", true, "ViewUrl", 1);

            //Act
            Artist artist = artistLineValuesProcessor.ProcessValues(values);

            //Assert
            Assert.Equal(expectedArtist, artist);
        }
    }
}
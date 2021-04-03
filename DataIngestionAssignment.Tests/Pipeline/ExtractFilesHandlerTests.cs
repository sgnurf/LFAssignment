using DataIngestion.TestAssignment.FileExtraction;
using DataIngestion.TestAssignment.Pipeline;
using Moq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace DataIngestionAssignment.Tests.Pipeline
{
    public class ExtractFilesHandlerTests
    {
        [Fact]
        public async Task Handle_ExtractionRequestedForEachFile()
        {
            //Arrange
            string destinationPath = "mockPath";
            string[] archivesToExtract = { "file1", "file2", "file3" };
            Mock<IFileExtractor> fileExtractor = new Mock<IFileExtractor>();
            CancellationToken cancellationToken = new CancellationToken();
            ExtractFilesRequest extractFilesRequest = new ExtractFilesRequest()
            {
                DestinationPath = destinationPath,
                Files = archivesToExtract
            };

            ExtractFilesHandler extractFilesHandler = new ExtractFilesHandler(fileExtractor.Object);

            //Act
            await extractFilesHandler.Handle(extractFilesRequest, cancellationToken);

            //Assert
            foreach (string file in archivesToExtract)
            {
                fileExtractor.Verify(e => e.Extract(file, destinationPath, cancellationToken), Times.Once);
            }
        }
    }
}
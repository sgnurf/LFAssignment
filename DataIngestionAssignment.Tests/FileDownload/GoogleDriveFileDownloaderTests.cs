using DataIngestion.TestAssignment.FileDownload;
using Google.Apis.Download;
using Moq;
using Storage.Net.Blobs;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace DataIngestionAssignment.Tests.FileDownload
{
    public class GoogleDriveFileDownloaderTests
    {
        [Fact]
        public async Task DownloadAsync_DownloadSucceeds_FileSavedToStorage()
        {
            //Arrange
            string fileId = "fileId";
            string fileContent = "fileContent";
            CancellationToken cancellationToken = new CancellationToken();
            string savedContent = null;
            Action<string> setSavedContent = (c => savedContent = c);
            (Mock<IGoogleDriveService> googleDriveService, Mock<IBlobStorage> blobStorage) = GetMockedServices(fileId, fileContent, cancellationToken, DownloadStatus.Completed, setSavedContent);

            GoogleDriveFileDownloader googleDriveFileDownloader = new GoogleDriveFileDownloader(googleDriveService.Object, blobStorage.Object);

            //Act
            await googleDriveFileDownloader.DownloadAsync(fileId, cancellationToken);

            //Assert
            Assert.Equal(fileContent, savedContent);
        }

        [Fact]
        public async Task DownloadAsync_DownloadFails_ExceptionRaised()
        {
            //Arrange
            string fileId = "fileId";
            string fileContent = "fileContent";
            CancellationToken cancellationToken = new CancellationToken();

            (Mock<IGoogleDriveService> googleDriveService, Mock<IBlobStorage> blobStorage) = GetMockedServices(fileId, fileContent, cancellationToken, DownloadStatus.Failed, null);

            GoogleDriveFileDownloader googleDriveFileDownloader = new GoogleDriveFileDownloader(googleDriveService.Object, blobStorage.Object);

            //Act

            //Assert
            await Assert.ThrowsAsync<Exception>(() => googleDriveFileDownloader.DownloadAsync(fileId, cancellationToken));
            blobStorage.Verify(s => s.WriteAsync(It.IsAny<string>(), It.IsAny<Stream>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        private static (Mock<IGoogleDriveService> googleDriveService, Mock<IBlobStorage> blobStorage) GetMockedServices(string fileId, string fileContent, CancellationToken cancellationToken, DownloadStatus downloadStatus, Action<string> setSavedContent)
        {
            var googleDriveService = new Mock<IGoogleDriveService>();
            Mock<IDownloadProgress> downloadProgress = new Mock<IDownloadProgress>();
            downloadProgress.SetupGet(p => p.Status).Returns(downloadStatus);

            googleDriveService.Setup(s => s.GetFile(fileId, It.IsAny<Stream>(), cancellationToken))
                .ReturnsAsync((string _, Stream stream, CancellationToken _) =>
                {
                    StreamWriter streamWriter = new StreamWriter(stream);
                    streamWriter.Write(fileContent);
                    streamWriter.Flush();
                    return downloadProgress.Object;
                });

            var blobStorage = new Mock<IBlobStorage>();
            blobStorage
                .Setup(s => s.WriteAsync(fileId, It.IsAny<Stream>(), false, cancellationToken))
                .Callback((string _, Stream stream, bool _, CancellationToken _) =>
                {
                    stream.Position = 0;
                    StreamReader sr = new StreamReader(stream);
                    setSavedContent?.Invoke(sr.ReadToEnd());
                });

            return (googleDriveService, blobStorage);
        }
    }
}
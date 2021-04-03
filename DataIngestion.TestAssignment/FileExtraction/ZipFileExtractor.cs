using ICSharpCode.SharpZipLib.Zip;
using Storage.Net.Blobs;
using System.IO;
using System.Threading.Tasks;

namespace DataIngestion.TestAssignment.FileExtraction
{
    public class ZipFileExtractor : IFileExtractor
    {
        private readonly IBlobStorage blobStorage;

        public ZipFileExtractor(IBlobStorage blobStorage)
        {
            this.blobStorage = blobStorage;
        }

        public async Task Extract(string archiveFile, string destinationFolder)
        {
            using (Stream zipFileSream = await blobStorage.OpenReadAsync(archiveFile))
            using (ZipInputStream zipReadStream = new ZipInputStream(zipFileSream))
            {
                ZipEntry zipEntry = zipReadStream.GetNextEntry();
                await blobStorage.WriteAsync(Path.Combine(destinationFolder, zipEntry.Name), zipReadStream);
            }
        }
    }
}
using System.IO.Compression;

namespace DataIngestion.TestAssignment.FileExtraction
{
    public class ZipFileExtractor : IFileExtractor
    {
        public void Extract(string archiveFile, string destinationFolder)
        {
            ZipFile.ExtractToDirectory(archiveFile, destinationFolder);
        }
    }
}
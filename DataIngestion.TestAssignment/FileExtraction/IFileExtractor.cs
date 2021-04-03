namespace DataIngestion.TestAssignment.FileExtraction
{
    public interface IFileExtractor
    {
        void Extract(string archiveFile, string destinationFolder);
    }
}
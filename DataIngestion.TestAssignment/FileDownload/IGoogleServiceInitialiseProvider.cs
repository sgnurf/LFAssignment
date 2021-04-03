using Google.Apis.Services;

namespace DataIngestion.TestAssignment.FileDownload
{
    public interface IGoogleServiceInitialiseProvider
    {
        BaseClientService.Initializer GetInitialiser();
    }
}
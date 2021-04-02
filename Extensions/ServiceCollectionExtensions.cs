using DataIngestion.TestAssignment.Configuration;
using DataIngestion.TestAssignment.FileDownload;
using DataIngestion.TestAssignment.FileExtraction;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DataIngestion.TestAssignment.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddFileDownloadServices(this IServiceCollection services)
        {
            services.AddSingleton<IGoogleServiceInitialiseProvider, GoogleServiceInitialiseProvider>();
            services.AddSingleton<IFileDownloader, GoogleDriveFileDownloader>();
        }

        public static void AddFileDownloadServicesConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<GoogleApiConfiguration>(configuration.GetSection("GoogleApi"));
            services.Configure<FilesToIngestConfiguration>(configuration.GetSection("FilesToIngest"));
        }

        public static void AddFileExtractionServices(this IServiceCollection services)
        {
            services.AddSingleton<IFileExtractor, ZipFileExtractor>();
        }

    }
}
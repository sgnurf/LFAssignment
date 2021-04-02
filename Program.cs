using DataIngestion.TestAssignment.Configuration;
using DataIngestion.TestAssignment.Extensions;
using DataIngestion.TestAssignment.FileDownload;
using DataIngestion.TestAssignment.FileExtraction;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Threading.Tasks;

namespace DataIngestion.TestAssignment
{
    public class Program
    {
        private static async Task Main(string[] args)
        {
            IServiceCollection services = ConfigureServices();
            ServiceProvider serviceProvider = services.BuildServiceProvider();

            IFileDownloader fileDownloader = serviceProvider.GetRequiredService<IFileDownloader>();
            var config = serviceProvider.GetRequiredService<IOptions<FilesToIngestConfiguration>>().Value;

            await Task.WhenAll(
                fileDownloader.DownloadAsync(config.Artist, @"c:\temp\artist"),
                fileDownloader.DownloadAsync(config.ArtistCollection, @"c:\temp\artistCollection"),
                fileDownloader.DownloadAsync(config.Collection, @"c:\temp\collection"),
                fileDownloader.DownloadAsync(config.CollectionMatch, @"c:\temp\collectionMatch")
            );

            IFileExtractor fileExtractor= serviceProvider.GetRequiredService<IFileExtractor>();
            fileExtractor.Extract(@"c:\temp\artist", @"c:\temp\job132");
            fileExtractor.Extract(@"c:\temp\artistCollection", @"c:\temp\job132");
            fileExtractor.Extract(@"c:\temp\collection", @"c:\temp\job132");
            fileExtractor.Extract(@"c:\temp\collectionMatch", @"c:\temp\job132");
        }

        private static IServiceCollection ConfigureServices()
        {
            IConfiguration configuration = GetConfiguration();

            IServiceCollection services = new ServiceCollection();

            services.AddFileDownloadServicesConfiguration(configuration);
            services.AddFileDownloadServices();
            services.AddFileExtractionServices();

            return services;
        }

        private static IConfiguration GetConfiguration()
        {
            string devEnvironmentVariable = Environment.GetEnvironmentVariable("NETCORE_ENVIRONMENT");
            bool isDevelopmentEnvironment = string.IsNullOrEmpty(devEnvironmentVariable) || devEnvironmentVariable.ToLower() == "development";

            IConfigurationBuilder configurationBuilder =
                new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            if (isDevelopmentEnvironment)
            {
                configurationBuilder.AddUserSecrets<Program>();
            }

            configurationBuilder.AddEnvironmentVariables();

            return configurationBuilder.Build();
        }
    }
}
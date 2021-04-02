using DataIngestion.TestAssignment.Configuration;
using DataIngestion.TestAssignment.InputModels;
using DataIngestion.TestAssignment.Extensions;
using DataIngestion.TestAssignment.FileDownload;
using DataIngestion.TestAssignment.FileExtraction;
using DataIngestion.TestAssignment.FileParsing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Nest;
using MediatR;
using DataIngestion.TestAssignment.Pipeline;

namespace DataIngestion.TestAssignment
{
    public class Program
    {
        private static async Task Main(string[] args)
        {
            IServiceCollection services = ConfigureServices();
            ServiceProvider serviceProvider = services.BuildServiceProvider();

            await serviceProvider
                .GetRequiredService<App>()
                .Run();
        }

        private static IServiceCollection ConfigureServices()
        {
            IConfiguration configuration = GetConfiguration();

            IServiceCollection services = new ServiceCollection();

            services.AddFileDownloadServicesConfiguration(configuration);
            services.AddIndexingConfiguration(configuration);

            services.AddApplication();
            services.AddFileDownloadServices();
            services.AddFileExtractionServices();
            services.AddFileParsingServices();
            services.AddDataStoreServices();
            services.AddIndexingServices();
            services.AddMediatR(typeof(Program));

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
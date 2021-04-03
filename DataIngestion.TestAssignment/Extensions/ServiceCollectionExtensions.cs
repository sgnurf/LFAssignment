using DataIngestion.TestAssignment.Configuration;
using DataIngestion.TestAssignment.InputModels;
using DataIngestion.TestAssignment.FileDownload;
using DataIngestion.TestAssignment.FileExtraction;
using DataIngestion.TestAssignment.FileParsing;
using DataIngestion.TestAssignment.FileParsing.LineParsing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using DataIngestion.TestAssignment.AlbumIndexing;
using Nest;
using Microsoft.Extensions.Options;
using System;
using DataIngestion.TestAssignment.DataStores;
using Storage.Net.Blobs;
using Storage.Net;

namespace DataIngestion.TestAssignment.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddApplication(this IServiceCollection services)
        {
            services.AddSingleton<App>();
        }

        public static void AddFileDownloadServices(this IServiceCollection services)
        {
            services.AddSingleton<IGoogleDriveService, GoogleDriveService>();
            services.AddSingleton<IFileDownloader, GoogleDriveFileDownloader>();
            services.AddSingleton<IBlobStorage>( sp =>
            {
                var configuration = sp.GetRequiredService<IOptions<StorageConfiguration>>();
                return StorageFactory.Blobs.FromConnectionString(configuration.Value.ConnectionString);
            });
        }

        public static void AddFileDownloadServicesConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<GoogleApiConfiguration>(configuration.GetSection("GoogleApi"));
            services.Configure<InputFiles>(configuration.GetSection("FilesToIngest"));
            services.Configure<StorageConfiguration>(configuration.GetSection("Storage"));
        }

        public static void AddFileExtractionServices(this IServiceCollection services)
        {
            services.AddSingleton<IFileExtractor, ZipFileExtractor>();
        }

        public static void AddFileParsingServices(this IServiceCollection services)
        {
            services.AddSingleton<ILineValuesProcessor<Artist>, ArtistLineValuesProcessor>();
            services.AddSingleton<ILineValuesProcessor<Collection>, CollectionLineValuesProcessor>();
            services.AddSingleton<ILineValuesProcessor<ArtistCollection>, ArtistCollectionLineParser>();
            services.AddSingleton<ILineValuesProcessor<CollectionMatch>, CollectionMatchLineValuesProcessor>();
            services.AddSingleton<IFileParser, FileParser>();
            services.AddSingleton<ILineParserFactory, LineParserFactory>();
            services.AddSingleton(typeof(ILineParser<>), typeof(LineParser<>));
        }

        public static void AddDataStoreServices(this IServiceCollection services)
        {
            AddDataStore<ArtistStore, IArtistProvider, long, Artist>();
            AddDataStore<CollectionStore, ICollectionProvider, long, Collection>();
            AddDataStore<CollectionMatchStore, ICollectionMatchProvider, long, CollectionMatch>();
            AddDataStore<ArtistCollectionStore, IArtistCollectionProvider, (long, long, int?), ArtistCollection>();

            void AddDataStore<TImplementation,TProviderInterface, TKey, TValue>()
                where TProviderInterface : class, IDataProvider<TKey, TValue>
                where TImplementation: class, TProviderInterface, IDataStore<TValue>

            {
                services.AddSingleton<TImplementation>();
                services.AddSingleton<TProviderInterface>(serviceProvider => serviceProvider.GetRequiredService<TImplementation>());
                services.AddSingleton<IDataStore<TValue>>(serviceProvider => serviceProvider.GetRequiredService<TImplementation>());
                services.AddSingleton<IDataProvider<TKey, TValue>>(serviceProvider => serviceProvider.GetRequiredService<TImplementation>());
            }
        }

        public static void AddIndexingServices(this IServiceCollection services)
        {
            services.AddTransient<IAlbumIndexer, AlbumIndexer>();
            services.AddSingleton<IAlbumProvider, AlbumProvider>();
            services.AddSingleton<IAlbumFactory, AlbumFactory>();
            services.AddTransient<IElasticClient>( (serviceProvider) =>
                {
                    IOptions<ElasticSearchConfiguration> options = serviceProvider.GetRequiredService<IOptions<ElasticSearchConfiguration>>();
                    var settings = new ConnectionSettings(new Uri(options.Value.Endpoint)).DefaultIndex(options.Value.IndexName);
                    return new ElasticClient(settings);
                }
            );
        }

        public static void AddIndexingConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<ElasticSearchConfiguration>(configuration.GetSection("ElasticSearch"));
        }
    }
}
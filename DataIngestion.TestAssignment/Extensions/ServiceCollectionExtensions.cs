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
            services.AddSingleton<IGoogleServiceInitialiseProvider, GoogleServiceInitialiseProvider>();
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
            services.AddSingleton<ILineParser<Artist>, ArtistLineParser>();
            services.AddSingleton<ILineParser<Collection>, CollectionLineParser>();
            services.AddSingleton<ILineParser<ArtistCollection>, ArtistCollectionLineParser>();
            services.AddSingleton<ILineParser<CollectionMatch>, CollectionMatchLineParser>();
            services.AddSingleton<IFileParser, FileParser>();
            services.AddSingleton<ILineParserFactory, LineParserFactory>();
        }

        public static void AddDataStoreServices(this IServiceCollection services)
        {
            AddDataStore<long, Artist>(a => a.Id);
            AddDataStore<long, Collection>(c => c.Id);
            AddDataStore<long, CollectionMatch>(cm => cm.CollectionId);

            services.AddSingleton<ArtistCollectionStore>();
            services.AddSingleton<IArtistCollectionProvider>(serviceProvider => serviceProvider.GetRequiredService<ArtistCollectionStore>());            
            services.AddSingleton<IDataStore<ArtistCollection>>(serviceProvider => serviceProvider.GetRequiredService<ArtistCollectionStore>());
            services.AddSingleton<IDataProvider<(long, long, int), ArtistCollection>>(serviceProvider => serviceProvider.GetRequiredService<ArtistCollectionStore>());

            void AddDataStore<TKey, TValue>(GetKey<TKey,TValue> keySelector)
            {
                services.AddSingleton<DataStore<TKey, TValue>>(serviceProvider => new DataStore<TKey, TValue>(keySelector));
                services.AddSingleton<IDataStore<TValue>>(serviceProvider => serviceProvider.GetRequiredService<DataStore<TKey, TValue>>());
                services.AddSingleton<IDataProvider<TKey, TValue>>(serviceProvider => serviceProvider.GetRequiredService<DataStore<TKey, TValue>>());
            }
        }

        public static void AddIndexingServices(this IServiceCollection services)
        {
            services.AddTransient<IAlbumIndexer, AlbumIndexer>();
            services.AddSingleton<IAlbumProvider, AlbumProvider>();
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
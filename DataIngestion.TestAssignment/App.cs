using DataIngestion.TestAssignment.Configuration;
using DataIngestion.TestAssignment.Pipeline;
using MediatR;
using Microsoft.Extensions.Options;
using System.IO;
using System.Threading.Tasks;

namespace DataIngestion.TestAssignment
{
    public class App
    {
        private readonly IMediator mediator;
        private readonly InputFiles filesToIngest;
        private readonly string extractedFilesFolder = Path.Combine(Path.GetTempPath(), "ExtractedFiles");

        public App(IMediator mediator, IOptions<InputFiles> filesToIngest)
        {
            this.mediator = mediator;
            this.filesToIngest = filesToIngest.Value;
        }

        public async Task Run()
        {
            string[] downloadedFiles = await DownloadFiles();
            await ExtractFiles(downloadedFiles);
            await LoadExtractedFiles();
            await IndexAlbums();
        }

        private async Task<string[]> DownloadFiles()
        {
            return await mediator.Send(new DownloadFilesRequest
            {
                Files = new string[]
                {
                    filesToIngest.Artist,
                    filesToIngest.ArtistCollection,
                    filesToIngest.Collection,
                    filesToIngest.CollectionMatch
                }
            });
        }

        private async Task ExtractFiles(string[] files)
        {
            await mediator.Send(new ExtractFilesRequest()
            {
                Files = files,
                DestinationPath = extractedFilesFolder
            });
        }

        private async Task LoadExtractedFiles()
        {
            await mediator.Send(new LoadFilesRequest()
            {
                files = new InputFiles
                {
                    Artist = Path.Combine(extractedFilesFolder, "artist"),
                    ArtistCollection = Path.Combine(extractedFilesFolder, "artist_collection"),
                    Collection = Path.Combine(extractedFilesFolder, "collection"),
                    CollectionMatch = Path.Combine(extractedFilesFolder, "collection_match")
                }
            });
        }

        private async Task IndexAlbums()
        {
            await mediator.Send(new IndexAlbumRequest());
        }
    }
}
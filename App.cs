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
            InputFiles files = await DownloadFiles();
            await ExtractFiles(files);
            await LoadExtractedFiles();
            await IndexAlbums();
        }

        private async Task ExtractFiles(InputFiles files)
        {
            await mediator.Send(new ExtractFilesRequest()
            {
                Files = new string[] { files.Artist, files.ArtistCollection, files.Collection, files.CollectionMatch },
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

        private async Task<InputFiles> DownloadFiles()
        {
            return await mediator.Send(new DownloadFilesRequest { Files = filesToIngest });
        }
    }
}
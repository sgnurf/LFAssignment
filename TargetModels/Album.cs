using System;

namespace DataIngestion.TestAssignment.TargetModels
{
    public record Album(
        long Id, 
        string Name, 
        string url,
        string upc,
        DateTime releaseDate,
        bool IsCompilation,
        string Label,
        string ImageUrl,
        AlbumArtist[] Artists);
}
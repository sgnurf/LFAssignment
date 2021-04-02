using System;

namespace DataIngestion.TestAssignment.InputModels
{
    public record Collection(
        long Id, 
        string Name, 
        string TitleVersion, 
        string SearchTerms, 
        int? ParentalAdvisoryId, 
        string ArtistDisplayName, 
        string ViewUrl, 
        string ArtworkUrl, 
        DateTime OriginalReleaseDate,
        DateTime ITunesReleaseDate, 
        string LabelStudio, 
        string ContentProviderName, 
        string Copyright, 
        string PLine, 
        int? MediaTypeId, 
        bool IsCompilation, 
        int? CollectionTypeId);
}
namespace DataIngestion.TestAssignment.InputModels
{
    public record Artist(long Id, string Name, bool IsActualArtist, string ViewUrl, int? artistType);
}
namespace DataIngestion.TestAssignment.InputModels
{
    public record ArtistCollection(long ArtistId, long CollectionId, bool IsPrimaryArtist, int RoleId);
}
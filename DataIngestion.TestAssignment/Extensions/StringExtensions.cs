namespace DataIngestion.TestAssignment.Extensions
{
    public static class StringExtensions
    {
        public static int? ParseIntegerOrNull(this string s)
        {
            return int.TryParse(s, out int i)
                ? i
                : null;
        }
    }
}
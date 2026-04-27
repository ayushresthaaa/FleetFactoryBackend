namespace FleetFactory.Infrastructure.Helpers
{
    public static class DateTimeHelper
    {
        public static DateTime UtcNow => DateTime.UtcNow; 
        public static DateTime NepalNow =>
                    TimeZoneInfo.ConvertTimeFromUtc(
                        DateTime.UtcNow,
                        TimeZoneInfo.FindSystemTimeZoneById("Asia/Kathmandu")
                    );
    }
}
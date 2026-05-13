namespace FleetFactory.Shared.Helpers
{
    public static class DateTimeHelper
    {
        public static DateTime UtcNow =>
            DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc);

        public static DateTime NepalNow =>
            DateTime.SpecifyKind(
                TimeZoneInfo.ConvertTimeFromUtc(
                    DateTime.UtcNow,
                    TimeZoneInfo.FindSystemTimeZoneById("Asia/Kathmandu")
                ),
                DateTimeKind.Utc
            );
    }
}//here the time was not specified
namespace Service
{
    public class Utils
    {
        public static DateTime? ConvertUTCToLocalDateTime(DateTime? utc)
        {
            if (utc == null) return null;
            // Specify the target timezone using its ID (e.g., "Asia/Bangkok" for ICT)
            TimeZoneInfo targetTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Asia/Bangkok");
            // Convert UTC DateTime to the local time of the specified timezone
            return TimeZoneInfo.ConvertTimeFromUtc((DateTime)utc, targetTimeZone);
        }
    }
}


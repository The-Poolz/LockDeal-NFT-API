namespace MetaDataAPI.Utils;

public static class TimeUtils
{
    public static DateTime FromUnixTimestamp(long timestamp)
    {
        var dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(timestamp);
        return dateTimeOffset.UtcDateTime;
    }
}

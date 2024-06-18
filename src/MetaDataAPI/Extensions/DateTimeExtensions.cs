using System.Numerics;

namespace MetaDataAPI.Extensions;

public static class DateTimeExtensions
{
    public static string DateTimeStringFormat(this DateTime dateTime) => $"{dateTime:MM/dd/yyyy} {dateTime:HH:mm:ss}";
    public static string DateTimeStringFormat(this BigInteger timestamp) => 
        DateTimeStringFormat(DateTimeOffset.FromUnixTimeSeconds((long)timestamp).UtcDateTime);
}
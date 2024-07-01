using Xunit;
using System.Numerics;
using FluentAssertions;
using MetaDataAPI.Extensions;

namespace MetaDataAPI.Tests.Extensions;

public class DateTimeExtensionsTests
{
    public class DateTimeStringFormat
    {
        [Theory]
        [MemberData(nameof(TestData_ForDateTime))]
        internal void ForDateTime_ShouldReturnCorrectFormat(DateTime dateTime, string expected)
        {
            var result = dateTime.DateTimeStringFormat();

            result.Should().Be(expected);
        }

        [Theory]
        [MemberData(nameof(TestData_ForBigInteger))]
        internal void ForBigInteger_ShouldReturnCorrectFormat(long timestamp, string expected)
        {
            var result = new BigInteger(timestamp).DateTimeStringFormat();

            result.Should().Be(expected);
        }

        public static IEnumerable<object[]> TestData_ForBigInteger()
        {
            yield return new object[] { 0, "01/01/1970 00:00:00" };
            yield return new object[] { 946684800, "01/01/2000 00:00:00" };
            yield return new object[] { 1672531199, "12/31/2022 23:59:59" };
        }

        public static IEnumerable<object[]> TestData_ForDateTime()
        {
            yield return new object[] { DateTime.UnixEpoch, "01/01/1970 00:00:00" };
            yield return new object[] { new DateTime(2000, 01, 01, 0, 0, 0, DateTimeKind.Utc), "01/01/2000 00:00:00" };
            yield return new object[] { new DateTime(2022, 12, 31, 23, 59, 59, DateTimeKind.Utc), "12/31/2022 23:59:59" };
        }
    }
}
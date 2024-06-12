using System.Numerics;

namespace MetaDataAPI.Extensions;

public static class BigIntegerExtensions
{
    public static decimal WeiToEth(this BigInteger value, byte decimals) =>
        (decimal)value / (decimal)Math.Pow(10, decimals);
}
using System.Numerics;

namespace MetaDataAPI.Utils;

public static class ConvertWei
{
    public static decimal WeiToEth(BigInteger value) =>
        (decimal)value / (decimal)Math.Pow(10, 18);
}
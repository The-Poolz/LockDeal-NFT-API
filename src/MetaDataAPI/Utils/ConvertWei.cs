using System.Numerics;

namespace MetaDataAPI.Utils;

public class ConvertWei
{
    private readonly byte decimals;

    public ConvertWei(byte decimals)
    {
        this.decimals = decimals;
    }

    public decimal WeiToEth(BigInteger value) =>
        (decimal)value / (decimal)Math.Pow(10, decimals);
}
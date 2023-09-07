using MetaDataAPI.Providers;
using System.Globalization;
using System.Numerics;

namespace MetaDataAPI.Models.Response;

public class BasePoolInfo
{
    public BasePoolInfo(string? rawMetadata, ProviderFactory providerFactory)
    {
        if (string.IsNullOrEmpty(rawMetadata))
        {
            throw new ArgumentNullException(nameof(rawMetadata), "Invalid data.");
        }
        var chunks = SplitHex(RemoveHexPrefix(rawMetadata));
        ProviderAddress = "0x" + chunks[1][24..];
        PoolId = BigInteger.Parse(chunks[2], NumberStyles.AllowHexSpecifier);
        VaultId = BigInteger.Parse(chunks[3], NumberStyles.AllowHexSpecifier);
        Owner = "0x" + chunks[4][24..];
        Token = providerFactory.GetErc20Token("0x" + chunks[5][24..]);
        Params = chunks.Skip(8)
            .Select(chunk => BigInteger.Parse(chunk, NumberStyles.AllowHexSpecifier)).ToArray();     
    }
    public string ProviderAddress { get; }
    public BigInteger PoolId { get; }
    public BigInteger VaultId { get;  }
    public string Owner { get; }
    public Erc20Token Token { get;  }
    public BigInteger[] Params { get; }
    private static string RemoveHexPrefix(string hex) =>
    hex.StartsWith("0x") ? hex[2..] : hex;

    private static string[] SplitHex(string hex) =>
        Enumerable.Range(0, hex.Length / 64)
            .Select(i => hex.Substring(i * 64, 64))
            .ToArray();

}

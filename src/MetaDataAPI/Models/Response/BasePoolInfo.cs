using MetaDataAPI.Providers;
using System.Globalization;
using System.Numerics;

namespace MetaDataAPI.Models.Response;

public class BasePoolInfo
{
    public BasePoolInfo(string? rawMetadata)
    {
        if (string.IsNullOrEmpty(rawMetadata))
        {
            throw new ArgumentNullException(nameof(rawMetadata), "Invalid data.");
        }

        var chunks = SplitHex(RemoveHexPrefix(rawMetadata));        
        PoolId = BigInteger.Parse(chunks[1], NumberStyles.AllowHexSpecifier);
        VaultId = BigInteger.Parse(chunks[2], NumberStyles.AllowHexSpecifier);
        Owner = "0x" + chunks[3][24..];
        Token = new Erc20Token("0x" + chunks[4][24..]);
        Params = chunks.Skip(6)
            .Select(chunk => BigInteger.Parse(chunk, NumberStyles.AllowHexSpecifier));
        Provider = ProviderFactory.Create("0x" + chunks[0][24..], PoolId, Token.Decimals, Params.ToArray());
    }
    public IProvider Provider { get; }
    public BigInteger PoolId { get; }
    public BigInteger VaultId { get;  }
    public string Owner { get; }
    public Erc20Token Token { get;  }
    public IEnumerable<BigInteger> Params { get; }
    private static string RemoveHexPrefix(string hex) =>
    hex.StartsWith("0x") ? hex[2..] : hex;

    private static string[] SplitHex(string hex) =>
        Enumerable.Range(0, hex.Length / 64)
            .Select(i => hex.Substring(i * 64, 64))
            .ToArray();

}

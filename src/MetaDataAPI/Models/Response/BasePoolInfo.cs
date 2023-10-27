using System.Text;
using System.Numerics;
using System.Globalization;
using MetaDataAPI.Providers;

namespace MetaDataAPI.Models.Response;

public class BasePoolInfo
{
    public ProviderFactory Factory { get; }
    public string ProviderAddress { get; }
    public string ProviderName { get; }
    public BigInteger PoolId { get; }
    public BigInteger VaultId { get;  }
    public string Owner { get; }
    public Erc20Token Token { get;  }
    public BigInteger[] Params { get; }

    public BasePoolInfo(string rawMetadata, ProviderFactory providerFactory)
    {
        Factory = providerFactory;
        var chunks = SplitHex(RemoveHexPrefix(rawMetadata));
        ProviderAddress = "0x" + chunks[1][24..];
        ProviderName = HexStringToString(chunks[9]);
        PoolId = BigInteger.Parse(chunks[3], NumberStyles.AllowHexSpecifier);
        VaultId = BigInteger.Parse(chunks[4], NumberStyles.AllowHexSpecifier);
        Owner = "0x" + chunks[5][24..];
        Token = providerFactory.GetErc20Token("0x" + chunks[6][24..]);
        Params = chunks.Skip(11).Select(chunk => BigInteger.Parse(chunk, NumberStyles.AllowHexSpecifier)).ToArray();
    }

    private static string RemoveHexPrefix(string hex) =>
        hex.StartsWith("0x") ? hex[2..] : hex;

    private static string[] SplitHex(string hex) =>
        Enumerable.Range(0, hex.Length / 64)
            .Select(i => hex.Substring(i * 64, 64))
            .ToArray();

    private static string HexStringToString(string hexString)
    {
        var result = new StringBuilder();

        for (var i = 0; i < hexString.Length; i += 2)
        {
            var hexChar = hexString.Substring(i, 2);
            var charValue = Convert.ToInt32(hexChar, 16);
            result.Append((char)charValue);
        }

        return result.ToString().Trim('\0');
    }
}

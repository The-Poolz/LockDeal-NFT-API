using System.Numerics;
using System.Globalization;

namespace MetaDataAPI.Utils;

public class MetadataParser
{
    private readonly string[] chunks;

    public MetadataParser(string? rawMetadata)
    {
        if (string.IsNullOrEmpty(rawMetadata))
        {
            throw new ArgumentNullException(nameof(rawMetadata), "Invalid data.");
        }

        chunks = SplitHex(RemoveHexPrefix(rawMetadata));
    }

    public string GetProviderAddress() => "0x" + chunks[0][24..];
    public BigInteger GetPoolId() => BigInteger.Parse(chunks[1], NumberStyles.AllowHexSpecifier);
    public string GetOwnerAddress() => "0x" + chunks[2][24..];
    public string GetTokenAddress() => "0x" + chunks[3][24..];
    public IEnumerable<BigInteger> GetProviderParameters() => chunks.Skip(6)
        .Select(chunk => BigInteger.Parse(chunk, NumberStyles.AllowHexSpecifier));

    private static string RemoveHexPrefix(string hex) =>
        hex.StartsWith("0x") ? hex[2..] : hex;

    private static string[] SplitHex(string hex) =>
        Enumerable.Range(0, hex.Length / 64)
            .Select(i => hex.Substring(i * 64, 64))
            .ToArray();
}
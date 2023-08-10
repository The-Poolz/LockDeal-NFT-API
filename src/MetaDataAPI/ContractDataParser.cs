using System.Numerics;
using System.Globalization;

namespace MetaDataAPI;

public static class ContractDataParser
{
    public static BasePoolInfo ParseContractData(string rawData)
    {
        if (string.IsNullOrEmpty(rawData))
        {
            throw new ArgumentNullException(nameof(rawData), "Invalid data.");
        }

        var chunks = SplitHex(RemoveHexPrefix(rawData));

        return new BasePoolInfo(
            provider: new Provider(chunks[0]),
            poolId: BigInteger.Parse(chunks[1], NumberStyles.AllowHexSpecifier),
            owner: "0x" + chunks[2][24..],
            token: "0x" + chunks[3][24..],
            parameters: chunks.Skip(6)
                .Select(chunk => BigInteger.Parse(chunk, NumberStyles.AllowHexSpecifier))
                .ToList()
        );
    }

    private static string RemoveHexPrefix(string hex) =>
        hex.StartsWith("0x") ? hex[2..] : hex;

    private static string[] SplitHex(string hex) =>
        Enumerable.Range(0, hex.Length / 64)
            .Select(i => hex.Substring(i * 64, 64))
            .ToArray();
}
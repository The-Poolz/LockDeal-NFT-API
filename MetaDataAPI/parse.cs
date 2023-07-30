using System.Globalization;
using System.Numerics;

namespace MetaDataAPI
{
    public class ContractDataParser
    {
        public static BasePoolInfo ParseContractData(string rawData)
        {
            if (string.IsNullOrEmpty(rawData))
            {
                throw new ArgumentException("Invalid data.", nameof(rawData));
            }

            // remove the '0x' prefix if it exists
            if (rawData.StartsWith("0x"))
            {
                rawData = rawData.Substring(2);
            }

            // split the rawData into 64-character chunks
            var chunks = Enumerable.Range(0, rawData.Length / 64)
                                   .Select(i => rawData.Substring(i * 64, 64))
                                   .ToArray();

            var PoolInfo = new BasePoolInfo();
            PoolInfo.Provider = new Provider(chunks[0]);
            PoolInfo.PoolId = BigInteger.Parse(chunks[1], NumberStyles.AllowHexSpecifier); // pool id from chunk 1
            PoolInfo.Owner = "0x" + chunks[2].Substring(24); // owner address, 40 chars from chunk 2
            PoolInfo.Token = "0x" + chunks[3].Substring(24); // token address, 40 chars from chunk 3

            // remaining chunks are params
            PoolInfo.Params = chunks.Skip(6)
                                          .Select(chunk => BigInteger.Parse(chunk, NumberStyles.AllowHexSpecifier))
                                          .ToList();

            return PoolInfo;
        }
    }

}

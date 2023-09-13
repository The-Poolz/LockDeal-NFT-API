using System.Text;
using System.Numerics;
using MetaDataAPI.Storage;
using Nethereum.Hex.HexTypes;
using EthSmartContractIO.Models;
using EthSmartContractIO.ContractIO;
using Nethereum.Hex.HexConvertors.Extensions;

namespace MetaDataAPI.Utils;

public class RpcCaller : IRpcCaller
{
    private static readonly ContractIO contractIO = new();

    public string GetMetadata(BigInteger poolId)
    {
        var param = new HexBigInteger(poolId).HexValue[2..].PadLeft(64, '0');
        return GetRawData(Environments.LockDealNftAddress, MethodSignatures.GetData + param);
    }

    public byte GetDecimals(string token)
    {
        if (token == "0x0000000000000000000000000000000000000000") return 0;
        var result = GetRawData(token, MethodSignatures.Decimals);
        return Convert.ToByte(result, 16);
    }

    public string GetName(string address)
    {
        return FromHexString(GetRawData(address, MethodSignatures.Name));
    }

    public string GetSymbol(string address)
    {
        return FromHexString(GetRawData(address, MethodSignatures.Symbol));
    }

    public BigInteger GetTotalSupply(string address)
    {
        var result = GetRawData(address, MethodSignatures.TotalSupply);
        return BigInteger.Parse(result, System.Globalization.NumberStyles.HexNumber);
    }

    private static string GetRawData(string address, string methodSignature)
    {
        var readRequest = new RpcRequest(
            rpcUrl: Environments.RpcUrl,
            to: address,
            data: methodSignature
        );
        return contractIO.ExecuteAction(readRequest);
    }

    private static string FromHexString(string hexString) => FromHexString(hexString[2..].HexToByteArray());
    private static string FromHexString(byte[] data) => Encoding.ASCII.GetString(data).Replace("\0", string.Empty)[2..];
}
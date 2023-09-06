using System.Numerics;
using MetaDataAPI.Storage;
using Nethereum.Hex.HexTypes;
using EthSmartContractIO.Models;
using EthSmartContractIO.ContractIO;
using System.Text;
using Nethereum.Hex.HexConvertors.Extensions;

namespace MetaDataAPI.Utils;

public static class RpcCaller
{
    private static readonly ContractIO contractIO = new();

    public static string GetMetadata(BigInteger poolId)
    {
        var param = new HexBigInteger(poolId).HexValue[2..].PadLeft(64, '0');
        return getRawData(Environments.LockDealNftAddress, MethodSignatures.GetData + param);
    }

    internal static string getRawData(string address,string methodSignature)
    {
        var readRequest = new RpcRequest(
            rpcUrl: Environments.RpcUrl,
            to: address,
            data: methodSignature
        );
        return contractIO.ExecuteAction(readRequest);
    }
    public static byte GetDecimals(string token)
    {
        var result = getRawData(token, MethodSignatures.Decimals);
        return Convert.ToByte(result, 16);
    }
    public static string GetName(string address)
    {
        var result = getRawData(address, MethodSignatures.Name);
        return FromHexString(result);
    }

    public static string GetSymbol(string address)
    {
        var result = getRawData(address, MethodSignatures.Symbol);
        return FromHexString(result);
    }
    public static string FromHexString(string hexString) => FromHexString(hexString[2..].HexToByteArray());
    public static string FromHexString(byte[] data) => Encoding.ASCII.GetString(data).Replace("\0", string.Empty)[2..];
    
}
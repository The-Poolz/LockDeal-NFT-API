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

        var readRequest = new RpcRequest(
            rpcUrl: Environments.RpcUrl,
            to: Environments.LockDealNftAddress,
            data: MethodSignatures.GetData + param
        );

        return contractIO.ExecuteAction(readRequest);
    }

    public static byte GetDecimals(string token)
    {
        var readRequest = new RpcRequest(
            rpcUrl: Environments.RpcUrl,
            to: token,
            data: MethodSignatures.Decimals
        );
        var result = contractIO.ExecuteAction(readRequest); 
        return Convert.ToByte(result, 16);
    }
    public static string GetName(string address)
    {
        var readRequest = new RpcRequest(
            rpcUrl: Environments.RpcUrl,
            to: address,
            data: MethodSignatures.Name
        );
        var result = contractIO.ExecuteAction(readRequest);
        return FromHexString(result[2..].HexToByteArray());
    }
    public static string FromHexString(byte[] data) => Encoding.ASCII.GetString(data).Replace("\0", string.Empty)[2..];
    
}
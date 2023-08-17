using System.Numerics;
using MetaDataAPI.Storage;
using Nethereum.Hex.HexTypes;
using EthSmartContractIO.Models;
using EthSmartContractIO.ContractIO;

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

    public static string GetDecimals(string token)
    {
        var param = string.Empty.PadLeft(64, '0');

        var readRequest = new RpcRequest(
            rpcUrl: Environments.RpcUrl,
            to: token,
            data: MethodSignatures.Decimals + param
        );

        return contractIO.ExecuteAction(readRequest);
    }
}
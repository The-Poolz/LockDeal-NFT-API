using System.Numerics;
using Nethereum.Hex.HexTypes;
using EthSmartContractIO.Models;
using EthSmartContractIO.ContractIO;

namespace MetaDataAPI.Utils;

public static class RpcCaller
{
    /// <summary>
    /// Signature of 'getData(uint256)' method.
    /// </summary>
    private const string MethodSignature = "0x0178fe3f";

    public static string GetMetadata(int id)
    {
        var param = new HexBigInteger(new BigInteger(id)).HexValue[2..].PadLeft(64, '0');

        var readRequest = new RpcRequest(
            rpcUrl: Environments.RpcUrl,
            to: Environments.LockDealNftAddress,
            data: MethodSignature + param
        );

        return new ContractIO().ExecuteAction(readRequest);
    }
}
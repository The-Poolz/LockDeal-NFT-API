﻿using System.Numerics;
using Nethereum.Hex.HexTypes;
using EthSmartContractIO.Models;
using EthSmartContractIO.ContractIO;

namespace MetaDataAPI.Utils;

public static class RpcCaller
{
    private static readonly ContractIO contractIO = new();

    /// <summary>
    /// Signature of 'getData(uint256)' method.
    /// </summary>
    public const string MethodSignature = "0x0178fe3f";

    public static string GetMetadata(BigInteger poolId)
    {
        var param = new HexBigInteger(poolId).HexValue[2..].PadLeft(64, '0');

        var readRequest = new RpcRequest(
            rpcUrl: Environments.RpcUrl,
            to: Environments.LockDealNftAddress,
            data: MethodSignature + param
        );

        return contractIO.ExecuteAction(readRequest);
    }
}
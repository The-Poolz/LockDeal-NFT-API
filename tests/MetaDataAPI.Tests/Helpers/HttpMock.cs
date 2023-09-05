﻿using System.Numerics;
using Flurl.Http.Testing;
using MetaDataAPI.Storage;
using Newtonsoft.Json.Linq;
using Nethereum.Hex.HexTypes;

namespace MetaDataAPI.Tests.Helpers;

internal static class HttpMock
{
    internal static string RpcUrl => "https://localhost:5050";

    internal static string DealMetadata => "0x0000000000000000000000002028c98ac1702e2bb934a3e88734ccae42d44338000000000000000000000000000000000000000000000000000000000000000000000000000000000000000057e0433551460e85dfc5a5ddaff4db199d0f960a00000000000000000000000066134461c865f824d294d8ca0d9080cc1acd05f600000000000000000000000000000000000000000000000000000000000000a000000000000000000000000000000000000000000000000000000000000000010000000000000000000000000000000000000000000000000000000000000000";
    internal static string LockMetadata => "0x000000000000000000000000d5df3f41cc1df2cc42f3b683dd71ecc38913e0d6000000000000000000000000000000000000000000000000000000000000000100000000000000000000000057e0433551460e85dfc5a5ddaff4db199d0f960a00000000000000000000000066134461c865f824d294d8ca0d9080cc1acd05f600000000000000000000000000000000000000000000000000000000000000a0000000000000000000000000000000000000000000000000000000000000000200000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000064bfb884";
    internal static string TimedMetadata => "0x0000000000000000000000005c0cb6dd68102f51dc112c3cec1c7090d27853bc00000000000000000000000000000000000000000000000000000000000000020000000000000000000000006063fba0fbd645d648c129854cce45a70dd8969100000000000000000000000066134461c865f824d294d8ca0d9080cc1acd05f600000000000000000000000000000000000000000000000000000000000000a0000000000000000000000000000000000000000000000000000000000000000400000000000000000000000000000000000000000000000000000000000003820000000000000000000000000000000000000000000000000000000064bfb8840000000000000000000000000000000000000000000000000000000064c13b8600000000000000000000000000000000000000000000000000000000000003b6";
    internal static string CollateralMetadata => "0x000000000000000000000000db65ce03690e7044ac12f5e2ab640e7a355e9407000000000000000000000000000000000000000000000000000000000000000c0000000000000000000000006063fba0fbd645d648c129854cce45a70dd8969100000000000000000000000066134461c865f824d294d8ca0d9080cc1acd05f600000000000000000000000000000000000000000000000000000000000000a0000000000000000000000000000000000000000000000000000000000000000200000000000000000000000000000000000000000000000000000000000003e80000000000000000000000000000000000000000000000000000000064c13b86";

    internal static string DecimalsRequest => "{\"jsonrpc\":\"2.0\",\"method\":\"eth_call\",\"params\":[{\"to\":\"0x66134461c865f824d294d8ca0d9080cc1acd05f6\",\"data\":\"0x313ce567\"},\"latest\"],\"id\":0}";

    internal static string DecimalsResponse => CreateRpcResponse("0x12");
    internal static string DealResponse => CreateRpcResponse(DealMetadata);
    internal static string LockResponse => CreateRpcResponse(LockMetadata);
    internal static string TimedResponse => CreateRpcResponse(TimedMetadata);
    internal static string CollateralResponse => CreateRpcResponse(CollateralMetadata);

    private static string CreateRpcResponse(string result) => new JObject
    {
        { "result", result }
    }.ToString();

    public static HttpTestSetup SetupRpcCall(this HttpTest httpTest, BigInteger poolId, string response)
    {
        var data = MethodSignatures.GetData + new HexBigInteger(poolId).HexValue[2..].PadLeft(64, '0');
        httpTest
            .ForCallsTo(RpcUrl)
            .WithRequestBody($"{{\"jsonrpc\":\"2.0\",\"method\":\"eth_call\",\"params\":[{{\"to\":\"0x57e0433551460e85dfc5a5ddaff4db199d0f960a\",\"data\":\"{data}\"}},\"latest\"],\"id\":0}}")
            .RespondWith(response);

        return httpTest;
    }
}
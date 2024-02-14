using System.Text;
using Nethereum.Web3;
using System.Numerics;
using MetaDataAPI.Storage;
using MetaDataAPI.Providers;
using Nethereum.Hex.HexTypes;
using Nethereum.Web3.Wrappers;
using EthSmartContractIO.Models;
using MetaDataAPI.Models.Response;
using EthSmartContractIO.ContractIO;
using MetaDataAPI.Models.RPC.Outputs;
using MetaDataAPI.Models.RPC.Messages;
using Nethereum.Hex.HexConvertors.Extensions;

namespace MetaDataAPI.Utils;

public class RpcCaller : IRpcCaller
{
    private static readonly ContractIO contractIO = new();
    private readonly ContractHandlerWrapper contractHandler;

    public RpcCaller(ContractHandlerWrapper? contractHandler = null)
    {
        this.contractHandler = contractHandler ?? new ContractHandlerWrapper(new Web3(Environments.RpcUrl).Eth.GetContractHandler(Environments.LockDealNftAddress));
    }

    public virtual List<BasePoolInfo> GetFullData(BigInteger poolId)
    {
        return contractHandler
            .QueryAsync<GetFullDataMessage, GetFullDataOutput>(new GetFullDataMessage { PoolId = poolId })
            .GetAwaiter()
            .GetResult()
            .PoolInfo
            .Select(x => new BasePoolInfo(x, new ProviderFactory(this)))
            .ToList();
    }

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
        return new HexBigInteger(result).Value;
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
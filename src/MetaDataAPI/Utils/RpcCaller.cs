using System.Numerics;
using MetaDataAPI.Storage;
using Nethereum.Hex.HexTypes;
using EthSmartContractIO.Models;
using EthSmartContractIO.ContractIO;
using System.Text;
using Nethereum.Hex.HexConvertors.Extensions;

namespace MetaDataAPI.Utils;

public class RpcCaller : IRpcCaller
{
    private static readonly ContractIO contractIO = new();
    private static readonly string rpcUrl = Environments.RpcUrl;
    public RpcCaller()  {  }
    public string GetMetadata(BigInteger poolId)
    {
        var param = new HexBigInteger(poolId).HexValue[2..].PadLeft(64, '0');
        var data = GetRawData(Environments.LockDealNftAddress, MethodSignatures.GetData + param);
        Console.WriteLine($"{poolId},{data}");
        return data;
    }

    internal static string GetRawData(string address, string methodSignature)
    {
        var readRequest = new RpcRequest(
            rpcUrl: rpcUrl,
            to: address,
            data: methodSignature
        );
        return contractIO.ExecuteAction(readRequest);
    }
    public byte GetDecimals(string token)
    {
        if (token == "0x0000000000000000000000000000000000000000") return 0;
        var result = GetRawData(token, MethodSignatures.Decimals);
        return Convert.ToByte(result, 16);
    }
    public string GetName(string address)
    {
        var result = FromHexString(GetRawData(address, MethodSignatures.Name));
        Console.WriteLine($"{address},{result}");
        return result;
    }

    public string GetSymbol(string address)
    {
        var result = FromHexString(GetRawData(address, MethodSignatures.Symbol));
        return result;
    }
    public static string FromHexString(string hexString) => FromHexString(hexString[2..].HexToByteArray());
    public static string FromHexString(byte[] data) => Encoding.ASCII.GetString(data).Replace("\0", string.Empty)[2..];
    
}
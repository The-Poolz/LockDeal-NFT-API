using System.Net;
using Nethereum.Util;
using System.Numerics;
using Amazon.Lambda.Core;
using EnvironmentManager;
using Newtonsoft.Json.Linq;
using Nethereum.Hex.HexTypes;
using EthSmartContractIO.Models;
using MetaDataAPI.Models.Response;
using EthSmartContractIO.ContractIO;
using Amazon.Lambda.APIGatewayEvents;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace MetaDataAPI;

public class Function
{
    /// <summary>
    /// Signature of 'getData(uint256)' method.
    /// </summary>
    private const string MethodSignature = "0x0178fe3f";
    private readonly EnvManager envManager = new();
    private readonly string lockDealNftAddress;
    private readonly string rpcUrl;

    public Function()
    {
        rpcUrl = envManager.GetEnvironmentValue<string>("RPC_URL", true);
        lockDealNftAddress = envManager.GetEnvironmentValue<string>("LOCK_DEAL_NFT_ADDRESS", true);
    }

    public APIGatewayProxyResponse FunctionHandler(APIGatewayProxyRequest request)
    {
        var id = int.Parse(request.QueryStringParameters["id"]);

        var metadata = GetMetadata(id);
        var result = ContractDataParser.ParseContractData(metadata);
        var responseBody = new Erc721Metadata(result);

        return new APIGatewayProxyResponse
        {
            StatusCode = (int)HttpStatusCode.OK,
            Body = JObject.FromObject(responseBody).ToString(),
            Headers = new Dictionary<string, string> { { "Content-Type", "application/json" } }
        };
    }

    private string GetMetadata(int id)
    {
        var param = new HexBigInteger(new BigInteger(id)).HexValue[2..].PadLeft(64, '0');

        var readRequest = new RpcRequest(
            rpcUrl: rpcUrl,
            to: lockDealNftAddress,
            data: MethodSignature + param
        );

        return new ContractIO().ExecuteAction(readRequest);
    }
}

using System.Net;
using Nethereum.Util;
using System.Numerics;
using Amazon.Lambda.Core;
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

    public static string GetMetadata(int id)
    {
        var function = Sha3Keccack.Current.CalculateHash("getData(uint256)")[..8];
        var param = new HexBigInteger(new BigInteger(id)).HexValue[2..].PadLeft(64, '0');

        var readRequest = new RpcRequest(
            rpcUrl: "https://endpoints.omniatech.io/v1/bsc/testnet/public",
            to: "0x57e0433551460e85dfC5a5DdafF4DB199D0F960A",
            data: "0x" + function + param
        );

        return new ContractIO().ExecuteAction(readRequest);
    }
}

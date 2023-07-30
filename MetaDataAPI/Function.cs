using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using EthSmartContractIO.ContractIO;
using EthSmartContractIO.Models;
using Nethereum.Hex.HexTypes;
using Nethereum.Util;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Numerics;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace MetaDataAPI;

public class Function
{
    public APIGatewayProxyResponse FunctionHandler(APIGatewayProxyRequest request, ILambdaContext context)
    {
        var id = int.Parse(request.QueryStringParameters["id"]);

        var metada = GetMetadata(id);
        var result = ContractDataParser.ParseContractData(metada);
        var metadata = new Erc721Metadata(result);

        return new APIGatewayProxyResponse
        {
            StatusCode = (int)HttpStatusCode.OK,
            Body = JObject.FromObject(metadata).ToString(),
            Headers = new Dictionary<string, string> { { "Content-Type", "application/json" } }
        };
    }

    public static string GetMetadata(int id)
    {
        var function = Sha3Keccack.Current.CalculateHash("getData(uint256)")[..8];
        var param = new HexBigInteger(new BigInteger(id)).HexValue[2..].PadLeft(64, '0');

        var readRequest = new RpcRequest(
            rpcUrl: "https://data-seed-prebsc-1-s1.bnbchain.org:8545",
            to: "0x57e0433551460e85dfC5a5DdafF4DB199D0F960A",
            data: "0x" + function + param
        );

        return new ContractIO().ExecuteAction(readRequest);
    }
}

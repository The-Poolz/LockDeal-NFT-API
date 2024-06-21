using System.Numerics;
using Amazon.Lambda.APIGatewayEvents;

namespace MetaDataAPI.Request;

public class LambdaRequest : APIGatewayProxyRequest
{
    public BigInteger? ChainId { get; }
    public BigInteger? PoolId { get; }
}
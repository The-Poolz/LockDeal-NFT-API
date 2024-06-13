using System.Numerics;
using MetaDataAPI.Utils;
using MetaDataAPI.Providers;
using Amazon.Lambda.APIGatewayEvents;

namespace MetaDataAPI.Models.Request;

public class LambdaRequest : APIGatewayProxyRequest
{
    private ErrorResponse? ParsePoolId(out BigInteger poolId)
    {
        poolId = 0;
        if (!QueryStringParameters.TryGetValue("id", out var idParam))
        {
            return ErrorResponse.MissingId;
        }

        if (!BigInteger.TryParse(idParam, out poolId))
        {
            return ErrorResponse.InvalidId;
        }
        return null;
    }

    public ErrorResponse? PoolIdToProvider(ProviderFactory providerFactory, out Provider? provider)
    {
        provider = null;

        var error = ParsePoolId(out var poolId);
        if (error != null) return error;

        if (!providerFactory.IsPoolIdWithinSupplyRange(poolId))
        {
            return ErrorResponse.PoolIdNotInRange;
        }

        provider = providerFactory.Create(poolId);

        if (poolId != provider.PoolInfo.PoolId)
        {
            return ErrorResponse.InvalidResponse;
        }

        return null;
    }
}
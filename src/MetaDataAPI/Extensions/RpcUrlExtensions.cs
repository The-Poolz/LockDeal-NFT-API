using EnvironmentManager.Extensions;

namespace MetaDataAPI.Extensions;

public static class RpcUrlExtensions
{
    public static string ToRpcUrl(this long chainId) => $"{Env.BASE_URL_OF_RPC.GetRequired()}{chainId}";
}
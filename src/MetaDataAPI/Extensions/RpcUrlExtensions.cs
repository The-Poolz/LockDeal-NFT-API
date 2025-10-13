using EnvironmentManager.Extensions;

namespace MetaDataAPI.Extensions;

public static class RpcUrlExtensions
{
    public static string ToRpcUrl(this long chainId) => $"{Environments.BASE_URL_OF_RPC.GetRequired()}{chainId}";
}
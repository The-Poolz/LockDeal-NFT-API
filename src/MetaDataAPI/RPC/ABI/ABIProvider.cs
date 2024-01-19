using Flurl.Http;
using MetaDataAPI.Storage;
using MetaDataAPI.RPC.ABI.Models;

namespace MetaDataAPI.RPC.ABI;

public static class ABIProvider
{
    public static string GetABI()
    {
        return $"{Environments.ApiUrl}{Environments.VersionName}".GetJsonAsync<APIResponse[]>()
            .GetAwaiter()
            .GetResult()
            [0].GetABI();
    }
}
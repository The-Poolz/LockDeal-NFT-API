using Flurl.Http;
using EnvironmentManager;
using MetaDataAPI.RPC.ABI.Models;

namespace MetaDataAPI.RPC.ABI;

public class ABIProvider
{
    private readonly string apiUrl;
    private readonly string versionName;

    public ABIProvider()
    {
        var envManager = new EnvManager();
        apiUrl = envManager.GetEnvironmentValue<string>("API_TO_RETRIEVE_ABI");
        versionName = envManager.GetEnvironmentValue<string>("VERSION_NAME");
    }

    public virtual string GetABI()
    {
        return $"{apiUrl}{versionName}".GetJsonAsync<APIResponse>()
            .GetAwaiter()
            .GetResult()
            .GetABI();
    }
}
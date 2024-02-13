using EnvironmentManager;

namespace MetaDataAPI.Storage;

public static class Environments
{
    private static readonly EnvManager envManager = new();
    public static string LockDealNftAddress => envManager.GetEnvironmentValue<string>("LOCK_DEAL_NFT_ADDRESS", true);
    public static string RpcUrl => envManager.GetEnvironmentValue<string>("RPC_URL", true);
    public static string NameOfStage => envManager.GetEnvironmentValue<string>("NAME_OF_STAGE", true);
    public static string ApiUrl => envManager.GetEnvironmentValue<string>("API_TO_RETRIEVE_ABI", true);
    public static string VersionName => envManager.GetEnvironmentValue<string>("VERSION_NAME", true);
}
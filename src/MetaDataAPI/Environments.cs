using EnvironmentManager;

namespace MetaDataAPI;

public static class Environments
{
    private static readonly EnvManager envManager = new();

    public static string DealAddress => envManager.GetEnvironmentValue<string>("DEAL_CONTRACT_ADDRESS", true).ToLower();
    public static string LockAddress => envManager.GetEnvironmentValue<string>("LOCK_CONTRACT_ADDRESS", true).ToLower();
    public static string TimedAddress => envManager.GetEnvironmentValue<string>("TIMED_CONTRACT_ADDRESS", true).ToLower();
    public static string RefundAddress => envManager.GetEnvironmentValue<string>("REFUND_CONTRACT_ADDRESS", true).ToLower();
    public static string BundleAddress => envManager.GetEnvironmentValue<string>("BUNDLE_CONTRACT_ADDRESS", true).ToLower();
    public static string CollateralAddress => envManager.GetEnvironmentValue<string>("COLLATERAL_CONTRACT_ADDRESS", true).ToLower();
}
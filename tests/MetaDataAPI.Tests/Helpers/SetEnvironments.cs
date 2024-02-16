namespace MetaDataAPI.Tests.Helpers;

public class SetEnvironments
{
    public SetEnvironments()
    {
        Environment.SetEnvironmentVariable("RPC_URL", "https://bsc-testnet.publicnode.com");
        Environment.SetEnvironmentVariable("LOCK_DEAL_NFT_ADDRESS", "0xe42876a77108E8B3B2af53907f5e533Cba2Ce7BE");
    }
}
namespace MetaDataAPI.Tests.Helpers;

public class SetEnvironments
{
    public SetEnvironments()
    {
        Environment.SetEnvironmentVariable("RPC_URL", "https://data-seed-prebsc-1-s1.binance.org:8545/");
        Environment.SetEnvironmentVariable("LOCK_DEAL_NFT_ADDRESS", "0xe42876a77108E8B3B2af53907f5e533Cba2Ce7BE");
        Environment.SetEnvironmentVariable("NFT_HTML_ENDPOINT", "https://test.poolz.finance/nft.html");
        Environment.SetEnvironmentVariable("TLY_API_KEY", "api key here");
    }
}
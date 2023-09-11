namespace MetaDataAPI.Tests.Helpers;

public class SetEnvironments
{
    public SetEnvironments()
    {
        Environment.SetEnvironmentVariable("LOCK_DEAL_NFT_ADDRESS", "0x57e0433551460e85dfC5a5DdafF4DB199D0F960A".ToLower());
        Environment.SetEnvironmentVariable("RPC_URL", HttpMock.RpcUrl);
    }
}
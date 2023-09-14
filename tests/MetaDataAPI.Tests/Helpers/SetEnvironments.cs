namespace MetaDataAPI.Tests.Helpers;

public class SetEnvironments
{
    public SetEnvironments()
    {
        Environment.SetEnvironmentVariable("DEAL_CONTRACT_ADDRESS", "0x6B31bE09cF4e2da92F130B1056717fEa06176CeD".ToLower());
        Environment.SetEnvironmentVariable("LOCK_CONTRACT_ADDRESS", "0xB9FD557C192939a3889080954D52c64eBA8E9Be3".ToLower());
        Environment.SetEnvironmentVariable("TIMED_CONTRACT_ADDRESS", "0x724A076a45ee73544685d4A9Fc2240B1C635711e".ToLower());
        Environment.SetEnvironmentVariable("REFUND_CONTRACT_ADDRESS", "0x7254A337D05d3965D7D3d8c1a94Cd1CFCD1b00d6".ToLower());
        Environment.SetEnvironmentVariable("BUNDLE_CONTRACT_ADDRESS", "0x70dECfD5e51C59EBdC8AcA96bf22da6aFF00b176".ToLower());
        Environment.SetEnvironmentVariable("COLLATERAL_CONTRACT_ADDRESS", "0x8Bf8Cf18c5cB5De075978394624674bA19b96d1B".ToLower());
        Environment.SetEnvironmentVariable("LOCK_DEAL_NFT_ADDRESS", "0xD40e523BCb4230FFA1126E301f4CA0294B8CF180".ToLower());
        Environment.SetEnvironmentVariable("RPC_URL", HttpMock.RpcUrl);
    }
}
namespace MetaDataAPI.Tests.Helpers;

public class SetEnvironments
{
    public SetEnvironments()
    {
        Environment.SetEnvironmentVariable("DEAL_CONTRACT_ADDRESS", "0x2028C98AC1702E2bb934A3E88734ccaE42d44338".ToLower());
        Environment.SetEnvironmentVariable("LOCK_CONTRACT_ADDRESS", "0xD5dF3f41Cc1Df2cc42F3b683dD71eCc38913e0d6".ToLower());
        Environment.SetEnvironmentVariable("TIMED_CONTRACT_ADDRESS", "0x5C0cB6dd68102f51DC112c3ceC1c7090D27853bc".ToLower());
        Environment.SetEnvironmentVariable("REFUND_CONTRACT_ADDRESS", "0x5eBa5A16A42241D4E1d427C9EC1E4C0AeC67e2A2".ToLower());
        Environment.SetEnvironmentVariable("BUNDLE_CONTRACT_ADDRESS", "0xF1Ce27BD46F1f94Ce8Dc4DE4C52d3D845EfC29F0".ToLower());
        Environment.SetEnvironmentVariable("COLLATERAL_CONTRACT_ADDRESS", "0xDB65cE03690e7044Ac12F5e2Ab640E7A355E9407".ToLower());
    }
}
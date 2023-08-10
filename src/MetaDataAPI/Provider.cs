using Newtonsoft.Json;
using MetaDataAPI.Providers;
using MetaDataAPI.Models.Types;

namespace MetaDataAPI;

public class Provider
{
    private readonly string address;

    public Provider(string rawAddress)
    {
        address = "0x" + rawAddress[24..];
    }

    public IReadOnlyCollection<string> ParamsNames => ProvidersParameters[address];

    public static Dictionary<string, IReadOnlyCollection<string>> ProvidersParameters => new()
    {
        {
            ProvidersAddresses[ProviderName.Deal],
            new[] { "LeftAmount" }
        },
        {
            ProvidersAddresses[ProviderName.Lock],
            new[] { "LeftAmount", "StartTime" }
        },
        {
            ProvidersAddresses[ProviderName.Timed],
            new[] { "LeftAmount", "StartTime", "FinishTime", "StartAmount" }
        },
        {
            ProvidersAddresses[ProviderName.Refund],
            new[] { "CollateralId", "RateToWei" }
        },
        {
            ProvidersAddresses[ProviderName.Bundle],
            new[] { "LastSubPoolId" }
        },
        {
            ProvidersAddresses[ProviderName.Collateral],
            new[] { "FinishTime" }
        }
    };

    public static Dictionary<ProviderName, string> ProvidersAddresses => new()
    {
        { ProviderName.Deal, "0x2028C98AC1702E2bb934A3E88734ccaE42d44338".ToLower() },
        { ProviderName.Lock, "0xD5dF3f41Cc1Df2cc42F3b683dD71eCc38913e0d6".ToLower() },
        { ProviderName.Timed, "0x5C0cB6dd68102f51DC112c3ceC1c7090D27853bc".ToLower() },
        { ProviderName.Refund, "0x5eBa5A16A42241D4E1d427C9EC1E4C0AeC67e2A2".ToLower() },
        { ProviderName.Bundle, "0xF1Ce27BD46F1f94Ce8Dc4DE4C52d3D845EfC29F0".ToLower() },
        { ProviderName.Collateral, "0xDB65cE03690e7044Ac12F5e2Ab640E7A355E9407".ToLower() }
    };
}

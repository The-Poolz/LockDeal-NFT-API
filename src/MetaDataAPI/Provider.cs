using MetaDataAPI.Models.Types;

namespace MetaDataAPI;

public class Provider
{
    private readonly string address;

    public Provider(string rawAddress)
    {
        address = "0x" + rawAddress[24..];
    }

    public IReadOnlyDictionary<string, string> ParamsNames => ProvidersParameters[address];

    public static Dictionary<string, IReadOnlyDictionary<string, string>> ProvidersParameters => new()
    {
        {
            ProvidersAddresses[ProviderName.Deal],
            new Dictionary<string, string>
            {
                { "LeftAmount", "number" }
            }
        },
        {
            ProvidersAddresses[ProviderName.Lock],
            new Dictionary<string, string>
            {
                { "LeftAmount", "number" },
                { "StartTime", "date" }
            }
        },
        {
            ProvidersAddresses[ProviderName.Timed],
            new Dictionary<string, string>
            {
                { "LeftAmount", "number" },
                { "StartTime", "date" },
                { "FinishTime", "date" },
                { "StartAmount", "number" }
            }
        },
        {
            ProvidersAddresses[ProviderName.Refund],
            new Dictionary<string, string>
            {
                { "CollateralId", "number" },
                { "RateToWei", "number" }
            }
        },
        {
            ProvidersAddresses[ProviderName.Bundle],
            new Dictionary<string, string>
            {
                { "LastSubPoolId", "number" }
            }
        },
        {
            ProvidersAddresses[ProviderName.Collateral],
            new Dictionary<string, string>
            {
                { "LeftAmount", "number" },
                { "FinishTime", "date" }
            }
        }
    };

    public static Dictionary<ProviderName, string> ProvidersAddresses => new()
    {
        { ProviderName.Deal, Environments.DealAddress },
        { ProviderName.Lock, Environments.LockAddress },
        { ProviderName.Timed, Environments.TimedAddress },
        { ProviderName.Refund, Environments.RefundAddress },
        { ProviderName.Bundle, Environments.BundleAddress },
        { ProviderName.Collateral, Environments.CollateralAddress }
    };
}

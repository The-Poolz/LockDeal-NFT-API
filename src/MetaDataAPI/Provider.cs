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
        { ProviderName.Deal, Environments.DealAddress },
        { ProviderName.Lock, Environments.LockAddress },
        { ProviderName.Timed, Environments.TimedAddress },
        { ProviderName.Refund, Environments.RefundAddress },
        { ProviderName.Bundle, Environments.BundleAddress },
        { ProviderName.Collateral, Environments.CollateralAddress }
    };
}

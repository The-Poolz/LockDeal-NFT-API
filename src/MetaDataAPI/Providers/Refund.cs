namespace MetaDataAPI.Providers;

internal class Refund : IProvider
{
    public string Name => nameof(Refund);
    public List<string> ParamsName => new List<string>() { { "CollateralId" }, { "RateToWei" } };
}

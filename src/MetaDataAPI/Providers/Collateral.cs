namespace MetaDataAPI.Providers;

public class Collateral : IProvider
{
    public string Name => nameof(Collateral);
    public List<string> ParamsName => new List<string>() { { "FinishTime" } };
}

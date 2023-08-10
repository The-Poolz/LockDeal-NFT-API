namespace MetaDataAPI.Providers;

public class Deal : IProvider
{
    public List<string> ParamsName => new List<string>() { { "LeftAmount" } };
    public string Name => nameof(Deal);
}

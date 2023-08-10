namespace MetaDataAPI.Providers;

public class Lock : IProvider
{
    public List<string> ParamsName => new List<string>() { { "LeftAmount" }, { "StartTime" } };
    public string Name => nameof(Lock);
}

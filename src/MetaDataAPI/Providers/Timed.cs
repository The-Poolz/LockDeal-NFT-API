namespace MetaDataAPI.Providers;

public class Timed : IProvider
{
    public List<string> ParamsName => new List<string>() { "LeftAmount", "StartTime", "FinishTime", "StartAmount" };

    public string Name => nameof(Timed);
}

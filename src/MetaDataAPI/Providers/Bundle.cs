namespace MetaDataAPI.Providers;

public class Bundle : IProvider
{
    public string Name => nameof(Bundle);
    public List<string> ParamsName => new() { { "LastSubPoolId" } };
}

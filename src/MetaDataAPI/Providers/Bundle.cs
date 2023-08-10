namespace MetaDataAPI.Providers;

internal class Bundle : IProvider
{
    public string Name => nameof(Bundle);
    public List<string> ParamsName => new() { { "LastSubPoolId" } };
}

namespace MetaDataAPI.Providers;

public interface IProvider
{
    public string Name { get; }
    public int ParamCount => ParamsName.Count;
    public List<string> ParamsName { get; }
}

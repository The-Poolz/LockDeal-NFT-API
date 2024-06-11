namespace MetaDataAPI.ImageGeneration;

public interface IUrlShortener
{
    public string Shorten(string url, string description);
}
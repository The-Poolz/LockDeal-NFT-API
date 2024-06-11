namespace MetaDataAPI.ImageGeneration.Services;

public interface IUrlShortener
{
    public string Shorten(string url, string description);
}
using TLY.ShortUrl;
using EnvironmentManager.Extensions;

namespace MetaDataAPI.ImageGeneration;

public class UrlShortener : IUrlShortener
{
    private readonly TlyContext context;

    public UrlShortener()
    {
        context = new TlyContext(Environments.TLY_API_KEY.Get());
    }

    public UrlShortener(TlyContext context)
    {
        this.context = context;
    }

    public string Shorten(string url, string description)
    {
        return context.GetShortUrlAsync(url, description)
            .GetAwaiter()
            .GetResult()
            .ShortUrl;
    }
}
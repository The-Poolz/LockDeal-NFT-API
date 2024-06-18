using Net.Urlify;
using Net.Urlify.Attributes;
using EnvironmentManager.Extensions;

namespace MetaDataAPI.Providers.Image.Models;

public class UrlifyProvider : Urlify
{
    [QueryStringProperty("url")]
    public string Url { get; }

    [QueryStringProperty("selector")]
    public string Selector { get; }

    public UrlifyProvider(AbstractProvider provider) : base((string)Environments.HTML_TO_IMAGE_ENDPOINT.Get())
    {
        Url = provider.BuildUrl();
        Selector = ".blockmodal";
    }
}
using Net.Urlify;
using Net.Urlify.Attributes;
using EnvironmentManager.Extensions;

namespace MetaDataAPI.Providers.Image.Models;

public class NftUrlifyModel : Urlify
{
    [QueryStringProperty("url")]
    public string Url { get; }

    [QueryStringProperty("selector")]
    public string Selector { get; }

    public NftUrlifyModel(BaseUrlifyModel baseUrlifyModel) : base((string)Environments.HTML_TO_IMAGE_ENDPOINT.Get())
    {
        Url = baseUrlifyModel.BuildUrl();
        Selector = ".blockmodal";
    }
}
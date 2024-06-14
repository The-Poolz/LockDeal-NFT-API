using Net.Urlify;
using Net.Urlify.Attributes;

namespace MetaDataAPI.Providers.Image.Models;

public class NftUrlifyModel : Urlify
{
    [QueryStringProperty("url")]
    public string Url { get; }

    [QueryStringProperty("selector")]
    public string Selector { get; }

    public NftUrlifyModel(BaseUrlifyModel baseUrlifyModel) : base("https://api3.poolz.finance/twitterbot.png")
    {
        Url = baseUrlifyModel.BuildUrl();
        Selector = ".blockmodal";
    }
}
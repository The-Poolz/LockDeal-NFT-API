using HandlebarsDotNet;
using EnvironmentManager.Extensions;
using MetaDataAPI.ImageGeneration.UrlifyModels;

namespace MetaDataAPI.ImageGeneration;

public class ImageRenderer : IImageRenderer
{
    public string RenderImage(BaseUrlifyModel urlify)
    {
        return RenderImage(urlify.BuildUrl());
    }

    public string RenderImage(string url)
    {
        return Handlebars.Compile(Environments.HTML_TO_IMAGE_ENDPOINT_TEMPLATE.Get())(new
        {
            Url = url
        });
    }
}
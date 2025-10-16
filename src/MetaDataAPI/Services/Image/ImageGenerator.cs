using PuppeteerSharp;
using HandlebarsDotNet;
using MetaDataAPI.Providers;
using EnvironmentManager.Extensions;
#if !DEBUG
using MetaDataAPI.Services.PuppeteerSharp;
#endif

namespace MetaDataAPI.Services.Image;

public static class ImageGenerator
{
    private const string ImageFileName = "Image.html";

    public static async Task<Stream> GenerateImageAsync(AbstractProvider provider)
    {
        var templatePath = ResolveTemplatePath();
        var source = await File.ReadAllTextAsync(templatePath);
        var temple = Handlebars.Compile(source);
        var html = temple(provider);

#if DEBUG
        await new BrowserFetcher().DownloadAsync();
        await using var browser = await Puppeteer.LaunchAsync(new LaunchOptions
        {
            Headless = true
        });
#else
        await using var browser = await HeadlessChromiumPuppeteerLauncher.LaunchAsync();
#endif
        await using var page = await browser.NewPageAsync();
        await page.SetContentAsync(html);
        var element = await page.QuerySelectorAsync("div.blockmodal");

#if DEBUG
        var hash = ImageService.CalculateImageHash(provider);
        await element.ScreenshotAsync($"{hash}.jpg", new ElementScreenshotOptions
        {
            Type = ScreenshotType.Jpeg,
            Quality = 100
        });
#endif

        var stream = await element.ScreenshotStreamAsync(new ElementScreenshotOptions
        {
            Type = ScreenshotType.Jpeg,
            Quality = 100
        });
        await browser.CloseAsync();
        return stream;
    }

    public static string ResolveTemplatePath()
    {
        var overridePath = Environments.OVERRIDE_IMAGE_TEMPLATE_PATH.Get();
        if (!string.IsNullOrWhiteSpace(overridePath) && File.Exists(overridePath)) return overridePath;

#if DEBUG
        var localPath = Path.Combine(Directory.GetCurrentDirectory(), ImageFileName);
        if (File.Exists(localPath)) return localPath;
#else
        var layerPath = Path.Combine("/opt", ImageFileName);
        if (File.Exists(layerPath)) return layerPath;
#endif

        throw new FileNotFoundException($"Unable to locate {ImageFileName} template in Lambda layer or local project.");
    }
}
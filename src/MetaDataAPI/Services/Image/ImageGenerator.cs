using PuppeteerSharp;
using HandlebarsDotNet;
using MetaDataAPI.Providers;

namespace MetaDataAPI.Services.Image;

public static class ImageGenerator
{
    public static async Task<Stream> GenerateImageAsync(AbstractProvider provider)
    {
        var source = await File.ReadAllTextAsync("./Services/Image/Image.html");
        var temple = Handlebars.Compile(source);
        var html = temple(provider);


        await new BrowserFetcher().DownloadAsync();
        await using var browser = await Puppeteer.LaunchAsync(new LaunchOptions { Headless = true });
        await using var page = await browser.NewPageAsync();
        await page.SetContentAsync(html);

        var hash = ImageService.CalculateImageHash(provider);
        await page.ScreenshotAsync($"{hash}.jpg", new ScreenshotOptions { Type = ScreenshotType.Jpeg, Quality = 100 });


        var stream = await page.ScreenshotStreamAsync(new ScreenshotOptions { Type = ScreenshotType.Jpeg, Quality = 100 });
        await browser.CloseAsync();
        return stream;
    }
}
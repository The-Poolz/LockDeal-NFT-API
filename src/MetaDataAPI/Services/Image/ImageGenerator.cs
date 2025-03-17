using HandlebarsDotNet;
using MetaDataAPI.Providers;
using PuppeteerSharp;

namespace MetaDataAPI.Services.Image
{
    public static class ImageGenerator
    {
        public static async Task<byte[]> GenerateImageAsync(AbstractProvider provider)
        {
            var source = await File.ReadAllTextAsync("image.html");
            var temple = Handlebars.Compile(source);
            var Html = temple(provider);
            var browser = await Puppeteer.LaunchAsync(new LaunchOptions { Headless = true });
            var page = await browser.NewPageAsync();
            await page.SetContentAsync(Html);
            return await page.ScreenshotDataAsync(new ScreenshotOptions() { Type = ScreenshotType.Jpeg, Quality = 100 });
        }
    }
}

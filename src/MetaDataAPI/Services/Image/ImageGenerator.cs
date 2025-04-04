﻿using PuppeteerSharp;
using HandlebarsDotNet;
using MetaDataAPI.Providers;
using Microsoft.Extensions.Logging;
using HeadlessChromium.Puppeteer.Lambda.Dotnet;

namespace MetaDataAPI.Services.Image;

public static class ImageGenerator
{
    public static async Task<Stream> GenerateImageAsync(AbstractProvider provider)
    {
        var source = await File.ReadAllTextAsync("./Services/Image/Image.html");
        var temple = Handlebars.Compile(source);
        var html = temple(provider);

#if DEBUG
        await new BrowserFetcher().DownloadAsync();
        await using var browser = await Puppeteer.LaunchAsync(new LaunchOptions
        {
            Headless = true
        });
#else
        var browserLauncher = new HeadlessChromiumPuppeteerLauncher(new LoggerFactory());
        await using var browser = await browserLauncher.LaunchAsync();
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
}
using Moq;
using Xunit;
using TLY.ShortUrl;
using FluentAssertions;
using MetaDataAPI.ImageGeneration.Services;

namespace MetaDataAPI.Tests.ImageGeneration.Services;

public class UrlShortenerTests
{
    public class Shorten
    {
        [Fact]
        internal void WhenShortenSuccess()
        {
            var url = "https://www.google.com";
            var description = "description";

            var context = new Mock<TlyContext>("api key here");
            var response = new ShortenedLinkResponse
            {
                LongUrl = url,
                Description = description,
                ShortUrl = "https://bnwrow"
            };
            context.Setup(x => x.GetShortUrlAsync(url, description, It.IsAny<string>(), It.IsAny<bool>()))
                .ReturnsAsync(response);

            var shortener = new UrlShortener(context.Object);

            var result = shortener.Shorten(url, description);

            result.Should().Be(response.ShortUrl);
        }
    }
}
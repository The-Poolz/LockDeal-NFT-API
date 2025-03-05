using Flurl.Http;
using Pinata.Client;
using Amazon.Lambda.Core;
using MetaDataAPI.Providers;
using Net.Cryptography.SHA256;
using System.Net.Http.Headers;
using System.Text;
using MetaDataAPI.Providers.Image;
using EnvironmentManager.Extensions;

namespace MetaDataAPI.Services.Image;

public class ImageService : IImageService
{
    private static readonly Config Config = new()
    {
        ApiKey = Environments.PINATA_API_KEY.Get(),
        ApiSecret = Environments.PINATA_API_SECRET.Get()
    };

    private readonly PinataClient _client = new(Config);

    public string GetImage(AbstractProvider provider)
    {
        var hashSource = CalculateImageHashSource(provider);
        var hash = hashSource.ToSha256();

        var filter = new Dictionary<string, object>
        {
            { "metadata[name]", $"{hash}.jpg" }
        };

        var response = _client.Data.PinList(filter).GetAwaiter().GetResult();

        if (!response.IsSuccess) LambdaLogger.Log($"Error occured while trying to receive image: {response.Error}");

        var ipfsPinHash = response.Count > 0 ? response.Rows[0].IpfsPinHash : UploadImage(provider);
        return $"ipfs://{ipfsPinHash}";
    }

    public string UploadImage(AbstractProvider provider)
    {
        var hashSource = CalculateImageHashSource(provider);
        var hash = hashSource.ToSha256();

        var response = _client.Pinning.PinFileToIpfsAsync(content => {
            var imageBytes = new UrlifyProvider(provider)
                .BuildUrl()
                .GetBytesAsync()
                .GetAwaiter()
                .GetResult();
            var fileContent = new StreamContent(new MemoryStream(imageBytes)) {
                Headers = {
                    ContentType = new MediaTypeHeaderValue("image/jpeg")
                }
            };

            content.AddPinataFile(fileContent, $"{hash}.jpg");
        }).GetAwaiter().GetResult();

        if (!response.IsSuccess) LambdaLogger.Log($"Error occured while trying upload image: {response.Error}");

        return response.IpfsHash; 
    }

    private static string CalculateImageHashSource(AbstractProvider provider) =>
        new StringBuilder($"{provider.ChainInfo.ChainId}-{provider.PoolId}-")
            .AppendJoin('-', provider.PoolInfo.Params)
            .ToString();
}
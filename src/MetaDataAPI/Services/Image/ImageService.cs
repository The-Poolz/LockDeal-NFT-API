using System.Text;
using Pinata.Client;
using Amazon.Lambda.Core;
using MetaDataAPI.Providers;
using Net.Cryptography.SHA256;
using System.Net.Http.Headers;
using EnvironmentManager.Extensions;
using MetaDataAPI.Services.Image.Models;

namespace MetaDataAPI.Services.Image;

public class ImageService
{
    private static readonly Config Config = new()
    {
        ApiKey = Env.PINATA_API_KEY.Get(),
        ApiSecret = Env.PINATA_API_SECRET.Get()
    };

    private readonly PinataClient _client = new(Config);

    public async Task<string> GetImageAsync(AbstractProvider provider)
    {
        var hash = CalculateImageHash(provider);

        var response = await _client.Data.PinList(queryParamFilters: new Dictionary<string, object>
        {
            { "metadata[name]", $"{hash}.jpg" }
        });

        if (!response.IsSuccess) LambdaLogger.Log($"Error occured while trying to receive image: {response.Error}");

        var logsEnabled = Env.LOG_IMAGE_ACTIONS.GetRequired<bool>();
        var fromIpfs = response.Count > 0;
        var ipfsPinHash = fromIpfs ? response.Rows[0].IpfsPinHash : await UploadImageAsync(provider);
        if (logsEnabled) LambdaLogger.Log(fromIpfs ? "Image has been loaded from IPFS." : "Image has been generated.");
        return $"ipfs://{ipfsPinHash}";
    }

    private async Task<string> UploadImageAsync(AbstractProvider provider)
    {
        var hash = CalculateImageHash(provider);
        var stream = await ImageGenerator.GenerateImageAsync(provider);
        var fileContent = new StreamContent(stream) {
            Headers = {
                ContentType = new MediaTypeHeaderValue("image/jpeg")
            }
        };

        var imageMetadata = new ImageWithMetadata(provider);
        var response = await _client.PinFileToIpfsAsync(content =>
        {
            content.AddPinataFile(fileContent, $"{hash}.jpg");
        }, imageMetadata);

        if (!response.IsSuccess) LambdaLogger.Log($"Error occured while trying upload image: {response.Error}");

        return response.IpfsHash; 
    }

    public static string CalculateImageHash(AbstractProvider provider) =>
        new StringBuilder($"{provider.ChainInfo.ChainId}-{provider.PoolId}-")
            .AppendJoin('-', provider.PoolInfo.Params)
            .ToString()
            .ToSha256();
}
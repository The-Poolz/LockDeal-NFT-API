using System.Text;
using Newtonsoft.Json;
using Amazon.DynamoDBv2;
using MetaDataAPI.Providers;
using Amazon.DynamoDBv2.Model;
using System.Security.Cryptography;

namespace MetaDataAPI.Utils;

public class DynamoDbHelper
{
    private readonly AmazonDynamoDBClient client;

    public DynamoDbHelper(AmazonDynamoDBClient client)
    {
        this.client = client;
    }

    public async Task<PutItemResponse> PutItemAsync(IProvider provider)
    {
        var jsonProvider = JsonConvert.SerializeObject(provider);
        var hash = StringToSha256(jsonProvider);

        var item = new Dictionary<string, AttributeValue>
        {
            { "Hash", new AttributeValue { S = hash } },
            { "Data", new AttributeValue { S = jsonProvider } },
            { "InsertedTime", new AttributeValue { S = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString() } },
        };

        var request = new PutItemRequest
        {
            TableName = "MetaDataCache",
            Item = item
        };

        return await client.PutItemAsync(request);

        //if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
        //{
        //    Console.WriteLine("Item successfully inserted.");
        //}
    }

    private string StringToSha256(string str)
    {
        using var sha256Hash = SHA256.Create();
        var bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(str));

        var builder = new StringBuilder();
        for (var i = 0; i < bytes.Length; i++)
        {
            builder.Append(i.ToString("x2"));
        }
        return builder.ToString();
    }
}
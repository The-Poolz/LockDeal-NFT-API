using System.Text;
using Newtonsoft.Json;
using Amazon.DynamoDBv2;
using MetaDataAPI.Providers;
using Amazon.DynamoDBv2.Model;
using System.Security.Cryptography;

namespace MetaDataAPI.Utils;

public class DynamoDb
{
    private readonly IAmazonDynamoDB client;

    public DynamoDb()
        : this(new AmazonDynamoDBClient())
    { }

    public DynamoDb(IAmazonDynamoDB client)
    {
        this.client = client;
    }

    public async Task PutItemAsync(IProvider provider)
    {
        var jsonProvider = JsonConvert.SerializeObject(provider);
        var hash = StringToSha256(jsonProvider);

        var item = await GetItemAsync(hash);
        if (item.Item.Count != 0) return;

        await client.PutItemAsync(new PutItemRequest
        {
            TableName = "MetaDataCache",
            Item = new Dictionary<string, AttributeValue>
            {
                { "Hash", new AttributeValue { S = hash } },
                { "Data", new AttributeValue { S = jsonProvider } },
                { "InsertedTime", new AttributeValue { S = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString() } },
            }
        });
    }

    public async Task<GetItemResponse> GetItemAsync(string hash)
    {
        return await client.GetItemAsync(new GetItemRequest
        {
            TableName = "MetaDataCache",
            Key = new Dictionary<string, AttributeValue>
            {
                { "Hash", new AttributeValue { S = hash } }
            }
        });
    }

    public static string StringToSha256(string str)
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
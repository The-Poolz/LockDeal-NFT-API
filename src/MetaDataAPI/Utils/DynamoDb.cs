using System.Text;
using Newtonsoft.Json;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using System.Security.Cryptography;
using MetaDataAPI.Models.DynamoDb;

namespace MetaDataAPI.Utils;

public class DynamoDb
{
    private const string TableName = "MetaDataCache";
    private const string PrimaryKey = "HashKey";
    private readonly IAmazonDynamoDB client;

    public DynamoDb()
        : this(new AmazonDynamoDBClient())
    { }

    public DynamoDb(IAmazonDynamoDB client)
    {
        this.client = client;
    }

    public string PutItem(List<DynamoDbItem> dynamoDbAttributes)
    {
        var jsonAttributes = JsonConvert.SerializeObject(dynamoDbAttributes);
        var hash = StringToSha256(jsonAttributes);

        var putRequest = new PutItemRequest
        {
            TableName = TableName,
            Item = new Dictionary<string, AttributeValue>
            {
                { PrimaryKey, new AttributeValue { S = hash } },
                { "Data", new AttributeValue { S = jsonAttributes } },
                { "InsertedTime", new AttributeValue { N = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString() } }
            },
            ConditionExpression = $"attribute_not_exists({PrimaryKey})"
        };

        TryPutItem(putRequest);

        return hash;
    }

    public static string StringToSha256(string str)
    {
        using var sha256Hash = SHA256.Create();
        var bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(str));

        var builder = new StringBuilder();
        foreach (var t in bytes)
        {
            builder.Append(t.ToString("x2"));
        }
        return builder.ToString();
    }

    private void TryPutItem(PutItemRequest putRequest)
    {
        client.PutItemAsync(putRequest)
            .ContinueWith(task =>
            {
                if (task.IsFaulted && task.Exception?.InnerExceptions.FirstOrDefault() is { } exception and not ConditionalCheckFailedException)
                    throw exception;
            })
            .GetAwaiter()
            .GetResult();
    }
}
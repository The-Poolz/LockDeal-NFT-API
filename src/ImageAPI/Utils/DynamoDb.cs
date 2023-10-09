using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;

namespace ImageAPI.Utils;

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

    public virtual async Task<GetItemResponse> GetItemAsync(string hash)
    {
        return await client.GetItemAsync(new GetItemRequest
        {
            TableName = TableName,
            Key = new Dictionary<string, AttributeValue>
            {
                { PrimaryKey, new AttributeValue { S = hash } }
            }
        });
    }

    public async Task UpdateItemAsync(string hash, string base64Image, string contentType)
    {
        var request = new UpdateItemRequest
        {
            TableName = TableName,
            Key = new Dictionary<string, AttributeValue>
            {
                { PrimaryKey, new AttributeValue { S = hash } }
            },
            AttributeUpdates = new Dictionary<string, AttributeValueUpdate>
            {
                {
                    "Image", new AttributeValueUpdate
                    {
                        Action = AttributeAction.PUT,
                        Value = new AttributeValue { S = base64Image }
                    }
                },
                {
                    "Content-Type", new AttributeValueUpdate
                    {
                        Action = AttributeAction.PUT,
                        Value = new AttributeValue { S = contentType }
                    }
                }
            }
        };

        await client.UpdateItemAsync(request);
    }
}
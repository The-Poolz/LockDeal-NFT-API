using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;

namespace ImageAPI.Utils;

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
}
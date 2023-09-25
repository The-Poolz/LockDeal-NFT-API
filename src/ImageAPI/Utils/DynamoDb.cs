﻿using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;

namespace ImageAPI.Utils;

public class DynamoDb
{
    private const string TableName = "MetaDataCache";
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
            TableName = TableName,
            Key = new Dictionary<string, AttributeValue>
            {
                { "Hash", new AttributeValue { S = hash } }
            }
        });
    }

    public void UpdateItem(string hash, string base64Image)
    {
        var request = new UpdateItemRequest
        {
            TableName = TableName,
            Key = new Dictionary<string, AttributeValue>
            {
                { "Hash", new AttributeValue { S = hash } }
            },
            AttributeUpdates = new Dictionary<string, AttributeValueUpdate>
            {
                {
                    "Image", new AttributeValueUpdate
                    {
                        Action = AttributeAction.PUT,
                        Value = new AttributeValue { S = base64Image }
                    }
                }
            }
        };

        client.UpdateItemAsync(request)
            .GetAwaiter()
            .GetResult();
    }
}
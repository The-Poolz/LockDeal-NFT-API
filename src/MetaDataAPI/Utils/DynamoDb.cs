﻿using System.Text;
using Newtonsoft.Json;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using System.Security.Cryptography;
using MetaDataAPI.Models.Response;

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

    public string PutItem(IEnumerable<Erc721Attribute> attributes)
    {
        var jsonAttributes = JsonConvert.SerializeObject(attributes);
        var hash = StringToSha256(jsonAttributes);

        var item = GetItemAsync(hash)
            .GetAwaiter()
            .GetResult();

        if (item.Item.Count == 0)
        {
            client.PutItemAsync(new PutItemRequest
            {
                TableName = "MetaDataCache",
                Item = new Dictionary<string, AttributeValue>
                {
                    { "Hash", new AttributeValue { S = hash } },
                    { "Data", new AttributeValue { S = jsonAttributes } },
                    { "InsertedTime", new AttributeValue { N = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString() } },
                }
            })
            .GetAwaiter()
            .GetResult();
        }

        return hash;
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
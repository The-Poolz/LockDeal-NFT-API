﻿using System.Text;
using Newtonsoft.Json;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using MetaDataAPI.Models.Response;
using System.Security.Cryptography;

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

    public string PutItem(IEnumerable<Erc721Attribute> attributes)
    {
        var jsonAttributes = JsonConvert.SerializeObject(attributes);
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
        for (var i = 0; i < bytes.Length; i++)
        {
            builder.Append(i.ToString("x2"));
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
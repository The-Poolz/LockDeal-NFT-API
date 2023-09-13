﻿using Moq;
using Xunit;
using ImageAPI.Utils;
using FluentAssertions;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;

namespace ImageAPI.Test.Utils;

public class DynamoDbTests
{
    [Fact]
    internal void Ctor_Default()
    {
        Environment.SetEnvironmentVariable("AWS_REGION", "us-west-2");
        var dynamoDb = new DynamoDb();

        dynamoDb.Should().NotBeNull();
    }

    [Fact]
    internal async Task GetItemAsync()
    {
        var hash = Guid.NewGuid().ToString();
        var expected = new GetItemResponse
        {
            Item = new Dictionary<string, AttributeValue>
            {
                { "Hash", new AttributeValue { S = hash } }
            }
        };
        var client = new Mock<IAmazonDynamoDB>();
        client.Setup(x => x.GetItemAsync(
            It.Is<GetItemRequest>(req => req.TableName == "MetaDataCache" && req.Key["Hash"].S == hash),
            It.IsAny<CancellationToken>())
        ).ReturnsAsync(expected);

        var result = await new DynamoDb(client.Object).GetItemAsync(hash);

        result.Should().BeEquivalentTo(expected);
    }
}
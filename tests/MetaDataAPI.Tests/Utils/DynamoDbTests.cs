using Moq;
using Xunit;
using FluentAssertions;
using Amazon.DynamoDBv2;
using MetaDataAPI.Utils;
using MetaDataAPI.Providers;
using Amazon.DynamoDBv2.Model;
using MetaDataAPI.Models.Response;
using MetaDataAPI.Models.Types;
using MetaDataAPI.Tests.Helpers;

namespace MetaDataAPI.Tests.Utils;

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

    [Fact]
    internal void PutItemAsync_AddNewItem()
    {
        var attributes = new Erc721Attribute[]
        {
            new("trait_Type", 1, DisplayType.Number)
        };
        var client = MockAmazonDynamoDB.MockClient();

        var result = new DynamoDb(client).PutItem(attributes);

        result.Should().BeEquivalentTo("000102030405060708090a0b0c0d0e0f101112131415161718191a1b1c1d1e1f");
    }

    [Fact]
    internal void PutItemAsync_SkipAddNewItem()
    {
        var attributes = new Erc721Attribute[]
        {
            new("trait_Type", 1, DisplayType.Number)
        };

        var client = MockAmazonDynamoDB.MockClient(getItemResponse: new GetItemResponse
        {
            Item = new Dictionary<string, AttributeValue>
            {
                { "Hash", new AttributeValue { S = "hash" } }
            }
        });

        var result = new DynamoDb(client).PutItem(attributes);

        result.Should().BeEquivalentTo("000102030405060708090a0b0c0d0e0f101112131415161718191a1b1c1d1e1f");
    }
}
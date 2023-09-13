using Moq;
using Xunit;
using FluentAssertions;
using Amazon.DynamoDBv2;
using MetaDataAPI.Utils;
using MetaDataAPI.Providers;
using Amazon.DynamoDBv2.Model;
using MetaDataAPI.Models.Response;
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
        var providerFactory = new ProviderFactory(new MockRpcCaller());
        var dealMetadata = StaticResults.MetaData[0];
        var provider = new DealProvider(new BasePoolInfo(dealMetadata, providerFactory));
        var client = MockAmazonDynamoDB.MockClient();

        var result = new DynamoDb(client).PutItemAsync(provider).GetAwaiter();
        result.GetResult();

        result.IsCompleted.Should().BeTrue();
    }

    [Fact]
    internal void PutItemAsync_SkipAddNewItem()
    {
        var providerFactory = new ProviderFactory(new MockRpcCaller());
        var dealMetadata = StaticResults.MetaData[0];
        var provider = new DealProvider(new BasePoolInfo(dealMetadata, providerFactory));
        var client = MockAmazonDynamoDB.MockClient(getItemResponse: new GetItemResponse
        {
            Item = new Dictionary<string, AttributeValue>
            {
                { "Hash", new AttributeValue { S = "hash" } }
            }
        });

        var result = new DynamoDb(client).PutItemAsync(provider).GetAwaiter();
        result.GetResult();

        result.IsCompleted.Should().BeTrue();
    }
}
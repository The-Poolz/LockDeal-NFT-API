using Moq;
using Xunit;
using FluentAssertions;
using Amazon.DynamoDBv2;
using MetaDataAPI.Utils;
using Amazon.DynamoDBv2.Model;
using MetaDataAPI.Models.Response;
using MetaDataAPI.Providers;
using MetaDataAPI.Tests.Helpers;

namespace MetaDataAPI.Tests.Utils;

public class DynamoDbTests
{
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
    internal void PutItemAsync()
    {
        var providerFactory = new ProviderFactory(new MockRpcCaller());
        var dealMetadata = StaticResults.MetaData[0];
        var provider = new DealProvider(new BasePoolInfo(dealMetadata, providerFactory));
        var client = new Mock<IAmazonDynamoDB>();
        client
            .Setup(x => x.PutItemAsync(It.IsAny<PutItemRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new PutItemResponse());

        client
            .Setup(x => x.GetItemAsync(It.IsAny<GetItemRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new GetItemResponse
            {
                Item = new Dictionary<string, AttributeValue>()
            });

        var result = new DynamoDb(client.Object).PutItemAsync(provider).GetAwaiter();
        result.GetResult();

        result.IsCompleted.Should().BeTrue();
    }
}
using Moq;
using Xunit;
using Amazon.DynamoDBv2;
using MetaDataAPI.Utils;
using Amazon.DynamoDBv2.Model;
using FluentAssertions;

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
        var client = new Mock<AmazonDynamoDBClient>();
        client.Setup(x => x.GetItemAsync(new GetItemRequest
        {
            TableName = "MetaDataCache",
            Key = new Dictionary<string, AttributeValue>
            {
                { "Hash", new AttributeValue { S = hash } }
            }
        }, default))
        .Returns(Task.FromResult(expected));

        var result = await new DynamoDb(client.Object).GetItemAsync(hash);

        result.Should().BeEquivalentTo(expected);
    }
}
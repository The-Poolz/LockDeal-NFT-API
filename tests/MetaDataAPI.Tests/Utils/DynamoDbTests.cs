using Xunit;
using FluentAssertions;
using MetaDataAPI.Utils;
using MetaDataAPI.Models.Types;
using MetaDataAPI.Tests.Helpers;
using MetaDataAPI.Models.Response;

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
}
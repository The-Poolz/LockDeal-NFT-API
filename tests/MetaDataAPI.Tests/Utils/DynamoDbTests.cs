using Xunit;
using FluentAssertions;
using MetaDataAPI.Utils;
using MetaDataAPI.Models.Types;
using MetaDataAPI.Tests.Helpers;
using MetaDataAPI.Models.Response;
using Newtonsoft.Json;

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

        var jsonAttributes = JsonConvert.SerializeObject(attributes);
        var expectedHash = DynamoDb.StringToSha256(jsonAttributes);

        result.Should().Be(expectedHash);
    }
}
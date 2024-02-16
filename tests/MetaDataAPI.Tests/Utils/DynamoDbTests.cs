using Xunit;
using FluentAssertions;
using MetaDataAPI.Utils;
using MetaDataAPI.Models.Types;
using MetaDataAPI.Tests.Helpers;
using MetaDataAPI.Models.Response;
using Newtonsoft.Json;
using MetaDataAPI.Models.DynamoDb;

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
        var providerName = "ProviderName";
        var attributes = new List<Erc721Attribute>
        {
            new("trait_Type", 1, DisplayType.Number)
        };

        var dynamoDbAttributes = new List<DynamoDbItem>
        {
            new(providerName, "TOKEN", 1, attributes)
        };

        var client = MockAmazonDynamoDB.MockClient();
        var result = new DynamoDb(client).PutItem(dynamoDbAttributes);

        var jsonAttributes = JsonConvert.SerializeObject(dynamoDbAttributes);
        var expectedHash = DynamoDb.StringToSha256(jsonAttributes);

        result.Should().Be(expectedHash);
    }
}
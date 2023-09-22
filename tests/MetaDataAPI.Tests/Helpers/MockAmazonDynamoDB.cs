using Moq;
using Amazon.DynamoDBv2;

namespace MetaDataAPI.Tests.Helpers;

internal static class MockAmazonDynamoDB
{
    internal static IAmazonDynamoDB MockClient() => new Mock<IAmazonDynamoDB>().Object;
}
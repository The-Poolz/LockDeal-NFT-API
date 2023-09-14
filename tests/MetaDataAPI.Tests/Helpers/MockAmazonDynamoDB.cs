using Moq;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;

namespace MetaDataAPI.Tests.Helpers;

internal static class MockAmazonDynamoDB
{
    internal static IAmazonDynamoDB MockClient(PutItemResponse? putItemResponse = null, GetItemResponse? getItemResponse = null)
    {
        var client = new Mock<IAmazonDynamoDB>();

        putItemResponse ??= new PutItemResponse();
        getItemResponse ??= new GetItemResponse
        {
            Item = new Dictionary<string, AttributeValue>()
        };

        client
            .Setup(x => x.PutItemAsync(It.IsAny<PutItemRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(putItemResponse);

        client
            .Setup(x => x.GetItemAsync(It.IsAny<GetItemRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(getItemResponse);

        return client.Object;
    }
}
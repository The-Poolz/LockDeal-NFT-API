using Newtonsoft.Json;
using Amazon.DynamoDBv2.Model;
using MetaDataAPI.Models.DynamoDb;

namespace ImageAPI.Extensions;

public static class GetItemResponseExtensions
{
    public static DynamoDbItem[] ParseAttributes(this GetItemResponse databaseItem) =>
        JsonConvert.DeserializeObject<DynamoDbItem[]>(databaseItem.Item["Data"].S)!;
}
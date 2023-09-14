namespace MetaDataAPI;

public static class ErrorMessages
{
    public const string missingIdMessage = "The 'id' parameter is missing.";
    public const string invalidIdMessage = "The 'id' parameter is not a valid BigInteger.";
    public const string invalidResponseMessage = "Id from metadata needs to be the same as Id from request.";
    public const string failedToCreateProviderMessage = "Failed to create provider.";
    public const string poolIdNotInRangeMessage = "The 'id' need to be less then total supply";
}
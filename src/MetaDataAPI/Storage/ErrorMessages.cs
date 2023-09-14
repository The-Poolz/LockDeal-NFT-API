namespace MetaDataAPI.Storage;

public static class ErrorMessages
{
    public const string MissingIdMessage = "The 'id' parameter is missing.";
    public const string InvalidIdMessage = "The 'id' parameter is not a valid BigInteger.";
    public const string InvalidResponseMessage = "Id from metadata needs to be the same as Id from request.";
    public const string FailedToCreateProviderMessage = "Failed to create provider.";
    public const string PoolIdNotInRangeMessage = "The 'id' need to be less then total supply";
}
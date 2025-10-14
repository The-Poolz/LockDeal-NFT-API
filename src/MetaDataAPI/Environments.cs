using EnvironmentManager.Attributes;

namespace MetaDataAPI;

public enum Environments
{
    [EnvironmentVariable(isRequired: true)]
    PINATA_API_KEY,
    [EnvironmentVariable(isRequired: true)]
    PINATA_API_SECRET,
    [EnvironmentVariable(isRequired: true)]
    EXTERNAL_URL,
    [EnvironmentVariable(isRequired: true)]
    GRAPHQL_STRAPI_URL,
    [EnvironmentVariable(isRequired: true)]
    BASE_URL_OF_RPC,
    [EnvironmentVariable(isRequired: true)]
    MULTI_CALL_V3_ADDRESS,
    [EnvironmentVariable(isRequired: true)]
    OVERRIDE_AWS_REGION
}
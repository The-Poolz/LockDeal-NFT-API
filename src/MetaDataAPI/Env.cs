using EnvironmentManager.Attributes;

namespace MetaDataAPI;

public enum Env
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
    [EnvironmentVariable(isRequired: false)]
    OVERRIDE_IMAGE_TEMPLATE_PATH,
    [EnvironmentVariable(isRequired: true, type: typeof(bool))]
    LOG_IMAGE_ACTIONS,
    [EnvironmentVariable(isRequired: true, type: typeof(int))]
    HTTP_CALL_TIMEOUT_IN_SECONDS
}
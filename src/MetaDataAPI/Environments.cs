using EnvironmentManager.Attributes;

namespace MetaDataAPI;

public enum Environments
{
    [EnvironmentVariable(isRequired: true)]
    PINATA_API_KEY,
    [EnvironmentVariable(isRequired: true)]
    PINATA_API_SECRET,
    [EnvironmentVariable(isRequired: true)]
    EXTERNAL_URL
}
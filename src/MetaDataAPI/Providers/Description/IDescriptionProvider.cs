using MetaDataAPI.Providers.PoolInformation;

namespace MetaDataAPI.Providers.Description;

public interface IDescriptionProvider
{
    public string Description(PoolInfo poolInfo);
}
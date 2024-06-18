using HandlebarsDotNet;
using MetaDataAPI.Providers.PoolInformation;

namespace MetaDataAPI.Providers.Description;

public class DescriptionProvider : IDescriptionProvider
{
    public string Description(PoolInfo poolInfo)
    {
        poolInfo.OnDescriptionCreating();

        return Handlebars.Compile(poolInfo.DescriptionTemplate)(poolInfo);
    }
}
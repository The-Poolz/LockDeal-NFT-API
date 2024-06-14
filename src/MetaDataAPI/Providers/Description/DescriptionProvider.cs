using HandlebarsDotNet;
using MetaDataAPI.Providers.PoolInformation;

namespace MetaDataAPI.Providers.Description;

public class DescriptionProvider : IDescriptionProvider
{
    public string Description(PoolInfo poolInfo)
    {
        return Handlebars.Compile(poolInfo.DescriptionTemplate)(poolInfo.DescriptionSource);
    }
}
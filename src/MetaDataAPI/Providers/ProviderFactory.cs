using System.Numerics;
using MetaDataAPI.Utils;
using MetaDataAPI.Models.Response;

namespace MetaDataAPI.Providers;

public static class ProviderFactory
{
    public static IProvider FromPoolId(BigInteger poolId) => FromMetdata(RpcCaller.GetMetadata(poolId));
    public static IProvider FromMetdata(string metadata) => FromPoolInfo(new BasePoolInfo(metadata));
    public static IProvider FromPoolInfo(BasePoolInfo basePoolInfo) => Create(basePoolInfo)!;
    internal static IProvider? Create(BasePoolInfo basePoolInfo)
    {    
        var name = RpcCaller.GetName(basePoolInfo.ProviderAddress);
        var objectToInstantiate = $"MetaDataAPI.Providers.{name}, MetaDataAPI";
        var objectType = Type.GetType(objectToInstantiate);
        return Activator.CreateInstance(objectType!, new object[] { basePoolInfo }) as IProvider;
    }
}
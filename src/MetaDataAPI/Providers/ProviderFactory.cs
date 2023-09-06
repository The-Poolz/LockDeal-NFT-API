using System.Numerics;
using MetaDataAPI.Utils;
using MetaDataAPI.Models.Response;

namespace MetaDataAPI.Providers;

public static class ProviderFactory
{
    public static IProvider Create(BigInteger poolId) => Create(RpcCaller.GetMetadata(poolId));
    public static IProvider Create(string metadata) => Create(new BasePoolInfo(metadata));
    public static IProvider Create(BasePoolInfo basePoolInfo) => Providers(basePoolInfo) ?? throw new ArgumentNullException();
    public static IProvider? Providers(BasePoolInfo basePoolInfo)
    {    
        var name = RpcCaller.GetName(basePoolInfo.ProviderAddress);
        var objectToInstantiate = $"MetaDataAPI.Providers.{name}, MetaDataAPI";
        var objectType = Type.GetType(objectToInstantiate);
        return Activator.CreateInstance(objectType!, new object[] { basePoolInfo }) as IProvider;
    }
}
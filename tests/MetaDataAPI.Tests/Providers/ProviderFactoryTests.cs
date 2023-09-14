using Xunit;
using System.Reflection;
using MetaDataAPI.Providers;
using MetaDataAPI.Tests.Helpers;
using MetaDataAPI.Models.Response;

namespace MetaDataAPI.Tests.Providers;

public class ProviderFactoryTests
{
    [Fact]
    internal void Providers_NumberOfInheritedClassesBeEqual()
    {
        var assembly = Assembly.Load("MetaDataAPI");
        var derivedTypes = assembly.GetTypes()
            .Where(t => t is { IsClass: true, IsAbstract: false } && t.IsSubclassOf(typeof(Provider)))
            .ToList();

        var countOfImplemented = ProviderFactory.Providers(new BasePoolInfo(StaticResults.MetaData[0], new ProviderFactory(new MockRpcCaller()))).ToArray().Length;

        Assert.Equal(countOfImplemented, derivedTypes.Count);
    }
}
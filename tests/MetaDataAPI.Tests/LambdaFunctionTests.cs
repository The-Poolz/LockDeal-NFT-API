using Moq;
using Xunit;
using System.Net;
using System.Numerics;
using FluentAssertions;
using MetaDataAPI.Providers;
using MetaDataAPI.Tests.Helpers;
using Amazon.Lambda.APIGatewayEvents;
using MetaDataAPI.Models.Response;
using poolz.finance.csharp.contracts.LockDealNFT.ContractDefinition;

namespace MetaDataAPI.Tests;

public class LambdaFunctionTests : SetEnvironments
{
    public LambdaFunctionTests()
    {
        Environment.SetEnvironmentVariable("AWS_REGION", "us-west-2");
        Environment.SetEnvironmentVariable("NAME_OF_STAGE", "test");
    }

    [Fact] 
    public void Ctor_WithoutParameters()
    {
        Environment.SetEnvironmentVariable("AWS_REGION", "us-west-2");
        var lambda = new LambdaFunction();
        lambda.Should().NotBeNull();
    }

    [Fact]
    public void FunctionHandler_ShouldReturnCorrectResponse()
    {
        var id = 0;
        var lambda = SetupLambdaFunction();
        var request = CreateRequest(id);

        var response = lambda.FunctionHandler(request);

        response.Body.Should().Contain($"Lock Deal NFT Pool: {id}");
        response.StatusCode.Should().Be((int)HttpStatusCode.OK);
        response.Headers.Should().Contain(new KeyValuePair<string, string>("Content-Type", "application/json"));
    }

    private static LambdaFunction SetupLambdaFunction()
    {
        var basePoolInfo = new[]
        {
            new BasePoolInfo
            {
                Name = "DealProvider",
                PoolId = 0,
                Token = "0x32a23a97daEd1F41fA6dFE72Cc33aD5bCBdf17E1",
                Owner = "0x000000000000000000000000000000000000dEaD",
                VaultId = 2,
                Provider = "0x70B0F2fd774376063faCC9178307cF1E18Ea3aF0",
                Params = new List<BigInteger> { 10000000000000000000 }
            }
        };
        var provider = new Mock<DealProvider>(basePoolInfo);

        provider.Setup(x => x.Token)
            .Returns(new Erc20Token("name", "symbol", "0x", 18));

        provider.Setup(x => x.GetErc721Metadata())
            .Returns(new Erc721Metadata(
                "Lock Deal NFT Pool: 0",
                "desc",
                "image",
                new Erc721Attribute[] { }
            ));

        var factory = new Mock<ProviderFactory>();
        factory.Setup(x => x.Create(It.IsAny<BigInteger>()))
            .Returns(provider.Object);
        return new LambdaFunction(factory.Object);
    }

    private static APIGatewayProxyRequest CreateRequest(int id)
    {
        return new APIGatewayProxyRequest
        {
            QueryStringParameters = new Dictionary<string, string> { { "id", id.ToString() } }
        };
    }
}
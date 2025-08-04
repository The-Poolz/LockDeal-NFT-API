using GraphQL;
using GraphQL.Client.Http;
using System.Net.Http.Headers;
using Poolz.Finance.CSharp.Strapi;
using GraphQL.Client.Abstractions;
using Net.Utils.GraphQL.Extensions;
using EnvironmentManager.Extensions;
using MetaDataAPI.Services.ChainsInfo;
using MetaDataAPI.Services.Strapi.Models;
using GraphQL.Client.Serializer.Newtonsoft;

namespace MetaDataAPI.Services.Strapi;

public class StrapiClient(IGraphQLClient graphQlClient) : IStrapiClient
{
    private const string NameOfLockDealNFT = "LockDealNFT";

    public StrapiClient()
        : this(
            graphQlClient: new GraphQLHttpClient(
                endPoint: Environments.GRAPHQL_STRAPI_URL.GetRequired(),
                serializer: new NewtonsoftJsonSerializer(),
                httpClient: new HttpClient
                {
                    DefaultRequestHeaders =
                    {
                        CacheControl = new CacheControlHeaderValue
                        {
                            NoCache = true,
                            NoStore = true,
                            MustRevalidate = true
                        }
                    }
                }
            )
        )
    { }

    public async Task<ChainInfo?> GetChainInfoAsync(long chainId)
    {
        var statusFilter = new GraphQlQueryParameter<PublicationStatus?>("status", "PublicationStatus", PublicationStatus.Published);
        var contractsFilter = new GraphQlQueryParameter<ComponentContractOnChainContractOnChainFiltersInput>("contractsFilter", new ComponentContractOnChainContractOnChainFiltersInput
        {
            Or = new[]
            {
                new ComponentContractOnChainContractOnChainFiltersInput
                {
                    ContractVersion = new ContractFiltersInput
                    {
                        NameVersion = new StringFilterInput { Contains = NameOfLockDealNFT }
                    }
                }
            }
        });
        var chainFilter = new GraphQlQueryParameter<ContractsOnChainFiltersInput>("chainFilter", new ContractsOnChainFiltersInput
        {
            Chain = new ChainFiltersInput
            {
                ChainId = new LongFilterInput
                {
                    Eq = chainId
                }
            }
        });

        var query = new QueryQueryBuilder()
            .WithContractsOnChains(new ContractsOnChainQueryBuilder()
                    .WithRpc()
                    .WithContracts(new ComponentContractOnChainContractOnChainQueryBuilder()
                            .WithContractVersion(new ContractQueryBuilder()
                                .WithNameVersion()
                            )
                            .WithAddress(),
                        contractsFilter
                    ),
                status: statusFilter,
                filters: chainFilter
            )
            .WithParameter(statusFilter)
            .WithParameter(contractsFilter)
            .WithParameter(chainFilter)
            .Build();

        var response = await graphQlClient.SendQueryAsync<StrapiDataResponse>(new GraphQLQuery(query));

        var data = response.EnsureNoErrors();

        var contractOnChain = data.ContractOnChains.FirstOrDefault();
        var lockDealNFT = contractOnChain?.Contracts.FirstOrDefault(x =>
            x.ContractVersion.NameVersion.Contains(NameOfLockDealNFT)
        )?.Address;

        return contractOnChain == null || lockDealNFT == null ? null : new ChainInfo(
            chainId,
            contractOnChain.Rpc,
            lockDealNFT
        );
    }
}
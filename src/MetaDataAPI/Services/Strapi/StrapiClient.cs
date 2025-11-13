using GraphQL;
using Poolz.Finance.CSharp.Strapi;
using GraphQL.Client.Abstractions;
using Net.Utils.GraphQL.Extensions;
using MetaDataAPI.Services.ChainsInfo;
using MetaDataAPI.Services.Strapi.Models;
using Poolz.Finance.CSharp.Polly.Extensions;

namespace MetaDataAPI.Services.Strapi;

public class StrapiClient(IGraphQLClient graphQlClient, IRetryExecutor retry) : IStrapiClient
{
    private const string NameOfLockDealNFT = "LockDealNFT";

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

        var response = await retry.ExecuteAsync(async token =>
            await graphQlClient.SendQueryAsync<StrapiDataResponse>(new GraphQLQuery(query), cancellationToken: token)
        );

        var data = response.EnsureNoErrors();

        var contractOnChain = data.ContractOnChains.FirstOrDefault();
        var lockDealNFT = contractOnChain?.Contracts.FirstOrDefault(x =>
            x.ContractVersion.NameVersion.Contains(NameOfLockDealNFT)
        )?.Address;

        return contractOnChain == null || lockDealNFT == null ? null : new ChainInfo(
            chainId,
            lockDealNFT
        );
    }
}
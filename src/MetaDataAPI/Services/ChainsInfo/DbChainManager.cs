using CovalentDb;
using CovalentDb.Types;
using System.Diagnostics.CodeAnalysis;
using ConfiguredSqlConnection.Extensions;

namespace MetaDataAPI.Services.ChainsInfo;

public class DbChainManager : IChainManager
{
    private readonly CovalentContext _context;

    public DbChainManager()
        : this(new DbContextFactory<CovalentContext>().Create(ContextOption.Staging, "Prod"))
    { }

    public DbChainManager(CovalentContext context)
    {
        _context = context;
    }

    public bool TryFetchChainInfo(long chainId, [NotNullWhen(true)] out ChainInfo? chainInfo)
    {
        chainInfo = _context.DownloaderSettings
            .Where(ds => ds.ChainId == chainId && ds.ResponseType == ResponseType.LDNFTContractApproved)
            .Join(_context.Chains,
                ds => ds.ChainId,
                c => c.ChainId,
                (ds, c) => new ChainInfo(chainId, c.RpcConnection, ds.ContractAddress))
            .FirstOrDefault();

        return chainInfo != null;
    }
}
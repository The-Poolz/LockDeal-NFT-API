using CovalentDb;
using System.Numerics;
using CovalentDb.Types;
using Net.Web3.EthereumWallet;

namespace MetaDataAPI.Services.ChainsInfo;

public class DbChainManager : IChainManager
{
    private readonly CovalentContext _context;

    public DbChainManager() : this(new CovalentContext()) { }

    public DbChainManager(CovalentContext context)
    {
        _context = context;
    }

    public bool TryFetchChainInfo(BigInteger chainId, out ChainInfo chainInfo)
    {
        chainInfo = new ChainInfo(-1, string.Empty, EthereumAddress.ZeroAddress);

        var address = GetContractAddress(chainId);
        if (string.IsNullOrEmpty(address)) return false;

        var rpcUrl = GetRpcConnection(chainId);
        if (string.IsNullOrEmpty(rpcUrl)) return false;

        chainInfo = new ChainInfo(chainId, rpcUrl, address);
        return true;
    }

    private string? GetContractAddress(BigInteger chainId)
    {
        return _context.DownloaderSettings
            .FirstOrDefault(x => x.ChainId == chainId && x.ResponseType == ResponseType.LDNFTContractApproved)
            ?.ContractAddress;
    }

    private string? GetRpcConnection(BigInteger chainId)
    {
        return _context.Chains
            .FirstOrDefault(x => x.ChainId == chainId)
            ?.RpcConnection;
    }
}
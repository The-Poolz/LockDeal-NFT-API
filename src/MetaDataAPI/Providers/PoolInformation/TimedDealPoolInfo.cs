using Nethereum.Web3;
using System.Numerics;
using MetaDataAPI.Extensions;
using MetaDataAPI.Erc20Manager;
using MetaDataAPI.Providers.Attributes;
using MetaDataAPI.Providers.Image.Models;
using MetaDataAPI.Providers.Attributes.Models;
using poolz.finance.csharp.contracts.LockDealNFT.ContractDefinition;

// ReSharper disable VirtualMemberCallInConstructor

namespace MetaDataAPI.Providers.PoolInformation;

public class TimedDealPoolInfo : LockDealPoolInfo
{
    [Erc721Attribute("finish time", DisplayType.Date)]
    public BigInteger FinishTime { get; }

    [Erc721Attribute("start amount", DisplayType.Number)]
    public decimal StartAmount { get; }

    public TimedDealPoolInfo(BasePoolInfo[] poolsInfo, Erc20Token erc20)
        : base(poolsInfo, erc20)
    {
        FinishTime = Params[2];
        StartAmount = Web3.Convert.FromWei(Params[3], erc20.Decimals);
    }

    public override string DescriptionTemplate =>
        "This NFT governs a time-locked pool containing {{LeftAmount}}/{{StartAmount}} units of the asset {{Erc20Token}}." +
        " Withdrawals are permitted in a linear fashion beginning at {{bigIntegerToDateTime StartTime}}, culminating in full access at {{bigIntegerToDateTime FinishTime}}.";

    public override IEnumerable<PropertyInfo> UrlifyProperties => new List<PropertyInfo>(base.UrlifyProperties)
    {
        new PropertyInfo("Finish time", FinishTime.DateTimeStringFormat(), 2)
    };
}
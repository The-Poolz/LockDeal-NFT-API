using System.Numerics;
using HandlebarsDotNet;
using MetaDataAPI.Extensions;
using MetaDataAPI.Erc20Manager;
using MetaDataAPI.Providers.Attributes;
using MetaDataAPI.Providers.Image.Models;
using MetaDataAPI.Providers.Attributes.Models;
using poolz.finance.csharp.contracts.LockDealNFT.ContractDefinition;

// ReSharper disable VirtualMemberCallInConstructor

namespace MetaDataAPI.Providers.PoolInformation;

public class LockDealPoolInfo : DealPoolInfo
{
    [Erc721Attribute("start time", DisplayType.Date)]
    public BigInteger StartTime { get; }
    
    public LockDealPoolInfo(BasePoolInfo[] poolsInfo, Erc20Token erc20)
        : base(poolsInfo, erc20)
    {
        StartTime = Params[1];
    }

    public override string DescriptionTemplate =>
        "This NFT securely locks {{LeftAmount}} units of the asset {{Erc20Token}}. " +
        "Access to these assets will commence on the designated start time of {{bigIntegerToDateTime StartTime}}.";

    public override void OnDescriptionCreating()
    {
        Handlebars.RegisterHelper("bigIntegerToDateTime", (writer, _, parameters) =>
        {
            if (parameters[0] is not BigInteger) return;

            var bigInteger = (BigInteger)parameters[0];
            writer.WriteSafeString(bigInteger.DateTimeStringFormat());
        });
    }

    public override IEnumerable<PropertyInfo> UrlifyProperties => new[]
    {
        new PropertyInfo("Start time", StartTime.DateTimeStringFormat(), 1)
    };
}
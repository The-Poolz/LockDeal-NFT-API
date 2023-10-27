﻿using MetaDataAPI.Models.Response;
using System.Numerics;
using MetaDataAPI.Utils;
using MetaDataAPI.Models.Types;

namespace MetaDataAPI.Tests.Helpers;

public static class StaticResults
{
    internal static Dictionary<BigInteger, string> MetaData = new()
    {
        { 0, "0x00000000000000000000000000000000000000000000000000000000000000200000000000000000000000005b007f06cb9708a1e419fe936ae826c4eba21a8d00000000000000000000000000000000000000000000000000000000000000e0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000006063fba0fbd645d648c129854cce45a70dd8969100000000000000000000000043d81a2cf49238484d6960de1df9d430c81cdffc0000000000000000000000000000000000000000000000000000000000000120000000000000000000000000000000000000000000000000000000000000000c4465616c50726f76696465720000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000100000000000000000000000000000000000000000000000000038d7ea4c68000" },
        { 1, "0x00000000000000000000000000000000000000000000000000000000000000200000000000000000000000005b007f06cb9708a1e419fe936ae826c4eba21a8d00000000000000000000000000000000000000000000000000000000000000e000000000000000000000000000000000000000000000000000000000000000010000000000000000000000000000000000000000000000000000000000000000000000000000000000000000f7b6236a8e5ed3600abfd7e952ded802df47a6e500000000000000000000000043d81a2cf49238484d6960de1df9d430c81cdffc0000000000000000000000000000000000000000000000000000000000000120000000000000000000000000000000000000000000000000000000000000000c4465616c50726f76696465720000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000100000000000000000000000000000000000000000000000000038d7ea4c68000" },
        { 2, "0x000000000000000000000000000000000000000000000000000000000000002000000000000000000000000039f5487929e68cc0242905d1f6e97d0c0992cb7700000000000000000000000000000000000000000000000000000000000000e000000000000000000000000000000000000000000000000000000000000000020000000000000000000000000000000000000000000000000000000000000000000000000000000000000000f7b6236a8e5ed3600abfd7e952ded802df47a6e500000000000000000000000043d81a2cf49238484d6960de1df9d430c81cdffc000000000000000000000000000000000000000000000000000000000000012000000000000000000000000000000000000000000000000000000000000000104c6f636b4465616c50726f7669646572000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000002000000000000000000000000000000000000000000000000000000000000000100000000000000000000000000000000000000000000000000000000656b0df6" },
        { 3, "0x000000000000000000000000000000000000000000000000000000000000002000000000000000000000000039f5487929e68cc0242905d1f6e97d0c0992cb7700000000000000000000000000000000000000000000000000000000000000e000000000000000000000000000000000000000000000000000000000000000030000000000000000000000000000000000000000000000000000000000000000000000000000000000000000f7b6236a8e5ed3600abfd7e952ded802df47a6e500000000000000000000000043d81a2cf49238484d6960de1df9d430c81cdffc000000000000000000000000000000000000000000000000000000000000012000000000000000000000000000000000000000000000000000000000000000104c6f636b4465616c50726f76696465720000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000020000000000000000000000000000000000000000000000008ac7230489e800000000000000000000000000000000000000000000000000000000000065681059" },
        { 4, "0x00000000000000000000000000000000000000000000000000000000000000200000000000000000000000003d0ca2259046d6660937864b67287269268d2b6100000000000000000000000000000000000000000000000000000000000000e000000000000000000000000000000000000000000000000000000000000000040000000000000000000000000000000000000000000000000000000000000000000000000000000000000000f7b6236a8e5ed3600abfd7e952ded802df47a6e500000000000000000000000043d81a2cf49238484d6960de1df9d430c81cdffc0000000000000000000000000000000000000000000000000000000000000120000000000000000000000000000000000000000000000000000000000000001154696d65644465616c50726f766964657200000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000040000000000000000000000000000000000000000000000008ac7230489e8000000000000000000000000000000000000000000000000000000000000653a4de500000000000000000000000000000000000000000000000000000000656810590000000000000000000000000000000000000000000000008ac7230489e80000" },
        { 5, "0x00000000000000000000000000000000000000000000000000000000000000200000000000000000000000003d0ca2259046d6660937864b67287269268d2b6100000000000000000000000000000000000000000000000000000000000000e000000000000000000000000000000000000000000000000000000000000000050000000000000000000000000000000000000000000000000000000000000000000000000000000000000000f7b6236a8e5ed3600abfd7e952ded802df47a6e500000000000000000000000043d81a2cf49238484d6960de1df9d430c81cdffc0000000000000000000000000000000000000000000000000000000000000120000000000000000000000000000000000000000000000000000000000000001154696d65644465616c50726f7669646572000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000400000000000000000000000000000000000000000000003635c9adc5dea0000000000000000000000000000000000000000000000000000000000000653a593d000000000000000000000000000000000000000000000000000000006568105900000000000000000000000000000000000000000000003635c9adc5dea00000" },
        { 6, "0x00000000000000000000000000000000000000000000000000000000000000200000000000000000000000009a3188e32bda4c47491186b7dbd412854a8e392100000000000000000000000000000000000000000000000000000000000000e0000000000000000000000000000000000000000000000000000000000000000600000000000000000000000000000000000000000000000000000000000000000000000000000000000000006063fba0fbd645d648c129854cce45a70dd8969100000000000000000000000043d81a2cf49238484d6960de1df9d430c81cdffc0000000000000000000000000000000000000000000000000000000000000120000000000000000000000000000000000000000000000000000000000000000e526566756e6450726f766964657200000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000030000000000000000000000000000000000000000000000056bc75e2d631000000000000000000000000000000000000000000000000000056bc75e2d631000000000000000000000000000000000000000000000000000000000000000000008" },
        { 7, "0x00000000000000000000000000000000000000000000000000000000000000200000000000000000000000005b007f06cb9708a1e419fe936ae826c4eba21a8d00000000000000000000000000000000000000000000000000000000000000e0000000000000000000000000000000000000000000000000000000000000000700000000000000000000000000000000000000000000000000000000000000010000000000000000000000009a3188e32bda4c47491186b7dbd412854a8e3921000000000000000000000000cd1ef832eb8a5a77842c440032e03c4330974d210000000000000000000000000000000000000000000000000000000000000120000000000000000000000000000000000000000000000000000000000000000c4465616c50726f7669646572000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000010000000000000000000000000000000000000000000000056bc75e2d63100000" },
        { 8, "0x0000000000000000000000000000000000000000000000000000000000000020000000000000000000000000e63de121ca8fc1c540dbcd432d1c9d3a5cdeb94a00000000000000000000000000000000000000000000000000000000000000e0000000000000000000000000000000000000000000000000000000000000000800000000000000000000000000000000000000000000000000000000000000000000000000000000000000006063fba0fbd645d648c129854cce45a70dd8969100000000000000000000000043d81a2cf49238484d6960de1df9d430c81cdffc00000000000000000000000000000000000000000000000000000000000001200000000000000000000000000000000000000000000000000000000000000012436f6c6c61746572616c50726f7669646572000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000030000000000000000000000000000000000000000000000008ac7230489e80000000000000000000000000000000000000000000000000000000000006b30434a0000000000000000000000000000000000000000000000056bc75e2d63100000" },
        { 9, "0x00000000000000000000000000000000000000000000000000000000000000200000000000000000000000005b007f06cb9708a1e419fe936ae826c4eba21a8d00000000000000000000000000000000000000000000000000000000000000e000000000000000000000000000000000000000000000000000000000000000090000000000000000000000000000000000000000000000000000000000000000000000000000000000000000e63de121ca8fc1c540dbcd432d1c9d3a5cdeb94a00000000000000000000000043d81a2cf49238484d6960de1df9d430c81cdffc0000000000000000000000000000000000000000000000000000000000000120000000000000000000000000000000000000000000000000000000000000000c4465616c50726f7669646572000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000010000000000000000000000000000000000000000000000000000000000000000" },
        { 10, "0x00000000000000000000000000000000000000000000000000000000000000200000000000000000000000005b007f06cb9708a1e419fe936ae826c4eba21a8d00000000000000000000000000000000000000000000000000000000000000e0000000000000000000000000000000000000000000000000000000000000000a0000000000000000000000000000000000000000000000000000000000000001000000000000000000000000e63de121ca8fc1c540dbcd432d1c9d3a5cdeb94a000000000000000000000000cd1ef832eb8a5a77842c440032e03c4330974d210000000000000000000000000000000000000000000000000000000000000120000000000000000000000000000000000000000000000000000000000000000c4465616c50726f7669646572000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000010000000000000000000000000000000000000000000000000000000000000000" },
        { 11, "0x00000000000000000000000000000000000000000000000000000000000000200000000000000000000000005b007f06cb9708a1e419fe936ae826c4eba21a8d00000000000000000000000000000000000000000000000000000000000000e0000000000000000000000000000000000000000000000000000000000000000b0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000e63de121ca8fc1c540dbcd432d1c9d3a5cdeb94a00000000000000000000000043d81a2cf49238484d6960de1df9d430c81cdffc0000000000000000000000000000000000000000000000000000000000000120000000000000000000000000000000000000000000000000000000000000000c4465616c50726f7669646572000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000010000000000000000000000000000000000000000000000008ac7230489e80000" },
        { 12, "0x00000000000000000000000000000000000000000000000000000000000000200000000000000000000000009a3188e32bda4c47491186b7dbd412854a8e392100000000000000000000000000000000000000000000000000000000000000e0000000000000000000000000000000000000000000000000000000000000000c00000000000000000000000000000000000000000000000000000000000000000000000000000000000000006063fba0fbd645d648c129854cce45a70dd8969100000000000000000000000043d81a2cf49238484d6960de1df9d430c81cdffc0000000000000000000000000000000000000000000000000000000000000120000000000000000000000000000000000000000000000000000000000000000e526566756e6450726f766964657200000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000060000000000000000000000000000000000000000000000056bc75e2d631000000000000000000000000000000000000000000000000000056bc75e2d63100000000000000000000000000000000000000000000000000000000000000000000e00000000000000000000000000000000000000000000000000000000653a6daf0000000000000000000000000000000000000000000000000000000065d3042f0000000000000000000000000000000000000000000000056bc75e2d63100000" },
        { 13, "0x00000000000000000000000000000000000000000000000000000000000000200000000000000000000000003d0ca2259046d6660937864b67287269268d2b6100000000000000000000000000000000000000000000000000000000000000e0000000000000000000000000000000000000000000000000000000000000000d00000000000000000000000000000000000000000000000000000000000000000000000000000000000000009a3188e32bda4c47491186b7dbd412854a8e392100000000000000000000000043d81a2cf49238484d6960de1df9d430c81cdffc0000000000000000000000000000000000000000000000000000000000000120000000000000000000000000000000000000000000000000000000000000001154696d65644465616c50726f766964657200000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000040000000000000000000000000000000000000000000000056bc75e2d6310000000000000000000000000000000000000000000000000000000000000653a6daf0000000000000000000000000000000000000000000000000000000065d3042f0000000000000000000000000000000000000000000000056bc75e2d63100000" },
        { 14, "0x0000000000000000000000000000000000000000000000000000000000000020000000000000000000000000e63de121ca8fc1c540dbcd432d1c9d3a5cdeb94a00000000000000000000000000000000000000000000000000000000000000e0000000000000000000000000000000000000000000000000000000000000000e00000000000000000000000000000000000000000000000000000000000000010000000000000000000000006063fba0fbd645d648c129854cce45a70dd89691000000000000000000000000cd1ef832eb8a5a77842c440032e03c4330974d2100000000000000000000000000000000000000000000000000000000000001200000000000000000000000000000000000000000000000000000000000000012436f6c6c61746572616c50726f7669646572000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000030000000000000000000000000000000000000000000000008ac7230489e80000000000000000000000000000000000000000000000000000000000006b30434a0000000000000000000000000000000000000000000000056bc75e2d63100000" },
        { 15, "0x00000000000000000000000000000000000000000000000000000000000000200000000000000000000000005b007f06cb9708a1e419fe936ae826c4eba21a8d00000000000000000000000000000000000000000000000000000000000000e0000000000000000000000000000000000000000000000000000000000000000f0000000000000000000000000000000000000000000000000000000000000001000000000000000000000000e63de121ca8fc1c540dbcd432d1c9d3a5cdeb94a000000000000000000000000cd1ef832eb8a5a77842c440032e03c4330974d210000000000000000000000000000000000000000000000000000000000000120000000000000000000000000000000000000000000000000000000000000000c4465616c50726f7669646572000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000010000000000000000000000000000000000000000000000000000000000000000" },
        { 16, "0x00000000000000000000000000000000000000000000000000000000000000200000000000000000000000005b007f06cb9708a1e419fe936ae826c4eba21a8d00000000000000000000000000000000000000000000000000000000000000e000000000000000000000000000000000000000000000000000000000000000100000000000000000000000000000000000000000000000000000000000000000000000000000000000000000e63de121ca8fc1c540dbcd432d1c9d3a5cdeb94a00000000000000000000000043d81a2cf49238484d6960de1df9d430c81cdffc0000000000000000000000000000000000000000000000000000000000000120000000000000000000000000000000000000000000000000000000000000000c4465616c50726f7669646572000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000010000000000000000000000000000000000000000000000000000000000000000" },
        { 17, "0x00000000000000000000000000000000000000000000000000000000000000200000000000000000000000005b007f06cb9708a1e419fe936ae826c4eba21a8d00000000000000000000000000000000000000000000000000000000000000e000000000000000000000000000000000000000000000000000000000000000110000000000000000000000000000000000000000000000000000000000000001000000000000000000000000e63de121ca8fc1c540dbcd432d1c9d3a5cdeb94a000000000000000000000000cd1ef832eb8a5a77842c440032e03c4330974d210000000000000000000000000000000000000000000000000000000000000120000000000000000000000000000000000000000000000000000000000000000c4465616c50726f7669646572000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000010000000000000000000000000000000000000000000000008ac7230489e80000" },
        {123,"0x0000000000000000000000000000000000000000000000000000000000000020000000000000000000000000b9fd557c192939a3889080954d52c64eba8e9be30000000000000000000000000000000000000000000000000000000000000016000000000000000000000000000000000000000000000000000000000000000000000000000000000000000070decfd5e51c59ebdc8aca96bf22da6aff00b17600000000000000000000000043d81a2cf49238484d6960de1df9d430c81cdffc00000000000000000000000000000000000000000000000000000000000000c0000000000000000000000000000000000000000000000000000000000000000200000000000000000000000000000000000000000000000000000000000061a8000000000000000000000000000000000000000000000000000000006507c124" },
    };

    internal static readonly Dictionary<string, string> Names = new()
    {
        { "0x43d81A2cf49238484d6960De1Df9D430c81cdffC".ToLower(),"Token Synthetic" },
        { "0x39f5487929e68CC0242905d1f6e97d0C0992cb77".ToLower(),"LockDealProvider"},
        { "0x5b007F06CB9708A1E419Fe936aE826C4eba21a8d".ToLower(),"DealProvider" },
        { "0x3D0Ca2259046D6660937864b67287269268D2b61".ToLower(),"TimedDealProvider" },
        { "0x9a3188e32Bda4C47491186b7DBd412854A8e3921".ToLower(),"RefundProvider"},
        { "0xcd1ef832eb8a5a77842c440032e03c4330974d21".ToLower(),"Main coin Synthetic"},
        { "0xE63De121ca8Fc1C540DBCd432d1c9D3A5cDEB94A".ToLower(),"CollateralProvider"},
        { "0x70decfd5e51c59ebdc8aca96bf22da6aff00b176","BundleProvider" }
    };

    public static readonly Dictionary<int, string> ExpectedDescription = new()
    {
        { 0, "This NFT represents immediate access to 0.001 units of the specified asset Token Synthetic (TST@0x43d...cdffc)."},
        { 1, "This NFT represents immediate access to 0.001 units of the specified asset Token Synthetic (TST@0x43d...cdffc)."},
        { 2, $"This NFT securely locks 0.000000000000000001 units of the asset Token Synthetic (TST@0x43d...cdffc). Access to these assets will commence on the designated start time of {TimeUtils.FromUnixTimestamp(1701514742)}."},
        { 3, $"This NFT securely locks 10 units of the asset Token Synthetic (TST@0x43d...cdffc). Access to these assets will commence on the designated start time of {TimeUtils.FromUnixTimestamp(1701318745)}."},
        { 4, $"This NFT governs a time-locked pool containing 10/10 units of the asset Token Synthetic (TST@0x43d...cdffc). Withdrawals are permitted in a linear fashion beginning at {TimeUtils.FromUnixTimestamp(1698319845)}, culminating in full access at {TimeUtils.FromUnixTimestamp(1701318745)}."},
        { 5, $"This NFT governs a time-locked pool containing 1000/1000 units of the asset Token Synthetic (TST@0x43d...cdffc). Withdrawals are permitted in a linear fashion beginning at {TimeUtils.FromUnixTimestamp(1698322749)}, culminating in full access at {TimeUtils.FromUnixTimestamp(1701318745)}."},
        { 6, "This NFT encompasses 0 units of the asset Token Synthetic (TST@0x43d...cdffc) with an associated refund rate of 50. Post rate calculation, the refundable amount in the primary asset Main coin Synthetic (TST@0xcd1...74d21) will be 0."},
        { 7, "This NFT represents immediate access to 100 units of the specified asset Main coin Synthetic (TST@0xcd1...74d21)."},
        { 8, $"Exclusively utilized by project administrators, this NFT serves as a secure vault for holding refundable tokens Token Synthetic (TST@0x43d...cdffc), for Main Coin Token Synthetic (TST@0x43d...cdffc). It holds 0 for the main coin collector, 0 for the token collector, and 10 for the main coin holder, valid until {TimeUtils.FromUnixTimestamp(1798325066)}."},
        { 9, "This NFT represents immediate access to 0 units of the specified asset Token Synthetic (TST@0x43d...cdffc)."},
        { 10, "This NFT represents immediate access to 0 units of the specified asset Main coin Synthetic (TST@0xcd1...74d21)."},
        { 11, "This NFT represents immediate access to 10 units of the specified asset Token Synthetic (TST@0x43d...cdffc)."},     
        { 12, "This NFT encompasses 50 units of the asset Token Synthetic (TST@0x43d...cdffc) with an associated refund rate of 50. Post rate calculation, the refundable amount in the primary asset Main coin Synthetic (TST@0xcd1...74d21) will be 2500."},
        { 13, $"This NFT governs a time-locked pool containing 100/100 units of the asset Token Synthetic (TST@0x43d...cdffc). Withdrawals are permitted in a linear fashion beginning at {TimeUtils.FromUnixTimestamp(1698327983)}, culminating in full access at {TimeUtils.FromUnixTimestamp(1708327983)}."},
        { 14, $"Exclusively utilized by project administrators, this NFT serves as a secure vault for holding refundable tokens Main coin Synthetic (TST@0xcd1...74d21), for Main Coin Main coin Synthetic (TST@0xcd1...74d21). It holds 0 for the main coin collector, 0 for the token collector, and 10 for the main coin holder, valid until {TimeUtils.FromUnixTimestamp(1798325066)}."},
        { 15, "This NFT represents immediate access to 0 units of the specified asset Main coin Synthetic (TST@0xcd1...74d21)."},
        { 16, "This NFT represents immediate access to 0 units of the specified asset Token Synthetic (TST@0x43d...cdffc)."},
        { 17, "This NFT represents immediate access to 10 units of the specified asset Main coin Synthetic (TST@0xcd1...74d21)."},
    };

    public static readonly Dictionary<int, List<Erc721Attribute>> ExpectedAttributes = new()
    {
        {0, new()
            {
                new("ProviderName", "DealProvider"),
                new("Collection", 0),
                new("LeftAmount", 0.001)
            }
        },
        {1, new()
            {
                new("ProviderName", "DealProvider"),
                new("Collection", 0),
                new("LeftAmount", 0.001)
            }
        },
        {2, new()
            {
                new("ProviderName", "LockDealProvider"),
                new("StartTime", 1701514742, DisplayType.Date),
                new("Collection", 0),
                new("LeftAmount", 0.000000000000000001)
            }
        },
        {3, new()
            {
                new("ProviderName", "LockDealProvider"),
                new("StartTime", 1701318745, DisplayType.Date),
                new("Collection", 0),
                new("LeftAmount", 10.0)
            }
        },
        {4, new()
            {
                new("ProviderName", "TimedDealProvider"),
                new("StartAmount", 10.0),
                new("FinishTime", 1701318745, DisplayType.Date),
                new("StartTime", 1698319845, DisplayType.Date),
                new("Collection", 0),
                new("LeftAmount", 10.0)
            }
        },
        {5, new()
            {
                new("ProviderName", "TimedDealProvider"),
                new("StartAmount", 1000.0),
                new("FinishTime", 1701318745, DisplayType.Date),
                new("StartTime", 1698322749, DisplayType.Date),
                new("Collection", 0),
                new("LeftAmount", 1000.0)
            }
        },
        {6, new()
            {
                new("ProviderName", "RefundProvider"),
                new("Rate", 50.0),
                new("MainCoinAmount", 0.0),
                new("MainCoinCollection", 1),
                new("SubProviderName", "DealProvider"),
                new("Collection", 0),
                new("LeftAmount", 0.0)
            }
        },
        {7, new()
            {
                new("ProviderName", "DealProvider"),
                new("Collection", 1),
                new("LeftAmount", 100.0)
            }
        },
        {8, new()
            {
                new("ProviderName", "CollateralProvider"),
                new("MainCoinCollection", 0),
                new("Collection", 1),
                new("MainCoinCollectorAmount", 0.0),
                new("TokenCollectorAmount", 0.0), 
                new("MainCoinHolderAmount", 10.0),
                new("FinishTimestamp", 1798325066, DisplayType.Date)
            }
        },
        {9, new()
            {
                new("ProviderName", "DealProvider"),
                new("Collection", 0),
                new("LeftAmount", 0.0)
            }
        },
        {10, new()
            {
                new("ProviderName", "DealProvider"),
                new("Collection", 0),
                new("LeftAmount", 0.0)
            }
        },
        {11, new()
            {
                new("ProviderName", "DealProvider"),
                new("Collection", 0),
                new("LeftAmount", 10.0)
            }
        },
        {12, new()
            {
                new("ProviderName", "RefundProvider"),
                new("Rate", 50.0),
                new("MainCoinAmount", 2500.0),
                new("MainCoinCollection", 1),
                new("SubProviderName", "DealProvider"),
                new("Collection", 0),
                new("LeftAmount", 50.0)
            }
        },
        {13, new()
            {
                new("ProviderName", "TimedDealProvider"),
                new("StartAmount", 100.0),
                new("FinishTime", 1708327983, DisplayType.Date),
                new("StartTime", 1698327983, DisplayType.Date),
                new("Collection", 0),
                new("LeftAmount", 100.0)
            }
        },
        {14, new()
            {
                new("ProviderName", "CollateralProvider"),
                new("MainCoinCollection", 1),
                new("Collection", 0),
                new("MainCoinCollectorAmount", 0.0),
                new("TokenCollectorAmount", 0.0),
                new("MainCoinHolderAmount", 10.0),
                new("FinishTimestamp", 1798325066, DisplayType.Date)
            }
        },
        {15, new()
            {
                new("ProviderName", "DealProvider"),
                new("Collection", 1),
                new("LeftAmount", 0.000000000000000001)
            }
        },
        {16, new()
            {
                new("ProviderName", "DealProvider"),
                new("Collection", 0),
                new("LeftAmount", 0.0)
            }
        },
        {17, new()
            {
                new("ProviderName", "DealProvider"),
                new("Collection", 1),
                new("LeftAmount", 10.0)
            }
        }
    };
}
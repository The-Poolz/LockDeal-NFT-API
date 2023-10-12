﻿using MetaDataAPI.Models.Response;
using System.Numerics;
using MetaDataAPI.Utils;
using MetaDataAPI.Models.Types;

namespace MetaDataAPI.Tests.Helpers;

public static class StaticResults
{
    internal static Dictionary<BigInteger, string> MetaData = new()
    {
        {0,"0x00000000000000000000000000000000000000000000000000000000000000200000000000000000000000006b31be09cf4e2da92f130b1056717fea06176ced00000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000d40e523bcb4230ffa1126e301f4ca0294b8cf18000000000000000000000000043d81a2cf49238484d6960de1df9d430c81cdffc00000000000000000000000000000000000000000000000000000000000000c000000000000000000000000000000000000000000000000000000000000000010000000000000000000000000000000000000000000000000000000000000000"},
        {1,"0x00000000000000000000000000000000000000000000000000000000000000200000000000000000000000006b31be09cf4e2da92f130b1056717fea06176ced000000000000000000000000000000000000000000000000000000000000000100000000000000000000000000000000000000000000000000000000000000000000000000000000000000006063fba0fbd645d648c129854cce45a70dd8969100000000000000000000000043d81a2cf49238484d6960de1df9d430c81cdffc00000000000000000000000000000000000000000000000000000000000000c00000000000000000000000000000000000000000000000000000000000000001000000000000000000000000000000000000000000000002b5e3af16b1880000"},
        {2,"0x0000000000000000000000000000000000000000000000000000000000000020000000000000000000000000b9fd557c192939a3889080954d52c64eba8e9be300000000000000000000000000000000000000000000000000000000000000020000000000000000000000000000000000000000000000000000000000000000000000000000000000000000d40e523bcb4230ffa1126e301f4ca0294b8cf18000000000000000000000000043d81a2cf49238484d6960de1df9d430c81cdffc00000000000000000000000000000000000000000000000000000000000000c0000000000000000000000000000000000000000000000000000000000000000200000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000064f878b2" },
        {3,"0x0000000000000000000000000000000000000000000000000000000000000020000000000000000000000000b9fd557c192939a3889080954d52c64eba8e9be3000000000000000000000000000000000000000000000000000000000000000300000000000000000000000000000000000000000000000000000000000000000000000000000000000000006063fba0fbd645d648c129854cce45a70dd8969100000000000000000000000043d81a2cf49238484d6960de1df9d430c81cdffc00000000000000000000000000000000000000000000000000000000000000c00000000000000000000000000000000000000000000000000000000000000002000000000000000000000000000000000000000000000002b5e3af16b18800000000000000000000000000000000000000000000000000000000000064f878b2" },
        {4,"0x0000000000000000000000000000000000000000000000000000000000000020000000000000000000000000724a076a45ee73544685d4a9fc2240b1c635711e00000000000000000000000000000000000000000000000000000000000000040000000000000000000000000000000000000000000000000000000000000000000000000000000000000000d40e523bcb4230ffa1126e301f4ca0294b8cf18000000000000000000000000043d81a2cf49238484d6960de1df9d430c81cdffc00000000000000000000000000000000000000000000000000000000000000c0000000000000000000000000000000000000000000000000000000000000000400000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000064f87a890000000000000000000000000000000000000000000000000000000064f87a89000000000000000000000000000000000000000000000002b5e3af16b1880000" },
        {5,"0x0000000000000000000000000000000000000000000000000000000000000020000000000000000000000000724a076a45ee73544685d4a9fc2240b1c635711e000000000000000000000000000000000000000000000000000000000000000500000000000000000000000000000000000000000000000000000000000000000000000000000000000000006063fba0fbd645d648c129854cce45a70dd8969100000000000000000000000043d81a2cf49238484d6960de1df9d430c81cdffc00000000000000000000000000000000000000000000000000000000000000c00000000000000000000000000000000000000000000000000000000000000004000000000000000000000000000000000000000000000002b5e3af16b18800000000000000000000000000000000000000000000000000000000000064f87a890000000000000000000000000000000000000000000000000000000064f87a89000000000000000000000000000000000000000000000002b5e3af16b1880000" },
        {6,"0x00000000000000000000000000000000000000000000000000000000000000200000000000000000000000007254a337d05d3965d7d3d8c1a94cd1cfcd1b00d6000000000000000000000000000000000000000000000000000000000000000600000000000000000000000000000000000000000000000000000000000000000000000000000000000000007254a337d05d3965d7d3d8c1a94cd1cfcd1b00d600000000000000000000000043d81a2cf49238484d6960de1df9d430c81cdffc00000000000000000000000000000000000000000000000000000000000000c0000000000000000000000000000000000000000000000000000000000000000300000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000008000000000000000000000000000000000000000000000002b5e3af16b1880000" },
        {7,"0x00000000000000000000000000000000000000000000000000000000000000200000000000000000000000006b31be09cf4e2da92f130b1056717fea06176ced000000000000000000000000000000000000000000000000000000000000000700000000000000000000000000000000000000000000000000000000000000000000000000000000000000007254a337d05d3965d7d3d8c1a94cd1cfcd1b00d600000000000000000000000043d81a2cf49238484d6960de1df9d430c81cdffc00000000000000000000000000000000000000000000000000000000000000c000000000000000000000000000000000000000000000000000000000000000010000000000000000000000000000000000000000000000000000000000000000" },
        {8,"0x00000000000000000000000000000000000000000000000000000000000000200000000000000000000000008bf8cf18c5cb5de075978394624674ba19b96d1b000000000000000000000000000000000000000000000000000000000000000800000000000000000000000000000000000000000000000000000000000000010000000000000000000000006063fba0fbd645d648c129854cce45a70dd89691000000000000000000000000cd1ef832eb8a5a77842c440032e03c4330974d2100000000000000000000000000000000000000000000000000000000000000c00000000000000000000000000000000000000000000000000000000000000002000000000000000000000000000000000000000000000002b5e3af16b1880000000000000000000000000000000000000000000000000000000000006516fea4" },
        {9,"0x00000000000000000000000000000000000000000000000000000000000000200000000000000000000000006b31be09cf4e2da92f130b1056717fea06176ced000000000000000000000000000000000000000000000000000000000000000900000000000000000000000000000000000000000000000000000000000000010000000000000000000000008bf8cf18c5cb5de075978394624674ba19b96d1b000000000000000000000000cd1ef832eb8a5a77842c440032e03c4330974d2100000000000000000000000000000000000000000000000000000000000000c000000000000000000000000000000000000000000000000000000000000000010000000000000000000000000000000000000000000000000000000000000000" },
        {10,"0x00000000000000000000000000000000000000000000000000000000000000200000000000000000000000006b31be09cf4e2da92f130b1056717fea06176ced000000000000000000000000000000000000000000000000000000000000000a00000000000000000000000000000000000000000000000000000000000000000000000000000000000000008bf8cf18c5cb5de075978394624674ba19b96d1b00000000000000000000000043d81a2cf49238484d6960de1df9d430c81cdffc00000000000000000000000000000000000000000000000000000000000000c00000000000000000000000000000000000000000000000000000000000000001000000000000000000000000000000000000000000000002b5e3af16b1880000" },
        {11,"0x00000000000000000000000000000000000000000000000000000000000000200000000000000000000000006b31be09cf4e2da92f130b1056717fea06176ced000000000000000000000000000000000000000000000000000000000000000b00000000000000000000000000000000000000000000000000000000000000010000000000000000000000008bf8cf18c5cb5de075978394624674ba19b96d1b000000000000000000000000cd1ef832eb8a5a77842c440032e03c4330974d2100000000000000000000000000000000000000000000000000000000000000c00000000000000000000000000000000000000000000000000000000000000001000000000000000000000000000000000000000000000002b5e3af16b1880000" },
        {12,"0x00000000000000000000000000000000000000000000000000000000000000200000000000000000000000007254a337d05d3965d7d3d8c1a94cd1cfcd1b00d6000000000000000000000000000000000000000000000000000000000000000c00000000000000000000000000000000000000000000000000000000000000000000000000000000000000006063fba0fbd645d648c129854cce45a70dd8969100000000000000000000000043d81a2cf49238484d6960de1df9d430c81cdffc00000000000000000000000000000000000000000000000000000000000000c00000000000000000000000000000000000000000000000000000000000000003000000000000000000000000000000000000000000000002b5e3af16b18800000000000000000000000000000000000000000000000000000000000000000008000000000000000000000000000000000000000000000002b5e3af16b1880000" },
        {13,"0x00000000000000000000000000000000000000000000000000000000000000200000000000000000000000006b31be09cf4e2da92f130b1056717fea06176ced000000000000000000000000000000000000000000000000000000000000000d00000000000000000000000000000000000000000000000000000000000000000000000000000000000000007254a337d05d3965d7d3d8c1a94cd1cfcd1b00d600000000000000000000000043d81a2cf49238484d6960de1df9d430c81cdffc00000000000000000000000000000000000000000000000000000000000000c00000000000000000000000000000000000000000000000000000000000000001000000000000000000000000000000000000000000000002b5e3af16b1880000" },
        {14,"0x00000000000000000000000000000000000000000000000000000000000000200000000000000000000000007254a337d05d3965d7d3d8c1a94cd1cfcd1b00d6000000000000000000000000000000000000000000000000000000000000000e00000000000000000000000000000000000000000000000000000000000000000000000000000000000000006063fba0fbd645d648c129854cce45a70dd8969100000000000000000000000043d81a2cf49238484d6960de1df9d430c81cdffc00000000000000000000000000000000000000000000000000000000000000c000000000000000000000000000000000000000000000000000000000000000030000000000000000000000000000000000000000000000056bc75e2d631000000000000000000000000000000000000000000000000000000000000000000010000000000000000000000000000000000000000000000002b5e3af16b1880000" },
        {15,"0x00000000000000000000000000000000000000000000000000000000000000200000000000000000000000006b31be09cf4e2da92f130b1056717fea06176ced000000000000000000000000000000000000000000000000000000000000000f00000000000000000000000000000000000000000000000000000000000000000000000000000000000000007254a337d05d3965d7d3d8c1a94cd1cfcd1b00d600000000000000000000000043d81a2cf49238484d6960de1df9d430c81cdffc00000000000000000000000000000000000000000000000000000000000000c000000000000000000000000000000000000000000000000000000000000000010000000000000000000000000000000000000000000000056bc75e2d63100000" },
        {16,"0x00000000000000000000000000000000000000000000000000000000000000200000000000000000000000008bf8cf18c5cb5de075978394624674ba19b96d1b000000000000000000000000000000000000000000000000000000000000001000000000000000000000000000000000000000000000000000000000000000010000000000000000000000006063fba0fbd645d648c129854cce45a70dd89691000000000000000000000000cd1ef832eb8a5a77842c440032e03c4330974d2100000000000000000000000000000000000000000000000000000000000000c000000000000000000000000000000000000000000000000000000000000000020000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000006509744d" },
        {17,"0x00000000000000000000000000000000000000000000000000000000000000200000000000000000000000006b31be09cf4e2da92f130b1056717fea06176ced000000000000000000000000000000000000000000000000000000000000001100000000000000000000000000000000000000000000000000000000000000010000000000000000000000008bf8cf18c5cb5de075978394624674ba19b96d1b000000000000000000000000cd1ef832eb8a5a77842c440032e03c4330974d2100000000000000000000000000000000000000000000000000000000000000c000000000000000000000000000000000000000000000000000000000000000010000000000000000000000000000000000000000000000000000000000000000" },
        {18,"0x00000000000000000000000000000000000000000000000000000000000000200000000000000000000000006b31be09cf4e2da92f130b1056717fea06176ced000000000000000000000000000000000000000000000000000000000000001200000000000000000000000000000000000000000000000000000000000000000000000000000000000000008bf8cf18c5cb5de075978394624674ba19b96d1b00000000000000000000000043d81a2cf49238484d6960de1df9d430c81cdffc00000000000000000000000000000000000000000000000000000000000000c000000000000000000000000000000000000000000000000000000000000000010000000000000000000000000000000000000000000000000000000000000000" },
        {19,"0x00000000000000000000000000000000000000000000000000000000000000200000000000000000000000006b31be09cf4e2da92f130b1056717fea06176ced000000000000000000000000000000000000000000000000000000000000001300000000000000000000000000000000000000000000000000000000000000010000000000000000000000008bf8cf18c5cb5de075978394624674ba19b96d1b000000000000000000000000cd1ef832eb8a5a77842c440032e03c4330974d2100000000000000000000000000000000000000000000000000000000000000c000000000000000000000000000000000000000000000000000000000000000010000000000000000000000000000000000000000000000000000000000000000" },
        {20,"0x000000000000000000000000000000000000000000000000000000000000002000000000000000000000000070decfd5e51c59ebdc8aca96bf22da6aff00b176000000000000000000000000000000000000000000000000000000000000001400000000000000000000000000000000000000000000000000000000000000000000000000000000000000006063fba0fbd645d648c129854cce45a70dd8969100000000000000000000000043d81a2cf49238484d6960de1df9d430c81cdffc00000000000000000000000000000000000000000000000000000000000000c0000000000000000000000000000000000000000000000000000000000000000200000000000000000000000000000000000000000000000000000000000061a80000000000000000000000000000000000000000000000000000000000000016" },
        {21,"0x00000000000000000000000000000000000000000000000000000000000000200000000000000000000000006b31be09cf4e2da92f130b1056717fea06176ced0000000000000000000000000000000000000000000000000000000000000015000000000000000000000000000000000000000000000000000000000000000000000000000000000000000070decfd5e51c59ebdc8aca96bf22da6aff00b17600000000000000000000000043d81a2cf49238484d6960de1df9d430c81cdffc00000000000000000000000000000000000000000000000000000000000000c000000000000000000000000000000000000000000000000000000000000000010000000000000000000000000000000000000000000000000000000000000000" },
        {22,"0x0000000000000000000000000000000000000000000000000000000000000020000000000000000000000000b9fd557c192939a3889080954d52c64eba8e9be30000000000000000000000000000000000000000000000000000000000000016000000000000000000000000000000000000000000000000000000000000000000000000000000000000000070decfd5e51c59ebdc8aca96bf22da6aff00b17600000000000000000000000043d81a2cf49238484d6960de1df9d430c81cdffc00000000000000000000000000000000000000000000000000000000000000c0000000000000000000000000000000000000000000000000000000000000000200000000000000000000000000000000000000000000000000000000000061a8000000000000000000000000000000000000000000000000000000006507c124" },
        {123,"0x0000000000000000000000000000000000000000000000000000000000000020000000000000000000000000b9fd557c192939a3889080954d52c64eba8e9be30000000000000000000000000000000000000000000000000000000000000016000000000000000000000000000000000000000000000000000000000000000000000000000000000000000070decfd5e51c59ebdc8aca96bf22da6aff00b17600000000000000000000000043d81a2cf49238484d6960de1df9d430c81cdffc00000000000000000000000000000000000000000000000000000000000000c0000000000000000000000000000000000000000000000000000000000000000200000000000000000000000000000000000000000000000000000000000061a8000000000000000000000000000000000000000000000000000000006507c124" },
    };

    internal static readonly Dictionary<string, string> Names = new()
    {
        { "0x43d81a2cf49238484d6960de1df9d430c81cdffc","Token Synthetic" },
        { "0xb9fd557c192939a3889080954d52c64eba8e9be3","LockDealProvider"},
        { "0x6b31be09cf4e2da92f130b1056717fea06176ced","DealProvider" },
        { "0x724a076a45ee73544685d4a9fc2240b1c635711e","TimedDealProvider" },
        { "0x7254a337d05d3965d7d3d8c1a94cd1cfcd1b00d6","RefundProvider"},
        { "0xcd1ef832eb8a5a77842c440032e03c4330974d21","Main coin Synthetic"},
        { "0x8bf8cf18c5cb5de075978394624674ba19b96d1b","CollateralProvider"},
        { "0x70decfd5e51c59ebdc8aca96bf22da6aff00b176","BundleProvider" }
    };

    public static readonly Dictionary<int, string> ExpectedDescription = new()
    {
        { 0, "This NFT represents immediate access to 0 units of the specified asset Token Synthetic (TST@0x43d...cdffc)."},
        { 1, "This NFT represents immediate access to 50 units of the specified asset Token Synthetic (TST@0x43d...cdffc)."},
        { 2, $"This NFT securely locks 0 units of the asset Token Synthetic (TST@0x43d...cdffc). Access to these assets will commence on the designated start time of {TimeUtils.FromUnixTimestamp(1694005426)}."},
        { 3, $"This NFT securely locks 50 units of the asset Token Synthetic (TST@0x43d...cdffc). Access to these assets will commence on the designated start time of {TimeUtils.FromUnixTimestamp(1694005426)}."},
        { 4, $"This NFT governs a time-locked pool containing 0/50 units of the asset Token Synthetic (TST@0x43d...cdffc). Withdrawals are permitted in a linear fashion beginning at {TimeUtils.FromUnixTimestamp(1694005897)}, culminating in full access at {TimeUtils.FromUnixTimestamp(1694005897)}."},
        { 5, $"This NFT governs a time-locked pool containing 50/50 units of the asset Token Synthetic (TST@0x43d...cdffc). Withdrawals are permitted in a linear fashion beginning at {TimeUtils.FromUnixTimestamp(1694005897)}, culminating in full access at {TimeUtils.FromUnixTimestamp(1694005897)}."},
        { 6, "This NFT encompasses 0 units of the asset Token Synthetic (TST@0x43d...cdffc) with an associated refund rate of 50. Post rate calculation, the refundable amount in the primary asset Main coin Synthetic (TST@0xcd1...74d21) will be 0."},
        { 7, "This NFT represents immediate access to 0 units of the specified asset Token Synthetic (TST@0x43d...cdffc)."},
        { 8, $"Exclusively utilized by project administrators, this NFT serves as a secure vault for holding refundable tokens Main coin Synthetic (TST@0xcd1...74d21), for Main Coin Main coin Synthetic (TST@0xcd1...74d21). It holds 0 for the main coin collector, 50 for the token collector, and 50 for the main coin holder, valid until {TimeUtils.FromUnixTimestamp(1696005796)}."},
        { 9, "This NFT represents immediate access to 0 units of the specified asset Main coin Synthetic (TST@0xcd1...74d21)."},
        { 10, "This NFT represents immediate access to 50 units of the specified asset Token Synthetic (TST@0x43d...cdffc)."},
        { 11, "This NFT represents immediate access to 50 units of the specified asset Main coin Synthetic (TST@0xcd1...74d21)."},     
        { 12, "This NFT encompasses 50 units of the asset Token Synthetic (TST@0x43d...cdffc) with an associated refund rate of 50. Post rate calculation, the refundable amount in the primary asset Main coin Synthetic (TST@0xcd1...74d21) will be 2500."},
        { 13, "This NFT represents immediate access to 50 units of the specified asset Token Synthetic (TST@0x43d...cdffc)."},
        { 14, "This NFT encompasses 100 units of the asset Token Synthetic (TST@0x43d...cdffc) with an associated refund rate of 50. Post rate calculation, the refundable amount in the primary asset Main coin Synthetic (TST@0xcd1...74d21) will be 5000."},
        { 15, "This NFT represents immediate access to 100 units of the specified asset Token Synthetic (TST@0x43d...cdffc)."},
        { 16, $"Exclusively utilized by project administrators, this NFT serves as a secure vault for holding refundable tokens Main coin Synthetic (TST@0xcd1...74d21), for Main Coin Main coin Synthetic (TST@0xcd1...74d21). It holds 0 for the main coin collector, 0 for the token collector, and 0 for the main coin holder, valid until {TimeUtils.FromUnixTimestamp(1695118413)}."},
        { 17, "This NFT represents immediate access to 0 units of the specified asset Main coin Synthetic (TST@0xcd1...74d21)."},
        { 18, "This NFT represents immediate access to 0 units of the specified asset Token Synthetic (TST@0x43d...cdffc)."},
        { 19, "This NFT represents immediate access to 0 units of the specified asset Main coin Synthetic (TST@0xcd1...74d21)."},
        { 20, "This NFT orchestrates a series of sub-pools to enable sophisticated asset management strategies. The following are the inner pools under its governance that holds total 0.000000000000025 Token Synthetic (TST@0x43d...cdffc):"},
    };

    public static readonly Dictionary<int, List<Erc721Attribute>> ExpectedAttributes = new()
    {
        {0, new()
            {
                new("ProviderName", "DealProvider"),
                new("Collection", 0),
                new("LeftAmount", 0.0)
            }
        },
        {1, new()
            {
                new("ProviderName", "DealProvider"),
                new("Collection", 0),
                new("LeftAmount", 50.0)
            }
        },
        {2, new()
            {
                new("ProviderName", "LockDealProvider"),
                new("StartTime", 1694005426, DisplayType.Date),
                new("Collection", 0),
                new("LeftAmount", 0.0)
            }
        },
        {3, new()
            {
                new("ProviderName", "LockDealProvider"),
                new("StartTime", 1694005426, DisplayType.Date),
                new("Collection", 0),
                new("LeftAmount", 50.0)
            }
        },
        {4, new()
            {
                new("ProviderName", "TimedDealProvider"),
                new("StartAmount", 50.0),
                new("FinishTime", 1694005897, DisplayType.Date),
                new("StartTime", 1694005897, DisplayType.Date),
                new("Collection", 0),
                new("LeftAmount", 0.0)
            }
        },
        {5, new()
            {
                new("ProviderName", "TimedDealProvider"),
                new("StartAmount", 50.0),
                new("FinishTime", 1694005897, DisplayType.Date),
                new("StartTime", 1694005897, DisplayType.Date),
                new("Collection", 0),
                new("LeftAmount", 50.0)
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
                new("Collection", 0),
                new("LeftAmount", 0.0)
            }
        },
        {8, new()
            {
                new("ProviderName", "CollateralProvider"),
                new("MainCoinCollection", 1),
                new("Collection", 0),
                new("MainCoinCollectorAmount", 0.0),
                new("TokenCollectorAmount", 50.0), 
                new("MainCoinHolderAmount", 50.0),
                new("FinishTimestamp", 1696005796, DisplayType.Date)
            }
        },
        {9, new()
            {
                new("ProviderName", "DealProvider"),
                new("Collection", 1),
                new("LeftAmount", 0.0)
            }
        },
        {10, new()
            {
                new("ProviderName", "DealProvider"),
                new("Collection", 0),
                new("LeftAmount", 50.0)
            }
        },
        {11, new()
            {
                new("ProviderName", "DealProvider"),
                new("Collection", 1),
                new("LeftAmount", 50.0)
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
                new("ProviderName", "DealProvider"),
                new("Collection", 0),
                new("LeftAmount", 50.0)
            }
        },
        {14, new()
            {
                new("ProviderName", "RefundProvider"),
                new("Rate", 50.0),
                new("MainCoinAmount", 5000.0),
                new("MainCoinCollection", 1),
                new("SubProviderName", "DealProvider"),
                new("Collection", 0),
                new("LeftAmount", 100.0)
            }
        },
        {15, new()
            {
                new("ProviderName", "DealProvider"),
                new("Collection", 0),
                new("LeftAmount", 100.0)
            }
        },
        {16, new()
            {
                new("ProviderName", "CollateralProvider"),
                new("MainCoinCollection", 1),
                new("Collection", 0),
                new("MainCoinCollectorAmount", 0.0),
                new("TokenCollectorAmount", 0.0),
                new("MainCoinHolderAmount", 0.0),
                new("FinishTimestamp", 1695118413, DisplayType.Date)
            }
        },
        {17, new()
            {
                new("ProviderName", "DealProvider"),
                new("Collection", 1),
                new("LeftAmount", 0.0)
            }
        },
        {18, new()
            {
                new("ProviderName", "DealProvider"),
                new("Collection", 0),
                new("LeftAmount", 0.0)
            }
        },
        {19, new()
            {
                new("ProviderName", "DealProvider"),
                new("Collection", 1),
                new("LeftAmount", 0.0)
            }
        },
        {20, new()
            {
                new("ProviderName", "BundleProvider"),
                new("Collection", 0),
                new("LeftAmount", 0.000000000000025),
                new("ProviderName_21", "DealProvider"),
                new("LeftAmount_21", 0.0),
                new("ProviderName_21", "LockDealProvider"),
                new("StartTime_22", 1695007012, DisplayType.Date),
                new("LeftAmount_22", 0.000000000000025)
            }
        }
    };
}
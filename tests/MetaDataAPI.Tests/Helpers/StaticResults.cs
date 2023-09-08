﻿using System.Numerics;

namespace MetaDataAPI.Tests.Helpers
{
    public static class StaticResults
    {
        public readonly static Dictionary<BigInteger, string> MetaData = new()
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
            {22,"0x0000000000000000000000000000000000000000000000000000000000000020000000000000000000000000b9fd557c192939a3889080954d52c64eba8e9be30000000000000000000000000000000000000000000000000000000000000016000000000000000000000000000000000000000000000000000000000000000000000000000000000000000070decfd5e51c59ebdc8aca96bf22da6aff00b17600000000000000000000000043d81a2cf49238484d6960de1df9d430c81cdffc00000000000000000000000000000000000000000000000000000000000000c0000000000000000000000000000000000000000000000000000000000000000200000000000000000000000000000000000000000000000000000000000061a8000000000000000000000000000000000000000000000000000000006507c124" }
            };
        public static readonly Dictionary<string, string> Names = new()
        {
            { "0x43d81a2cf49238484d6960de1df9d430c81cdffc","Token Synthetic" },
            { "0xb9fd557c192939a3889080954d52c64eba8e9be3","LockDealProvider"},
            { "0x6b31be09cf4e2da92f130b1056717fea06176ced","DealProvider" },
            { "0x724a076a45ee73544685d4a9fc2240b1c635711e","TimedDealProvider" },
            { "0x7254a337d05d3965d7d3d8c1a94cd1cfcd1b00d6","RefundProvider"},
            { "0xcd1ef832eb8a5a77842c440032e03c4330974d21","Main coin Synthetic"},
            { "0x8bf8cf18c5cb5de075978394624674ba19b96d1b","CollateralProvider"},
            {"0x70decfd5e51c59ebdc8aca96bf22da6aff00b176","BundleProvider" }
        };
    }
}

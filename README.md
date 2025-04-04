# LockDeal-NFT-API

The LockDeal-NFT-API repository contains two C# AWS Lambda functions designed to fetch and render NFT (Non-Fungible Token) images via an API.
This is to use with [LockDeal-NFT](https://github.com/The-Poolz/LockDealNFT)

## Overview

1. [MetaDataAPI](https://github.com/The-Poolz/LockDeal-NFT-API/tree/master/src/MetaDataAPI): This function performs an RPC request to the "LockDealNFT" contract to invoke the "getData" method.
It accepts a specific PoolId provided by the user.
The function then parses the received data into ERC721 standard JSON and also stores the specific NFT attributes in a DynamoDB database.

2. [ImageAPI](https://github.com/The-Poolz/LockDeal-NFT-API/tree/master/src/ImageAPI): This function fetches NFT attributes from the DynamoDB database using a hash.
It then returns a generated NFT image with the attributes printed on it.

## API Links

- metadata:
  - https://nft.poolz.finance/metadata?chainId={chainId}&poolId={poolId}
  - https://nft.poolz.finance/metadata/{chainId}/{poolId}
  - example: https://nft.poolz.finance/metadata/56/1

- image: ipfs://{hash}
  - example: [ipfs://bafybeihjurj5l3odrtnutkjwuqr3koobpizhq6l3m4yhqhfzsqebvds7hu](ipfs://bafybeihjurj5l3odrtnutkjwuqr3koobpizhq6l3m4yhqhfzsqebvds7hu)

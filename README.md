# LockDeal-NFT-API

The LockDeal-NFT-API repository contains two C# AWS Lambda functions designed to fetch and render NFT (Non-Fungible Token) images via an API.

## Overview

1. `MetaDataAPI`: This function performs an RPC request to the "LockDealNFT" contract to invoke the "getData" method.
It accepts a specific PoolId provided by the user.
The function then parses the received data into ERC721 standard JSON and also stores the specific NFT attributes in a DynamoDB database.

2. `ImageAPI`: This function fetches NFT attributes from the DynamoDB database using a hash.
It then returns a generated NFT image with the attributes printed on it.

## API Links

- metadata: https://nft.poolz.finance/test/metadata?id=0
- image: https://nft.poolz.finance/test/image?id=HASH_HERE

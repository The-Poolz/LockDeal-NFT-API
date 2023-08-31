# MetaDataAPI

## Overview

This AWS Lambda function is designed to receive metadata from LockDealNFT contract.
It retrieves metadata for various financial pool types and returns the information in JSON format.
The Lambda function categorizes providers into "Advanced" and "Simple" based on the complexity of their logic.

Docs about each provider metadata can found [here](https://github.com/The-Poolz/LockDealNFT/wiki/Meta-Data).

## Functionality

1. The `FunctionHandler` method from `Function` class, handles incoming API Gateway proxy requests.
   - It extracts the `id` from the query string parameter.
   - Retrieves metadata using the `RpcCaller` and processes it using `MetadataParser`.

2. The `ProviderFactory` class:
   - Manages the creation of provider instances based on their names or addresses.
   - Provides a dictionary of provider addresses and their corresponding names.

3. The `AttributesService` class:
   - Retrieves attributes for various provider types based on `poolId` and `decimals`.

4. The "Simple" Providers:
   - `DealProvider`: Retrieves attributes related to deal provider.
   - `LockProvider`: Retrieves attributes for locked provider types.
   - `TimedProvider`: Retrieves attributes for timed provider types.

5. The "Advanced" Providers:
   - `RefundProvider`: Retrieves attributes involving rates, main coins, and tokens.
   - `CollateralProvider`: Retrieves attributes related to collateral pools.
   - `BundleProvider`: Retrieves attributes for bundled provider types.

## Provider Categorization

### Simple Providers

These providers involve straightforward attribute conversions and retrieval:

1. `DealProvider`: Retrieves attributes related to deal provider.
2. `LockProvider`: Retrieves attributes for locked provider types.
3. `TimedProvider`: Retrieves attributes for timed provider types.

### Advanced Providers

These providers involve more complex logic and interaction with various attributes and services:

1. `RefundProvider`: Retrieves refund-related attributes, rates, main coins, and tokens.
2. `CollateralProvider`: Retrieves attributes for collateral pools, including main coins and tokens.
3. `BundleProvider`: Retrieves attributes for bundled provider types, iterating over different IDs.

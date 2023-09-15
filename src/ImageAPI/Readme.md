# ImageAPI

## Overview
The ImageAPI Lambda function is responsible for creating an NFT image based on attributes fetched from a DynamoDB database.
The attributes are fetched using a hash, which is the unique identifier of the NFT.

## Flow
1. `API Invocation`: The client invokes the API by making a GET request to `https://nft.poolz.finance/test/image?id=HASH_HERE`, where `HASH_HERE` is the unique identifier for the NFT attributes.

2. `Input Validation`: The function first validates the provided `hash` from the query string. If it's missing or invalid, a `400 Bad Request` status code is returned.

3. `DynamoDB Query`: The DynamoDb class is used to asynchronously fetch NFT attributes from the DynamoDB table named `MetaDataCache`.
This table is queried using the hash value provided as primary key.

4. `Resource Loading`: The ResourcesLoader class loads the image and font resources required for generating the NFT image. These resources are embedded in the project.

5. `Text Rendering`: The ImageProcessor class takes the fetched attributes and draws them on the loaded image using the loaded font.
Drawing options are created using the `CreateTextOptions` method, which sets the origin, tab width, wrapping length, and alignment of the text.

6. `Image Generation`: The ImageProcessor then generates the final NFT image in PNG format and converts it into a Base64 encoded string.

7. `API Response`: Finally, a `200 OK` status code is returned, with the generated Base64 encoded string in the body. In the event of any error, a `500 Internal Server Error` status code is returned.

## Exception Handling
- 400 Bad Request: Returned if the `hash` parameter in the query string is missing or invalid.

- 500 Internal Server Error: Returned if any exception occurs while processing the request, e.g., failing to fetch data from DynamoDB, issues during image rendering, etc.

## API URL

- https://nft.poolz.finance/test/image?id=HASH_HERE

﻿using Moq;
using Xunit;
using System.Net;
using ImageAPI.Utils;
using FluentAssertions;
using Amazon.DynamoDBv2.Model;
using Amazon.Lambda.APIGatewayEvents;

namespace ImageAPI.Test;

public class LambdaFunctionTests
{
    public static APIGatewayProxyResponse ValidResponse => new()
    {
        IsBase64Encoded = true,
        StatusCode = (int)HttpStatusCode.OK,
        Body = "base64_encoded_image_data",
        Headers = new Dictionary<string, string>
        {
            { "Content-Type", "image/png" }
        }
    };

    public LambdaFunctionTests()
    {
        Environment.SetEnvironmentVariable("AWS_REGION", "us-west-2");
    }

    [Fact]
    internal async Task RunAsync_DynamoDbContainsImage_ShouldReturnResponse()
    {
        var dynamoDb = new Mock<DynamoDb>();
        dynamoDb.Setup(x => x.GetItemAsync("0x1"))
            .ReturnsAsync(new GetItemResponse
            {
                Item = new Dictionary<string, AttributeValue>
                {
                    {
                        "Data", new AttributeValue
                        {
                            S = "[{\"trait_type\":\"ProviderName\",\"value\":\"DealProvider\"},{\"trait_type\":\"Collection\",\"value\":0},{\"trait_type\":\"LeftAmount\",\"value\":50.0}]"
                        }
                    },
                    {
                        "Image", new AttributeValue
                        {
                            S = ValidResponse.Body
                        }
                    }
                }
            });
        var request = new APIGatewayProxyRequest
        {
            QueryStringParameters = new Dictionary<string, string>
            {
                { "hash", "0x1" }
            }
        };

        var result = await new LambdaFunction(dynamoDb.Object).RunAsync(request);

        result.Body.Should().Be(ValidResponse.Body);
        result.Headers.Should().BeEquivalentTo(ValidResponse.Headers);
        result.IsBase64Encoded.Should().Be(ValidResponse.IsBase64Encoded);
        result.StatusCode.Should().Be(ValidResponse.StatusCode);
    }

    [Fact]
    internal async Task RunAsync_BuildImage_ShouldReturnResponse()
    {
        var dynamoDb = new Mock<DynamoDb>();
        dynamoDb.Setup(x => x.GetItemAsync("0x1"))
            .ReturnsAsync(new GetItemResponse
            {
                Item = new Dictionary<string, AttributeValue>
                {
                    {
                        "Data", new AttributeValue
                        {
                            S = "[{\"ProviderName\":\"DealProvider\",\"Attributes\":[{\"trait_type\":\"Collection\",\"value\":2},{\"trait_type\":\"LeftAmount\",\"value\":13572.37461}]}]"
                        }
                    }
                }
            });
        var request = new APIGatewayProxyRequest
        {
            QueryStringParameters = new Dictionary<string, string>
            {
                { "hash", "0x1" }
            }
        };

        var result = await new LambdaFunction(dynamoDb.Object).RunAsync(request);

        result.Body.Should().NotBe(string.Empty);
        result.Headers.Should().BeEquivalentTo(ValidResponse.Headers);
        result.IsBase64Encoded.Should().Be(ValidResponse.IsBase64Encoded);
        result.StatusCode.Should().Be(ValidResponse.StatusCode);
    }

    [Fact]
    internal async Task RunAsync_ShouldReturnResponse_WrongInput()
    {
        var request = new APIGatewayProxyRequest
        {
            QueryStringParameters = new Dictionary<string, string>
            {
                { "not-hash", "value" }
            }
        };

        var result = await new LambdaFunction().RunAsync(request);

        result.Should().BeEquivalentTo(ResponseBuilder.WrongInput());
    }

    [Fact]
    internal async Task RunAsync_ShouldReturnResponse_WrongHash()
    {
        var dynamoDb = new Mock<DynamoDb>();
        dynamoDb.Setup(x => x.GetItemAsync(It.IsAny<string>()))
            .ReturnsAsync(new GetItemResponse
            {
                Item = new Dictionary<string, AttributeValue>()
            });
        var request = new APIGatewayProxyRequest
        {
            QueryStringParameters = new Dictionary<string, string>
            {
                { "hash", "0x1" }
            }
        };

        var result = await new LambdaFunction(dynamoDb.Object).RunAsync(request);

        result.Should().BeEquivalentTo(ResponseBuilder.WrongHash());
    }

    [Fact]
    internal async Task RunAsync_ShouldReturnResponse_GeneralError()
    {
        var dynamoDb = new Mock<DynamoDb>();
        dynamoDb.Setup(x => x.GetItemAsync("0x1"))
            .ReturnsAsync(new GetItemResponse
            {
                Item = new Dictionary<string, AttributeValue>
                {
                    {
                        "Data", new AttributeValue
                        {
                            S = "[{\"trait_type\":\"Collection\",\"value\":0},{\"trait_type\":\"LeftAmount\",\"value\":50.0}]"
                        }
                    }
                }
            });
        var request = new APIGatewayProxyRequest
        {
            QueryStringParameters = new Dictionary<string, string>
            {
                { "hash", "0x1" }
            }
        };

        var result = await new LambdaFunction(dynamoDb.Object).RunAsync(request);

        result.Should().BeEquivalentTo(ResponseBuilder.GeneralError());
    }
}
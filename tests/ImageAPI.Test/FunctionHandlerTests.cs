﻿using Moq;
using Xunit;
using System.Net;
using Amazon.DynamoDBv2.Model;
using ImageAPI.Utils;
using FluentAssertions;
using Amazon.Lambda.APIGatewayEvents;

namespace ImageAPI.Test;

public class FunctionHandlerTests
{
    public static APIGatewayProxyResponse ValidResponse => new()
    {
        IsBase64Encoded = true,
        StatusCode = (int)HttpStatusCode.OK,
        Body = string.Empty,
        Headers = new Dictionary<string, string>
        {
            { "Content-Type", "image/png" }
        }
    };

    public FunctionHandlerTests()
    {
        Environment.SetEnvironmentVariable("AWS_REGION", "us-west-2");
    }

    [Fact]
    internal void FunctionHandler_ShouldReturnResponse_WrongInput()
    {
        var request = new APIGatewayProxyRequest
        {
            QueryStringParameters = new Dictionary<string, string>
            {
                { "not-hash", "value" }
            }
        };

        var result = new LambdaFunction().Run(request);

        result.Should().BeEquivalentTo(ResponseBuilder.WrongInput());
    }

    [Fact]
    internal void FunctionHandler_ShouldReturnResponse_ImageResponse()
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

        var result = new LambdaFunction(dynamoDb.Object).Run(request);

        result.Body.Should().NotBe(ValidResponse.Body);
        result.Headers.Should().BeEquivalentTo(ValidResponse.Headers);
        result.IsBase64Encoded.Should().Be(ValidResponse.IsBase64Encoded);
        result.StatusCode.Should().Be(ValidResponse.StatusCode);
    }

    [Fact]
    internal void FunctionHandler_ShouldReturnResponse_WrongHash()
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

        var result = new LambdaFunction(dynamoDb.Object).Run(request);

        result.Should().BeEquivalentTo(ResponseBuilder.WrongHash());
    }

    [Fact]
    internal void FunctionHandler_ShouldReturnResponse_GeneralError()
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

        var result = new LambdaFunction(dynamoDb.Object).Run(request);

        result.Should().BeEquivalentTo(ResponseBuilder.GeneralError());
    }
}
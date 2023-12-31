﻿using System.Net;
using Amazon.Lambda.APIGatewayEvents;

namespace ImageAPI.Utils;

public static class ResponseBuilder
{
    public const string MissingHashMessage = "Missing 'hash' parameter.";
    public const string ItemNotFoundMessage = "Item by provided 'hash' not found.";
    public const string GeneralErrorMessage = "Something went wrong.";
    public static APIGatewayProxyResponse GetResponse(HttpStatusCode statusCode, string body, string contentType, bool isBase64Encoded) => new()
    {
        IsBase64Encoded = isBase64Encoded,
        StatusCode = (int)statusCode,
        Body = body,
        Headers = new Dictionary<string, string>
        {
            { "Content-Type", contentType }
        }
    };
    public static APIGatewayProxyResponse WrongInput() => GetResponse(HttpStatusCode.BadRequest, MissingHashMessage, "text/plain", false);
    public static APIGatewayProxyResponse WrongHash() => GetResponse(HttpStatusCode.NotFound, ItemNotFoundMessage, "text/plain", false);
    public static APIGatewayProxyResponse GeneralError() => GetResponse(HttpStatusCode.InternalServerError, GeneralErrorMessage, "text/plain", false);
}

using Xunit;
using System.Net;
using Amazon.Lambda.APIGatewayEvents;
using ImageAPI.Utils;

namespace ImageAPI.Test.Utils;

public class ResponseBuilderTests
{
    [Fact]
    public void WrongInput()
    {
        var expectedResponse = new APIGatewayProxyResponse
        {
            StatusCode = (int)HttpStatusCode.BadRequest,
            Body = "Missing or invalid id parameter",
            Headers = new Dictionary<string, string> { { "Content-Type", "text/plain" } }
        };

        var result = ResponseBuilder.WrongInput();

        Assert.Equal(expectedResponse.StatusCode, result.StatusCode);
        Assert.Equal(expectedResponse.Body, result.Body);
        Assert.Equal(expectedResponse.Headers, result.Headers);
    }

    [Fact]
    public void ImageResponse()
    {
        var base64Image = "some string";
        var expectedResponse = new APIGatewayProxyResponse
        {
            IsBase64Encoded = true,
            StatusCode = (int)HttpStatusCode.OK,
            Body = base64Image,
            Headers = new Dictionary<string, string> { { "Content-Type", "image/png" } }
        };

        var result = ResponseBuilder.ImageResponse(base64Image);

        Assert.Equal(expectedResponse.IsBase64Encoded, result.IsBase64Encoded);
        Assert.Equal(expectedResponse.StatusCode, result.StatusCode);
        Assert.Equal(expectedResponse.Body, result.Body);
        Assert.Equal(expectedResponse.Headers, result.Headers);
    }

    [Fact]
    public void GeneralError()
    {
        var expectedResponse = new APIGatewayProxyResponse
        {
            StatusCode = (int)HttpStatusCode.InternalServerError,
            Body = "Something went wrong",
            Headers = new Dictionary<string, string> { { "Content-Type", "text/plain" } }
        };

        var result = ResponseBuilder.GeneralError();

        Assert.Equal(expectedResponse.StatusCode, result.StatusCode);
        Assert.Equal(expectedResponse.Body, result.Body);
        Assert.Equal(expectedResponse.Headers, result.Headers);
    }
}

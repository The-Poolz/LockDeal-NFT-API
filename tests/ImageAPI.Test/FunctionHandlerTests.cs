using Xunit;
using ImageAPI.Utils;
using FluentAssertions;
using Amazon.Lambda.APIGatewayEvents;

namespace ImageAPI.Test;

public class FunctionHandlerTests
{
    [Fact]
    internal async Task FunctionHandler_ShouldReturnResponse_WrongInput()
    {
        var request = new APIGatewayProxyRequest
        {
            QueryStringParameters = new Dictionary<string, string>
            {
                { "id", "not number" }
            }
        };

        var result = await new LambdaFunction().RunAsync(request);

        result.Should().BeEquivalentTo(ResponseBuilder.WrongInput());
    }
}
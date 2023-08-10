using Amazon.Lambda.APIGatewayEvents;
using System.Net;

namespace ImageAPI
{
    public static class ResponseBuilder
    {
        public static APIGatewayProxyResponse WrongInput()
            => new APIGatewayProxyResponse
            {
                StatusCode = (int)HttpStatusCode.BadRequest,
                Body = "Missing or invalid id parameter",
                Headers = new Dictionary<string, string> { { "Content-Type", "text/plain" } }
            };
        public static APIGatewayProxyResponse ImageResponse(string base64Image)
            => new APIGatewayProxyResponse
            {
                IsBase64Encoded = true,
                StatusCode = (int)HttpStatusCode.OK,
                Body = base64Image,
                Headers = new Dictionary<string, string> { { "Content-Type", "image/png" } }
            };
        public static APIGatewayProxyResponse GeneralError()
            => new APIGatewayProxyResponse
            {
                StatusCode = (int)HttpStatusCode.InternalServerError,
                Body = "Something went wrong",
                Headers = new Dictionary<string, string> { { "Content-Type", "text/plain" } }
            };
    }
}

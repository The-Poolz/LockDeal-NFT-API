using GraphQL.Client.Http;
using System.Net.Http.Headers;
using MetaDataAPI.Services.Http;
using EnvironmentManager.Extensions;
using GraphQL.Client.Serializer.Newtonsoft;

namespace MetaDataAPI.Services.Strapi;

public class StrapiGraphQLClient(IHttpClientFactory httpClientFactory) : GraphQLHttpClient(
    endPoint: Endpoint,
    serializer: new NewtonsoftJsonSerializer(),
    httpClient: CreateHttpClient(httpClientFactory)
)
{
    private static readonly string Endpoint = Env.GRAPHQL_STRAPI_URL.GetRequired();

    private static HttpClient CreateHttpClient(IHttpClientFactory httpClientFactory)
    {
        return httpClientFactory.Create(Endpoint, headers =>
        {
            headers.CacheControl = new CacheControlHeaderValue
            {
                NoCache = true,
                NoStore = true,
                MustRevalidate = true
            };
        });
    }
}

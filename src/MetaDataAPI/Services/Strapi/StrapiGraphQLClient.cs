using GraphQL.Client.Http;
using System.Net.Http.Headers;
using MetaDataAPI.Services.Http;
using GraphQL.Client.Abstractions;
using EnvironmentManager.Extensions;
using GraphQL.Client.Serializer.Newtonsoft;

namespace MetaDataAPI.Services.Strapi;

public class StrapiGraphQLClient(IHttpClientFactory httpClientFactory) :  GraphQLHttpClient(
    endPoint: new Uri(Env.GRAPHQL_STRAPI_URL.GetRequired()),
    serializer: new NewtonsoftJsonSerializer(),
    httpClient: httpClientFactory.Create(Env.GRAPHQL_STRAPI_URL.GetRequired(), x => x.CacheControl = new CacheControlHeaderValue
    {
        NoCache = true,
        NoStore = true,
        MustRevalidate = true
    })
), IGraphQLClient
{ }

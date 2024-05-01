using Azure.Core;
using Azure.Identity;
using CallingAPIAADTest.Controllers;
using OpenTelemetry.Trace;
using System.Diagnostics;
using System.Text.Json;

namespace CallingAPIAADTest
{
    public class DownstreamAPIClient : IDownstreamAPIClient
    {
        private readonly ILogger<DownstreamAPIClient> _logger;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly DownstreamAPIOptions _options;

        public DownstreamAPIClient(ILogger<DownstreamAPIClient> logger, IHttpClientFactory httpClientFactory, DownstreamAPIOptions options)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
            _options = options;
        }

        public async Task<CallingAPIResponse> CallDownstreamAPI()
        {
            var accessToken = await GetAccessToken();
            
            var httpClient = _httpClientFactory.CreateClient();
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken.Token}");
            
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, _options.APIEndpointURL);
            var httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);
            _logger.LogInformation("Calling downstream API");

            var activity = Activity.Current;

            try
            {
                throw new Exception("Test exception1");
            }
            // If an exception is thrown, catch it and set the activity status to "Ok".
            catch (Exception ex)
            {
                activity?.SetStatus(ActivityStatusCode.Ok);
                activity?.RecordException(ex);
            }

            var response = await ProcessResponse(httpResponseMessage, accessToken);
            return response;
        }

        private async Task<AccessToken> GetAccessToken()
        {
            DefaultAzureCredential credential = new();
            TokenRequestContext tokenRequestContext = new(new[]
            {
                _options.Audience
            });

            var accessToken = await credential.GetTokenAsync(tokenRequestContext);
            return accessToken;
        }

        private static async Task<CallingAPIResponse> ProcessResponse(HttpResponseMessage httpResponseMessage, AccessToken accessToken)
        {
            var response = new CallingAPIResponse
            {
                API1Response = "Response from API1",
                AuthToken = accessToken.Token
            };

            if (!httpResponseMessage.IsSuccessStatusCode)
            {
                response.API2Response = $"Got a {httpResponseMessage.StatusCode} status code from downstream API";
                return response;
            }

            using var contentStream = await httpResponseMessage.Content.ReadAsStreamAsync();
            var downstreamResponse = await JsonSerializer.DeserializeAsync<CalledAPIResponse>(contentStream);

            if ((downstreamResponse?.API2Response) == null)
            {
                response.API2Response = "Didn't get a response";
                return response;
            }
             
            response.API2Response = downstreamResponse.API2Response;
            return response;
        }
    }
}

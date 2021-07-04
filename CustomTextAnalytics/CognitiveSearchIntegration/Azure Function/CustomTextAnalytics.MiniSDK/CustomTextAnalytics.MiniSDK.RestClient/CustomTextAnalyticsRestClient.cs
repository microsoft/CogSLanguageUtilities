using CustomTextAnalytics.MiniSDK.RestClient.Enums;
using CustomTextAnalytics.MiniSDK.RestClient.Models.AnalyzeApi;
using CustomTextAnalytics.MiniSDK.RestClient.Models.GetJobResultApi;
using CustomTextAnalytics.MiniSDK.RestClient.Pipeline;
using CustomTextAnalytics.MiniSDK.RestClient.Utilities;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using static CustomTextAnalytics.MiniSDK.RestClient.Pipeline.HttpPipeline;

namespace CustomTextUtilities.MiniSDK
{
    public class CustomTextAnalyticsRestClient
    {
        private static readonly HttpPipeline _httpClient = new HttpPipeline();
        private string _endpointUrl;
        private string _serviceKey;
        private readonly string _customTextAnalyticsBaseUrl = "text/analytics/v3.1-preview.ct.1";

        public CustomTextAnalyticsRestClient(string endpointUrl, string serviceKey)
        {
            _endpointUrl = endpointUrl;
            _serviceKey = serviceKey;
        }

        public async Task<string> StartAnalyzeCustomEntitiesAsync(string documentText, string modelId)
        {
            // prepare api data
            var url = string.Format("{0}/{1}/analyze", _endpointUrl, _customTextAnalyticsBaseUrl);
            var headers = new Dictionary<string, string>
            {
                ["Ocp-Apim-Subscription-Key"] = _serviceKey
            };

            var body = new AnalyzeApiRequestBody(documentText, modelId);

            // make network call
            var response = await _httpClient.SendHttpRequestAsync(method: HttpRequestMethod.POST, url: url, urlParameters: null, headers: headers, body);

            // extract job id from header
            var operationLocationHeader = GetHeaderValue(response, "operation-location");
            var jobId = Helpers.ExtractJobIdFromLocationHeader(operationLocationHeader);

            return jobId;
        }

        public async Task<GetJobResultApiResponse> GetAnalyzeJobInfo(string jobId)
        {
            // prepare api data
            var url = string.Format("{0}/{1}/analyze/jobs/{2}", _endpointUrl, _customTextAnalyticsBaseUrl, jobId);
            var headers = new Dictionary<string, string>
            {
                ["Ocp-Apim-Subscription-Key"] = _serviceKey
            };

            // make network call
            var response = await _httpClient.SendHttpRequestAsync<object>(method: HttpRequestMethod.GET, url: url, urlParameters: null, headers: headers);

            // parse result
            var result = JsonConvert.DeserializeObject<GetJobResultApiResponse>(await response.Content.ReadAsStringAsync());

            return result;
        }
    }
}

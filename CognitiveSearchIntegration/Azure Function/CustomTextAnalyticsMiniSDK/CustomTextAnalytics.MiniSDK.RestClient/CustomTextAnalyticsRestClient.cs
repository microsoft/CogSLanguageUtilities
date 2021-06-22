using CustomTextAnalytics.MiniSDK.RestClient.Enums;
using CustomTextAnalytics.MiniSDK.RestClient.Utilities;
using CustomTextUtilities.MiniSDK.Models.AnalyzeCustomEntities;
using CustomTextUtilities.MiniSDK.Models.GetJobResult;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static CustomTextUtilities.MiniSDK.HttpPipeline;

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
            // api data
            var url = string.Format("{0}/{1}/analyze", _endpointUrl, _customTextAnalyticsBaseUrl);
            var headers = new Dictionary<string, string>
            {
                ["Ocp-Apim-Subscription-Key"] = _serviceKey
            };

            var body = new AnalyzeCustomEntitiesRequestBody(documentText, modelId);

            // make network call
            var response = await _httpClient.SendHttpRequestAsync(method: HttpRequestMethod.POST, url: url, urlParameters: null, headers: headers, body);

            var jobId = ValueExtractor.GetJobId(response);

            return jobId;
        }

        public async Task<GetJobResultResponse> GetAnalyzeJobInfo(string jobId)
        {
            // api data
            var url = string.Format("{0}/{1}/analyze/jobs/{2}", _endpointUrl, _customTextAnalyticsBaseUrl, jobId);
            var headers = new Dictionary<string, string>
            {
                ["Ocp-Apim-Subscription-Key"] = _serviceKey
            };

            // make network call
            var response = await _httpClient.SendHttpRequestAsync<object>(method: HttpRequestMethod.GET, url: url, urlParameters: null, headers: headers);

            var result = JsonConvert.DeserializeObject<GetJobResultResponse>(await response.Content.ReadAsStringAsync());

            return result;
        }

        public async Task<bool> WaitForJobUntilDone(string jobId, int maxWaitingTime = 0)
        {
            // prepare algorithm
            var doneValuesList = Enum.GetNames(typeof(JobDoneStatus)).Select(value => value.ToLower());

            var jobDoneStatusSet = new HashSet<string>(doneValuesList);


            var startTimeStamp = DateTime.Now;
            while (true)
            {
                // check job status is done
                var jobInfo = await GetAnalyzeJobInfo(jobId);
                var jobStatus = jobInfo.Status.ToLower();
                if (jobDoneStatusSet.Contains(jobStatus))
                {
                    return true;
                }

                // check for timeouts
                if (maxWaitingTime > 0)
                {
                    var nowTimeStamp = DateTime.Now;
                    var diff = nowTimeStamp.Subtract(startTimeStamp).TotalSeconds;
                    if (diff > maxWaitingTime)
                    {
                        break;
                    }
                }
            }
            return false;
        }
    }
}

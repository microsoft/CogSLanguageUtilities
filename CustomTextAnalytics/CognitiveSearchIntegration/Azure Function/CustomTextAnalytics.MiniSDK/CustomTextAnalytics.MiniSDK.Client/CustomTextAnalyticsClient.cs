using CustomTextAnalytics.MiniSDK.Client.Enums;
using CustomTextAnalytics.MiniSDK.Client.Models;
using CustomTextUtilities.MiniSDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomTextAnalytics.MiniSDK.Client
{
    public class CustomTextAnalyticsClient
    {
        private CustomTextAnalyticsRestClient _restClient;
        public CustomTextAnalyticsClient(string endpointUrl, string serviceKey)
        {
            _restClient = new CustomTextAnalyticsRestClient(endpointUrl, serviceKey);
        }

        public async Task<CustomEntity[]> AnalyzeCustomEntitiesAsync(string documentText, string modelId, int timeoutInSeconds = 0)
        {
            var jobId = await StartAnalyzeCustomEntitiesAsync(documentText, modelId);
            //await WaitForJobUntilDone(jobId, timeoutInSeconds);
            var jobResult = await GetAnalyzeJobInfo(jobId);
            return jobResult.GetResultEntities();
        }

        public async Task<string> StartAnalyzeCustomEntitiesAsync(string documentText, string modelId)
        {
            var jobId = await _restClient.StartAnalyzeCustomEntitiesAsync(documentText, modelId);
            return jobId;
        }

        public async Task<AnalyzeJobInfo> GetAnalyzeJobInfo(string jobId)
        {
            var result = await _restClient.GetAnalyzeJobInfo(jobId);
            return AnalyzeJobInfo.FormGenerated(result);
        }

        public async Task<bool> WaitForJobUntilDone(string jobId, int timeoutInSeconds = 0)
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
                if (timeoutInSeconds > 0)
                {
                    var nowTimeStamp = DateTime.Now;
                    var diff = nowTimeStamp.Subtract(startTimeStamp).TotalSeconds;
                    if (diff > timeoutInSeconds)
                    {
                        break;
                    }
                }
            }
            return false;
        }
    }
}

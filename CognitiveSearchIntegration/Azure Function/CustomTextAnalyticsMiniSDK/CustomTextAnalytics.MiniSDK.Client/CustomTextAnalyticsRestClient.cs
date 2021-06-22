using CustomTextAnalytics.MiniSDK.Client.Models;
using System.Threading.Tasks;

namespace CustomTextUtilities.MiniSDK.Client
{
    public class CustomTextAnalyticsClient
    {
        private CustomTextAnalyticsRestClient _restClient;
        public CustomTextAnalyticsClient(string endpointUrl, string serviceKey)
        {
            _restClient = new CustomTextAnalyticsRestClient(endpointUrl, serviceKey);
        }

        public async Task<ExtractedEntitiesResult> AnalyzeCustomEntitiesAsync(string documentText, string modelId)
        {
            var jobId = await StartAnalyzeCustomEntitiesAsync(documentText, modelId);
            await WaitForJobUntilDone(jobId);
            return await GetAnalyzeJobInfo(jobId);
        }

        public async Task<string> StartAnalyzeCustomEntitiesAsync(string documentText, string modelId)
        {
            var jobId = await _restClient.StartAnalyzeCustomEntitiesAsync(documentText, modelId);
            return jobId;
        }

        public async Task<ExtractedEntitiesResult> GetAnalyzeJobInfo(string jobId)
        {
            var result = await _restClient.GetAnalyzeJobInfo(jobId);
            return ExtractedEntitiesResult.FromResponse(result);
        }

        public async Task WaitForJobUntilDone(string jobId, int maxWaitingTime = 0)
        {
            await _restClient.WaitForJobUntilDone(jobId, maxWaitingTime);
        }
    }
}

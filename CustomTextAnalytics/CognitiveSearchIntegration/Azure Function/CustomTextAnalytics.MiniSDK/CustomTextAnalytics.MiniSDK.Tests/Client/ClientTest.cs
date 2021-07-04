using CustomTextAnalytics.MiniSDK.Client;
using System;
using System.Threading.Tasks;
using Xunit;

namespace CustomTextAnalytics.MiniSDK.Tests.Client
{
    public class ClientTest
    {
        private static CustomTextAnalyticsClient _customTextClient;
        private static readonly string _customTextResourceEndpointUrl = Environment.GetEnvironmentVariable("CustomTextResourceEndpointUrl");
        private static readonly string _customTextResourceKey = Environment.GetEnvironmentVariable("CustomTextResourceKey");
        private static readonly string _customTextModelId = Environment.GetEnvironmentVariable("CustomTextModelId");

        public ClientTest()
        {
            _customTextClient = new CustomTextAnalyticsClient(_customTextResourceEndpointUrl, _customTextResourceKey);
        }
        public static TheoryData AnalyzeCustomEntitiesAsyncTestData()
        {
            var customTextModelId = _customTextModelId;
            var testText = "Capital Call #20 for Berkshire Multifamily Debt Fund II, L.P. (the “Fund” or";

            return new TheoryData<string, string>
            {
                {
                    customTextModelId,
                    testText
                }
            };
        }

        [Theory]
        [MemberData(nameof(AnalyzeCustomEntitiesAsyncTestData))]
        public async Task AnalyzeCustomEntitiesAsyncTestAsync(string modelId, string testText)
        {
            var result = await _customTextClient.AnalyzeCustomEntitiesAsync(testText, modelId);
        }

        public static TheoryData StartAnalyzeCustomEntitiesAsyncTestData()
        {
            var customTextModelId = _customTextModelId;
            var testText = "Capital Call #20 for Berkshire Multifamily Debt Fund II, L.P. (the “Fund” or";

            return new TheoryData<string, string>
            {
                {
                    customTextModelId,
                    testText
                }
            };
        }

        [Theory]
        [MemberData(nameof(StartAnalyzeCustomEntitiesAsyncTestData))]
        public async Task StartAnalyzeCustomEntitiesAsyncTest(string modelId, string testText)
        {
            var result = await _customTextClient.StartAnalyzeCustomEntitiesAsync(testText, modelId);
            Assert.NotNull(result);
        }

        public static TheoryData GetAnalyzeJobInfoAsyncTestData()
        {
            var jobId = "be024ac0-24dc-4f8f-876c-1a9f327f81bc";

            return new TheoryData<string>
            {
                {
                    jobId
                }
            };
        }

        [Theory]
        [MemberData(nameof(GetAnalyzeJobInfoAsyncTestData))]
        public async Task GetAnalyzeJobInfoAsyncTest(string jobId)
        {
            var result = await _customTextClient.GetAnalyzeJobInfo(jobId);
            Assert.NotNull(result.Status);
        }
    }


}

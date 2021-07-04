using CustomTextAnalytics.MiniSDK.Client;
using System.Threading.Tasks;
using Xunit;

namespace CustomTextUtilities.MiniSDK.Tests
{
    public class ClientTest
    {
        public static TheoryData AnalyzeCustomEntitiesAsyncTestData()
        {
            var customTextResourceEndpointUrl = "https://cognitivesearchintegrationcustomtextresource.cognitiveservices.azure.com";
            var customTextResourceKey = "0a6b7248fa794016bcbeed4442fbdbed";
            var customTextModelId = "a0b4c4c0-ec29-418d-967f-0e59a29b0dac_Extraction_latest";
            var testText = "Capital Call #20 for Berkshire Multifamily Debt Fund II, L.P. (the “Fund” or";

            return new TheoryData<string, string, string, string>
            {
                {
                    customTextResourceEndpointUrl,
                    customTextResourceKey,
                    customTextModelId,
                    testText
                }
            };
        }

        [Theory]
        [MemberData(nameof(AnalyzeCustomEntitiesAsyncTestData))]
        public async Task AnalyzeCustomEntitiesAsyncTestAsync(string resourceEndpointUrl, string resourceKey, string modelId, string testText)
        {
            // create client
            var client = new CustomTextAnalyticsClient(resourceEndpointUrl, resourceKey);

            // submit document
            var result = await client.AnalyzeCustomEntitiesAsync(testText, modelId);
        }
    }


}

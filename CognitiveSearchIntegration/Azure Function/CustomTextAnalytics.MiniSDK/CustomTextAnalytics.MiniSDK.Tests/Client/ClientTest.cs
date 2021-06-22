using CustomTextUtilities.MiniSDK.Client;
using System.Threading.Tasks;
using Xunit;

namespace CustomTextUtilities.MiniSDK.Tests
{
    public class ClientTest
    {
        public static TheoryData AnalyzeCustomEntitiesAsyncTestData()
        {
            var resourceEndpointUrl = "https://cognitivesearchintegrationcustomtextresource.cognitiveservices.azure.com";
            var resourceKey = "0a6b7248fa794016bcbeed4442fbdbed";
            var modelId = "a0b4c4c0-ec29-418d-967f-0e59a29b0dac_Extraction_latest";
            return new TheoryData<string, string, string>
            {
                {
                    resourceEndpointUrl,
                    resourceKey,
                    modelId
                }
            };
        }

        [Theory]
        [MemberData(nameof(AnalyzeCustomEntitiesAsyncTestData))]
        public async Task AnalyzeCustomEntitiesAsyncTestAsync(string resourceEndpointUrl, string resourceKey, string modelId)
        {
            // create client
            var client = new CustomTextAnalyticsClient(resourceEndpointUrl, resourceKey);

            // submit document
            var text = "hello world";
            var result = await client.AnalyzeCustomEntitiesAsync(text, modelId);
        }
    }


}

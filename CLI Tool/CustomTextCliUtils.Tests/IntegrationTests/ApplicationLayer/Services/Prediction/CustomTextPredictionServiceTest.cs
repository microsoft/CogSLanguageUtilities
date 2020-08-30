using CustomTextCliUtils.ApplicationLayer.Exceptions;
using CustomTextCliUtils.ApplicationLayer.Exceptions.Prediction;
using CustomTextCliUtils.ApplicationLayer.Services.Prediction;
using CustomTextCliUtils.ApplicationLayer.SystemServices.HttpHandler;
using Xunit;

namespace CustomTextCliUtils.Tests.IntegrationTests.ApplicationLayer.Services.Prediction
{
    public class CustomTextPredictionServiceTest
    {
        // Test Prediction Mapping
        // ######################################################################
        public static TheoryData TestParsingData()
        {
            var httpHandler = new HttpHandler();

            return new TheoryData<string, string, string, IHttpHandler, string, CliException>
            {
                {
                    "cc0c5afc3ddc475e96cbdccd4fceef40",
                    "https://nayergroup.cognitiveservices.azure.com",
                    "5c0df28e-335a-4ff7-8580-91172fd57422",
                    httpHandler,
                    "asdasdasd",
                    null
                },
                {
                    // wrong key
                    "cc0c5afc3ddc471236cbdccd4fceef40",
                    "https://nayergroup.cognitiveservices.azure.com",
                    "5c0df28e-335a-4ff7-8580-91172fd57422",
                    httpHandler,
                    "asdasdasd",
                    new UnauthorizedRequestException("asd", "asd")
                }
            };
        }

        [Theory]
        [MemberData(nameof(TestParsingData))]
        public void TestPrediction(string customTextKey, string endpointUrl, string appId, IHttpHandler httpHandler, string inputText, CliException expectedException)
        {
            /* TEST NOTES
             * *************
             * we only care about prediction result
             * i.e. prediction results maps to our object correctly
             * 
             * we don't care about the actual values in the object
             * because the service provider (in this case CustomText team)
             * may optimize their engine
             * rendering the values in our "ExpectedResult" object in correct
             * */
            // act
            if (expectedException == null)
            {
                var predictionService = new CustomTextPredictionService(httpHandler, customTextKey, endpointUrl, appId);
                var actualResult = predictionService.GetPrediction(inputText);
                // validate object values aren't null
                Assert.NotNull(actualResult.Prediction.PositiveClassifiers);
                Assert.NotNull(actualResult.Prediction.Classifiers);
                Assert.NotNull(actualResult.Prediction.Extractors);
            }
            else
            {
                Assert.Throws(expectedException.GetType(), () => {
                    var predictionService = new CustomTextPredictionService(httpHandler, customTextKey, endpointUrl, appId);
                    predictionService.GetPrediction(inputText);
                });
            }
        }
    }
}

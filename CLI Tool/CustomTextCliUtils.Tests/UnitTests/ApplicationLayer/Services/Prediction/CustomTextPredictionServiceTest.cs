using CustomTextCliUtils.ApplicationLayer.Exceptions;
using CustomTextCliUtils.ApplicationLayer.Exceptions.Prediction;
using CustomTextCliUtils.ApplicationLayer.Services.Prediction;
using CustomTextCliUtils.ApplicationLayer.SystemServices.HttpHandler;
using Moq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Xunit;

namespace CustomTextCliUtils.Tests.UnitTests.ApplicationLayer.Services.Prediction
{
    public class CustomTextPredictionServiceTest
    {
        // Test Prediction Mapping
        // ######################################################################
        public static TheoryData TestParsingData()
        {
            return new TheoryData<string, string, string, System.Net.HttpStatusCode, string, string , CliException>
            {
                {
                    "cc0c5afc3ddc475e96cbdccd4fceef40",
                    "https://nayergroup.cognitiveservices.azure.com",
                    "5c0df28e-335a-4ff7-8580-91172fd57422",
                    System.Net.HttpStatusCode.Accepted,
                    "{   \"prediction\":{      \"positiveClassifiers\":[               ],      \"classifiers\":{         \"MergerArticle\":{            \"score\":0.0531884171         }      },      \"extractors\":{               }   }}",
                    "asdasdasd",
                    null
                },
                {
                    "cc0c5afc3ddc475e96cbdccd4fceef40",
                    "https://nayergroup.cognitiveservices.azure.com",
                    "5c0df28e-335a-4ff7-8580-91172fd57422",
                    System.Net.HttpStatusCode.Unauthorized,
                    "{   \"prediction\":{      \"positiveClassifiers\":[               ],      \"classifiers\":{         \"MergerArticle\":{            \"score\":0.0531884171         }      },      \"extractors\":{               }   }}",
                    "asdasdasd",
                    new UnauthorizedRequestException("asd", "asd")
                }
            };
        }

        [Theory(Skip = "not yet fully mocked!" )]
        [MemberData(nameof(TestParsingData))]
        public void TestPrediction(string customTextKey, string endpointUrl, string appId, System.Net.HttpStatusCode statusCode, string responseContentMsg, string inputText, CliException expectedException)
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

            // arrange
            var mockHttpHandler = new Mock<IHttpHandler>();
            var response = new HttpResponseMessage();
            response.StatusCode = statusCode;
            response.Content = new StringContent(responseContentMsg, Encoding.UTF8, "application/json");
            mockHttpHandler.Setup(handler => handler.SendJsonPostRequest(It.IsAny<string>(),
                It.IsAny<Object>(),
                It.IsAny<Dictionary<string, string>>(),
                It.IsAny<Dictionary<string, string>>()
                )
            ).Returns(response);


            // act
            if (expectedException == null)
            {
                var predictionService = new CustomTextPredictionService(mockHttpHandler.Object, customTextKey, endpointUrl, appId);
                var actualResult = predictionService.GetPrediction(inputText);
                // validate object values aren't null
                Assert.NotNull(actualResult.Prediction.PositiveClassifiers);
                Assert.NotNull(actualResult.Prediction.Classifiers);
                Assert.NotNull(actualResult.Prediction.Extractors);
            }
            else
            {
                Assert.Throws(expectedException.GetType(), () => {
                    var predictionService = new CustomTextPredictionService(mockHttpHandler.Object, customTextKey, endpointUrl, appId);
                    predictionService.GetPrediction(inputText);
                });
            }
        }
    }
}

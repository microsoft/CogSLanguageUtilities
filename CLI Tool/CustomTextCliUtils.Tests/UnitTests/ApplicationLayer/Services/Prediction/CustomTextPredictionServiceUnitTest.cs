﻿using Microsoft.CustomTextCliUtils.ApplicationLayer.Exceptions;
using Microsoft.CustomTextCliUtils.ApplicationLayer.Exceptions.Prediction;
using Microsoft.CustomTextCliUtils.ApplicationLayer.Helpers.HttpHandler;
using Microsoft.CustomTextCliUtils.ApplicationLayer.Modeling.Enums.Prediction;
using Microsoft.CustomTextCliUtils.ApplicationLayer.Modeling.Models.Prediction;
using Microsoft.CustomTextCliUtils.ApplicationLayer.Modeling.Models.Prediction.CustomTextErrorResponse;
using Microsoft.CustomTextCliUtils.ApplicationLayer.Services.Prediction;
using Microsoft.CustomTextCliUtils.Tests.Configs;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using Xunit;

namespace Microsoft.CustomTextCliUtils.Tests.UnitTests.ApplicationLayer.Services.Prediction
{
    public class CustomTextPredictionServiceUnitTest
    {
        // Test Prediction Mapping
        // ######################################################################
        public static TheoryData TestParsingData()
        {
            var customTextKey = Secrets.CustomTextKey;
            var invalidCustomTextKey = "cc0c5afc3ddc123e96cbdcdd4fceef40";
            var invalidAppId = "3b42f28e-335a-8cf7-1310-91172fd57533";
            var customTextEndpoint = Secrets.CustomTextEndpoint;
            var appId = Secrets.CustomTextAppId;
            return new TheoryData<string, string, string, string, CliException>
            {
                {
                    customTextKey,
                    "https://nayergroup.cognitiveservices.azure.com",
                    appId,
                    "asdasdasd",
                    null
                },
                {
                    invalidCustomTextKey,
                    customTextEndpoint,
                    appId,
                    "asdasdasd",
                    new UnauthorizedRequestException("asd", "asd")
                },
                {
                    invalidAppId,
                    customTextEndpoint,
                    appId,
                    "asdasdasd",
                    new ResourceNotFoundExcption(appId)
                },
                {
                    customTextKey,
                    customTextEndpoint,
                    appId,
                    "",
                    new BadRequestException("Bad Request Exception Message")
                }
            };
        }
        [Theory]
        [MemberData(nameof(TestParsingData))]
        public void TestPrediction(string customTextKey, string endpointUrl, string appId, string inputText, CliException expectedException)
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
            // mock post submit prediction request
            mockHttpHandler.Setup(handler => handler.SendJsonPostRequest(
                It.IsAny<string>(),
                It.IsAny<Object>(),
                It.IsAny<Dictionary<string, string>>(),
                It.IsAny<Dictionary<string, string>>()
                )
            ).Returns(GetPredictionRequestHttpResponseMessage(expectedException));
            // mock get operation status 
            mockHttpHandler.Setup(handler => handler.SendGetRequest(
                It.Is<string>(s => s.Contains("status")),
                It.IsAny<Dictionary<string, string>>(),
                It.IsAny<Dictionary<string, string>>()
                )
            ).Returns(GetStatusHttpResponseMessage(expectedException));
            // mock get prediction result
            mockHttpHandler.Setup(handler => handler.SendGetRequest(
                It.Is<string>(s => !s.Contains("status")),
                It.IsAny<Dictionary<string, string>>(),
                It.IsAny<Dictionary<string, string>>()
                )
            ).Returns(GetResultHttpResponseMessage(expectedException));

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
        private HttpResponseMessage GetPredictionRequestHttpResponseMessage(CliException exception)
        {
            var errorResponseContentMsg = JsonConvert.SerializeObject(new CustomTextErrorResponse
            {
                Error = new Error()
            });
            if (exception == null)
            {
                string postResponseContentMsg = JsonConvert.SerializeObject(new CustomTextQueryResponse
                {
                    OperationId = "816fadd6-fe81-458b-b7f8-007fdc270440_637343424000000000",
                    Status = CustomTextPredictionResponseStatus.notstarted
                });
                var postResponse = new HttpResponseMessage();
                postResponse.StatusCode = HttpStatusCode.Accepted;
                postResponse.Content = new StringContent(postResponseContentMsg, Encoding.UTF8, "application/json");
                return postResponse;
            }
            if (exception.GetType().Equals(typeof(UnauthorizedRequestException)))
            {
                var postResponse = new HttpResponseMessage();
                postResponse.StatusCode = HttpStatusCode.Unauthorized;
                postResponse.Content = new StringContent(errorResponseContentMsg, Encoding.UTF8, "application/json");
                return postResponse;
            }
            if (exception.GetType().Equals(typeof(ResourceNotFoundExcption)))
            {
                var postResponse = new HttpResponseMessage();
                postResponse.StatusCode = HttpStatusCode.NotFound;
                postResponse.Content = new StringContent(errorResponseContentMsg, Encoding.UTF8, "application/json");
                return postResponse;
            }
            if (exception.GetType().Equals(typeof(BadRequestException)))
            {
                var postResponse = new HttpResponseMessage();
                postResponse.StatusCode = HttpStatusCode.BadRequest;
                postResponse.Content = new StringContent(errorResponseContentMsg, Encoding.UTF8, "application/json");
                return postResponse;
            }
            return null;
        }
        private HttpResponseMessage GetStatusHttpResponseMessage(CliException exception)
        {
            if (exception == null)
            {
                // get status response
                string getStatusResponseContentMsg = JsonConvert.SerializeObject(new CustomTextQueryResponse
                {
                    OperationId = "816fadd6-fe81-458b-b7f8-007fdc270440_637343424000000000",
                    Status = CustomTextPredictionResponseStatus.succeeded
                });
                var getStatusResponse = new HttpResponseMessage();
                getStatusResponse.StatusCode = HttpStatusCode.OK;
                getStatusResponse.Content = new StringContent(getStatusResponseContentMsg, Encoding.UTF8, "application/json");
                return getStatusResponse;
            }
            return null;
        }
        private HttpResponseMessage GetResultHttpResponseMessage(CliException exception)
        {
            if (exception == null)
            {
                // get result response
                string getResultResponseContentMsg = "{ \"prediction\":{ \"positiveClassifiers\":[ ], \"classifiers\":{ \"MergerArticle\":{ \"score\":0.0531884171 } }, \"extractors\":{ } }}";
                var getResultResponse = new HttpResponseMessage();
                getResultResponse.StatusCode = HttpStatusCode.OK;
                getResultResponse.Content = new StringContent(getResultResponseContentMsg, Encoding.UTF8, "application/json");
                return getResultResponse;
            }
            return null;
        }
    }
}
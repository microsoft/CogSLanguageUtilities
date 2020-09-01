﻿using CustomTextCliUtils.ApplicationLayer.Modeling.Models.Prediction;
using Microsoft.CustomTextCliUtils.ApplicationLayer.Exceptions;
using Microsoft.CustomTextCliUtils.ApplicationLayer.Exceptions.Prediction;
using Microsoft.CustomTextCliUtils.ApplicationLayer.Helpers.HttpHandler;
using Microsoft.CustomTextCliUtils.ApplicationLayer.Modeling.Enums.Prediction;
using Microsoft.CustomTextCliUtils.ApplicationLayer.Modeling.Models.Prediction;
using Microsoft.CustomTextCliUtils.ApplicationLayer.Modeling.Models.Prediction.CustomTextErrorResponse;
using Microsoft.CustomTextCliUtils.ApplicationLayer.Modeling.Models.Prediction.CustomTextResponse;
using Microsoft.CustomTextCliUtils.Configs.Consts;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;

namespace Microsoft.CustomTextCliUtils.ApplicationLayer.Services.Prediction
{
    public class CustomTextPredictionService : IPredictionService
    {
        private readonly string _customTextKey;
        private readonly string _endpointUrl;
        private readonly string _appId;
        private readonly IHttpHandler _httpHandler;

        public CustomTextPredictionService(IHttpHandler httpHandler, string customTextKey, string endpointUrl, string appId)
        {
            _customTextKey = customTextKey;
            _endpointUrl = endpointUrl;
            _appId = appId;
            _httpHandler = httpHandler;
            TestConnection();
        }

        private void TestConnection()
        {
            var testQuery = "test";
            SendPredictionRequest(testQuery);
        }

        public CustomTextPredictionResponse GetPrediction(string query)
        {
            if (query.Length > Constants.CustomTextPredictionMaxCharLimit)
            {
                throw new CustomTextPredictionMaxCharExceededException(query.Length);
            }
            // send prediction request
            var operationId = SendPredictionRequest(query);
            // wait until operation is finished
            CustomTextQueryResponse operationStatus;
            do
            {
                operationStatus = PingStatus(operationId);
            }
            while (operationStatus.Status == CustomTextPredictionResponseStatus.notstarted || operationStatus.Status == CustomTextPredictionResponseStatus.running);
            // get result
            if (operationStatus.Status == CustomTextPredictionResponseStatus.succeeded)
            {
                var prediction = GetResult(operationId);
                return prediction;
            }
            else
            {
                if (string.IsNullOrEmpty(operationStatus.ErrorDetails))
                {
                    throw new PredictionOperationFailedException(operationId);
                }
                throw new PredictionOperationFailedException(operationId, operationStatus.ErrorDetails);
            }
        }

        private string SendPredictionRequest(string queryText)
        {
            var requestUrl = string.Format("{0}/luis/prediction/v4.0-preview/documents/apps/{1}/slots/production/predictText?log=true&%24expand=classifier%2Cextractor", _endpointUrl, _appId);
            var requestBody = new CustomTextQueryRequest(queryText);
            var headers = new Dictionary<string, string>
            {
                ["Ocp-Apim-Subscription-Key"] = _customTextKey
            };
            var response = _httpHandler.SendJsonPostRequest(requestUrl, requestBody, headers, null);
            if (response.StatusCode == HttpStatusCode.Accepted)
            {
                var responseString = response.Content.ReadAsStringAsync().ConfigureAwait(false).GetAwaiter().GetResult();
                var responseContent = JsonConvert.DeserializeObject<CustomTextQueryResponse>(responseString);
                return responseContent.OperationId;
            }
            else
            {
                HandleExceptionResponseCodes(response, requestUrl);
                return null;
            }
        }

        private CustomTextQueryResponse PingStatus(string operationId)
        {
            var requestUrl = string.Format("{0}/luis/prediction/v4.0-preview/documents/apps/{1}/slots/production/operations/{2}/predictText/status", _endpointUrl, _appId, operationId);
            var headers = new Dictionary<string, string>
            {
                ["Ocp-Apim-Subscription-Key"] = _customTextKey
            };
            var response = _httpHandler.SendGetRequest(requestUrl, headers, null);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var responseString = response.Content.ReadAsStringAsync().ConfigureAwait(false).GetAwaiter().GetResult();
                var responseContent = JsonConvert.DeserializeObject<CustomTextQueryResponse>(responseString);
                return responseContent;
            }
            else
            {
                HandleExceptionResponseCodes(response, requestUrl);
                return null;
            }
        }

        private CustomTextPredictionResponse GetResult(string operationId)
        {
            var requestUrl = string.Format("{0}/luis/prediction/v4.0-preview/documents/apps/{1}/slots/production/operations/{2}/predictText", _endpointUrl, _appId, operationId);
            var headers = new Dictionary<string, string>
            {
                ["Ocp-Apim-Subscription-Key"] = _customTextKey
            };
            HttpResponseMessage response = _httpHandler.SendGetRequest(requestUrl, headers, null);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var responseString = response.Content.ReadAsStringAsync().ConfigureAwait(false).GetAwaiter().GetResult();
                return JsonConvert.DeserializeObject<CustomTextPredictionResponse>(responseString);
            }
            else
            {
                HandleExceptionResponseCodes(response, requestUrl);
                return null;
            }
        }

        private void HandleExceptionResponseCodes(HttpResponseMessage response, string url)
        {
            var responseBody = response.Content.ReadAsStringAsync().ConfigureAwait(false).GetAwaiter().GetResult();
            var errorResponse = JsonConvert.DeserializeObject<CustomTextErrorResponse>(responseBody);
            switch (response.StatusCode)
            {
                case HttpStatusCode.Unauthorized:
                    throw new UnauthorizedRequestException(url, _customTextKey);
                case HttpStatusCode.NotFound:
                    throw new ResourceNotFoundExcption(errorResponse.Error.Message);
                case HttpStatusCode.BadRequest:
                    throw new BadRequestException(errorResponse.Error.Message);
                default:
                    throw new CliException(errorResponse.Error.Message);
            }
        }
    }
}

using Microsoft.CogSLanguageUtilities.Definitions.APIs.Helpers.HttpHandler;
using Microsoft.CogSLanguageUtilities.Definitions.APIs.Services;
using Microsoft.CogSLanguageUtilities.Definitions.Models.CustomText.Api.AppModels.Response;
using Microsoft.CogSLanguageUtilities.Definitions.Models.CustomText.Api.LabeledExamples.Response;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Microsoft.CogSLanguageUtilities.Core.Services.CustomText
{
    class CustomTextAuthoringService : ICustomTextAuthoringService
    {
        private readonly string _customTextKey;
        private readonly string _endpointUrl;
        private readonly string _appId;
        private readonly IHttpHandler _httpHandler;

        public CustomTextAuthoringService(IHttpHandler httpHandler, string customTextKey, string endpointUrl, string appId)
        {
            _customTextKey = customTextKey;
            _endpointUrl = endpointUrl;
            _appId = appId;
            _httpHandler = httpHandler;
            TestConnectionAsync().ConfigureAwait(false).GetAwaiter().GetResult();
        }

        public async Task<CustomTextGetLabeledExamplesResponse> GetLabeledExamples()
        {
            var requestUrl = string.Format("{0}/luis/authoring/v4.0-preview/documents/apps/{1}/examples", _endpointUrl, _appId);
            var parameters = new Dictionary<string, string>
            {
                ["enableNestedChildren"] = "true",
            };
            var headers = new Dictionary<string, string>
            {
                ["Ocp-Apim-Subscription-Key"] = _customTextKey
            };
            var response = await _httpHandler.SendGetRequestAsync(requestUrl, headers, parameters);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var responseString = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<CustomTextGetLabeledExamplesResponse>(responseString);
            }
            else
            {
                //await HandleExceptionResponseCodesAsync(response, requestUrl);
                return null;
            }
        }

        public async Task<Dictionary<string, string>> GetModelsDictionary()
        {
            // get application models
            var applicationModels = await GetApplicationModels();

            // map models
            var result = new Dictionary<string, string>();
            AddModelToDictionaryRecursively(result, applicationModels.Models);
            return result;
        }

        private void AddModelToDictionaryRecursively(Dictionary<string, string> modelsDictionary, List<Model> models)
        {
            models.ForEach(m =>
            {
                modelsDictionary.Add(m.Id, m.Name);
                if (m.Children != null)
                {
                    AddModelToDictionaryRecursively(modelsDictionary, m.Children);
                }
            });
        }
        private async Task<CustomTextGetModelsResponse> GetApplicationModels()
        {
            var requestUrl = string.Format("{0}/luis/authoring/v4.0-preview/documents/apps/{1}/models", _endpointUrl, _appId);
            var headers = new Dictionary<string, string>
            {
                ["Ocp-Apim-Subscription-Key"] = _customTextKey
            };
            var response = await _httpHandler.SendGetRequestAsync(requestUrl, headers, null);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var responseString = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<CustomTextGetModelsResponse>(responseString);
            }
            else
            {
                //await HandleExceptionResponseCodesAsync(response, requestUrl);
                return null;
            }
        }

        private async Task TestConnectionAsync()
        {
            var testQuery = "test";
            //await SendPredictionRequestAsync(testQuery);
        }
    }
}

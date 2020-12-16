using Microsoft.Azure.CognitiveServices.Language.LUIS.Runtime;
using Microsoft.Azure.CognitiveServices.Language.LUIS.Runtime.Models;
using Microsoft.CogSLanguageUtilities.Core.Helpers.Mappers.Luis;
using Microsoft.CogSLanguageUtilities.Definitions.APIs.Services;
using Microsoft.CogSLanguageUtilities.Definitions.Models.Luis;
using System;
using System.Threading.Tasks;

namespace Microsoft.CogSLanguageUtilities.Core.Services.Luis
{
    class LuisPredictionService : ILuisPredictionService
    {
        LUISRuntimeClient _client;
        Guid _appId;

        public LuisPredictionService(string endpoint, string key, string appId)
        {
            _appId = Guid.Parse(appId);
            var credentials = new ApiKeyServiceClientCredentials(key);
            _client = new LUISRuntimeClient(credentials) { Endpoint = endpoint };
        }

        public async Task<CustomLuisResponse> Predict(string query)
        {
            var request = new PredictionRequest { Query = query };
            var response = await _client.Prediction.GetSlotPredictionAsync(_appId, "Production", request);
            return LuisOutputMapper.MapToCustomLuisRespone(response);
        }

    }
}

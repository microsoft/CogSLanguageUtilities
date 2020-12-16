using Microsoft.Azure.CognitiveServices.Language.LUIS.Runtime.Models;
using Microsoft.CogSLanguageUtilities.Definitions.Configs.Consts;
using Microsoft.CogSLanguageUtilities.Definitions.Models.IAP;
using Microsoft.CogSLanguageUtilities.Definitions.Models.Luis;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.CogSLanguageUtilities.Core.Helpers.Mappers.Luis
{
    public class LuisOutputMapper
    {
        public static CustomLuisResponse MapToCustomLuisRespone(PredictionResponse luisResponse)
        {
            return new CustomLuisResponse
            {
                Query = luisResponse.Query,
                TopIntent = luisResponse.Prediction.TopIntent,
                Intents = MapLuisPredictionIntents(luisResponse.Prediction.Intents)
            };
        }

        private static Dictionary<string, double?> MapLuisPredictionIntents(IDictionary<string, Intent> intents)
        {
            return intents.ToDictionary(intent => intent.Key, intent => intent.Value.Score);
        }

        private static List<Extraction> MapLuisPredictionEntities(JObject extractors)
        {
            List<Extraction> entities = new List<Extraction>();
            var instance = extractors[Constants.InstanceKey];
            foreach (var entry in extractors)
            {
                if (entry.Key != Constants.InstanceKey)
                {
                    JArray entityArray = (JArray)entry.Value;
                    JArray instanceArray = (JArray)instance[entry.Key];
                    for (int i = 0; i < entityArray.Count; i++)
                    {
                        entities.Add(new Extraction
                        {
                            EntityName = instanceArray[i][Constants.TypeKey].ToString(),
                            Position = instanceArray[i][Constants.StartIndexKey].ToObject<int>(),
                            Children = entityArray[i] is JObject ? MapLuisPredictionEntities((JObject)entityArray[i]) : null
                        });
                    }
                }
            }
            return entities;
        }
    }
}

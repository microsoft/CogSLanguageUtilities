using Microsoft.CogSLanguageUtilities.Definitions.Models.CustomText.PredictionApi.Response.Result;
using Microsoft.LuisModelEvaluation.Models.Input;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.CogSLanguageUtilities.Core.Helpers.Utilities
{
    public class BatchTestingMapper
    {
        private const string instanceKey = "$instance";
        private const string typeKey = "type";
        private const string startIndexKey = "startIndex";
        private const string lengthKey = "length";

        public static List<TestingExample> MapCustomText(
            IDictionary<string, CustomTextPredictionResponse> customTextResponse,
            IDictionary<string, PredictionObject> labeledData)
        {
            List<TestingExample> result = new List<TestingExample>();
            var zipped = customTextResponse.Zip(labeledData, (predicted, labeled) => new { predicted, labeled });
            foreach (var entry in customTextResponse)
            {
                var p = new PredictionObject
                {
                    Classification = entry.Value.Prediction.PositiveClassifiers.FirstOrDefault(),
                    Entities = GetCustomTextEntitiesRecursive(entry.Value.Prediction.Extractors)
                };
                result.Add(new TestingExample
                {
                    PredictedData = p,
                    LabeledData = labeledData[entry.Key]
                });
            }
            return result;
        }

        private static List<Entity> GetCustomTextEntitiesRecursive(JObject extractors)
        {
            List<Entity> entities = new List<Entity>();
            var instance = extractors[instanceKey];
            foreach (var entry in extractors)
            {
                if (entry.Key != instanceKey)
                {
                    JArray entityArray = (JArray)entry.Value;
                    JArray instanceArray = (JArray)instance[entry.Key];
                    for (int i = 0; i < entityArray.Count; i++)
                    {
                        entities.Add(new Entity
                        {
                            Name = instanceArray[i][typeKey].ToString(),
                            StartPosition = instanceArray[i][startIndexKey].ToObject<int>(),
                            EndPosition = instanceArray[i][startIndexKey].ToObject<int>() + instanceArray[i][lengthKey].ToObject<int>(),
                            Children = entityArray[i] is JObject ? GetCustomTextEntitiesRecursive((JObject)entityArray[i]) : null
                        });
                    }
                }
            }
            return entities;
        }
    }
}

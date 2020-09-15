using Microsoft.CogSLanguageUtilities.Definitions.Models.CustomText.Api.LabeledExamples.Response;
using Microsoft.CogSLanguageUtilities.Definitions.Models.CustomText.Api.Prediction.Response.Result;
using Microsoft.LuisModelEvaluation.Models.Input;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.CogSLanguageUtilities.Core.Helpers.Mappers.EvaluationNuget
{
    public class BatchTestingInputMapper
    {
        private const string instanceKey = "$instance";
        private const string typeKey = "type";
        private const string startIndexKey = "startIndex";
        private const string lengthKey = "length";

        public static PredictionObject MapCutomTextResponse(CustomTextPredictionResponse customTextResponse)
        {
            return new PredictionObject
            {
                Classification = customTextResponse.Prediction.PositiveClassifiers.FirstOrDefault(),
                Entities = GetCustomTextEntitiesRecursive(customTextResponse.Prediction.Extractors)
            };
        }

        public static List<Entity> MapCustomTextExamplesToEntitiesRecursively(List<MiniDoc> inputEntities, Dictionary<string, string> modelsDictionary)
        {
            return inputEntities.Select(e =>
            {
                return new Entity
                {
                    Name = modelsDictionary[e.ModelId],
                    StartPosition = e.StartCharIndex,
                    EndPosition = e.EndCharIndex,
                    Children = e.Children != null ? MapCustomTextExamplesToEntitiesRecursively(e.Children, modelsDictionary) : null
                };
            }).ToList();
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

using Microsoft.CogSLanguageUtilities.Definitions.Enums.CustomText;
using Microsoft.CogSLanguageUtilities.Definitions.Models.CustomText.Api.AppModels.Response;
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
                Classification = customTextResponse.Prediction.PositiveClassifiers ?? new List<string>(),
                Entities = GetCustomTextEntitiesRecursive(customTextResponse.Prediction.Extractors)
            };
        }

        public static List<Entity> MapCustomTextMiniDocsToEntitiesRecursively(List<MiniDoc> inputMiniDocs, Dictionary<string, string> modelsDictionary)
        {
            return inputMiniDocs.SelectMany(d => d.PositiveExtractionLabels).Select(e => MapCustomTextExtractionLabelsToEntitiesRecursively(e, modelsDictionary)).ToList();
        }

        public static List<Model> MapCustomTextEntitiesToModelsRecursively(List<CustomTextModel> customTextModels)
        {
            return customTextModels.SelectMany(m =>
            {
                List<Model> models = new List<Model>();
                if (m.TypeId != (int)ModelType.Cl)
                {
                    models.Add(new Model
                    {
                        Name = m.Name,
                        Type = m.ReadableType
                    });
                    if (m.Children?.Any() == true)
                    {
                        models.AddRange(MapCustomTextEntitiesToModelsRecursively(m.Children));
                    }
                }
                return models;
            }).ToList();
        }

        public static List<Model> MapCustomTextClassesToModels(List<CustomTextModel> customTextModels)
        {
            List<Model> models = new List<Model>();
            customTextModels.ForEach(m =>
            {
                if (m.TypeId == (int)ModelType.Cl)
                {
                    models.Add(new Model
                    {
                        Name = m.Name,
                        Type = m.ReadableType
                    });
                }
            });
            return models;
        }

        private static Entity MapCustomTextExtractionLabelsToEntitiesRecursively(PositiveExtractionLabel extractionLabel, Dictionary<string, string> modelsDictionary)
        {
            if (extractionLabel == null)
            {
                return null;
            }
            return new Entity
            {
                Name = modelsDictionary[extractionLabel.ModelId],
                StartPosition = extractionLabel.StartCharIndex,
                EndPosition = extractionLabel.EndCharIndex,
                Children = extractionLabel.Children.Select(c => MapCustomTextExtractionLabelsToEntitiesRecursively(c, modelsDictionary)).ToList()
            };
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

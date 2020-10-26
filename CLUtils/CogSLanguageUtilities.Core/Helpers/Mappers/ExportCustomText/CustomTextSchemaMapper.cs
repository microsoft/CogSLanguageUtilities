using Microsoft.CogSLanguageUtilities.Definitions.Enums.CustomText;
using Microsoft.CogSLanguageUtilities.Definitions.Models.CustomText.Api.AppModels.Response;
using Microsoft.CogSLanguageUtilities.Definitions.Models.CustomText.Api.LabeledExamples.Response;
using Microsoft.CogSLanguageUtilities.Definitions.Models.CustomText.Schema;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.CogSLanguageUtilities.Core.Helpers.Mappers.ExportCustomText
{
    public class CustomTextSchemaMapper
    {
        private static List<CustomTextSchemaModel> GetClassifiersFromCustomTextModels(List<CustomTextModel> customTextModels)
        {
            List<CustomTextSchemaModel> classifiers = new List<CustomTextSchemaModel>();
            customTextModels.ForEach(m =>
            {
                if (m.TypeId == ModelType.Cl)
                {
                    classifiers.Add(new CustomTextSchemaModel
                    {
                        Id = m.Id,
                        Name = m.Name,
                        TypeId = m.TypeId,
                        ReadableType = m.ReadableType
                    });
                }
            });
            return classifiers;
        }

        private static List<CustomTextSchemaModel> GetExtractorsFromCustomTextModelsRecursively(List<CustomTextModel> customTextModels)
        {
            if (customTextModels == null || customTextModels.Count == 0)
            {
                return null;
            }
            List<CustomTextSchemaModel> extractors = new List<CustomTextSchemaModel>();
            customTextModels.ForEach(m =>
            {
                if (m.TypeId != ModelType.Cl)
                {
                    extractors.Add(new CustomTextSchemaModel
                    {
                        Id = m.Id,
                        Name = m.Name,
                        TypeId = m.TypeId,
                        ReadableType = m.ReadableType,
                        Children = GetExtractorsFromCustomTextModelsRecursively(m.Children)
                    });
                }
            });
            return extractors;
        }

        public static CustomTextSchema MapCustomTextModelsToSchema(List<CustomTextModel> models, List<CustomTextExample> examples)
        {
            return new CustomTextSchema
            {
                Classifiers = GetClassifiersFromCustomTextModels(models),
                Extractors = GetExtractorsFromCustomTextModelsRecursively(models),
                Examples = GetExamplesFromCustomTextLabeledExamples(examples)
            };
        }

        private static List<CustomTextSchemaExample> GetExamplesFromCustomTextLabeledExamples(List<CustomTextExample> examples)
        {

            return null;
        }
    }
}

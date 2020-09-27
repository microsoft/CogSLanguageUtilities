using Microsoft.CogSLanguageUtilities.Definitions.Enums.CustomText;
using Microsoft.CogSLanguageUtilities.Definitions.Models.CustomText.Api.AppModels.Response;
using Microsoft.CogSLanguageUtilities.Definitions.Models.CustomText.Schema;
using System.Collections.Generic;

namespace Microsoft.CogSLanguageUtilities.Core.Helpers.Mappers.ExportCustomText
{
    public class CustomTextSchemaMapper
    {
        private static List<CustomTextSchemaModel> GetClassifiersFromCustomTextModels(List<CustomTextModel> customTextModels)
        {
            List<CustomTextSchemaModel> classifiers = new List<CustomTextSchemaModel>();
            customTextModels.ForEach(m =>
            {
                if (m.TypeId == (int)ModelType.Cl)
                {
                    classifiers.Add(new CustomTextSchemaModel
                    {
                        Name = m.Name,
                        Type = m.ReadableType
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
                if (m.TypeId != (int)ModelType.Cl)
                {
                    extractors.Add(new CustomTextSchemaModel
                    {
                        Name = m.Name,
                        Type = m.ReadableType,
                        Children = GetExtractorsFromCustomTextModelsRecursively(m.Children)
                    });
                }
            });
            return extractors;
        }

        public static CustomTextSchema MapCustomTextModelsToSchema(List<CustomTextModel> models)
        {
            return new CustomTextSchema
            {
                Classifiers = GetClassifiersFromCustomTextModels(models),
                Extractors = GetExtractorsFromCustomTextModelsRecursively(models)
            };
        }
    }
}

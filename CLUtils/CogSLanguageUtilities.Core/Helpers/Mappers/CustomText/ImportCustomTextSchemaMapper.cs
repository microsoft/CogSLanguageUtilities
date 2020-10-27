// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
using Microsoft.CogSLanguageUtilities.Definitions.Models.CustomText.Api.AppModels.Response;
using Microsoft.CogSLanguageUtilities.Definitions.Models.CustomText.Api.LabeledExamples.Response;
using Microsoft.CogSLanguageUtilities.Definitions.Models.CustomText.Schema;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.CogSLanguageUtilities.Core.Helpers.Mappers.CustomText
{
    public class ImportCustomTextSchemaMapper
    {
        public static CustomTextModel MapClassifier(CustomTextSchemaModel classifier)
        {
            return new CustomTextModel
            {
                Name = classifier.Name,
                Description = classifier.Description
            };
        }

        public static CustomTextModel MapExtractor(CustomTextSchemaModel extractor)
        {
            return new CustomTextModel
            {
                Name = extractor.Name,
                Description = extractor.Description,
                Children = MapExtractorChildren(extractor.Children)
            };
        }

        private static List<CustomTextModel> MapExtractorChildren(List<CustomTextSchemaModel> customTextModels)
        {
            if (customTextModels == null || customTextModels.Count == 0)
            {
                return null;
            }
            List<CustomTextModel> extractors = new List<CustomTextModel>();
            customTextModels.ForEach(m =>
            {
                extractors.Add(new CustomTextModel
                {
                    Id = m.Id,
                    Name = m.Name,
                    Description = m.Description,
                    TypeId = m.TypeId,
                    ReadableType = m.ReadableType,
                    Children = MapExtractorChildren(m.Children)
                });
            });
            return extractors;
        }

        public static MiniDoc MapMiniDoc(CustomTextSchemaMiniDoc miniDoc)
        {
            return new MiniDoc
            {
                EndCharIndex = miniDoc.EndCharIndex,
                ModelId = miniDoc.ModelId,
                PositiveExtractionLabels = MapPositiveExtractionLabels(miniDoc.PositiveExtractionLabels),
                StartCharIndex = miniDoc.StartCharIndex
            };
        }

        private static List<PositiveExtractionLabel> MapPositiveExtractionLabels(List<CustomTextSchemaPositiveExtractionLabel> positiveExtractionLabels)
        {
            if (positiveExtractionLabels == null || positiveExtractionLabels.Count == 0)
            {
                return null;
            }
            return positiveExtractionLabels.Select(p =>
            new PositiveExtractionLabel
            {
                EndCharIndex = p.EndCharIndex,
                ModelId = p.ModelId,
                StartCharIndex = p.StartCharIndex,
                Children = MapPositiveExtractionLabels(p.Children)
            }).ToList();
        }
    }
}

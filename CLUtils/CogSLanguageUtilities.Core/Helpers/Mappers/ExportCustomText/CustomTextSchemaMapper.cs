// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
using Microsoft.CogSLanguageUtilities.Definitions.Enums.CustomText;
using Microsoft.CogSLanguageUtilities.Definitions.Models.CustomText.Api.AppModels.Response;
using Microsoft.CogSLanguageUtilities.Definitions.Models.CustomText.Api.LabeledExamples.Response;
using Microsoft.CogSLanguageUtilities.Definitions.Models.CustomText.Schema;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.CogSLanguageUtilities.Core.Helpers.Mappers.ExportCustomText
{
    public class CustomTextSchemaMapper
    {
        public static CustomTextSchema MapCustomTextModelsToSchema(List<CustomTextModel> models, List<CustomTextExample> examples)
        {
            return new CustomTextSchema
            {
                Classifiers = GetClassifiersFromCustomTextModels(models),
                Extractors = GetExtractorsFromCustomTextModelsRecursively(models),
                Examples = GetExamplesFromCustomTextLabeledExamples(examples)
            };
        }

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
                        Description = m.Description,
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
                        Description = m.Description,
                        TypeId = m.TypeId,
                        ReadableType = m.ReadableType,
                        Children = GetExtractorsFromCustomTextModelsRecursively(m.Children)
                    });
                }
            });
            return extractors;
        }

        private static List<CustomTextSchemaExample> GetExamplesFromCustomTextLabeledExamples(List<CustomTextExample> examples)
        {
            return examples.Select(e => new CustomTextSchemaExample
            {
                Document = MapDocumentToSchema(e.Document),
                ClassificationLabels = MapClassificationLabelsToSchema(e.ClassificationLabels),
                MiniDocs = MapMiniDocsToSchema(e.MiniDocs)
            }).ToList();
        }

        private static List<CustomTextSchemaMiniDoc> MapMiniDocsToSchema(List<MiniDoc> miniDocs)
        {
            return miniDocs.Select(d =>
                new CustomTextSchemaMiniDoc
                {
                    EndCharIndex = d.EndCharIndex,
                    ModelId = d.ModelId,
                    PositiveExtractionLabels = MapPositiveExtractionLabelstoSchema(d.PositiveExtractionLabels),
                    StartCharIndex = d.StartCharIndex
                }).ToList();
        }

        private static List<CustomTextSchemaPositiveExtractionLabel> MapPositiveExtractionLabelstoSchema(List<PositiveExtractionLabel> positiveExtractionLabels)
        {
            if (positiveExtractionLabels == null || positiveExtractionLabels.Count == 0)
            {
                return null;
            }
            return positiveExtractionLabels.Select(p =>
            new CustomTextSchemaPositiveExtractionLabel
            {
                EndCharIndex = p.EndCharIndex,
                ModelId = p.ModelId,
                StartCharIndex = p.StartCharIndex,
                Children = MapPositiveExtractionLabelstoSchema(p.Children)
            }).ToList();

        }

        private static CustomTextSchemaDocument MapDocumentToSchema(Document document)
        {
            return new CustomTextSchemaDocument
            {
                DocumentId = document.DocumentId,
                ExampleId = document.ExampleId,
                LastModifiedTimestamp = document.LastModifiedTimestamp
            };
        }

        private static List<CustomTextSchemaClassificationLabel> MapClassificationLabelsToSchema(List<ClassificationLabel> labels)
        {
            return labels.Select(l =>
            {
                return new CustomTextSchemaClassificationLabel
                {
                    Label = l.Label,
                    ModelId = l.ModelId
                };
            }).ToList();
        }
    }
}

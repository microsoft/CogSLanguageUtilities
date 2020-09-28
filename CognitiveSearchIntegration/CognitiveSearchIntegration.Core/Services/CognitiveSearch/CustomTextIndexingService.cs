using Azure.Search.Documents.Indexes.Models;
using Microsoft.CognitiveSearchIntegration.Definitions.APIs.Services;
using Microsoft.CognitiveSearchIntegration.Definitions.Consts;
using Microsoft.CognitiveSearchIntegration.Definitions.Models.CognitiveSearch.Schema;
using Microsoft.CognitiveSearchIntegration.Definitions.Models.CustomText.Schema;
using System.Collections.Generic;

namespace Microsoft.CognitiveSearchIntegration.Core.Services.CognitiveSearch
{
    public class CustomTextIndexingService : ICustomTextIndexingService
    {
        public CustomSkillSchema CreateCustomSkillSchema(CustomTextSchema schema, string indexName)
        {
            CustomSkillSchema customSkillSchema = new CustomSkillSchema()
            {
                name = indexName + Constants.SkillSetSuffix
            };
            List<Output> outputs = new List<Output>();

            Output classifierOutput = new Output()
            {
                name = "Classes",
                targetName = "Classes"
            };

            outputs.Add(classifierOutput);

            foreach (CustomTextSchemaModel model in schema.Extractors)
            {
                //skillset
                Output output = new Output()
                {
                    name = model.Name,
                    targetName = model.Name
                };

                outputs.Add(output);
            }
            customSkillSchema.outputs = outputs;
            return customSkillSchema;
        }

        public SearchIndex CreateIndex(CustomTextSchema schema, string indexName)
        {
            List<SearchField> indexFields = new List<SearchField>();

            //classifiers
            SearchField classifierIndexField = new SearchField("Classes", SearchFieldDataType.Collection(SearchFieldDataType.String));

            indexFields.Add(classifierIndexField);

            // extractors
            foreach (CustomTextSchemaModel model in schema.Extractors)
            {
                SearchField indexField = new SearchField(
                    model.Name,
                    model.Children == null || model.Children.Count == 0 ? SearchFieldDataType.Collection(SearchFieldDataType.String) : SearchFieldDataType.Collection(SearchFieldDataType.Complex));

                ExploreChildren(model.Children, indexField.Fields);

                indexFields.Add(indexField);
            }

            return new SearchIndex(indexName)
            {
                Fields = indexFields
            };
        }

        public SearchIndexer CreateIndexer(CustomTextSchema schema, string indexName)
        {
            var indexer = new SearchIndexer(indexName + Constants.IndexerSuffix, indexName + Constants.DataSourceSuffix, indexName)
            {
                SkillsetName = indexName + Constants.SkillSetSuffix
            };

            //classifiers
            indexer.OutputFieldMappings.Add(new FieldMapping("/document/content/Classes")
            {
                TargetFieldName = "Classes"
            });

            // extractors
            foreach (CustomTextSchemaModel model in schema.Extractors)
            {
                indexer.OutputFieldMappings.Add(new FieldMapping($"/document/content/{model.Name}")
                {
                    TargetFieldName = model.Name
                });

            }
            return indexer;
        }

        private void ExploreChildren(List<CustomTextSchemaModel> children, IList<SearchField> fields)
        {
            if (children != null)
            {
                foreach (CustomTextSchemaModel child in children)
                {
                    SearchField field = new SearchField(
                        child.Name,
                        child.Children == null || child.Children.Count == 0 ? SearchFieldDataType.String : SearchFieldDataType.Complex)
                    {
                        IsFacetable = true,
                        IsFilterable = true,
                        IsSearchable = true
                    };

                    ExploreChildren(child.Children, field.Fields);

                    fields.Add(field);
                }
            }
        }
    }
}

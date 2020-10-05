using Azure.Search.Documents.Indexes.Models;
using Microsoft.CognitiveSearchIntegration.Definitions.APIs.Services;
using Microsoft.CognitiveSearchIntegration.Definitions.Models.CognitiveSearch.Api;
using Microsoft.CognitiveSearchIntegration.Definitions.Models.CognitiveSearch.Api.Indexer;
using Microsoft.CognitiveSearchIntegration.Definitions.Models.CustomText.Schema;
using System.Collections.Generic;

namespace Microsoft.CognitiveSearchIntegration.Core.Services.CognitiveSearch
{
    public class CustomTextIndexingService : ICustomTextIndexingService
    {
        public SkillSet CreateSkillSetSchema(CustomTextSchema schema, string skillSetName, string customTextSkillName, string azureFunctionUrl)
        {
            CustomSkillSchema customSkillSchema = new CustomSkillSchema()
            {
                name = customTextSkillName,
                uri = azureFunctionUrl
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
                Output output = new Output()
                {
                    name = model.Name,
                    targetName = model.Name
                };
                outputs.Add(output);
            }
            customSkillSchema.outputs = outputs;
            return new SkillSet
            {
                Name = skillSetName,
                Description = "Custom Text Skillset",
                Skills = new List<CustomSkillSchema> { customSkillSchema }
            };
        }

        public SearchIndex CreateIndex(CustomTextSchema schema, string indexName)
        {
            List<SearchField> indexFields = new List<SearchField>();

            // id
            indexFields.Add(new SearchField("id", SearchFieldDataType.String)
            {
                IsKey = true
            });

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

        public Indexer CreateIndexer(CustomTextSchema schema, string indexerName, string dataSourceName, string skillSetName, string indexName)
        {
            var outputFieldMappings = new List<IndexerFieldMapping>();
            //classifiers
            outputFieldMappings.Add(new IndexerFieldMapping
            {
                SourceFieldName = "/document/content/Classes",
                TargetFieldName = "Classes"
            });

            // extractors
            foreach (CustomTextSchemaModel model in schema.Extractors)
            {
                outputFieldMappings.Add(new IndexerFieldMapping
                {
                    SourceFieldName = $"/document/content/{model.Name}",
                    TargetFieldName = model.Name
                });
            }

            List<IndexerFieldMapping> fieldMappings = new List<IndexerFieldMapping>
            {
                new IndexerFieldMapping
                {
                    SourceFieldName = "metadata_storage_name",
                    TargetFieldName = "id",
                    MappingFunction = new MappingFunction
                    {
                        Name = "base64Encode"
                    }
                }
            };

            var indexerParameters = new IndexerParameters
            {
                Configuration = new IndexerConfiguration
                {
                    IndexedFileNameExtensions = ".txt"
                }
            };

            var indexer = new Indexer
            {
                Name = indexerName,
                DataSourceName = dataSourceName,
                TargetIndexName = indexName,
                SkillsetName = skillSetName,
                FieldMappings = fieldMappings,
                OutputFieldMappings = outputFieldMappings,
                Parameters = indexerParameters
            };
            return indexer;
        }

        private void ExploreChildren(List<CustomTextSchemaModel> children, IList<SearchField> fields)
        {
            if (children != null)
            {
                foreach (CustomTextSchemaModel child in children)
                {
                    var hasChildren = child.Children != null && child.Children.Count > 0;
                    SearchField field = hasChildren ? new SearchField(child.Name, SearchFieldDataType.Collection(SearchFieldDataType.Complex)) :
                        new SearchField(child.Name, SearchFieldDataType.Collection(SearchFieldDataType.String))
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

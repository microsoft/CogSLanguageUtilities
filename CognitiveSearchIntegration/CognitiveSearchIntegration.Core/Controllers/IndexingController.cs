using Microsoft.CognitiveSearchIntegration.Definitions.APIs.Services;
using Microsoft.CognitiveSearchIntegration.Definitions.Consts;
using Microsoft.CognitiveSearchIntegration.Definitions.Enums.Logger;
using Microsoft.CognitiveSearchIntegration.Definitions.Models.CognitiveSearch.Indexer;
using Microsoft.CognitiveSearchIntegration.Definitions.Models.CustomText.Schema;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Microsoft.CognitiveSearchIntegration.Core.Controllers
{
    public class IndexingController
    {
        private IStorageService _storageService;
        private ICustomTextIndexingService _customTextIndexingService;
        private ICognitiveSearchService _cognitiveSearchService;
        private ILoggerService _loggerService;
        private IndexerConfigs _indexerConfigs;

        public IndexingController(
            IStorageService storageService,
            ICustomTextIndexingService customTextIndexingService,
            ICognitiveSearchService cognitiveSearchService,
            ILoggerService loggerService,
            IndexerConfigs indexerConfigs)
        {
            _storageService = storageService;
            _customTextIndexingService = customTextIndexingService;
            _cognitiveSearchService = cognitiveSearchService;
            _loggerService = loggerService;
            _indexerConfigs = indexerConfigs;
        }
        public async Task IndexCustomText(string schemaPath, string indexName)
        {
            // initialize resource names
            var dataSourceName = indexName.ToLower() + Constants.DataSourceSuffix;
            var indexerName = indexName.ToLower() + Constants.IndexerSuffix;
            var skillSetName = indexName.ToLower() + Constants.SkillsetSuffix;
            var customTextSkillName = indexName.ToLower() + Constants.CustomSkillSuffix;
            var schemaFileName = Path.GetFileName(schemaPath);

            try
            {
                // read schema
                _loggerService.LogOperation(OperationType.ReadingSchema, schemaFileName);
                var schemaString = await _storageService.ReadAsStringFromAbsolutePathAsync(schemaPath);
                var schema = JsonConvert.DeserializeObject<CustomTextSchema>(schemaString);

                // create models (index & skillset)
                var searchIndex = _customTextIndexingService.CreateIndex(schema, indexName);
                var customSkill = _customTextIndexingService.CreateSkillSetSchema(schema, skillSetName, customTextSkillName, _indexerConfigs.AzureFunctionUrl);
                var searchIndexer = _customTextIndexingService.CreateIndexer(schema, indexerName, dataSourceName, skillSetName, indexName);

                // indexing pipeline
                _loggerService.LogOperation(OperationType.CreateDataSource, $"{dataSourceName}");
                await _cognitiveSearchService.CreateDataSourceConnectionAsync(indexName, _indexerConfigs.DataSourceContainerName, _indexerConfigs.DataSourceConnectionString);

                _loggerService.LogOperation(OperationType.CreatingSearchIndex, $"{indexerName}");
                await _cognitiveSearchService.CreateIndexAsync(searchIndex);

                _loggerService.LogOperation(OperationType.CreatingSkillSet, $"{skillSetName}");
                await _cognitiveSearchService.CreateSkillSetAsync(customSkill);

                _loggerService.LogOperation(OperationType.CreatingIndexer, $"{indexerName}");
                await _cognitiveSearchService.CreateIndexerAsync(searchIndexer);

                // log success message
                _loggerService.LogSuccessMessage("Indexing Application Was Successfull!");
            }
            catch (Exception e)
            {
                // TODOs: logger should take in an exception in order to make message clear to why it failed
                _loggerService.LogError("Indexing Operation Failed!");
            }
        }
    }
}

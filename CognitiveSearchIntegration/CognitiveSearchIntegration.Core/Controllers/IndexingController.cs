using Microsoft.CognitiveSearchIntegration.Definitions.APIs.Services;
using Microsoft.CognitiveSearchIntegration.Definitions.Models.CognitiveSearch.Indexer;
using Microsoft.CognitiveSearchIntegration.Definitions.Models.CustomText.Schema;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace Microsoft.CognitiveSearchIntegration.Core.Controllers
{
    public class IndexingController
    {
        private IStorageService _storageService;
        private ICustomTextIndexingService _customTextIndexingService;
        private ICognitiveSearchService _cognitiveSearchService;
        private IndexerConfigs _indexerConfigs;

        public IndexingController(
            IStorageService storageService,
            ICustomTextIndexingService customTextIndexingService,
            ICognitiveSearchService cognitiveSearchService,
            IndexerConfigs indexerConfigs)
        {
            _storageService = storageService;
            _customTextIndexingService = customTextIndexingService;
            _cognitiveSearchService = cognitiveSearchService;
            _indexerConfigs = indexerConfigs;
        }
        public async Task IndexCustomText(string schemaPath, string indexName)
        {
            // read schema
            var schemaString = await _storageService.ReadAsStringFromAbsolutePathAsync(schemaPath);
            var schema = JsonConvert.DeserializeObject<CustomTextSchema>(schemaString);

            // create index & skillset
            var searchIndex = _customTextIndexingService.CreateIndex(schema, indexName);
            var customSkill = _customTextIndexingService.CreateCustomSkillSchema(schema, indexName, _indexerConfigs.AzureFunctionUrl);
            var searchIndexer = _customTextIndexingService.CreateIndexer(schema, indexName);

            await _cognitiveSearchService.CreateDataSourceConnectionAsync(indexName, _indexerConfigs.DataSourceContainerName, _indexerConfigs.DataSourceConnectionString);
            await _cognitiveSearchService.CreateIndexAsync(searchIndex);
            await _cognitiveSearchService.CreateSkillSetAsync(customSkill);
            await _cognitiveSearchService.CreateIndexerAsync(searchIndexer);

            // populate index

        }
    }
}

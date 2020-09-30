using Microsoft.CognitiveSearchIntegration.Definitions.APIs.Services;
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
        private string _connectionString;
        private string _containerName;
        private string _azureFunctionUrl;

        public IndexingController(
            IStorageService storageService,
            ICustomTextIndexingService customTextIndexingService,
            ICognitiveSearchService cognitiveSearchService,
            string connectionString,
            string containerName,
            string azureFunctionUrl)
        {
            _storageService = storageService;
            _customTextIndexingService = customTextIndexingService;
            _cognitiveSearchService = cognitiveSearchService;
            _containerName = containerName;
            _connectionString = connectionString;
            _azureFunctionUrl = azureFunctionUrl;
        }
        public async Task IndexCustomText(string schemaPath, string indexName)
        {
            // read schema
            var schemaString = await _storageService.ReadAsStringFromAbsolutePathAsync(schemaPath);
            var schema = JsonConvert.DeserializeObject<CustomTextSchema>(schemaString);

            // create index & skillset
            var searchIndex = _customTextIndexingService.CreateIndex(schema, indexName);
            var customSkill = _customTextIndexingService.CreateCustomSkillSchema(schema, indexName, _azureFunctionUrl);
            var searchIndexer = _customTextIndexingService.CreateIndexer(schema, indexName);

            await _cognitiveSearchService.CreateDataSourceConnectionAsync(indexName, _containerName, _connectionString);
            await _cognitiveSearchService.CreateIndexAsync(searchIndex);
            await _cognitiveSearchService.CreateSkillSetAsync(customSkill);
            await _cognitiveSearchService.CreateIndexerAsync(searchIndexer);

            // populate index

        }
    }
}

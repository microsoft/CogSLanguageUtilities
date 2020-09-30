using Azure.Search.Documents.Indexes.Models;
using Microsoft.CognitiveSearchIntegration.Definitions.Models.CognitiveSearch.Api;
using Microsoft.CognitiveSearchIntegration.Definitions.Models.CognitiveSearch.Api.Indexer;
using Microsoft.CognitiveSearchIntegration.Definitions.Models.CustomText.Schema;

namespace Microsoft.CognitiveSearchIntegration.Definitions.APIs.Services
{
    public interface ICustomTextIndexingService
    {
        public SkillSet CreateCustomSkillSchema(CustomTextSchema schema, string indexName, string azureFunctionUrl);
        public SearchIndex CreateIndex(CustomTextSchema schema, string indexName);
        public Indexer CreateIndexer(CustomTextSchema schema, string indexName);
    }
}
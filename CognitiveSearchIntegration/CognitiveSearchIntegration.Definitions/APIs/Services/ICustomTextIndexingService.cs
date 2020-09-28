using Azure.Search.Documents.Indexes.Models;
using Microsoft.CognitiveSearchIntegration.Definitions.Models.CognitiveSearch.Schema;
using Microsoft.CognitiveSearchIntegration.Definitions.Models.CustomText.Schema;

namespace Microsoft.CognitiveSearchIntegration.Definitions.APIs.Services
{
    public interface ICustomTextIndexingService
    {
        CustomSkillSchema CreateCustomSkillSchema(CustomTextSchema schema, string indexName);
        SearchIndex CreateIndex(CustomTextSchema schema, string indexName);
        SearchIndexer CreateIndexer(CustomTextSchema schema, string indexName);
    }
}
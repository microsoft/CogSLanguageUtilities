using Azure.Search.Documents.Indexes.Models;
using Microsoft.CognitiveSearchIntegration.Definitions.Models.CognitiveSearch.Schema;
using System.Threading.Tasks;

namespace Microsoft.CognitiveSearchIntegration.Definitions.APIs.Services
{
    public interface ICognitiveSearchService
    {
        public Task CreateIndexAsync(SearchIndex index);
        public Task CreateSkillAsync(CustomSkillSchema schema);
        public Task CreateIndexerAsync(SearchIndexer indexer);
        public Task CreateDataSourceConnectionAsync(string indexName, string containerName, string connectionString);
    }
}
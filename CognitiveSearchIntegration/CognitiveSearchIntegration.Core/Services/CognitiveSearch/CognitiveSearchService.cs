using Azure;
using Azure.Search.Documents.Indexes;
using Azure.Search.Documents.Indexes.Models;
using Microsoft.CognitiveSearchIntegration.Definitions.APIs.Helpers;
using Microsoft.CognitiveSearchIntegration.Definitions.APIs.Services;
using Microsoft.CognitiveSearchIntegration.Definitions.Consts;
using Microsoft.CognitiveSearchIntegration.Definitions.Models.CognitiveSearch.Api;
using Microsoft.CognitiveSearchIntegration.Definitions.Models.CognitiveSearch.Api.Indexer;
using Microsoft.CogSLanguageUtilities.Definitions.Exceptions;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Microsoft.CognitiveSearchIntegration.Core.Services.CognitiveSearch
{
    public class CognitiveSearchService : ICognitiveSearchService
    {
        string _endpoint;
        string _key;
        private IHttpHandler _httpHandler;
        private SearchIndexClient _searchIndexClient;
        private SearchIndexerClient _searchIndexerClient;

        public CognitiveSearchService(IHttpHandler httpHandler, string endpoint, string apiKey)
        {
            _httpHandler = httpHandler;
            _endpoint = endpoint;
            _key = apiKey;
            Uri serviceEndpoint = new Uri(endpoint);
            AzureKeyCredential credential = new AzureKeyCredential(apiKey);
            _searchIndexClient = new SearchIndexClient(serviceEndpoint, credential);
            _searchIndexerClient = new SearchIndexerClient(serviceEndpoint, credential);
        }

        public async Task CreateIndexAsync(SearchIndex index)
        {
            await _searchIndexClient.DeleteIndexAsync(index.Name);
            await _searchIndexClient.CreateIndexAsync(index);
            // TODO: handle exceptions
        }

        public async Task CreateDataSourceConnectionAsync(string indexName, string containerName, string connectionString)
        {
            SearchIndexerDataContainer searchIndexerDataContainer = new SearchIndexerDataContainer(containerName);
            SearchIndexerDataSourceConnection searchIndexerDataSourceConnection = new SearchIndexerDataSourceConnection(
                indexName + Constants.DataSourceSuffix,
                SearchIndexerDataSourceType.AzureBlob,
                connectionString,
                searchIndexerDataContainer);
            await _searchIndexerClient.CreateDataSourceConnectionAsync(searchIndexerDataSourceConnection);
            // TODO: handle exceptions
        }

        public async Task CreateIndexerAsync(Indexer indexer)
        {
            var requestUrl = $"{_endpoint}/indexers";
            var headers = new Dictionary<string, string>
            {
                ["api-key"] = _key
            };
            var parameters = new Dictionary<string, string>
            {
                [Constants.CognitiveSearchApiVersionHeader] = Constants.CognitiveSearchApiVersion
            };
            var response = await _httpHandler.SendJsonPostRequestAsync(requestUrl, indexer, headers, parameters);
            if (response.StatusCode != HttpStatusCode.Created)
            {
                throw new CliException("Indexer failed" + await response.Content.ReadAsStringAsync());
            }
        }

        public async Task CreateSkillSetAsync(SkillSet skillset)
        {
            var requestUrl = $"{_endpoint}/skillsets";
            var headers = new Dictionary<string, string>
            {
                ["api-key"] = _key
            };
            var parameters = new Dictionary<string, string>
            {
                [Constants.CognitiveSearchApiVersionHeader] = Constants.CognitiveSearchApiVersion
            };
            var response = await _httpHandler.SendJsonPostRequestAsync(requestUrl, skillset, headers, parameters);
            if (response.StatusCode != HttpStatusCode.Created)
            {
                throw new CliException("Skill failed" + await response.Content.ReadAsStringAsync());
            }
        }
    }
}

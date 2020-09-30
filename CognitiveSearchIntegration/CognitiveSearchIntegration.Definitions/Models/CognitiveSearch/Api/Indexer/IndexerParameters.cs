using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.CognitiveSearchIntegration.Definitions.Models.CognitiveSearch.Api.Indexer
{
    public class IndexerParameters
    {
        [JsonProperty("maxFailedItems")]
        public long? MaxFailedItems { get; set; }

        [JsonProperty("batchSize")]
        public long? BatchSize { get; set; }

        [JsonProperty("configuration")]
        public IndexerConfiguration Configuration { get; set; }
    }
}

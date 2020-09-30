using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.CognitiveSearchIntegration.Definitions.Models.CognitiveSearch.Api.Indexer
{
    public class IndexerConfiguration
    {
        [JsonProperty("parsingMode")]
        public string ParsingMode { get; set; }

        [JsonProperty("indexedFileNameExtensions")]
        public string IndexedFileNameExtensions { get; set; }

        [JsonProperty("imageAction")]
        public string ImageAction { get; set; }

        [JsonProperty("dataToExtract")]
        public string DataToExtract { get; set; }

        [JsonProperty("executionEnvironment")]
        public string ExecutionEnvironment { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.CognitiveSearchIntegration.Definitions.Models.CognitiveSearch.Indexer
{
    public class IndexerConfigs
    {
        // blob data source
        public string DataSourceContainerName;
        public string DataSourceConnectionString;

        // azure function
        public string AzureFunctionUrl;
    }
}

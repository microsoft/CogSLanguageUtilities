using Microsoft.CustomTextCliUtils.ApplicationLayer.Factories.Storage;
using Newtonsoft.Json;

namespace Microsoft.CustomTextCliUtils.ApplicationLayer.Modeling.Models.Configs
{
    public class TextAnalyticsConfigModel
    {
        [JsonProperty("azure-resource-endpoint")]
        public string AzureResourceEndpoint { get; set; }

        [JsonProperty("azure-resource-key")]
        public string AzureResourceKey { get; set; }

        [JsonProperty("default_operations")]
        public TextAnalyticsDefaultOperationsConfigModel DefaultOperations { get; set; }

        public TextAnalyticsConfigModel()
        {
            DefaultOperations = new TextAnalyticsDefaultOperationsConfigModel();
        }
    }
}

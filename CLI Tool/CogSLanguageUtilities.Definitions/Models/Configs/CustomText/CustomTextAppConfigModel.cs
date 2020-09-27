using Newtonsoft.Json;

namespace Microsoft.CogSLanguageUtilities.Definitions.Models.Configs.CustomText
{
    public class CustomTextAppConfigModel
    {
        [JsonProperty("azure-resource-key")]
        public string AzureResourceKey { get; set; }

        [JsonProperty("azure-resource-endpoint")]
        public string AzureResourceEndpoint { get; set; }

        [JsonProperty("app-id")]
        public string AppId { get; set; }
    }
}

using Newtonsoft.Json;

namespace Microsoft.CognitiveSearchIntegration.ViewLayer.Cli.Configs.ConfigModels
{
    public partial class AzureFunctionConfigModel
    {
        [JsonProperty("endpointUrl")]
        public string EndpointUrl { get; set; }

        [JsonProperty("apiKey")]
        public string ApiKey { get; set; }
    }
}

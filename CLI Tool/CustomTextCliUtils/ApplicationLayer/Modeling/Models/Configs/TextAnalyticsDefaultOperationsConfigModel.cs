using Newtonsoft.Json;

namespace Microsoft.CustomTextCliUtils.ApplicationLayer.Modeling.Models.Configs
{
    public class TextAnalyticsDefaultOperationsConfigModel
    {
        [JsonProperty("sentiment")]
        public bool Sentiment { get; set; }

        [JsonProperty("ner")]
        public bool Ner { get; set; }

        [JsonProperty("keyphrase")]
        public bool Keyphrase { get; set; }
    }
}

using Microsoft.CustomTextCliUtils.Configs.Consts;
using Newtonsoft.Json;

namespace Microsoft.CogSLanguageUtilities.Definitions.Models.Models.Configs
{
    public class TextAnalyticsDefaultOperationsConfigModel
    {
        [JsonProperty(ConfigKeys.TextAnalyticsSentiment)]
        public bool Sentiment { get; set; }

        [JsonProperty(ConfigKeys.TextAnalyticsNer)]
        public bool Ner { get; set; }

        [JsonProperty(ConfigKeys.TextAnalyticsKeyphrase)]
        public bool Keyphrase { get; set; }
    }
}

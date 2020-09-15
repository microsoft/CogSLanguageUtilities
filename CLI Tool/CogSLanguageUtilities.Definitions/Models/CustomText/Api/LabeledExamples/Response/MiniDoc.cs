using Newtonsoft.Json;
using System.Collections.Generic;

namespace Microsoft.CogSLanguageUtilities.Definitions.Models.CustomText.Api.LabeledExamples.Response
{
    public class MiniDoc
    {
        [JsonProperty("modelId")]
        public string ModelId { get; set; }

        [JsonProperty("startCharIndex")]
        public int StartCharIndex { get; set; }

        [JsonProperty("endCharIndex")]
        public int EndCharIndex { get; set; }

        [JsonProperty("positiveExtractionLabels", NullValueHandling = NullValueHandling.Ignore)]
        public MiniDoc[] PositiveExtractionLabels { get; set; }

        [JsonProperty("children", NullValueHandling = NullValueHandling.Ignore)]
        public List<MiniDoc> Children { get; set; }
    }
}

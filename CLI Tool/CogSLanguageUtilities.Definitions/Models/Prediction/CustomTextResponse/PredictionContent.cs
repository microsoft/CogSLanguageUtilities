using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace Microsoft.CogSLanguageUtilities.Definitions.Models.Models.Prediction.CustomTextResponse
{
    public class PredictionContent
    {
        [JsonProperty(PropertyName = "positiveClassifiers", Order = 0)]
        public List<string> PositiveClassifiers { get; set; }

        [JsonProperty(PropertyName = "classifiers", Order = 1, NullValueHandling = NullValueHandling.Ignore)]
        public Dictionary<string, ClassifierV4Preview> Classifiers { get; set; }


        [JsonProperty(PropertyName = "extractors", Order = 2)]
        public JObject Extractors { get; set; }
    }
}

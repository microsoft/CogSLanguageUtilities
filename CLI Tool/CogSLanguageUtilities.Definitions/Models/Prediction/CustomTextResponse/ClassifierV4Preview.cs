using Newtonsoft.Json;

namespace Microsoft.CogSLanguageUtilities.Definitions.Models.Models.Prediction.CustomTextResponse
{
    public class ClassifierV4Preview
    {
        [JsonProperty(PropertyName = "score", NullValueHandling = NullValueHandling.Ignore, Order = 0)]
        public float? Score { get; set; }
    }
}

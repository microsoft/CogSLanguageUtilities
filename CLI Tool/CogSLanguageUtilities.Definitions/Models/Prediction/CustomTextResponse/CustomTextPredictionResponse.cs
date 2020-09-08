using Newtonsoft.Json;

namespace Microsoft.CogSLanguageUtilities.Definitions.Models.Models.Prediction.CustomTextResponse
{
    public class CustomTextPredictionResponse
    {
        [JsonProperty(PropertyName = "Prediction", Order = 0)]
        public PredictionContent Prediction { get; set; }
    }
}
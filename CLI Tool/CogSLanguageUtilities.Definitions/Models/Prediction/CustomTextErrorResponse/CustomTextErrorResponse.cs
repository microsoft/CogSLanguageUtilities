using Newtonsoft.Json;

namespace Microsoft.CogSLanguageUtilities.Definitions.Models.Models.Prediction.CustomTextErrorResponse
{
    public class CustomTextErrorResponse
    {
        [JsonProperty("error")]
        public Error Error { get; set; }
    }
}

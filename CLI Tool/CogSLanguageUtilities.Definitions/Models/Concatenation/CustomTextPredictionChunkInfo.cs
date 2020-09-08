using Microsoft.CogSLanguageUtilities.Definitions.Models.Chunker;
using Microsoft.CogSLanguageUtilities.Definitions.Models.CustomText.PredictionApi.Response.Result;

namespace Microsoft.CogSLanguageUtilities.Definitions.Models.Concatenation
{
    public class CustomTextPredictionChunkInfo
    {
        public ChunkInfo ChunkInfo;
        public CustomTextPredictionResponse CustomTextPredictionResponse { get; set; }
    }
}

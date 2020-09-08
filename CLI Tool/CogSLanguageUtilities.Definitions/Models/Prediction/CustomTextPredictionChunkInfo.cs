using Microsoft.CogSLanguageUtilities.Definitions.Models.Models.Chunker;
using Microsoft.CogSLanguageUtilities.Definitions.Models.Models.Prediction.CustomTextResponse;
using Newtonsoft.Json;

namespace Microsoft.CogSLanguageUtilities.Definitions.Models.Models.Prediction
{
    public class CustomTextPredictionChunkInfo
    {
        public ChunkInfo ChunkInfo;
        public CustomTextPredictionResponse CustomTextPredictionResponse { get; set; }
    }
}

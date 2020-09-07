using Azure.AI.TextAnalytics;
using Microsoft.CustomTextCliUtils.ApplicationLayer.Modeling.Models.Chunker;
using Microsoft.CustomTextCliUtils.ApplicationLayer.Modeling.Models.TextAnalytics;
using System.Collections.Generic;

namespace Microsoft.CustomTextCliUtils.ApplicationLayer.Services.Concatenation
{
    public interface IConcatenationService
    {
        public List<TextAnalyticsPredictionChunkInfo> ConcatTextAnalytics(ChunkInfo[] chunkedText, AnalyzeSentimentResultCollection sentimentResponse, RecognizeEntitiesResultCollection nerResponse, ExtractKeyPhrasesResultCollection keyphraseResponse);
    }
}

using Azure.AI.TextAnalytics;
using Microsoft.CogSLanguageUtilities.Definitions.Models.Chunker;
using Microsoft.CogSLanguageUtilities.Definitions.Models.Concatenation;
using System.Collections.Generic;

namespace Microsoft.CogSLanguageUtilities.Definitions.APIs.Services
{
    public interface IConcatenationService
    {
        public List<TextAnalyticsPredictionChunkInfo> ConcatTextAnalytics(ChunkInfo[] chunkedText, List<AnalyzeSentimentResult> sentimentResponse, List<RecognizeEntitiesResult> nerResponse, List<ExtractKeyPhrasesResult> keyphraseResponse);
    }
}

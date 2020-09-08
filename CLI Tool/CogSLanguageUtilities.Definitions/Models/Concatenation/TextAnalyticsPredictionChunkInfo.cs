using Azure.AI.TextAnalytics;
using Microsoft.CogSLanguageUtilities.Definitions.Models.Chunker;

namespace Microsoft.CogSLanguageUtilities.Definitions.Models.Concatenation
{
    public class TextAnalyticsPredictionChunkInfo
    {
        public ChunkInfo ChunkInfo;
        public AnalyzeSentimentResult SentimentResponse;
        public RecognizeEntitiesResult NerResponse;
        public ExtractKeyPhrasesResult KeyphraseResponse;
    }
}

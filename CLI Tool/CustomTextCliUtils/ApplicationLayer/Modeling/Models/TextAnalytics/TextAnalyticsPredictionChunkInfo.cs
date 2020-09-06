using Azure.AI.TextAnalytics;

namespace Microsoft.CustomTextCliUtils.ApplicationLayer.Modeling.Models.TextAnalytics
{
    public class TextAnalyticsPredictionChunkInfo
    {
        public AnalyzeSentimentResult sentimentResponse;
        public RecognizeEntitiesResult nerResponse;
        public ExtractKeyPhrasesResult keyphraseResponse;
    }
}

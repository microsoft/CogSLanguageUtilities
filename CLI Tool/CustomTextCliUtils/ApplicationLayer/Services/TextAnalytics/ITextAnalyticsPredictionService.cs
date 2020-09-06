using Azure.AI.TextAnalytics;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.CustomTextCliUtils.ApplicationLayer.Services.TextAnalytics
{
    public interface ITextAnalyticsPredictionService
    {
        public Task<AnalyzeSentimentResultCollection> PredictSentimentBatchAsync(List<string> queries);
        public Task<RecognizeEntitiesResultCollection> PredictNerBatchAsync(List<string> queries);
        public Task<ExtractKeyPhrasesResultCollection> PredictKeyphraseBatchAsync(List<string> queries);
    }
}

using Azure;
using System;
using Azure.AI.TextAnalytics;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.CustomTextCliUtils.ApplicationLayer.Services.TextAnalytics
{
    public class TextAnalyticsPredictionService : ITextAnalyticsPredictionService
    {
        private readonly TextAnalyticsClient _textAnalyticsClient;
        private readonly string _predictionLanguage;

        public TextAnalyticsPredictionService(string textAnalyticsKey, string textAnalyticsEndpoint, string predictionLanguage)
        {
            var credentials = new AzureKeyCredential(textAnalyticsKey);
            var endpoint = new Uri(textAnalyticsEndpoint);
            _textAnalyticsClient = new TextAnalyticsClient(endpoint, credentials);
            _predictionLanguage = predictionLanguage;
        }

        public async Task<AnalyzeSentimentResultCollection> PredictSentimentBatchAsync(List<string> queries)
        {
            var response =  await _textAnalyticsClient.AnalyzeSentimentBatchAsync(queries, language: _predictionLanguage);
            return response.Value;
        }
        public async Task<RecognizeEntitiesResultCollection> PredictNerBatchAsync(List<string> queries)
        {
            var response = await _textAnalyticsClient.RecognizeEntitiesBatchAsync(queries, language: _predictionLanguage);
            return response.Value;
        }

        public async Task<ExtractKeyPhrasesResultCollection> PredictKeyphraseBatchAsync(List<string> queries)
        {
            var response = await _textAnalyticsClient.ExtractKeyPhrasesBatchAsync(queries, language: _predictionLanguage);
            return response.Value;
        }
    }
}

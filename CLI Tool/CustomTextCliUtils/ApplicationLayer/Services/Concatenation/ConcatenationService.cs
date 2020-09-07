using Azure.AI.TextAnalytics;
using Microsoft.CustomTextCliUtils.ApplicationLayer.Modeling.Models.TextAnalytics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.CustomTextCliUtils.ApplicationLayer.Services.Concatenation
{
    class ConcatenationService : IConcatenationService
    {
        public List<TextAnalyticsPredictionChunkInfo> ConcatTextAnalytics(AnalyzeSentimentResultCollection sentimentResponse, RecognizeEntitiesResultCollection nerResponse, ExtractKeyPhrasesResultCollection keyphraseResponse)
        {
            // TODO: return size of each response must be equal!
            var sentimentCount = sentimentResponse != null ? sentimentResponse.Count : -1;
            var nerCount = nerResponse != null ? nerResponse.Count : -1;
            var keyphraseCount = keyphraseResponse != null ? keyphraseResponse.Count : -1;
            var listCount = Math.Max(sentimentCount, Math.Max(nerCount, keyphraseCount));

            // prepare
            var sentimentArr = sentimentResponse?.ToArray();
            var nerArr = nerResponse?.ToArray();
            var keyphraseArr = keyphraseResponse?.ToArray();

            var result = new List<TextAnalyticsPredictionChunkInfo>();
            for (int i = 0; i < listCount; i++)
            {
                var currChunkInfo = new TextAnalyticsPredictionChunkInfo
                {
                    sentimentResponse = i < sentimentCount ? sentimentArr[i] : null,
                    nerResponse = i < nerCount ? nerArr[i] : null,
                    keyphraseResponse = i < keyphraseCount ? keyphraseArr[i] : null
                };
                result.Add(currChunkInfo);
            }
            return result;
        }
    }
}

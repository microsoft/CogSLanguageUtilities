using Azure.AI.TextAnalytics;
using Microsoft.CustomTextCliUtils.ApplicationLayer.Modeling.Models.TextAnalytics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.CustomTextCliUtils.ApplicationLayer.Services.Concatenation
{
    public interface IConcatenationService
    {
        public List<TextAnalyticsPredictionChunkInfo> ConcatTextAnalytics(AnalyzeSentimentResultCollection sentimentResponse, RecognizeEntitiesResultCollection nerResponse, ExtractKeyPhrasesResultCollection keyphraseResponse);
    }
}

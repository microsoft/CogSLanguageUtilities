using Azure.AI.TextAnalytics;
using Microsoft.CustomTextCliUtils.ApplicationLayer.Exceptions;
using Microsoft.CustomTextCliUtils.ApplicationLayer.Services.TextAnalytics;
using Microsoft.CustomTextCliUtils.Tests.Configs;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace CustomTextCliUtils.Tests.IntegrationTests.ApplicationLayer.Services.TextAnalytics
{
    public class TextAnalyticsPredictionServiceTest
    {
       public static TheoryData PredictSentimentBatchAsyncTestData()
        {
            return new TheoryData<string, string, string, List<string>, CliException>
            {
                {
                    Secrets.TextAnalyticsKey,
                    Secrets.TextAnalyticsEndpoint,
                    "en",
                    new List<string> {"Ahmed's restaurant is great. Let's go eath there soon.", "Book me a flight to Cairo"},
                    null
                }
            };
        }

        [Theory]
        [MemberData(nameof(PredictSentimentBatchAsyncTestData))]
        public async Task PredictSentimentBatchAsyncTest(string key, string endpoint, string language, List<string> queries, CliException expectedException)
        {
            // prepare
            ITextAnalyticsPredictionService predictionService = new TextAnalyticsPredictionService(key, endpoint, language);

            if (expectedException == null)
            {
                // act
                var result = await predictionService.PredictSentimentBatchAsync(queries);

                // assert
                Assert.NotNull(result);
                Assert.NotEmpty(result);
                foreach (AnalyzeSentimentResult r in result)
                {
                    Assert.NotNull(r.DocumentSentiment);
                    Assert.NotNull(r.DocumentSentiment.ConfidenceScores);
                    Assert.NotNull(r.DocumentSentiment.Sentences);
                    Assert.NotEmpty(r.DocumentSentiment.Sentences);
                }
            }
            else
            {
                await Assert.ThrowsAsync(expectedException.GetType(), async () => await predictionService.PredictSentimentBatchAsync(queries));
            }
        }

        public static TheoryData PredictNerBatchAsyncTestData()
        {
            return new TheoryData<string, string, string, List<string>, CliException>
            {
                {
                    Secrets.TextAnalyticsKey,
                    Secrets.TextAnalyticsEndpoint,
                    "en",
                    new List<string> {"Ahmed's restaurant is great. Let's go eath there soon.", "Book me a flight to Cairo"},
                    null
                }
            };
        }

        [Theory]
        [MemberData(nameof(PredictNerBatchAsyncTestData))]
        public async Task PredictNerBatchAsyncTest(string key, string endpoint, string language, List<string> queries, CliException expectedException)
        {
            // prepare
            ITextAnalyticsPredictionService predictionService = new TextAnalyticsPredictionService(key, endpoint, language);

            if (expectedException == null)
            {
                // act
                var result = await predictionService.PredictNerBatchAsync(queries);

                // assert
                Assert.NotNull(result);
                Assert.NotEmpty(result);
                foreach (RecognizeEntitiesResult r in result)
                {
                    Assert.NotNull(r.Entities);
                    Assert.NotEmpty(r.Entities);
                    foreach (CategorizedEntity e in r.Entities)
                    {
                        Assert.NotNull(e.Text);
                    }
                }
            }
            else
            {
                await Assert.ThrowsAsync(expectedException.GetType(), async () => await predictionService.PredictNerBatchAsync(queries));
            }
        }

        public static TheoryData PredictKeyphraseBatchAsyncTestData()
        {
            return new TheoryData<string, string, string, List<string>, CliException>
            {
                {
                    Secrets.TextAnalyticsKey,
                    Secrets.TextAnalyticsEndpoint,
                    "en",
                    new List<string> {"Ahmed's restaurant is great. Let's go eath there soon.", "Book me a flight to Cairo"},
                    null
                }
            };
        }

        [Theory]
        [MemberData(nameof(PredictSentimentBatchAsyncTestData))]
        public async Task PredictKeyphraseBatchAsyncTest(string key, string endpoint, string language, List<string> queries, CliException expectedException)
        {
            // prepare
            ITextAnalyticsPredictionService predictionService = new TextAnalyticsPredictionService(key, endpoint, language);

            if (expectedException == null)
            {
                // act
                var result = await predictionService.PredictKeyphraseBatchAsync(queries);

                // assert
                Assert.NotNull(result);
                Assert.NotEmpty(result);
                foreach (ExtractKeyPhrasesResult r in result)
                {
                    Assert.NotNull(r.KeyPhrases);
                    Assert.NotEmpty(r.KeyPhrases);
                }
            }
            else
            {
                await Assert.ThrowsAsync(expectedException.GetType(), async () => await predictionService.PredictKeyphraseBatchAsync(queries));
            }
        }
    }
}

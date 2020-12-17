using Microsoft.CogSLanguageUtilities.Definitions.APIs.Services;
using Microsoft.CogSLanguageUtilities.Definitions.Enums.IAP;
using Microsoft.CogSLanguageUtilities.Definitions.Models.IAP;
using Microsoft.CogSLanguageUtilities.Definitions.Models.Luis;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.CogSLanguageUtilities.Core.Services.IAP
{
    public class IAPResultGenerator : IIAPResultGenerator
    {
        public ProcessedTranscript GenerateResult(IDictionary<long, CustomLuisResponse> luisPredictions, ChannelType channel, string transcriptId)
        {

            var meta = new Meta
            {
                Channel = channel,
                TranscriptId = transcriptId
            };

            var conversations = luisPredictions.Where(prediction => prediction.Value.Entities != null)
                .Select(prediction =>
            {
                return new Conversation
                {
                    Text = prediction.Value.Query,
                    Timestamp = prediction.Key,
                    Extractions = prediction.Value.Entities
                };
            });

            var sortedConversations = conversations.OrderBy(c => c.Timestamp).ToList();
            return new ProcessedTranscript
            {
                Conversation = sortedConversations,
                Meta = meta
            };
        }
    }
}

using Microsoft.CogSLanguageUtilities.Definitions.APIs.Services;
using Microsoft.CogSLanguageUtilities.Definitions.Enums.IAP;
using Microsoft.CogSLanguageUtilities.Definitions.Models.IAP;
using Microsoft.CogSLanguageUtilities.Definitions.Models.Luis;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.CogSLanguageUtilities.Core.Services.IAP
{
    public class IAPTranscriptGenerator : IIAPTranscriptGenerator
    {
        public ProcessedTranscript GenerateTranscript(IDictionary<long, CustomLuisResponse> luisPredictions, ChannelType channel, string transcriptId)
        {

            var meta = new Meta
            {
                Channel = channel,
                TranscriptId = transcriptId
            };

            var conversations = luisPredictions.Select(prediction =>
            {
                return new Conversation
                {
                    Text = prediction.Value.Query,
                    Timestamp = prediction.Key,
                    Extractions = prediction.Value.Entities
                };
            }).ToList();

            return new ProcessedTranscript
            {
                Conversation = conversations,
                Meta = meta
            };
        }
    }
}

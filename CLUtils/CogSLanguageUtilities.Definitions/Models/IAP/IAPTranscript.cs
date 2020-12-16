using Microsoft.CogSLanguageUtilities.Definitions.Enums.IAP;
using System.Collections.Generic;

namespace Microsoft.CogSLanguageUtilities.Definitions.Models.IAP
{
    public class IAPTranscript
    {
        public string Id { get; set; }
        public ChannelType Channel { get; set; }
        public List<ConversationUtterance> Utterances { get; set; }
    }
}

// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
using Microsoft.CogSLanguageUtilities.Definitions.Enums.IAP;
using Microsoft.CogSLanguageUtilities.Definitions.Models.IAP;
using Microsoft.CogSLanguageUtilities.Definitions.Models.Luis;
using System.Collections.Generic;

namespace Microsoft.CogSLanguageUtilities.Definitions.APIs.Services
{
    public interface IIAPTranscriptGenerator
    {
        ProcessedTranscript GenerateTranscript(Dictionary<long, CustomLuisResponse> luisPredictions, ChannelType channel, string transcriptId);
    }
}
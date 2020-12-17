// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
using Microsoft.IAPUtilities.Definitions.Enums.IAP;
using Microsoft.IAPUtilities.Definitions.Models.IAP;
using Microsoft.IAPUtilities.Definitions.Models.Luis;
using System.Collections.Generic;

namespace Microsoft.IAPUtilities.Definitions.APIs.Services
{
    public interface IIAPResultGenerator
    {
        ProcessedTranscript GenerateResult(IDictionary<long, CustomLuisResponse> luisPredictions, ChannelType channel, string transcriptId);
    }
}
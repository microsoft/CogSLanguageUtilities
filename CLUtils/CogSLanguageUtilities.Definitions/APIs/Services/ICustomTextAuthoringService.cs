// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
﻿using Microsoft.CogSLanguageUtilities.Definitions.Configs.Consts;
using Microsoft.CogSLanguageUtilities.Definitions.Models.CustomText.Api.AppModels.Response;
using Microsoft.CogSLanguageUtilities.Definitions.Models.CustomText.Api.LabeledExamples.Response;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.CogSLanguageUtilities.Definitions.APIs.Services
{
    public interface ICustomTextAuthoringService
    {
        public Task<CustomTextGetLabeledExamplesResponse> GetLabeledExamples(int skip = 0, int take = Constants.CustomTextExamplesPageSize);
        Task<Dictionary<string, string>> GetModelsDictionary();
        public Task<CustomTextGetModelsResponse> GetApplicationModels();
        public Task<object> AddApplicationClassifierAsync(CustomTextModel classifier);
        public Task<object> AddApplicationExtractorAsync(CustomTextModel extractor);
        public Task<object> AddDocumentClassifierLabelAsync(string documentId, string classifierId, bool classifierLabel);
        public Task<object> AddDocumentExtractorMiniDocsAsync(string documentId, string extractorId, IEnumerable<MiniDoc> miniDocs);
    }
}
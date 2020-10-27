// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
using Microsoft.CogSLanguageUtilities.Definitions.Models.Enums.Storage;
using System.Threading.Tasks;

namespace Microsoft.CogSLanguageUtilities.Definitions.APIs.Controllers
{
    public interface IImportController
    {
        Task ImportSchemaAsync(StorageType sourceStorageType, string schemaPath);
    }
}
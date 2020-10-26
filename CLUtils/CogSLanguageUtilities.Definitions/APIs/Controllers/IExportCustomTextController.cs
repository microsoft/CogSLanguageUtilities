// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
using Microsoft.CogSLanguageUtilities.Definitions.Models.Enums.Storage;
using System.Threading.Tasks;

namespace Microsoft.CogSLanguageUtilities.Core.Controllers
{
    public interface IExportCustomTextController
    {
        Task ExportSchema(StorageType destinationStorageType);
    }
}
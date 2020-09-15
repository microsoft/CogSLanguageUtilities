﻿using Microsoft.CogSLanguageUtilities.Definitions.Models.Enums.Logger;
using System;
using System.Collections.Generic;

namespace Microsoft.CogSLanguageUtilities.Definitions.APIs.Services
{
    public interface ILoggerService
    {
        public void Log(string message);

        public void LogError(Exception e);

        public void LogOperation(OperationType operationType, string message);

        public void LogParsingResult(IEnumerable<string> convertedFiles, IDictionary<string, string> failedFiles);
    }
}

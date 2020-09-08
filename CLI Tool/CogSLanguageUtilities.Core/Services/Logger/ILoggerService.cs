using Microsoft.CogSLanguageUtilities.Definitions.Models.Enums;
using System;
using System.Collections.Generic;

namespace Microsoft.CustomTextCliUtils.ApplicationLayer.Services.Logger
{
    public interface ILoggerService
    {
        public void Log(string message);

        public void LogError(Exception e);

        public void LogOperation(OperationType operationType, string message);

        public void LogParsingResult(IEnumerable<string> convertedFiles, IDictionary<string, string> failedFiles);
    }
}

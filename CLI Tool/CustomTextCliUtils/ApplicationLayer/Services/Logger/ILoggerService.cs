using Microsoft.CustomTextCliUtils.ApplicationLayer.Modeling.Enums.Logger;
using System;
using System.Collections.Generic;

namespace Microsoft.CustomTextCliUtils.ApplicationLayer.Services.Logger
{
    public interface ILoggerService
    {
        void Log(string message);

        void LogError(Exception e);

        void LogOperation(OperationType operationType, string message);

        public void LogParsingResult(List<string> convertedFiles, Dictionary<string, string> failedFiles);
    }
}

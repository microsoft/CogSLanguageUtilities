using Microsoft.LuisModelEvaluation.Models.Input;
using Microsoft.LuisModelEvaluation.Models.Result;
using System.Collections.Generic;

namespace Microsoft.CogSLanguageUtilities.Definitions.APIs.Services
{
    public interface IBatchTestingService
    {
        public BatchTestResponse RunBatchTest(IEnumerable<TestingExample> testData);
    }
}

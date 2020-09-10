using Microsoft.CogSLanguageUtilities.Definitions.APIs.Services;
using Microsoft.LuisModelEvaluation.Controllers;
using Microsoft.LuisModelEvaluation.Models.Input;
using Microsoft.LuisModelEvaluation.Models.Result;
using System.Collections.Generic;

namespace Microsoft.CogSLanguageUtilities.Core.Services.Evaluation
{
    public class BatchTestingService : IBatchTestingService
    {
        public BatchTestResponse RunBatchTest(IEnumerable<TestingExample> testData)
        {
            var evaluation = new EvaluationController();
            return evaluation.EvaluateModel(testData, true);
        }
    }
}

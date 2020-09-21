using Microsoft.CogSLanguageUtilities.Core.Helpers.Mappers.EvaluationNuget;
using Microsoft.CogSLanguageUtilities.Definitions.APIs.Services;
using Microsoft.CogSLanguageUtilities.Definitions.Models.Evaluation;
using Microsoft.LuisModelEvaluation.Controllers;
using Microsoft.LuisModelEvaluation.Models.Input;
using System.Collections.Generic;

namespace Microsoft.CogSLanguageUtilities.Core.Services.Evaluation
{
    public class BatchTestingService : IBatchTestingService
    {
        public BatchTestingOutput RunBatchTest(IEnumerable<TestingExample> testData, List<Model> entities, List<Model> classes)
        {
            var evaluation = new EvaluationController();
            return BatchTestingOutputMapper.MapEvaluationOutput(evaluation.EvaluateModel(testData, true, entities, classes));
        }
    }
}

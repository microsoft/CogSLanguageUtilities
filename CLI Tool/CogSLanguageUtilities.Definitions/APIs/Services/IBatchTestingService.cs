using Microsoft.CogSLanguageUtilities.Definitions.Models.Evaluation;
using Microsoft.LuisModelEvaluation.Models.Input;
using System.Collections.Generic;

namespace Microsoft.CogSLanguageUtilities.Definitions.APIs.Services
{
    public interface IBatchTestingService
    {
        public BatchTestingOutput RunBatchTest(IEnumerable<TestingExample> testData, List<Model> entities, List<Model> classes);
    }
}

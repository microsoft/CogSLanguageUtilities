using Microsoft.CogSLanguageUtilities.Definitions.Models.Evaluation;
using Microsoft.CogSLanguageUtilities.Definitions.Models.Evaluation.Stats;
using System.Collections.Generic;

namespace Microsoft.CogSLanguageUtilities.Definitions.APIs.Services
{
    public interface IEvaluationService
    {
        public List<ModelStats> EvaluateClassification(IEnumerable<KeyValuePair<string, string>> classes);
        public List<ModelStats> EvaluateEntities(IEnumerable<Entity> entities);

    }
}

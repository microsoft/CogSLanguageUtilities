using Microsoft.CogSLanguageUtilities.Definitions.APIs.Services;
using Microsoft.CogSLanguageUtilities.Definitions.Models.Evaluation;
using Microsoft.CogSLanguageUtilities.Definitions.Models.Evaluation.Stats;
using System;
using System.Collections.Generic;

namespace Microsoft.CogSLanguageUtilities.Core.Services.Evaluation
{
    public class EvaluationService : IEvaluationService
    {
        public List<ModelStats> EvaluateClassification(IEnumerable<KeyValuePair<string, string>> classes)
        {
            var confusionMatrix = CreateClassificationConfusionMatrix(classes);
            return CalculateClassificationScores(confusionMatrix);
        }

        private List<ModelStats> CalculateClassificationScores(IDictionary<string, ConfusionMatrix> confusionMatrix)
        {
            var result = new List<ModelStats>();
            foreach (var entry in confusionMatrix)
            {
                result.Add(new ModelStats
                {
                    ModelName = entry.Key,
                    Precision = entry.Value.CalculatePrecision(),
                    Recall = entry.Value.CalculateRecall(),
                    FScore = entry.Value.CalculateFScore()
                });
            }
            return result;
        }

        private IDictionary<string, ConfusionMatrix> CreateClassificationConfusionMatrix(IEnumerable<KeyValuePair<string, string>> classes)
        {
            var confusionMatrix = new Dictionary<string, ConfusionMatrix>();
            foreach (var entry in classes)
            {
                if (!confusionMatrix.TryGetValue(entry.Key, out ConfusionMatrix labeledConfusionCount))
                {
                    // Initialize if not in dictionary to avoid null errors
                    labeledConfusionCount = new ConfusionMatrix();
                }

                if (entry.Key == entry.Value)
                {
                    labeledConfusionCount.TruePositives++;
                }
                else
                {
                    labeledConfusionCount.FalseNegatives++;
                    if (!confusionMatrix.TryGetValue(entry.Value, out ConfusionMatrix predictedConfusionCount))
                    {
                        predictedConfusionCount = new ConfusionMatrix();
                    }
                    predictedConfusionCount.FalsePositives++;
                }
            }
            return confusionMatrix;
        }

        public List<ModelStats> EvaluateEntities(IEnumerable<Entity> entities)
        {
            throw new System.NotImplementedException();
        }
    }
}

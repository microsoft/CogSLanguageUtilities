// Copyright (c) Microsoft Corporation. All rights reserved.

using System.Collections.Generic;
using Microsoft.LUIS.Endpoint.Operations.Models;
using Microsoft.LUIS.Runtime.DataStructures;

namespace Microsoft.LUIS.Endpoint.Operations
{
    public class LuisBatchTestOperation
    {

        public LuisBatchTestOperation()
        {
        }

        public BatchTestResponse RunOperationInternalAsync(
            IEnumerable<TestingExample> testData,
            bool verbose = false,
            IEnumerable<Model> entities = null,
            IEnumerable<Model> classes = null)
        {

            // Intialize the batch test operation helper
            var batchTestOperationHelper = new LuisBatchTestOperationsHelper(entities, classes);

            foreach (var testCase in testData)
            {
                // Intent model stats aggregation
                batchTestOperationHelper.AggregateIntentStats(testCase.LabeledData.Classification, testCase.ActualData.Classification);

                // Prepare utterance stats
                var utteranceStats = new UtteranceStats
                {
                    UtteranceText = testCase.Text,
                    LabeledIntentName = testCase.LabeledData.Classification,
                    PredictedIntentName = testCase.ActualData.Classification
                };

                // Populate False entities and Aggregate Entity MUC model stats
                batchTestOperationHelper.PopulateUtteranceAndEntityStats(testCase.LabeledData.Entities, testCase.ActualData.Entities, utteranceStats);
            }

            // Calculate precision, recall and fScore for Intent models
            var intentModelsStats = new List<ModelStats>(batchTestOperationHelper.IntentsStats.Count);
            foreach (var intentConfusion in batchTestOperationHelper.IntentsStats.Values)
            {
                intentModelsStats.Add(new ModelStats
                {
                    ModelName = intentConfusion.ModelName,
                    ModelType = intentConfusion.ModelType,
                    Precision = intentConfusion.CalculatePrecision(),
                    Recall = intentConfusion.CalculateRecall(),
                    FScore = intentConfusion.CalculateFScore(),
                    EntityTextFScore = null,
                    EntityTypeFScore = null
                });
            }

            // Calculate precision, recall and fScore for Entity models
            var entityModelsStats = new List<ModelStats>(batchTestOperationHelper.EntityStats.Count);
            foreach (var entitiesEvaluation in batchTestOperationHelper.EntityStats.Values)
            {
                entityModelsStats.Add(new ModelStats
                {
                    ModelName = entitiesEvaluation.ModelName,
                    ModelType = entitiesEvaluation.ModelType,
                    Precision = entitiesEvaluation.CalculatePrecision(),
                    Recall = entitiesEvaluation.CalculateRecall(),
                    FScore = entitiesEvaluation.CalculateFScore(),
                    EntityTextFScore = verbose ? entitiesEvaluation.CalculateTextFScore() : (double?)null,
                    EntityTypeFScore = verbose ? entitiesEvaluation.CalculateTypeFScore() : (double?)null
                });
            }

            return new BatchTestResponse
            {
                IntentModelsStats = intentModelsStats,
                EntityModelsStats = entityModelsStats,
                UtterancesStats = batchTestOperationHelper.UtterancesStats
            };
        }
    }
}

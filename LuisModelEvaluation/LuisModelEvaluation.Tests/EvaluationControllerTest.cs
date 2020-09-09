using Microsoft.LuisModelEvaluation.Controllers;
using Microsoft.LuisModelEvaluation.Models.Input;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using Xunit;

namespace Microsoft.LuisModelEvaluation.Tests
{
    public class EvaluationControllerTest

    {
        [Fact]
        public void EvaluateClassification()
        {

            var entity = new Entity
            {
                Name = "testEntity",
                StartPosition = 1,
                EndPosition = 4,
                Children = null
            };
            var entities = new List<Entity>()
                    {
                        entity
                    };
            var predictionData = new PredictionObject
            {
                Classification = "testClass",
                Entities = entities
            };
            var list = new List<TestingExample>();
            list.Add(new TestingExample
            {
                ActualData = predictionData,
                LabeledData = predictionData
            });
            var evaluationController = new EvaluationController();
            var result = evaluationController.RunOperationInternalAsync(list);
            var resString = JsonConvert.SerializeObject(result);
            Console.WriteLine(resString);
            return;
        }
    }
}

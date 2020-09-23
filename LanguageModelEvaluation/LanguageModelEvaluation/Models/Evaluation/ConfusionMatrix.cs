<<<<<<< HEAD:CustomTextAnalytics/CliTool/LuisModelEvaluation/LuisModelEvaluation/Models/Evaluation/ConfusionMatrix.cs
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
﻿namespace Microsoft.LuisModelEvaluation.Models.Evaluation
=======
﻿namespace Microsoft.LanguageModelEvaluation.Models.Evaluation
>>>>>>> 7975186 ([nuget] rename LuisModelEvaluation to LanguageModelEvaluation):LanguageModelEvaluation/LanguageModelEvaluation/Models/Evaluation/ConfusionMatrix.cs
{
    public class ConfusionMatrix
    {
        public string ModelName { get; set; }

        public string ModelType { get; set; }

        public int TruePositives { get; set; }

        public int TrueNegatives { get; set; }

        public int FalsePositives { get; set; }

        public int FalseNegatives { get; set; }

        public double CalculatePrecision()
        {
            return (TruePositives + FalsePositives) == 0 ? 0 : (double)TruePositives / (TruePositives + FalsePositives);
        }

        public double CalculateRecall()
        {
            return (TruePositives + FalseNegatives) == 0 ? 0 : (double)TruePositives / (TruePositives + FalseNegatives);
        }

        public double CalculateFScore()
        {
            double precision = CalculatePrecision();
            double recall = CalculateRecall();
            return (precision + recall) == 0 ? 0 : 2 * precision * recall / (precision + recall);
        }
    }
}
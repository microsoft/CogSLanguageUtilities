using Microsoft.Research.DICE.Models;
using System.Collections.Generic;

namespace Microsoft.LUIS.Endpoint.Operations.Models
{
    public class PredictionData
    {
        public string Classification { get; set; }
        public List<Entity> Entities { get; set; }
    }
}

using System.Collections.Generic;

namespace Microsoft.LanguageModelEvaluation.Models.Input
{
    public class Entity
    {
        public int StartPosition { get; set; }

        public int EndPosition { get; set; }

        public List<Entity> Children { get; set; }

        public string Name { get; set; }
    }
}

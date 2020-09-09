// Copyright (c) Microsoft Corporation. All rights reserved.

using System.Collections.Generic;

namespace Microsoft.Research.DICE.Models
{
    public class Entity
    {
        public int StartPosition { get; set; }

        public int EndPosition { get; set; }

        public List<Entity> Children { get; set; }

        public string Name { get; set; }
    }
}

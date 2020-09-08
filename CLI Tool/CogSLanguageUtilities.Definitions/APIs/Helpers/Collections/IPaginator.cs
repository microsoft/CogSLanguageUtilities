﻿using System.Collections.Generic;

namespace Microsoft.CogSLanguageUtilities.Definitions.APIs.Helpers.Collections
{
    public interface IPaginator<T>
    {
        public bool HasNext();
        public IEnumerable<T> GetNextPage();
    }
}

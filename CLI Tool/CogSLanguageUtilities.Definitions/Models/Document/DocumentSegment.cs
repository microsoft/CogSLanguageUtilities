﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.CogSLanguageUtilities.Definitions.Models.Document
{
    public class DocumentSegment
    {
        public DocumentElement RootElement { get; set; }
        public List<DocumentSegment> Children { get; set; }
    }
}

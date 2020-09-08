using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.CogSLanguageUtilities.Definitions.Exceptions.Prediction
{
    public class ResourceNotFoundExcption : CliException
    {
        public ResourceNotFoundExcption(string message)
            : base(message)
        { }
    }
}

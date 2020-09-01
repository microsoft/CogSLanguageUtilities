using Microsoft.CustomTextCliUtils.ApplicationLayer.Modeling.Models.Parser;
using Microsoft.CustomTextCliUtils.ApplicationLayer.Services.Chunker;
using System.IO;
using System.Threading.Tasks;

namespace Microsoft.CustomTextCliUtils.ApplicationLayer.Services.Parser
{
    public interface IParserService
    {
        Task<ParseResult> ParseFile(Stream file);

        void ValidateFileType(string fileType);
    }
}

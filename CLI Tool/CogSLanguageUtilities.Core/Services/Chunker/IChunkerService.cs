using Microsoft.CogSLanguageUtilities.Definitions.Models.Enums.Misc;
using Microsoft.CogSLanguageUtilities.Definitions.Models.Chunker;
using Microsoft.CogSLanguageUtilities.Definitions.Models.Parser;
using System.Collections.Generic;

namespace Microsoft.CustomTextCliUtils.ApplicationLayer.Services.Chunker
{
    public interface IChunkerService
    {
        public List<ChunkInfo> Chunk(ParseResult parseResult, ChunkMethod chunkMethod, int charLimit);

        public List<ChunkInfo> Chunk(string text, int charLimit);

        public void ValidateFileType(string fileName);
    }
}

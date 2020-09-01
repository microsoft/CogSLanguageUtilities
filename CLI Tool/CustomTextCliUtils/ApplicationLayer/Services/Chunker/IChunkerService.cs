using Microsoft.CustomTextCliUtils.ApplicationLayer.Modeling.Enums.Misc;
using Microsoft.CustomTextCliUtils.ApplicationLayer.Modeling.Models.Chunker;
using Microsoft.CustomTextCliUtils.ApplicationLayer.Modeling.Models.Parser;
using System.Collections.Generic;

namespace Microsoft.CustomTextCliUtils.ApplicationLayer.Services.Chunker
{
    public interface IChunkerService
    {
        List<ChunkInfo> Chunk(ParseResult parseResult, ChunkMethod chunkMethod, int charLimit);

        List<ChunkInfo> Chunk(string text, int charLimit);

        void ValidateFileType(string fileName);
    }
}

﻿using CustomTextCliUtils.AppController.Models.Enums;
using CustomTextCliUtils.AppController.Models.ServiceModels.Parser;
using System.Collections.Generic;

namespace CustomTextCliUtils.AppController.Services.Chunker
{
    interface IChunkerService
    {
        public List<string> Chunk(ParseResult parseResult, ChunkMethod chunkMethod, int charLimit);
    }
}

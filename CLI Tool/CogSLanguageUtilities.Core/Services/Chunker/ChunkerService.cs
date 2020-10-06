﻿using Microsoft.CogSLanguageUtilities.Definitions.APIs.Services;
using Microsoft.CogSLanguageUtilities.Definitions.Models.Chunker;
using Microsoft.CogSLanguageUtilities.Definitions.Models.Enums.Chunker;
using Microsoft.CogSLanguageUtilities.Definitions.Models.Parser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.CogSLanguageUtilities.Core.Services.Chunker
{
    public class ChunkerService : IChunkerService
    {
        public List<ChunkInfo> Chunk(ParsedDocument parseResult, ChunkMethod chunkMethod, int charLimit)
        {
            switch (chunkMethod)
            {
                case ChunkMethod.NoChunking:
                    return ApplyNoChunking(parseResult);
                case ChunkMethod.Char:
                    return ChunkByCharacterLimit(parseResult, charLimit);
                case ChunkMethod.Page:
                    return ChunkByPage(parseResult, charLimit);
                default:
                    throw new NotSupportedException($"The chunk type {chunkMethod} isn't supported.");
            }
        }

        /*
         *  Join all elements into a single string
         */
        private List<ChunkInfo> ApplyNoChunking(ParsedDocument parsingResult)
        {
            var finalText = new StringBuilder();
            foreach (var element in parsingResult.Elements)
            {
                finalText.Append(element.Text + Environment.NewLine);
            }
            var text = finalText.ToString().Trim();
            var firstPage = parsingResult.Elements.FirstOrDefault()?.PageNumber;
            var lastPage = parsingResult.Elements.LastOrDefault()?.PageNumber;
            return new List<ChunkInfo>
            {
                new ChunkInfo(1, text, firstPage, lastPage)
            };
        }

        /*
         *  Intuition:
         *      To construct a page, we concatenate elements in the same page to the chunk without exceeding the character limit
         *  Considerations:
         *      1- Chunk doesn't exceed character limit
         *  Special cases:
         *      1- Element length bigger than character limit
         *          - Element will be split into different chunks
         */
        private List<ChunkInfo> ChunkByPage(ParsedDocument parsingResult, int charLimit)
        {
            var pages = new List<ChunkInfo>();
            var currentChunkNumber = 1;
            var currentPageNumber = parsingResult.Elements.FirstOrDefault()?.PageNumber;
            var currentChunk = new StringBuilder();
            parsingResult.Elements.ForEach(e =>
            {
                if ((e.PageNumber != currentPageNumber || e.Text.Length + currentChunk.Length > charLimit) && currentChunk.Length > 0)
                {
                    pages.Add(new ChunkInfo(currentChunkNumber, currentChunk.ToString(), currentPageNumber, currentPageNumber));
                    currentPageNumber = e.PageNumber;
                    currentChunkNumber++;
                    currentChunk.Clear();
                }
                if (e.Text.Length > charLimit)
                {
                    HandleParagraphLengthGreaterThanCharLimit(e.Text, charLimit, ref currentChunkNumber, pages, currentPageNumber);
                }
                else
                {
                    currentChunk.Append(e.Text + Environment.NewLine);
                }
            });
            // handle last page
            if (currentChunk.Length > 0)
            {
                pages.Add(new ChunkInfo(currentChunkNumber, currentChunk.ToString(), currentPageNumber, currentPageNumber));
            }
            return pages;
        }

        /*
         *  Intuition:
         *      To construct a chunk, we concatenate elements to the chunk without exceeding the character limit
         *  Considerations:
         *      1- Chunk doesn't exceed character limit
         *  Special cases:
         *      1- Element length bigger than character limit
         *          - Element will be split into different chunks
         */
        private List<ChunkInfo> ChunkByCharacterLimit(ParsedDocument parsingResult, int charLimit)
        {
            var characterChunks = new List<ChunkInfo>();
            var currentChunkNumber = 1;
            var chunkStartPage = parsingResult.Elements.FirstOrDefault()?.PageNumber;
            var chunkEndPage = parsingResult.Elements.FirstOrDefault()?.PageNumber;
            var currentChunk = new StringBuilder();
            parsingResult.Elements.ForEach(e =>
            {
                if ((e.Text.Length + currentChunk.Length > charLimit) && currentChunk.Length > 0)
                {
                    characterChunks.Add(new ChunkInfo(currentChunkNumber, currentChunk.ToString(), chunkStartPage, chunkEndPage));
                    currentChunkNumber++;
                    chunkStartPage = e.PageNumber;
                    currentChunk.Clear();
                }
                chunkEndPage = e.PageNumber;
                if (e.Text.Length > charLimit)
                {
                    HandleParagraphLengthGreaterThanCharLimit(e.Text, charLimit, ref currentChunkNumber, characterChunks, e.PageNumber);
                }
                else
                {
                    currentChunk.Append(e.Text + Environment.NewLine);
                }
            });
            // handle remaining text
            if (currentChunk.Length > 0)
            {
                characterChunks.Add(new ChunkInfo(currentChunkNumber, currentChunk.ToString(), chunkStartPage, chunkEndPage));
            }
            return characterChunks;
        }

        private void HandleParagraphLengthGreaterThanCharLimit(string paragraphText, int charLimit, ref int currentChunkNumber, List<ChunkInfo> chunks, int? pageNumber)
        {
            var blocks = SplitTextToBlocks(paragraphText, charLimit, ".");
            foreach (var block in blocks)
            {
                chunks.Add(new ChunkInfo(currentChunkNumber++, block, pageNumber, pageNumber));
            }
        }

        private List<string> SplitTextToBlocks(string paragraphText, int charLimit, string delimiter)
        {
            var blocks = new List<string>();
            var lines = paragraphText.Split(delimiter);
            var currentBlock = new StringBuilder();
            foreach (var line in lines)
            {
                if (line.Length + currentBlock.Length > charLimit)
                {
                    blocks.Add(currentBlock.ToString());
                    currentBlock.Clear();
                }
                if (line.Length > charLimit)
                {
                    // handle infinite loop
                    if (delimiter == " ")
                    {
                        var blocksByLength = SplitStringByLength(line, charLimit);
                        blocks.AddRange(blocksByLength);
                    }
                    var blocksByWords = SplitTextToBlocks(line, charLimit, " ");
                    blocks.AddRange(blocksByWords);
                }
                else
                {
                    currentBlock.Append(line + delimiter);
                }
            }
            if (currentBlock.Length > 0)
            {
                blocks.Add(currentBlock.ToString());
            }
            return blocks;
        }

        private List<string> SplitStringByLength(string str, int length)
        {
            if (string.IsNullOrEmpty(str) || length < 1)
            {
                throw new ArgumentException();
            }
            return Enumerable.Range(0, str.Length / length)
                            .Select(i => str.Substring(i * length, length)).ToList();
        }

        public List<ChunkInfo> Chunk(string text, int charLimit)
        {
            throw new NotImplementedException();
        }

        public void ValidateFileType(string fileName)
        { }
    }
}
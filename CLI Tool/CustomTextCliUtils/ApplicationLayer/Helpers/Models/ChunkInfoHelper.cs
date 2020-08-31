﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CustomTextCliUtils.ApplicationLayer.Helpers.Models
{
    public class ChunkInfoHelper
    {
        public static string GetChunkFileName(string fileName, int chunkNumber)
        {
            return $"{Path.GetFileNameWithoutExtension(fileName)}_{chunkNumber + 1}.txt";
        }

        public static string GetChunksummary(string text)
        {
            var firstSpaceIndex = text.IndexOf(' ');
            var lastSpaceIndex = text.LastIndexOf(' ');
            if (text.Length < 10)
            {
                return text;
            }
            if (firstSpaceIndex == -1 || lastSpaceIndex == -1)
            {
                return string.Format("{0} ... {1}", text.Substring(0, 3), text.Substring(text.Length - 4));
            }
            else
            {
                return string.Format("{0} ... {1}", text.Substring(0, firstSpaceIndex), text.Substring(lastSpaceIndex + 1));
            }
        }
    }
}
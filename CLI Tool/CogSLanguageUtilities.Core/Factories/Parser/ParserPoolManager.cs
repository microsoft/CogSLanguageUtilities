﻿using Microsoft.CogSLanguageUtilities.Core.Services.Parser;
using Microsoft.CogSLanguageUtilities.Definitions.APIs.Factories.Parser;
using Microsoft.CogSLanguageUtilities.Definitions.APIs.Services;
using Microsoft.CogSLanguageUtilities.Definitions.Configs.Consts;
using Microsoft.CogSLanguageUtilities.Definitions.Exceptions.Parser;
using Microsoft.CogSLanguageUtilities.Definitions.Models.Configs.Parser;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.CogSLanguageUtilities.Core.Factories.Parser
{
    /*
     * this is an object pool
     * we don't initialize parsing service until it is requested
     */
    public class ParserPoolManager : IParserPoolManager
    {
        private ParserConfigModel _allParsersConfigs;
        private string[] _validDocTypes;
        private MSReadParserService _msreadParser;
        private DocxParserService _docxParser;
        private PlainTextParserService _textParser;

        public ParserPoolManager(ParserConfigModel allParsersConfigs)
        {
            _allParsersConfigs = allParsersConfigs;

            // create valid types array
            var list = new List<string>();
            list.AddRange(Constants.MsReadValidFileTypes);
            list.AddRange(Constants.DocxValidFileTypes);
            _validDocTypes = list.ToArray();
        }

        public IParserService GetParser(string fileType, string fileName = "")
        {
            // plain text files
            if (Constants.PlainTextValidFileTypes.Contains(fileType, StringComparer.OrdinalIgnoreCase))
            {
                if (_textParser == null)
                {
                    _textParser = new PlainTextParserService();
                }
                return _textParser;
            }
            // msread supported files
            else if (Constants.MsReadValidFileTypes.Contains(fileType, StringComparer.OrdinalIgnoreCase))
            {
                if (_msreadParser == null)
                {
                    _msreadParser = new MSReadParserService(_allParsersConfigs.MsRead.CognitiveServiceEndPoint, _allParsersConfigs.MsRead.CongnitiveServiceKey);
                }
                return _msreadParser;
            }
            // docx supported files
            else if (Constants.DocxValidFileTypes.Contains(fileType, StringComparer.OrdinalIgnoreCase))
            {
                if (_docxParser == null)
                {
                    _docxParser = new DocxParserService();
                }
                return _docxParser;
            }
            throw new UnsupportedFileTypeException(fileName, fileType, _validDocTypes);
        }
    }
}

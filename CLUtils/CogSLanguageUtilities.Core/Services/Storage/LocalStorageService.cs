// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
﻿using Microsoft.CogSLanguageUtilities.Definitions.APIs.Services;
using Microsoft.CogSLanguageUtilities.Definitions.Exceptions.Storage;
using Microsoft.CogSLanguageUtilities.Definitions.Models.Enums.Storage;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.CogSLanguageUtilities.Core.Services.Storage
{
    /*
    * some notes:
    *      - we use file exists in all reading methods, in order to throw our custom exception in case file wan't found
    */
    public class LocalStorageService : IStorageService
    {
        private readonly string _sourceDirectory;
        private readonly string _destinationDirectory;

        public LocalStorageService(string sourceDirectory, string destinationDirectory)
        {
            if (!Directory.Exists(sourceDirectory))
            {
                throw new FolderNotFoundException(sourceDirectory);
            }
            if (!Directory.Exists(destinationDirectory))
            {
                throw new FolderNotFoundException(destinationDirectory);
            }
            _sourceDirectory = sourceDirectory;
            _destinationDirectory = destinationDirectory;
        }

        public LocalStorageService(string targetDirectory)
        {
            if (!Directory.Exists(targetDirectory))
            {
                throw new FolderNotFoundException(targetDirectory);
            }
        }

        public async Task<string[]> ListFilesAsync()
        {
            return await Task.FromResult(Directory.GetFiles(_sourceDirectory).Select(i => Path.GetFileName(i)).ToArray());
        }

        public async Task<Stream> ReadFileAsync(string fileName)
        {
            string filePath = Path.Combine(_sourceDirectory, fileName);
            if (await FileExists(fileName))
            {
                try
                {
                    FileStream fs = File.OpenRead(filePath);
                    return await Task.FromResult(fs as Stream);
                }
                catch (UnauthorizedAccessException)
                {
                    throw new UnauthorizedFileAccessException(AccessType.Read.ToString(), Path.Combine(_sourceDirectory, fileName));
                }
            }
            else
            {
                throw new Definitions.Exceptions.Storage.FileNotFoundException(filePath);
            }
        }

        public async Task<string> ReadFileAsStringAsync(string fileName)
        {
            var filePath = Path.Combine(_sourceDirectory, fileName);
            if (await FileExists(fileName))
            {
                return await File.ReadAllTextAsync(filePath);
            }
            else
            {
                throw new Definitions.Exceptions.Storage.FileNotFoundException(filePath);
            }
        }

        public async Task StoreDataAsync(string data, string fileName)
        {
            try
            {
                string filePath = Path.Combine(_destinationDirectory, fileName);
                await File.WriteAllTextAsync(filePath, data);
            }
            catch (UnauthorizedAccessException)
            {
                throw new UnauthorizedFileAccessException(AccessType.Write.ToString(), fileName);
            }
        }

        public async Task<string> ReadAsStringFromAbsolutePathAsync(string filePath)
        {
            if (await FileExistsAbsolutePath(filePath))
            {
                return await File.ReadAllTextAsync(filePath);
            }
            else
            {
                throw new Definitions.Exceptions.Storage.FileNotFoundException(filePath);
            }
        }

        public Task<bool> FileExists(string fileName)
        {
            var filePath = Path.Combine(_sourceDirectory, fileName);
            return Task.FromResult(File.Exists(filePath));
        }

        private Task<bool> FileExistsAbsolutePath(string filePath)
        {
            return Task.FromResult(File.Exists(filePath));
        }

        public Task CreateDirectoryAsync(string directoryName)
        {
            var completePath = Path.Combine(_destinationDirectory, directoryName);
            Directory.CreateDirectory(completePath);
            return Task.CompletedTask;
        }

        public async Task StoreDataToDirectoryAsync(string data, string directoryName, string fileName)
        {
            var relativePath = Path.Combine(directoryName, fileName);
            await StoreDataAsync(data, relativePath);
        }
    }
}

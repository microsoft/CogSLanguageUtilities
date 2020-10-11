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
        private readonly string _targetDirectory;

        public LocalStorageService(string targetDirectory)
        {
            if (!Directory.Exists(targetDirectory))
            {
                throw new FolderNotFoundException(targetDirectory);
            }
            _targetDirectory = targetDirectory;
        }

        public async Task<string[]> ListFilesAsync()
        {
            return await Task.FromResult(Directory.GetFiles(_targetDirectory).Select(i => Path.GetFileName(i)).ToArray());
        }

        public async Task<Stream> ReadFileAsync(string fileName)
        {
            string filePath = Path.Combine(_targetDirectory, fileName);
            if (await FileExists(fileName))
            {
                try
                {
                    FileStream fs = File.OpenRead(filePath);
                    return await Task.FromResult(fs as Stream);
                }
                catch (UnauthorizedAccessException)
                {
                    throw new UnauthorizedFileAccessException(AccessType.Read.ToString(), Path.Combine(_targetDirectory, fileName));
                }
            }
            else
            {
                throw new Definitions.Exceptions.Storage.FileNotFoundException(filePath);
            }
        }

        public async Task<string> ReadFileAsStringAsync(string fileName)
        {
            var filePath = Path.Combine(_targetDirectory, fileName);
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
                string filePath = Path.Combine(_targetDirectory, fileName);
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

        public async Task<bool> FileExists(string fileName)
        {
            var filePath = Path.Combine(_targetDirectory, fileName);
            return await Task.Run(() =>
            {
                return File.Exists(filePath);
            });
        }

        private async Task<bool> FileExistsAbsolutePath(string filePath)
        {
            return await Task.Run(() =>
            {
                return File.Exists(filePath);
            });
        }

        public async Task CreateDirectoryAsync(string directoryName)
        {
            await Task.Run(() =>
            {
                var completePath = Path.Combine(_targetDirectory, directoryName);
                return Directory.CreateDirectory(completePath);
            });
        }

        public async Task StoreDataToDirectoryAsync(string data, string directoryName, string fileName)
        {
            var relativePath = Path.Combine(directoryName, fileName);
            await StoreDataAsync(data, relativePath);
        }
    }
}

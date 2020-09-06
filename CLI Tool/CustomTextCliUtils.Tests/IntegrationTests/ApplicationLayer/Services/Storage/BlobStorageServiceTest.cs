﻿using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.CustomTextCliUtils.ApplicationLayer.Exceptions.Storage;
using Microsoft.CustomTextCliUtils.ApplicationLayer.Services.Storage;
using Microsoft.CustomTextCliUtils.Tests.Configs;
using System;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace Microsoft.CustomTextCliUtils.Tests.IntegrationTests.ApplicationLayer.Services.Storage
{
    public class BlobStorageServiceTest : IDisposable
    {
        private static readonly string _connectionString = Secrets.StorageAccountConnectionString;
        private readonly string _testContainerName;
        private readonly BlobContainerClient _blobContainerClient;

        public BlobStorageServiceTest()
        {
            _testContainerName = DateTimeOffset.Now.ToUnixTimeMilliseconds().ToString();
            BlobServiceClient blobServiceClient = new BlobServiceClient(_connectionString);
            _blobContainerClient = blobServiceClient.CreateBlobContainer(_testContainerName);
        }

        public void Dispose()
        {
            _blobContainerClient.Delete();
        }

        public static TheoryData BlobStorageConnectionTestData()
        {
            string invalidConnectionString = "DefaultEndpointsProtocol=https;AccountName=invalid;AccountKey=5UvtQ8CiXwDXg63QyEgtReW3s31KTXMvT5UfjnX1XgAW1DU390nKAlkCeBn7DUyDgaaQdm5TZt3iB7DfdUlD5A==;EndpointSuffix=core.windows.net";
            string invalidContainerName = "nonexistentcontainer";
            return new TheoryData<string, string, Exception>
            {
                {
                    _connectionString,
                    null,
                    null
                },
                {
                    invalidConnectionString,
                    null,
                    new InvalidBlobStorageConnectionStringException(invalidConnectionString)
                },
                {
                    _connectionString,
                    invalidContainerName,
                    new BlobContainerNotFoundException(invalidContainerName)
                }
            };
        }

        [Theory]
        [MemberData(nameof(BlobStorageConnectionTestData))]
        public void BlobStorageConnectionTest(string connectionString, string containerName, Exception excpectedException)
        {
            containerName = containerName ?? _testContainerName;
            if (excpectedException == null)
            {
                new BlobStorageService(connectionString, containerName);
            }
            else
            {
                Assert.Throws(excpectedException.GetType(), () => new BlobStorageService(connectionString, containerName));
            }
        }

        [Fact]
        public void StoreDataTest()
        {
            string fileName = "storageTest.txt";
            string expected = "StoreDataTest text for testing";
            string actual = "";
            IStorageService storageService = new BlobStorageService(_connectionString, _testContainerName);
            storageService.StoreDataAsync(expected, fileName);
            BlobClient blobClient = _blobContainerClient.GetBlobClient(fileName);
            BlobDownloadInfo download = blobClient.Download();
            using (StreamReader sr = new StreamReader(download.Content))
            {
                actual = sr.ReadToEnd();
            }
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task ReadFileTest()
        {
            // Write file to blob store
            string fileName = "storageTest.txt";
            string expected = "ReadFileTest text for testing";
            string actual = "";
            UploadFileHelper(fileName, expected);
            IStorageService storageService = new BlobStorageService(_connectionString, _testContainerName);
            Stream file = await storageService.ReadFileAsync(fileName);
            using (StreamReader sr = new StreamReader(file))
            {
                actual = sr.ReadToEnd();
            }
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task ReadFileAsStringTestAsync()
        {
            // Write file to blob store
            string fileName = "storageTest.txt";
            string expected = "ReadFileAsStringTest text for testing";
            UploadFileHelper(fileName, expected);
            IStorageService storageService = new BlobStorageService(_connectionString, _testContainerName);
            string actual = await storageService.ReadFileAsStringAsync(fileName);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ListFilesTest()
        {
            string[] expectedFiles = new string[] { "file1", "file2", "file3" };
            Array.ForEach(expectedFiles, fileName =>
            {
                UploadFileHelper(fileName, "text");
            });
            IStorageService storageService = new BlobStorageService(_connectionString, _testContainerName);
            string[] actualFiles = storageService.ListFiles();
            Assert.Equal(expectedFiles, actualFiles);
        }

        private void UploadFileHelper(string fileName, string expected)
        {
            BlobClient blobClient = _blobContainerClient.GetBlobClient(fileName);
            using (MemoryStream stream = new MemoryStream())
            {
                using (StreamWriter sw = new StreamWriter(stream))
                {
                    sw.Write(expected);
                    sw.Flush();
                    stream.Position = 0;
                    blobClient.Upload(stream, overwrite: true);
                }
            }
        }

        private void DeleteAllBlobs()
        {
            Parallel.ForEach(_blobContainerClient.GetBlobs(), x => _blobContainerClient.DeleteBlob(x.Name));
        }
    }
}
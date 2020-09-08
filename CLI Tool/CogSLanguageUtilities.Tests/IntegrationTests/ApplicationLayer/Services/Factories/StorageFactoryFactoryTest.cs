using Microsoft.CogSLanguageUtilities.Core.Factories.Storage;
using Microsoft.CogSLanguageUtilities.Definitions.Models.Enums.Misc;
using System;
using Xunit;

namespace Microsoft.CustomTextCliUtils.Tests.IntegrationTests.ApplicationLayer.Services.Factories
{
    public class StorageFactoryFactoryTest
    {
        public static TheoryData StorageFactoryCreationTestData()
        {
            return new TheoryData<TargetStorage, Type>
            {
                {
                    TargetStorage.Destination,
                    typeof(DestinationStorageFactory)
                },
                {
                    TargetStorage.Source,
                    typeof(SourceStorageFactory)
                }
            };
        }

        [Theory]
        [MemberData(nameof(StorageFactoryCreationTestData))]
        public void StorageFactoryCreationTest(TargetStorage targetStorage, Type factoryType)
        {
            StorageFactoryFactory factoryFactory = new StorageFactoryFactory();
            IStorageFactory storageFactory = factoryFactory.CreateStorageFactory(targetStorage);
            Assert.True(storageFactory.GetType().Equals(factoryType));
        }
    }
}

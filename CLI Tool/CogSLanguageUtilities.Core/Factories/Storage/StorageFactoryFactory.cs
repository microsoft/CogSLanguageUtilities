﻿using Microsoft.CogSLanguageUtilities.Definitions.Models.Enums;
using System;

namespace Microsoft.CustomTextCliUtils.ApplicationLayer.Factories.Storage
{
    public class StorageFactoryFactory : IStorageFactoryFactory
    {
        public IStorageFactory CreateStorageFactory(TargetStorage targetStorage)
        {
            switch (targetStorage)
            {
                case TargetStorage.Source:
                    return new SourceStorageFactory();
                case TargetStorage.Destination:
                    return new DestinationStorageFactory();
                default:
                    throw new NotSupportedException($"The target storage {targetStorage} isn't supported.");
            }
        }
    }
}

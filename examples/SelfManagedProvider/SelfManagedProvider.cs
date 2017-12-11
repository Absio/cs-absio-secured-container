using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Absio.Sdk.Container;
using Absio.Sdk.Providers;
using IContainer = Absio.Sdk.Container.IContainer;

namespace SelfManagedProvider
{
    // TODO - docs on classes and methods
    public class SelfManagedProvider : ServerCacheOfsProvider
    {
        public new async Task<SecuredContainer> CreateAsync(byte[] content, object customHeaderObject = null,
            List<ContainerAccessLevel> accessLevels = null, string type = null)
        {
            var container = ServerProvider.BuildNewContainer(content, customHeaderObject, accessLevels, type);
            var securedContainer = await ContainerEncryptionService.EncryptAsync(container);

            var encryptedData = securedContainer.Bytes();
            var newContainer = await SecuredContainer.CreateAsync(null, securedContainer.Metadata);

            await ServerProvider.SecuredContainerMapper.CreateOrUpdateAsync(newContainer);
            await OfsProvider.CreateAsync(newContainer);

            return await SecuredContainer.CreateAsync(encryptedData, newContainer.Metadata);
        }

        public async Task<IContainer> GetAsync(Guid containerId, byte[] encryptedData)
        {
            var metadata = await GetMetadataAsync(containerId);
            var newContainer = await base.CreateAsync(encryptedData, metadata);
            return await ContainerEncryptionService.DecryptAsync(newContainer);
        }

        public new async Task<SecuredContainer> UpdateAsync(Guid containerId, byte[] content = null,
            object customHeaderObject = null,
            List<ContainerAccessLevel> accessLevels = null, string type = null)
        {
            var container = ServerProvider.BuildNewContainer(content, customHeaderObject, accessLevels, type);
            var securedContainer = await ContainerEncryptionService.EncryptAsync(container);

            var encryptedData = securedContainer.Bytes();
            var newContainer = await SecuredContainer.CreateAsync(null, securedContainer.Metadata);

            await ServerProvider.SecuredContainerMapper.CreateOrUpdateAsync(newContainer);
            await OfsProvider.CreateOrUpdateAsync(newContainer);

            return await SecuredContainer.CreateAsync(encryptedData, newContainer.Metadata);
        }

        public async Task<SecuredContainer> UpdateContentAndHeaderAsync(Guid containerId, byte[] content = null, object customHeaderObject = null)
        {
            var metadata = await GetMetadataAsync(containerId);
            var container = ServerProvider.BuildNewContainer(content, customHeaderObject, metadata.ContainerAccessLevels, metadata.Type);
            return await ContainerEncryptionService.EncryptAsync(container);
        }

        #region Broken ServerCacheOfs Methods - do not use

        public new async Task<IContainer> GetAsync(Guid containerId)
        {
            throw new AccessViolationException("This method is not supported.");
        }

        public new async Task<SecuredContainer> UpdateContentAsync(Guid containerId, byte[] content = null)
        {
            throw new AccessViolationException("This method is not supported.");
        }

        public new async Task<SecuredContainer> UpdateHeaderAsync(Guid containerId, object customHeaderObject = null)
        {
            throw new AccessViolationException("This method is not supported.");
        }

        #endregion
    }
}

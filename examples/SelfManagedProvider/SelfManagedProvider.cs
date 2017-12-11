using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Absio.Sdk.Container;
using Absio.Sdk.Exceptions;
using Absio.Sdk.Providers;

namespace SelfManagedProvider
{
    /// <summary>
    /// This class extends the ServerCacheOfsProvider class to show one way you could return encrypted content to a user to let them manage it themselves.
    /// There are many ways one could accomplish this.  This is just one way.
    /// </summary>
    public class SelfManagedProvider : ServerCacheOfsProvider
    {
        /// <summary>
        ///     Creates and persists the metadata for a new SecuredContainer on the Absio Broker application, caches it in the OFS and returns it 
        ///     (with the encrytped data).  Use the returned value to learn the assigned ID.
        /// </summary>
        /// <remarks>Calling this method requires an authenticated session.  See LoginAsync for authenticating.</remarks>
        /// <param name="content">The container payload.</param>
        /// <param name="customHeaderObject">
        /// 
        /// The optional header portion of the container. This object will be serialized as
        /// JSON. Default: null.
        /// </param>
        /// <param name="accessLevels">
        ///     Optional. This is the defined access for all users.  If null, the user creating the container will get full access
        ///     (ContainerAccessLevel.DefaultOwnerPermissions). If any access is defined then the user creating the container
        ///     will get the defined access or no access if none is defined.  By default this list is null.
        /// </param>
        /// <param name="type">Optional string used to categorize the Container. Default: null.</param>
        /// <returns>The created SecuredContainer.</returns>
        /// <exception cref="InvalidOperationException">Thrown if the session is not authenticated.</exception>
        /// <exception cref="AlreadyExistsException">Thrown if a container with this ID exists on the Absio Broker application already.</exception>
        /// <exception cref="NotFoundException">Thrown if a user with access does not exist on the Absio Broker application.</exception>
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

        /// <summary>
        /// This pulls the metadata from the system (checks the cache first and Broker application second) and uses it 
        /// to decrypt the passed in encrypted data. The results are packaged and returned in an IContainer.
        /// </summary>
        /// <param name="containerId">The id of the metadata stored in the system.</param>
        /// <param name="encryptedData">The data to decrypt using the stored metadata.</param>
        /// <returns>The decrytped container (with associated metadata).</returns>
        public async Task<IContainer> GetAsync(Guid containerId, byte[] encryptedData)
        {
            var metadata = await GetMetadataAsync(containerId);
            var securedContainer = await SecuredContainer.CreateAsync(encryptedData, metadata);
            return await ContainerEncryptionService.DecryptAsync(securedContainer);
        }

        /// <summary>
        ///     Update the metadata of the secured container with the given ID on the Absio Broker application
        ///     and then cache the resulting container in the OFS.  In additino encrypt the passed in content and header
        ///     using that metadata and return the results as a secured container.
        /// </summary>
        /// <remarks>Calling this method requires an authenticated session.  See LoginAsync for authenticating.</remarks>
        /// <param name="containerId">The ID of the secured container.</param>
        /// <param name="content">The content to be encrypted and returned in the secured container.</param>
        /// <param name="customHeaderObject">The header to be encrypted and returned in the secured container.  Defaults to null</param>
        /// <param name="accessLevels">
        ///     Optional. This is the defined access for all users.  If null, the user updating the container will get full access
        ///     (ContainerAccessLevel.DefaultOwnerPermissions). If any access is defined then the user creating the container
        ///     will get the defined access or no access if none is defined.  By default this list is null.
        /// </param>
        /// <param name="type">Optional. This is used to categorize the Container. Default: null.</param>
        /// <returns>The updated SecuredContainer.</returns>
        /// <exception cref="InvalidOperationException">Thrown if the session is not authenticated.</exception>
        /// <exception cref="InsufficientPermissionsException">Thrown if the user does not have sufficient permissions to update all aspects of the secured container.</exception>
        public new async Task<SecuredContainer> UpdateAsync(Guid containerId, byte[] content = null,
            object customHeaderObject = null,
            List<ContainerAccessLevel> accessLevels = null, string type = null)
        {
            var container = ServerProvider.BuildNewContainer(content, customHeaderObject, accessLevels, type);
            var securedContainer = await ContainerEncryptionService.EncryptAsync(container);

            var encryptedData = securedContainer.Bytes();
            var wrongIdMetadata = securedContainer.Metadata;
            var metadata = new ContainerMetadata(containerId, wrongIdMetadata.CreatedAt, wrongIdMetadata.CreatedBy, wrongIdMetadata.Length, wrongIdMetadata.ModifiedAt, wrongIdMetadata.ModifiedBy, wrongIdMetadata.Type);
            metadata.ContainerAccessLevels = wrongIdMetadata.ContainerAccessLevels;
            var newContainer = await SecuredContainer.CreateAsync(null, metadata);


            await ServerProvider.SecuredContainerMapper.CreateOrUpdateAsync(newContainer);
            await OfsProvider.CreateOrUpdateAsync(newContainer);

            return await SecuredContainer.CreateAsync(encryptedData, newContainer.Metadata);
        }

        /// <summary>
        /// This is used to update (encrypt) the content and header.  The new container keys and updated RKBs in the access are stored appropriately. 
        /// </summary>
        /// <param name="containerId">The ID of the secured container.</param>
        /// <param name="content">The new content to encrypt.</param>
        /// <param name="customHeaderObject">The new header to encrypt.</param>
        /// <returns>The encrypted result as a secured container.</returns>
        public async Task<SecuredContainer> UpdateContentAndHeaderAsync(Guid containerId, byte[] content = null,
            object customHeaderObject = null)
        {
            var metadata = await GetMetadataAsync(containerId);
            var container = ServerProvider.BuildNewContainer(content, customHeaderObject,
                metadata.ContainerAccessLevels, metadata.Type);
            return await ContainerEncryptionService.EncryptAsync(container);
        }

        #region Invalid Methods

        /// <summary>
        /// This method from the ServerCacheOfsProvider does not make sense to call since the content is manged by the caller.  It will fail if called.
        /// </summary>
        /// <param name="containerId"></param>
        /// <returns></returns>
        public new async Task<IContainer> GetAsync(Guid containerId)
        {
            throw new AccessViolationException("This method is not supported.");
        }

        /// <summary>
        /// This method from the ServerCacheOfsProvider does not make sense to call since the content and header are managed by the caller.  It will fail if called.
        /// </summary>
        /// <param name="containerId"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public new async Task<SecuredContainer> UpdateContentAsync(Guid containerId, byte[] content = null)
        {
            throw new AccessViolationException("This method is not supported.");
        }

        /// <summary>
        /// This method from the ServerCacheOfsProvider does not make sense to call since the content and header are managed by the caller.  It will fail if called.
        /// </summary>
        /// <param name="containerId"></param>
        /// <param name="customHeaderObject"></param>
        /// <returns></returns>
        public new async Task<SecuredContainer> UpdateHeaderAsync(Guid containerId, object customHeaderObject = null)
        {
            throw new AccessViolationException("This method is not supported.");
        }

        #endregion
    }
}
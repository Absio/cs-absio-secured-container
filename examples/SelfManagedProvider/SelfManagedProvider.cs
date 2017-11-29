using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Absio.Sdk.Container;
using Absio.Sdk.Providers;

namespace SelfManagedProvider
{
    public class SelfManagedProvider : BaseProvider
    {
        public ServerCacheOfsProvider ServerCacheOfsProvider { get; }

        public SelfManagedProvider()
        {
            ServerCacheOfsProvider = new ServerCacheOfsProvider();
        }

        /// <summary>
        ///     Initialize the Container Provider.
        /// </summary>
        public void Initialize(string serverUrl, string apiKey, string applicationName, String ofsRootDirectory = null,
            bool partitionDataByUser = false)
        {
            ServerCacheOfsProvider.Initialize(serverUrl, apiKey, applicationName, ofsRootDirectory,
                partitionDataByUser);
        }

        /// <summary>
        ///     Save the container data so that it is not stored on the server / ofs
        /// </summary>
        public async Task<SecuredContainer> CreateAsync(byte[] content, object customHeaderObject = null,
            List<ContainerAccessLevel> accessLevels = null, string type = null)
        {
            var container = ServerCacheOfsProvider.ServerProvider.BuildNewContainer(content, customHeaderObject, accessLevels, type);
            var securedContainer = await ServerCacheOfsProvider.ContainerEncryptionService.EncryptAsync(container);

            var encryptedData = securedContainer.Bytes();
            var newContainer = await SecuredContainer.CreateAsync(new byte[0], securedContainer.Metadata);

            await ServerCacheOfsProvider.ServerProvider.SecuredContainerMapper.CreateOrUpdateAsync(newContainer);
            await ServerCacheOfsProvider.OfsProvider.CreateAsync(newContainer);

            return await SecuredContainer.CreateAsync(encryptedData, newContainer.Metadata);
        }

        /// <summary>
        ///     Delete the container keys & access info from the server and ofs.
        /// </summary>
        public async Task DeleteAsync(Guid containerId)
        {
            await ServerCacheOfsProvider.DeleteUserAsync();
        }

        /// <summary>
        ///     Return a container that's as populated as it can be.
        /// </summary>
        public async Task<IContainer> GetAsync(Guid containerId, byte[] encryptedData)
        {
            var securedContainer = await ServerCacheOfsProvider.ServerProvider.GetSecuredContainerAsync(containerId);
            var newContainer = await SecuredContainer.CreateAsync(encryptedData, securedContainer.Metadata);
            return await ServerCacheOfsProvider.ContainerEncryptionService.DecryptAsync(newContainer);
        }

        public async Task LoginAsync(Guid userId, string password = null, string passphrase = null)
        {
            await ServerCacheOfsProvider.LogInAsync(userId, password, passphrase);
        }

        public void Logout()
        {
            ServerCacheOfsProvider.Logout();
        }

        public async Task RegisterAsync(string password, string passphrase, string reminder = null)
        {
            await ServerCacheOfsProvider.RegisterAsync(password, passphrase, reminder);
        }

    }
}

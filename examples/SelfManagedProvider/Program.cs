using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Absio.Sdk.Crypto.Util;
using Absio.Sdk.Exceptions;

namespace SelfManagedProvider
{
    class Program
    {
        internal const string ApiKey = "7b86390a-38af-4ecc-b9b7-2c663ab39bea";
        internal const string ApplicationName = "AbsioSdk Tests";
        internal const string ServerUrl = "https://apiint.absio.com";
        internal static string OfsRoot = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "data";

        static void Main(string[] args)
        {
            Work().Wait();
        }

        private static async Task Work()
        {
            //This container is created on the server and in the OFS, but the content is not stored in either location.
            var selfProvider = new SelfManagedProvider();
            selfProvider.Initialize(ServerUrl, ApiKey, ApplicationName, OfsRoot);

            await selfProvider.RegisterAsync("Password#1", "Passphrase#1");
            Debug.Assert(selfProvider.UserId != null, "selfProvider.UserId != null");
            Guid userId = (Guid) selfProvider.UserId; 
            
            selfProvider.Logout();

            await selfProvider.LogInAsync(userId, "Password#1", "Passphrase#1");

            //Now you, the app developer, decide where to store the data.  Here's an example:
            var content = "Content";
            string path = Path.GetTempFileName();
            var type = "type";
            var container = await selfProvider.CreateAsync(Encoding.ASCII.GetBytes(content), "Custom Header", null, type);

            using (FileStream fs = File.Open(path, FileMode.OpenOrCreate))
            {
                await fs.WriteAsync(container.Bytes(), 0, container.Bytes().Length);
            }

            //Now to fetch the container back and merge it with your encrypted data:
            var fileBytes = File.ReadAllBytes(path);

            if (!GeneralUtils.ListEquals(fileBytes, container.Bytes()))
            {
                throw new ArgumentException("The file contents was not the expected container.");
            }

            var newContainer = await selfProvider.GetAsync(container.Metadata.Id, fileBytes);
            var content2 = Encoding.ASCII.GetString(newContainer.Content);

            if (content != content2)
            {
                throw new ArgumentException("The container content did not match.");
            }

            if (newContainer.Metadata.Type != type)
            {
                throw new ArgumentException("The container types did not match.");
            }

            var newContent = "new content";
            var newHeader = "new header";
            var newType = "new type";
            var updatedContainer = await selfProvider.UpdateAsync(container.Metadata.Id, Encoding.ASCII.GetBytes(newContent), newHeader, null, newType);
            var decryptedUpdatedContainer = await selfProvider.GetAsync(container.Metadata.Id, updatedContainer.Bytes());
            var decryptedUpdatedContent = Encoding.ASCII.GetString(decryptedUpdatedContainer.Content);

            if (newContent != decryptedUpdatedContent)
            {
                throw new ArgumentException("The decrypted content was not the same as the original.");
            }

            if (decryptedUpdatedContainer.Metadata.Type != newType)
            {
                throw new ArgumentException("The decrypted type was not the same as the original.");
            }

            await selfProvider.DeleteAsync(container.Metadata.Id);

            try
            {
                await selfProvider.GetAsync(container.Metadata.Id, updatedContainer.Bytes());
                throw new ArgumentException("The container was available after a delete request.");
            }
            catch (NotFoundException )
            {
            }

            await selfProvider.DeleteUserAsync();
        }
    }
}

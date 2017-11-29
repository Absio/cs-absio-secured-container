using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Absio.Sdk.Container;
using Absio.Sdk.Providers;

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

            await selfProvider.LoginAsync(new Guid("083c92bd-607b-447c-9cd4-6a6294e04250"), "Password#1", "Passphrase#1");

            //Now you, the app developer, decide where to store the data.  Here's an example:
            var content = "Content";
            string path = @"d:\temp\MyTest.container";
            var container = await selfProvider.CreateAsync(Encoding.ASCII.GetBytes(content), "Custom Header");

            using (FileStream fs = File.Open(path, FileMode.OpenOrCreate))
            {
                await fs.WriteAsync(container.Bytes(), 0, container.Bytes().Length);
            }

            //Now to fetch the container back and merge it with your encrypted data:
            var fileBytes = File.ReadAllBytes(path);
            var newContainer = await selfProvider.GetAsync(container.Metadata.Id, fileBytes);
            var content2 = Encoding.ASCII.GetString(newContainer.Content);
        }
    }
}

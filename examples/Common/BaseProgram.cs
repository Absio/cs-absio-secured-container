using System;
using System.Collections.Generic;
using Absio.Sdk.Events;
using Absio.Sdk.Providers;
using CommandLine;

namespace Common
{
    public class BaseProgram
    {
        protected static IAbsioProvider Provider;
        protected static ProviderType Type = ProviderType.ServerCacheOfs;
        protected static Guid? UserId;
        protected static string ApiKey;
        protected static string ServerUrl;
        protected static string BaseDir;

        protected enum ProviderType
        {
            Ofs,
            Server,
            ServerCacheOfs
        }

        protected static string SessionInfo
        {
            get
            {
                var sessionInfo = "Api Key : " + ApiKey;
                sessionInfo += "\n";
                sessionInfo += "Host : " + ServerUrl;
                sessionInfo += "\n";
                sessionInfo += "Provider : " + Type + "\n";

                if (UserId != null)
                {
                    sessionInfo += "User : " + UserId;
                    sessionInfo += "\n";
                }
                return sessionInfo;
            }
        }

        protected static bool SetProviderType(string providerType)
        {
            // Set the provider type.
            if (ProviderType.Ofs.ToString().Equals(providerType))
            {
                Type = ProviderType.Ofs;
            }
            else if (ProviderType.Server.ToString().Equals(providerType))
            {
                Type = ProviderType.Server;
            }
            else if (string.IsNullOrEmpty(providerType) || ProviderType.ServerCacheOfs.ToString().Equals(providerType))
            {
                Type = ProviderType.ServerCacheOfs;
            }
            else
            {
                return false;
            }

            return true;
        }

        protected static void InitializeProvider(string applicationName)
        {
            switch (Type)
            {
                case ProviderType.ServerCacheOfs:
                    var serverCacheOfsProvider = new ServerCacheOfsProvider();
                    serverCacheOfsProvider.Initialize(ServerUrl, ApiKey, applicationName, BaseDir, true);
                    Provider = serverCacheOfsProvider;
                    break;
                case ProviderType.Ofs:
                    var ofsProvider = new OfsProvider();
                    ofsProvider.Initialize(BaseDir, true);
                    Provider = ofsProvider;
                    break;
                case ProviderType.Server:
                    var serverProvider = new ServerProvider();
                    serverProvider.Initialize(ServerUrl, ApiKey, applicationName);
                    Provider = serverProvider;
                    break;
            }

            Console.WriteLine("Using the {0} provider.", Type.ToString());
        }

        protected static void ChangeCredentialsWithProvider(string password, string passphrase, string reminder)
        {
            switch (Type)
            {
                case ProviderType.ServerCacheOfs:
                    ((ServerCacheOfsProvider)Provider).ChangeCredentialsAsync(password, passphrase, reminder).Wait();
                    break;
                case ProviderType.Ofs:
                    ((OfsProvider)Provider).ChangeCredentialsAsync(password, passphrase).Wait();
                    break;
                case ProviderType.Server:
                    ((ServerProvider)Provider).ChangeCredentialsAsync(password, passphrase, reminder).Wait();
                    break;
            }
        }

        protected static Guid RegisterWithProvider(string password, string passphrase, string reminder)
        {
            switch (Type)
            {
                case ProviderType.ServerCacheOfs:
                    ((ServerCacheOfsProvider)Provider).RegisterAsync(password, passphrase, reminder).Wait();
                    break;
                case ProviderType.Ofs:
                    ((OfsProvider)Provider).RegisterAsync(password, passphrase).Wait();
                    break;
                case ProviderType.Server:
                    ((ServerProvider)Provider).RegisterAsync(password, passphrase, reminder).Wait();
                    break;
            }

            return (Guid)Provider.UserId;
        }

        protected static string GetReminderFromProvider(Guid userId)
        {
            string reminder = null;
            switch (Type)
            {
                case ProviderType.ServerCacheOfs:
                    reminder = ((ServerCacheOfsProvider)Provider).GetReminderAsync(userId).Result;
                    break;
                case ProviderType.Server:
                    reminder = ((ServerProvider)Provider).GetReminderAsync(userId).Result;
                    break;
            }

            return reminder;
        }

        protected static List<ContainerEvent> GetEventsFromProvider(EventActionType? actionType = null,
            long? startingId = null, long? endingId = null, Guid? containerId = null, string containerType = null)
        {
            List<ContainerEvent> events = null;
            switch (Type)
            {
                case ProviderType.ServerCacheOfs:
                    events = ((ServerCacheOfsProvider)Provider).GetEventsAsync(actionType, startingId, endingId, containerId, containerType).Result;
                    break;
                case ProviderType.Server:
                    events = ((ServerProvider)Provider).GetEventsAsync(actionType, startingId, endingId, containerId, containerType).Result;
                    break;
            }

            return events;
        }

        protected static void ResetSession(string applicationName)
        {
            Provider.Logout();
            UserId = null;
            InitializeProvider(applicationName);
        }

        protected class ProgramOptions
        {
            #region Properties, Indexers

            [Option(Required = true)]
            public string ApiKey { get; set; }

            [Option(Required = true)]
            public string Url { get; set; }

            [Option(Required = false)]
            public string Provider { get; set; }

            #endregion
        }
    }
}
using System;
using System.Collections.Generic;
using Absio.Sdk.Events;
using Absio.Sdk.Providers;
using CommandLine;

namespace Common
{
    public class BaseProgram
    {
        protected static IAbsioProvider _provider;
        protected static ProviderType _providerType = ProviderType.ServerCacheOfs;
        protected static Guid? _userId;
        protected static string _apiKey;
        protected static string _serverUrl;
        protected static string _baseDir;

        protected static string ApiKey => _apiKey;
        protected static string ServerUrl => _serverUrl;

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
                sessionInfo += "Provider : " + _providerType + "\n";

                if (_userId != null)
                {
                    sessionInfo += "User : " + _userId;
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
                _providerType = ProviderType.Ofs;
            }
            else if (ProviderType.Server.ToString().Equals(providerType))
            {
                _providerType = ProviderType.Server;
            }
            else if (ProviderType.ServerCacheOfs.ToString().Equals(providerType))
            {
                _providerType = ProviderType.ServerCacheOfs;
            }
            else
            {
                return false;
            }

            return true;
        }

        protected static void InitializeProvider(string applicationName)
        {
            switch (_providerType)
            {
                case ProviderType.ServerCacheOfs:
                    var serverCacheOfsProvider = new ServerCacheOfsProvider();
                    serverCacheOfsProvider.Initialize(_serverUrl, _apiKey, applicationName, _baseDir, true);
                    _provider = serverCacheOfsProvider;
                    break;
                case ProviderType.Ofs:
                    var ofsProvider = new OfsProvider();
                    ofsProvider.Initialize(_baseDir, true);
                    _provider = ofsProvider;
                    break;
                case ProviderType.Server:
                    var serverProvider = new ServerProvider();
                    serverProvider.Initialize(_serverUrl, _apiKey, applicationName);
                    _provider = serverProvider;
                    break;
            }

            Console.WriteLine("Using the {0} provider.", _providerType.ToString());
        }

        protected static void ChangeCredentialsWithProvider(string password, string passphrase, string reminder)
        {
            switch (_providerType)
            {
                case ProviderType.ServerCacheOfs:
                    ((ServerCacheOfsProvider)_provider).ChangeCredentialsAsync(password, passphrase, reminder).Wait();
                    break;
                case ProviderType.Ofs:
                    ((OfsProvider)_provider).ChangeCredentialsAsync(password, passphrase).Wait();
                    break;
                case ProviderType.Server:
                    ((ServerProvider)_provider).ChangeCredentialsAsync(password, passphrase, reminder).Wait();
                    break;
            }
        }

        protected static Guid RegisterWithProvider(string password, string passphrase, string reminder)
        {
            switch (_providerType)
            {
                case ProviderType.ServerCacheOfs:
                    ((ServerCacheOfsProvider)_provider).RegisterAsync(password, passphrase, reminder).Wait();
                    break;
                case ProviderType.Ofs:
                    ((OfsProvider)_provider).RegisterAsync(password, passphrase).Wait();
                    break;
                case ProviderType.Server:
                    ((ServerProvider)_provider).RegisterAsync(password, passphrase, reminder).Wait();
                    break;
            }

            return (Guid)_provider.UserId;
        }

        protected static string GetReminderFromProvider(Guid userId)
        {
            string reminder = null;
            switch (_providerType)
            {
                case ProviderType.ServerCacheOfs:
                    reminder = ((ServerCacheOfsProvider)_provider).GetReminderAsync(userId).Result;
                    break;
                case ProviderType.Server:
                    reminder = ((ServerProvider)_provider).GetReminderAsync(userId).Result;
                    break;
            }

            return reminder;
        }

        protected static List<ContainerEvent> GetEventsFromProvider(EventActionType? actionType = null,
            long? startingId = null, long? endingId = null, Guid? containerId = null, string containerType = null)
        {
            List<ContainerEvent> events = null;
            switch (_providerType)
            {
                case ProviderType.ServerCacheOfs:
                    events = ((ServerCacheOfsProvider)_provider).GetEventsAsync(actionType, startingId, endingId, containerId, containerType).Result;
                    break;
                case ProviderType.Server:
                    events = ((ServerProvider)_provider).GetEventsAsync(actionType, startingId, endingId, containerId, containerType).Result;
                    break;
            }

            return events;
        }

        protected static void ResetSession(string applicationName)
        {
            _provider.Logout();
            _userId = null;
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
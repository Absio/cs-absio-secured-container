using Absio.Sdk.Providers;
using Colorful;
using CommandLine;
using Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using OpenSSL.Core;
using Console = Colorful.Console;

// Warnings disabled based on CommandLine requirements
// ReSharper disable ClassNeverInstantiated.Local
// ReSharper disable UnusedAutoPropertyAccessor.Local
namespace UserManagementUtility
{
    public class Program : BaseProgram
    {
        #region Static Fields and Constants
        public const string ApplicationName = "cs-cli-user-mgmt-app";

        #endregion

        #region Properties, Indexers

        private static string AbsioConsoleHeader
        {
            get
            {
                var absioConsoleHeader = new Figlet().ToAscii("Absio").ConcreteValue;
                absioConsoleHeader += "\n\n";
                absioConsoleHeader += "User Management Utility";
                absioConsoleHeader += "\n\n";
                return absioConsoleHeader;
            }
        }

        #endregion

        #region Private and Protected Methods

        public static void Main(string[] args)
        {
            FIPS.Enabled = true;
            ProgramOptions parsed = null;
            IEnumerable<Error> notParsed = null;
            Parser.Default.ParseArguments<ProgramOptions>(args)
                .WithParsed(options => parsed = options)
                .WithNotParsed(errors => notParsed = errors);
            if (notParsed != null)
            {
                ShowMessageAndWaitForKeyPress("An error occurred.");
                return;
            }

            _apiKey = parsed.ApiKey;
            _serverUrl = parsed.Url;
            if (!SetProviderType(parsed.Provider))
            {
                ShowMessageAndWaitForKeyPress("Invalid Provider Type.  Must be Ofs, Server or ServerCacheOfs.");
                return;
            }
            _baseDir = Path.Combine(Directory.GetCurrentDirectory(), "data");
            InitializeProvider(ApplicationName);

            //Start prompt with completions
            ShowLoggedOutPrompt();
        }

        private static void ShowLoggedInPrompt()
        {
            Console.Clear();

            // Show startup message
            var startupMsg = new Figlet().ToAscii("Absio").ConcreteValue;
            startupMsg += SessionInfo;

            var eval = new LoggedInCommandEvaluator();

            var prompt = _userId + "> ";

            InteractivePrompt.Run(
                (command, listCommand, completions) => eval.HandleCommand(command) + Environment.NewLine, prompt,
                startupMsg, LoggedInCommandEvaluator.Commands);
        }

        private static void ShowLoggedOutPrompt()
        {
            Console.Clear();

            // Show startup message
            var startupMsg = AbsioConsoleHeader;
            startupMsg += SessionInfo;

            var eval = new LoggedOutCommandEvaluator();

            var prompt = "absio> ";
            InteractivePrompt.Run(
                (command, listCommand, completions) => eval.HandleCommand(command) + Environment.NewLine, prompt,
                startupMsg, LoggedOutCommandEvaluator.Commands);
        }

        private static void ShowMessageAndWaitForKeyPress(string message)
        {
            Console.WriteLine(message + "\n\n" + "Press any key to continue...");
            Console.ReadKey();
        }

        #endregion

        #region  Classes

        public class LoggedInCommandEvaluator
        {
            #region Static Fields and Constants

            public static readonly List<string> Commands = new List<string>
            {
                "changecredentials",
                "changeprovider",
                "deleteuser",
                "getreminder",
                "logout"
            };

            #endregion

            #region Public and Internal Methods

            public string HandleCommand(string command)
            {
                Parser.Default
                    .ParseArguments<DeleteUserOptions, ChangeCredentialsOptions, ChangeProviderOptions, 
                        GetReminderOptions, LogoutOptions>(command.Split())
                    .MapResult(
                        (DeleteUserOptions opts) => RunDeleteUserCommand(),
                        (ChangeCredentialsOptions opts) => RunChangeCredentialsCommand(opts),
                        (ChangeProviderOptions opts) => RunChangeProviderCommand(opts),
                        (GetReminderOptions opts) => RunGetReminderCommand(opts),
                        (LogoutOptions opts) => RunLogOutCommand(),
                        errs => HandleErrors());

                return string.Empty;
            }

            #endregion

            #region Private and Protected Methods

            private string HandleErrors()
            {
                Console.ReadKey();
                return string.Empty;
            }

            private string RunChangeCredentialsCommand(ChangeCredentialsOptions opts)
            {
                string message = null;
                try
                {
                    ChangeCredentialsWithProvider(opts.Password, opts.Passphrase, opts.Reminder);

                    message = "Passphase and reminder changed.";

                    Console.WriteLine(message);
                }
                catch (AggregateException e)
                {
                    Console.WriteLine(e.InnerException?.Message);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }

                return message;
            }

            private string RunChangeProviderCommand(ChangeProviderOptions opts)
            {
                var providerType = opts.Provider;

                if (!SetProviderType(providerType))
                {
                    var error = "Invalid Provider Type.  Must be Ofs, Server or ServerCacheOfs.";
                    Console.WriteLine(error);
                    return error;
                }

                // Get the KeyRing...
                var keyRing = _provider.KeyRing;

                // reset the provider
                ResetSession(ApplicationName);

                // Authenticate with the KeyRing.
                _provider.LogInAsync(keyRing);

                return string.Empty;
            }

            private string RunDeleteUserCommand()
            {
                Console.WriteLine("Are you sure you want to delete this user? Yes/No");
                var response = Console.ReadLine();

                if (response.Equals("y", StringComparison.CurrentCultureIgnoreCase) ||
                    response.Equals("yes", StringComparison.CurrentCultureIgnoreCase))

                {
                    try
                    {
                        _provider.DeleteUserAsync().Wait();
                        ShowMessageAndWaitForKeyPress("User has been deleted.");
                        ShowLoggedOutPrompt();
                    }
                    catch (AggregateException e)
                    {
                        Console.WriteLine(e.InnerException?.Message);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }

                return string.Empty;
            }

            private string RunGetReminderCommand(GetReminderOptions opts)
            {
                string reminder = null;
                try
                {
                    var userId = new Guid(opts.UserId);
                    reminder = GetReminderFromProvider(userId);

                    Console.WriteLine("Reminder : " + reminder);
                }
                catch (AggregateException e)
                {
                    Console.WriteLine(e.InnerException?.Message);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }

                return reminder;
            }

            private string RunLogOutCommand()
            {
                ResetSession(ApplicationName);

                ShowMessageAndWaitForKeyPress("User logged out.");

                ShowLoggedOutPrompt();
                return string.Empty;
            }

            #endregion

            #region  Classes

            [Verb("changecredentials", HelpText =
                "Changes a user's password, backup passphrase, and optional passphrase reminder.")]
            public class ChangeCredentialsOptions
            {
                #region Properties, Indexers

                [Option(Required = true)]
                public string Passphrase { get; set; }

                [Option(Required = true)]
                public string Password { get; set; }

                [Option(Required = false)]
                public string Reminder { get; set; }

                #endregion
            }

            [Verb("deleteuser", HelpText = "Deletes a user from the Absio server and removes all local data.")]
            public class DeleteUserOptions
            {
            }

            [Verb("logout", HelpText = "Logs out a user and resets the session.")]
            public class LogoutOptions
            {
            }

            #endregion
        }

        public class LoggedOutCommandEvaluator
        {
            #region Static Fields and Constants

            public static readonly List<string> Commands =
                new List<string> {"register", "login", "getreminder", "resetpassword", "changeprovider"};

            #endregion

            #region Public and Internal Methods

            public string HandleCommand(string command)
            {
                Parser.Default
                    .ParseArguments<RegisterOptions, LoginOptions, GetReminderOptions, ChangeProviderOptions>(
                        command.Split())
                    .MapResult(
                        (RegisterOptions opts) => RunRegisterCommand(opts),
                        (LoginOptions opts) => RunLoginCommand(opts),
                        (GetReminderOptions opts) => RunGetReminderCommand(opts),
                        (ChangeProviderOptions opts) => RunChangeProviderCommand(opts),
                        errs => HandleErrors());

                return "";
            }

            #endregion

            #region Private and Protected Methods

            private string HandleErrors()
            {
                Console.ReadKey();
                return string.Empty;
            }

            private string RunGetReminderCommand(GetReminderOptions opts)
            {
                string reminder = null;
                try
                {
                    var userId = new Guid(opts.UserId);
                    reminder = GetReminderFromProvider(userId);

                    Console.WriteLine("Reminder : " + reminder);
                }
                catch (AggregateException e)
                {
                    Console.WriteLine(e.InnerException?.Message);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }

                return reminder;
            }

            private string RunLoginCommand(LoginOptions opts)
            {
                try
                {
                    var userId = new Guid(opts.UserId);
                    _provider.LogInAsync(userId, opts.Password, opts.Passphrase).Wait();
                    _userId = userId;
                    ShowLoggedInPrompt();
                }
                catch (AggregateException e)
                {
                    Console.WriteLine(e.InnerException?.Message);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }

                return "";
            }

            private string RunRegisterCommand(RegisterOptions opts)
            {
                try
                {
                    _userId = RegisterWithProvider(opts.Password, opts.Passphrase, opts.Reminder);
                    ShowLoggedInPrompt();
                }
                catch (AggregateException e)
                {
                    Console.WriteLine(e.InnerException?.Message);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }

                return "";
            }

            private string RunChangeProviderCommand(ChangeProviderOptions opts)
            {
                var providerType = opts.Provider;

                if (!SetProviderType(providerType))
                {
                    var error = "Invalid Provider Type.  Must be Ofs, Server or ServerCacheOfs.";
                    Console.WriteLine(error);
                    return error;
                }

                // reset the provider
                ResetSession(ApplicationName);

                return string.Empty;
            }

            #endregion
        }

        [Verb("changeprovider", HelpText = "Changes the provider.  Must be: Ofs, Server or ServerCacheOfs.")]
        public class ChangeProviderOptions
        {
            #region Properties, Indexers

            [Option(Required = true)]
            public string Provider { get; set; }

            #endregion
        }

        [Verb("login", HelpText = "Logs a user into the Absio service.")]
        public class LoginOptions
        {
            #region Properties, Indexers

            [Option(Required = false)]
            public string Passphrase { get; set; }

            [Option(Required = false)]
            public string Password { get; set; }

            [Option(Required = true)]
            public string UserId { get; set; }

            #endregion
        }

        [Verb("register", HelpText = "Registers a user with the Absio server.")]
        public class RegisterOptions
        {
            #region Properties, Indexers

            [Option(Required = true)]
            public string Passphrase { get; set; }

            [Option(Required = true)]
            public string Password { get; set; }

            [Option(Required = false)]
            public string Reminder { get; set; }

            #endregion
        }

        #endregion

        [Verb("getreminder", HelpText = "Retrieves a user's backup reminder.")]
        public class GetReminderOptions
        {
            #region Properties, Indexers

            [Option(Required = true)]
            public string UserId { get; set; }

            #endregion
        }
    }
}
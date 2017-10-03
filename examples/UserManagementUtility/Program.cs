using System;
using System.Collections.Generic;
using System.IO;
using Absio.Sdk.Crypto;
using Absio.Sdk.Session;
using Colorful;
using CommandLine;
using Common;
using Console = Colorful.Console;

// Warnings disabled based on CommandLine requirements
// ReSharper disable ClassNeverInstantiated.Local
// ReSharper disable UnusedAutoPropertyAccessor.Local

namespace UserManagementUtility
{
    internal class Program
    {
        #region Static Fields and Constants

        private static SecuredContainerSession _session;
        private static SessionAttributes _sessionAttributes;
        private static Guid? _userId;

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

        public static string SessionInfo
        {
            get
            {
                var sessionInfo = "Api Key : " + _sessionAttributes.ApiKey;
                sessionInfo += "\n";
                sessionInfo += "Host : " + _sessionAttributes.ServerUrl;
                sessionInfo += "\n";
                if (_userId != null)
                {
                    sessionInfo += "User : " + _userId;
                    sessionInfo += "\n";
                }
                return sessionInfo;
            }
        }

        #endregion

        #region Private and Protected Methods

        private static void Main(string[] args)
        {
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

            _sessionAttributes = new SessionAttributes(parsed.ApiKey, parsed.Url,
                Path.Combine(Directory.GetCurrentDirectory(), "data"));
            _session = new SecuredContainerSession(_sessionAttributes);

            //Start prompt with completions
            ShowLoggedOutPrompt();
        }

        private static void ShowLoggedInPrompt()
        {
            Console.Clear();

            // Show startup message
            var startupMsg = new Figlet().ToAscii("Absio").ConcreteValue;
            startupMsg += "\n";
            startupMsg += "Api Key : " + _sessionAttributes.ApiKey;
            startupMsg += "\n";
            startupMsg += "Host : " + _sessionAttributes.ServerUrl;
            startupMsg += "\n";
            startupMsg += "User : " + _userId;
            startupMsg += "\n";

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

        [Verb("getreminder", HelpText = "Retrieves a user's backup reminder.")]
        public class GetReminderOptions
        {
            #region Properties, Indexers

            [Option(Required = true)]
            public string UserId { get; set; }

            #endregion
        }

        public class LoggedInCommandEvaluator
        {
            #region Static Fields and Constants

            public static readonly List<string> Commands = new List<string>
            {
                "changepassword",
                "changepassphrase",
                "deleteuser",
                "getreminder",
                "logout"
            };

            #endregion

            #region Public and Internal Methods

            public string HandleCommand(string command)
            {
                Parser.Default
                    .ParseArguments<DeleteUserOptions, ChangeCredentialsOptions, GetReminderOptions, LogoutOptions>(
                        command.Split())
                    .MapResult(
                        (DeleteUserOptions opts) => RunDeleteUserCommand(),
                        (ChangeCredentialsOptions opts) => RunChangeCredentialsCommand(opts),
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

            private static void ResetSession()
            {
                _session.Logout();
                _userId = null;
                _session = new SecuredContainerSession(_sessionAttributes);
            }

            private string RunChangeCredentialsCommand(ChangeCredentialsOptions opts)
            {
                string message = null;
                try
                {
                    _session.ChangeCredentialsAsync(opts.Password, opts.Passphrase, opts.Reminder).Wait();

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

            private string RunDeleteUserCommand()
            {
                Console.WriteLine("Are you sure you want to delete this user? Yes/No");
                var response = Console.ReadLine();

                if (response.Equals("y", StringComparison.CurrentCultureIgnoreCase) ||
                    response.Equals("yes", StringComparison.CurrentCultureIgnoreCase))

                {
                    try
                    {
                        _session.DeleteUserAsync().Wait();
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
                    reminder = _session.GetBackupReminderAsync(userId).Result;

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
                ResetSession();

                ShowMessageAndWaitForKeyPress("User logged out.");

                ShowLoggedOutPrompt();
                return string.Empty;
            }

            #endregion

            #region  Classes

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
                new List<string> {"register", "login", "getreminder", "resetpassword"};

            #endregion

            #region Public and Internal Methods

            public string HandleCommand(string command)
            {
                Parser.Default
                    .ParseArguments<RegisterOptions, LoginOptions, GetReminderOptions>(
                        command.Split())
                    .MapResult(
                        (RegisterOptions opts) => RunRegisterCommand(opts),
                        (LoginOptions opts) => RunLoginCommand(opts),
                        (GetReminderOptions opts) => RunGetReminderCommand(opts),
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
                    reminder = _session.GetBackupReminderAsync(userId).Result;

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
                    _session.LogInAsync(userId, opts.Password, opts.Passphrase).Wait();
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
                    _userId = _session.RegisterAsync(opts.Password, opts.Passphrase, opts.Reminder).Result;
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

        public class ProgramOptions
        {
            #region Properties, Indexers

            [Option(Required = true)]
            public string ApiKey { get; set; }

            [Option(Required = true)]
            public string Url { get; set; }

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
    }
}
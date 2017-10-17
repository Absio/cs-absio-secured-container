using System;
using System.CodeDom;
using System.Collections.Generic;
using System.IO;
using Absio.Sdk.Container;
using Absio.Sdk.Events;
using Absio.Sdk.Exceptions;
using Absio.Sdk.Session;
using Colorful;
using CommandLine;
using Common;
using Console = Colorful.Console;

// Warnings disabled based on CommandLine requirements
// ReSharper disable ClassNeverInstantiated.Local
// ReSharper disable UnusedAutoPropertyAccessor.Local

namespace ContainerCrudUtility
{
    internal class Program
    {
        #region Static Fields and Constants

        private static SecuredContainerSession _session;
        private static SessionAttributes _sessionAttributes;
        private static Guid _userId;
        public const string ApplicationName = "cs-cli-crud-app";

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
                Path.Combine(Directory.GetCurrentDirectory(), "data"), false,
                ApplicationName);
            _session = new SecuredContainerSession(_sessionAttributes);

            //Start prompt with completions
            ShowLoggedOutPrompt(_sessionAttributes);
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

        private static void ShowLoggedOutPrompt(SessionAttributes sessionAttributes)
        {
            Console.Clear();

            // Show startup message
            var startupMsg = new Figlet().ToAscii("Absio").ConcreteValue;
            startupMsg += "\n\n";
            startupMsg += "Container CRUD Utility";
            startupMsg += "\n\n";
            startupMsg += "Api Key : " + sessionAttributes.ApiKey;
            startupMsg += "\n";
            startupMsg += "Host : " + sessionAttributes.ServerUrl;
            startupMsg += "\n";

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
                "logout",
                "create",
                "read",
                "update",
                "delete",
                "listevents"
            };

            #endregion

            #region Public and Internal Methods

            public string HandleCommand(string command)
            {
                Parser.Default
                    .ParseArguments<LogoutOptions, CreateOptions, ReadOptions, UpdateOptions,
                        DeleteOptions, ListEventsOptions>(command.Split())
                    .MapResult(
                        (LogoutOptions opts) => RunLogOutCommand(),
                        (CreateOptions opts) => RunCreateCommand(opts),
                        (ReadOptions opts) => RunReadCommand(opts),
                        (UpdateOptions opts) => RunUpdateCommand(opts),
                        (DeleteOptions opts) => RunDeleteCommand(opts),
                        (ListEventsOptions opts) => RunListEventsCommand(opts),
                        errs => HandleErrors());

                return string.Empty;
            }

            #endregion

            #region Private and Protected Methods

            private class AccessOptions
            {
                #region Properties, Indexers

                [Option("expiration", Required = false)]
                public DateTime? Expiration { get; set; }

                [Option("permission", Required = false)]
                public int? Permission { get; set; }

                [Option("userId", Required = true)]
                public Guid UserId { get; set; }

                #endregion
            }

            private string HandleErrors()
            {
                Console.ReadKey();
                return string.Empty;
            }

            private void PrintContainerToConsole(IContainer container)
            {
                Console.WriteLine($"Container ID : {container.Id.ToString()}");
                Console.WriteLine($"Type : {container.Metadata.Type}");
                Console.WriteLine($"Size : {container.Metadata.Length} bytes");
                Console.WriteLine($"Header : {container.ContainerHeader?.CustomHeader}");
                string content = null;
                if (container.Content != null)
                {
                    content = System.Text.Encoding.Default.GetString(container.Content);
                }
                Console.WriteLine($"Content : {content}");
                if (container.Metadata.CreatedAt != null)
                {
                    Console.WriteLine(
                        $"Created : {container.Metadata.CreatedAt} by {container.Metadata.CreatedBy.ToString()}");
                }
                if (container.Metadata.ModifiedAt != null)
                {
                    Console.WriteLine(
                        $"Modified : {container.Metadata.ModifiedAt} by {container.Metadata.ModifiedBy.ToString()}");
                }
                Console.WriteLine();
                if (container.Metadata.ContainerAccessLevels.Count > 0)
                {
                    Console.WriteLine("Access :");
                    foreach (var access in container.Metadata.ContainerAccessLevels)
                    {
                        Console.WriteLine($"User : {access.UserId}");
                        Console.WriteLine($"Permissions : {access.Permissions}");
                        if (access.ExpiresAt != null)
                        {
                            Console.WriteLine($"Expiration : {access.ExpiresAt}");
                        }
                        Console.WriteLine();
                    }
                }
            }

            private void PrintEventsToConsole(List<ContainerEvent> events)
            {
                Console.WriteLine("Event ID : Container ID : Type : Action");
                Console.WriteLine();
                foreach (var containerEvent in events)
                {
                    Console.WriteLine(
                        $"{containerEvent.EventId} : {containerEvent.ContainerId} : {containerEvent.Type} : {containerEvent.ActionType.ToString()}");
                }
            }

            private static void ResetSession()
            {
                _session.Logout();
                _session = new SecuredContainerSession(_sessionAttributes);
            }

            private string RunCreateCommand(CreateOptions options)
            {
                var file = options.File;

                byte[] content;
                using (var fileStream = File.OpenRead(file))
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        fileStream.CopyTo(memoryStream);
                        content = memoryStream.ToArray();
                    }
                }

                var type = options.Type;
                var header = options.Header;

                Console.WriteLine("Would you like to grant access to other users? Yes/No");
                var response = Console.ReadLine();

                List<ContainerAccessLevel> access = null;

                if (response.Equals("y", StringComparison.CurrentCultureIgnoreCase) ||
                    response.Equals("yes", StringComparison.CurrentCultureIgnoreCase))
                {
                    Console.WriteLine("Enter user ID, ");

                    access = new List<ContainerAccessLevel>();

                    while (true)
                    {
                        response = Console.ReadLine();

                        if (response.Equals("done", StringComparison.CurrentCultureIgnoreCase) ||
                            string.IsNullOrEmpty(response))
                        {
                            break;
                        }

                        var parserResult = Parser.Default.ParseArguments<AccessOptions>(response.Split());
                        var accessOptions = parserResult.MapResult(opts => opts, null);
                        var permission = (Permission) accessOptions.Permission;
                        var accessLevel =
                            new ContainerAccessLevel(accessOptions.UserId, permission, accessOptions.Expiration);
                        access.Add(accessLevel);
                    }
                }

                var containerId = _session.CreateAsync(content, header, access, type).Result;

                Console.WriteLine($"Successfully created container : {containerId}");
                Console.WriteLine();
                PrintContainerToConsole(_session.ReadAsync(containerId).Result);

                return string.Empty;
            }

            private string RunDeleteCommand(DeleteOptions options)
            {
                Console.WriteLine("Are you sure you want to delete this container? Yes/No");
                var response = Console.ReadLine();

                if (response.Equals("y", StringComparison.CurrentCultureIgnoreCase) ||
                    response.Equals("yes", StringComparison.CurrentCultureIgnoreCase))

                {
                    try
                    {
                        _session.DeleteAsync(new Guid(options.Id)).Wait();
                        ShowMessageAndWaitForKeyPress("Container has been deleted.");
                    }
                    catch (AggregateException e)
                    {
                        if (e.InnerExceptions.Count > 0 && e.InnerExceptions[0] is NotFoundException)
                        {
                            Console.WriteLine($"Cannot delete the container since none was found with ID: {options.Id}.");
                        }
                        else
                        {
                            Console.WriteLine($"There was some error trying to delete the following ID: {options.Id}.");
                        }

                        return string.Empty;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }

                return string.Empty;
            }

            private string RunListEventsCommand(ListEventsOptions options)
            {
                try
                {
                    EventActionType? action = null;
                    if (options.Action != null)
                    {
                        action = (EventActionType?) Enum.Parse(typeof(EventActionType), options.Action);
                    }

                    Guid? containerId = null;
                    if (options.ContainerId != null)
                    {
                        containerId = Guid.Parse(options.ContainerId);
                    }

                    var events = _session.GetEventsAsync(action, options.StartingId, options.EndingId,
                        containerId, options.ContainerType).Result;
                    PrintEventsToConsole(events);
                }
                catch (AggregateException e)
                {
                    Console.WriteLine(e.InnerException?.Message);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }

                return string.Empty;
            }

            private string RunLogOutCommand()
            {
                ResetSession();

                ShowMessageAndWaitForKeyPress("User logged out.");

                ShowLoggedOutPrompt(_sessionAttributes);
                return string.Empty;
            }

            private string RunReadCommand(ReadOptions options)
            {
                try
                {
                    var container = _session.ReadAsync(options.Id).Result;
                    PrintContainerToConsole(container);

                    if (!string.IsNullOrEmpty(options.File))
                    {
                        using (var stream = File.OpenWrite(options.File))
                        {
                            stream.WriteAsync(container.Content, 0, container.Content.Length).Wait();
                            Console.WriteLine($"Container decrypted to {options.File}");
                        }
                    }
                }
                catch (AggregateException e)
                {
                    if (e.InnerExceptions.Count > 0 && e.InnerExceptions[0] is NotFoundException)
                    {
                        Console.WriteLine($"Container not found with ID: {options.Id}.");
                    }
                    else
                    {
                        Console.WriteLine($"There was some error trying to read the following ID: {options.Id}.");
                    }
                }

                return string.Empty;
            }

            private string RunUpdateCommand(UpdateOptions options)
            {
                byte[] content = null;
                if (!string.IsNullOrEmpty(options.File))
                {
                    using (var fileStream = File.OpenRead(options.File))
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            fileStream.CopyTo(memoryStream);
                            content = memoryStream.ToArray();
                        }
                    }
                }

                var type = options.Type;
                var header = options.Header;

                List<ContainerAccessLevel> access = null;
                if (header != null || options.File != null)
                {
                    Console.WriteLine("Would you like to grant access to other users? Yes/No");
                    var response = Console.ReadLine();
                    if (response.Equals("y", StringComparison.CurrentCultureIgnoreCase) ||
                        response.Equals("yes", StringComparison.CurrentCultureIgnoreCase))
                    {
                        Console.WriteLine("Enter user ID, ");

                        access = new List<ContainerAccessLevel>();

                        while (true)
                        {
                            response = Console.ReadLine();

                            if (response.Equals("done", StringComparison.CurrentCultureIgnoreCase) ||
                                string.IsNullOrEmpty(response))
                            {
                                break;
                            }

                            var parserResult = Parser.Default.ParseArguments<AccessOptions>(response.Split());
                            var accessOptions = parserResult.MapResult(opts => opts, null);
                            var permission = (Permission) accessOptions.Permission;
                            var accessLevel =
                                new ContainerAccessLevel(accessOptions.UserId, permission, accessOptions.Expiration);
                            access.Add(accessLevel);
                        }
                    }
                }
                var containerId = options.Id;
                try
                {
                    _session.UpdateAsync(containerId, content, header, access, type).Wait();
                }
                catch (AggregateException e)
                {
                    if (e.InnerExceptions.Count > 0 && e.InnerExceptions[0] is NotFoundException)
                    {
                        Console.WriteLine($"Cannot upate the container since none was found with ID: {options.Id}.");
                    }
                    else
                    {
                        Console.WriteLine($"There was some error trying to update the following ID: {options.Id}.");
                        foreach (Exception ex in e.InnerExceptions)
                        {
                            Console.WriteLine($"Next Exception: {ex.Message}.");
                        }
                    }

                    return string.Empty;
                }

                Console.WriteLine($"Successfully updated container : {containerId.ToString()}");
                Console.WriteLine();
                PrintContainerToConsole(_session.ReadAsync(containerId).Result);

                return string.Empty;
            }

            #endregion

            #region  Classes

            [Verb("create")]
            public class CreateOptions
            {
                #region Properties, Indexers

                [Option("file", Required = true)]
                public string File { get; set; }

                [Option("type", Required = false)]
                public string Type { get; set; }

                [Option("header", Required = false)]
                public string Header { get; set; }

                #endregion
            }

            [Verb("delete")]
            public class DeleteOptions
            {
                #region Properties, Indexers

                [Option("id", Required = true)]
                public string Id { get; set; }

                #endregion
            }

            [Verb("logout")]
            public class LogoutOptions
            {
            }

            [Verb("listevents")]
            public class ListEventsOptions
            {
                #region Properties, Indexers

                [Option("action", Required = false)]
                public string Action { get; set; }

                [Option("containerId", Required = false)]
                public string ContainerId { get; set; }

                [Option("containerType", Required = false)]
                public string ContainerType { get; set; }

                [Option("endingId", Required = false)]
                public long? EndingId { get; set; }

                [Option("startingId", Required = false)]
                public long? StartingId { get; set; }

                #endregion
            }

            [Verb("read")]
            public class ReadOptions
            {
                #region Properties, Indexers

                [Option("file", Required = false)]
                public string File { get; set; }

                [Option("id", Required = true)]
                public Guid Id { get; set; }

                #endregion
            }

            [Verb("update")]
            public class UpdateOptions
            {
                #region Properties, Indexers

                [Option("file", Required = false)]
                public string File { get; set; }

                [Option(Required = true)]
                public Guid Id { get; set; }

                [Option("type", Required = false)]
                public string Type { get; set; }

                [Option("header", Required = false)]
                public string Header { get; set; }

                #endregion
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

            #region  Classes

            [Verb("getreminder", HelpText = "Retrieves a user's backup reminder.")]
            public class GetReminderOptions
            {
                #region Properties, Indexers

                [Option(Required = true)]
                public string UserId { get; set; }

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

        #endregion
    }
}
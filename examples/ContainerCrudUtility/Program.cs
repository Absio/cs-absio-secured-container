using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using Absio.Sdk.Container;
using Absio.Sdk.Events;
using Absio.Sdk.Exceptions;
using Absio.Sdk.Providers;
using Colorful;
using CommandLine;
using Common;
using Console = Colorful.Console;

// Warnings disabled based on CommandLine requirements
// ReSharper disable ClassNeverInstantiated.Local
// ReSharper disable UnusedAutoPropertyAccessor.Local

namespace ContainerCrudUtility
{
    internal class Program : BaseProgram
    {
        #region Static Fields and Constants

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

        private static string AbsioConsoleHeader
        {
            get
            {
                var absioConsoleHeader = new Figlet().ToAscii("Absio").ConcreteValue;
                absioConsoleHeader += "\n\n";
                absioConsoleHeader += "Container CRUD Utility";
                absioConsoleHeader += "\n\n";
                return absioConsoleHeader;
            }
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
                "logout",
                "changeprovider",
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
                        DeleteOptions, ListEventsOptions, ChangeProviderOptions>(command.Split())
                    .MapResult(
                        (LogoutOptions opts) => RunLogOutCommand(),
                        (CreateOptions opts) => RunCreateCommand(opts),
                        (ChangeProviderOptions opts) => RunChangeProviderCommand(opts),
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

            private void PrintContainerToConsole(IContainer container, bool open=false)
            {
                Console.WriteLine($"Container ID : {container.Id.ToString()}");
                Console.WriteLine($"Type : {container.Metadata.Type}");
                Console.WriteLine($"Size : {container.Metadata.Length} bytes");
                Console.WriteLine($"Header : {container.ContainerHeader?.Data}");
                string content = null;
                if (container.Content != null)
                {
                    if (container.Metadata.Type != "File" || container.Content.Length < 100000)
                    {
                        content = Encoding.Default.GetString(container.Content);
                    }
                    else
                    {
                        content = "Too big of a file: " + container.ContainerHeader?.Data;
                    }
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

                // Check to see if we should have the OS open the file.
                if (open && "File".Equals(container.Metadata.Type) && container.ContainerHeader?.Data != null)
                {
                    var fileName = container.ContainerHeader.Data;
                    var file = Path.GetTempPath() + fileName;
                    // Write it...
                    using (var stream = File.OpenWrite(file))
                    {
                        stream.WriteAsync(container.Content, 0, container.Content.Length).Wait();
                    }

                    // Open it...
                    try
                    {
                        Process.Start(file);

                    }
                    catch (Exception e)
                    {
                        System.Console.WriteLine(e);
                    }
                }

                Console.WriteLine($"End print for Container ID : {container.Id.ToString()}");
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

            private string RunCreateCommand(CreateOptions options)
            {
                byte[] content;
                string header = null;
                string type = null;
                if (options.File != null)
                {
                    type = "File";
                    header = Path.GetFileName(options.File);
                    using (var fileStream = File.OpenRead(options.File))
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            fileStream.CopyTo(memoryStream);
                            content = memoryStream.ToArray();
                        }
                    }
                    
                }
                else if (options.Data != null)
                {
                    content = Encoding.ASCII.GetBytes(options.Data);
                }
                else
                {
                    content = null;
                }

                if (content == null)
                {
                    var error = "You must specify a file or data for the content.";
                    Console.WriteLine(error);
                    return error;
                }

                if (type == null)
                {
                    type = options.Type;
                    
                }
                if (header == null)
                {
                    header = options.Header;
                    
                }
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

                var securedContainer = _provider.CreateAsync(content, header, access, type).Result;

                Console.WriteLine($"Successfully created container : {securedContainer.Metadata.Id}");
                Console.WriteLine();
                PrintContainerToConsole(_provider.GetAsync(securedContainer.Metadata.Id).Result);

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
                        _provider.DeleteAsync(new Guid(options.Id)).Wait();
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

                    var events = GetEventsFromProvider(action, options.StartingId, options.EndingId,
                        containerId, options.ContainerType);
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
                ResetSession(ApplicationName);

                ShowMessageAndWaitForKeyPress("User logged out.");

                ShowLoggedOutPrompt();
                return string.Empty;
            }

            private string RunReadCommand(ReadOptions options)
            {
                try
                {
                    var container = _provider.GetAsync(options.Id).Result;
                    PrintContainerToConsole(container, true);

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
                    _provider.UpdateAsync(containerId, content, header, access, type).Wait();
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
                PrintContainerToConsole(_provider.GetAsync(containerId).Result);

                return string.Empty;
            }

            #endregion

            #region  Classes

            [Verb("create")]
            public class CreateOptions
            {
                #region Properties, Indexers

                [Option("file", Required = false)]
                public string File { get; set; }

                [Option("data", Required = false)]
                public string Data { get; set; }

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
                new List<string> {"register", "login", "getreminder", "resetpassword", "changeprovider" };

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

        [Verb("changeprovider", HelpText = "Changes the provider.  Must be: Ofs, Server or ServerCacheOfs.")]
        public class ChangeProviderOptions
        {
            #region Properties, Indexers

            [Option(Required = true)]
            public string Provider { get; set; }

            #endregion
        }

        #endregion
    }
}
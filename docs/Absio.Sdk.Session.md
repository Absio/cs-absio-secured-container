## `ISession`

Interface representing a secure session with the Absio SDK for local and server storage.
```csharp
public interface Absio.Sdk.Session.ISession

```

Properties

| Type | Name | Summary | 
| --- | --- | --- | 
| `IContainerDatabaseStorage` | ContainerDatabaseStorage | Storage mechanism for storing IContainer metadata in the local database. | 
| `IEncryptionService` | EncryptionService | The service responsible for IContainer encryption. | 
| `IOfsStorage` | OfsStorage | Storage mechanism for storing IContainer in the local obfuscating file system. | 
| `IServerStorage` | ServerStorage | Storage mechanism for storing IContainer on the Absio API server. | 
| `Guid` | UserId | The logged-in user's ID. | 


## `SecuredContainerSession`

The top level for interaction with the Absio SDK.
```csharp
public class Absio.Sdk.Session.SecuredContainerSession
    : IDisposable

```

Properties

| Type | Name | Summary | 
| --- | --- | --- | 
| `IContainerProvider` | CurrentContainerProvider | Active IContainerProvider which is currently used for operations with containers. | 
| `Boolean` | IsDisposed | If the sesson has been disposed. | 
| `Boolean` | IsSessionInitialized | If the session has been initialized via RegisterAsync or LogInAsync. | 


Methods

| Type | Name | Summary | 
| --- | --- | --- | 
| `Task` | ChangeBackupCredentialsAsync(`BackupCredentials` backupCredentials, `String` currentPassphrase, `String` currentPassword) | Changes the currently authenticated user's backup credentials. | 
| `Task` | ChangePasswordAsync(`String` currentPassword, `String` newPassword, `String` currentPassphrase) | Changes the currently authenticated user's password using their backup credential passphrase. | 
| `Task<Guid>` | CreateAsync(`Byte[]` content, `Object` customHeaderObject = null, `List<ContainerAccessLevel>` accessLevels = null, `String` type = null) | Creates a new SecuredContainer with the provided content.  IContainerProvider.CreateAsync | 
| `Task` | DeleteAsync(`Guid` containerId) | Deletes the Container with the given ID.  See IContainerProvider.DeleteAsync | 
| `Task` | DeleteUserAsync() | Deletes the authenticated user from the Absio API server and deletes all local data. | 
| `void` | Dispose() | Dispose session. Is equal to logout | 
| `Task<List<AccessNotification>>` | GetAccessNotificationsAsync(`Guid` containerId) | Fetches all AccessNotification for a Container with the given ID. | 
| `Task<IContainer>` | GetAsync(`Guid` containerId) | Fetches a Container object with the given ID.  See IContainerProvider.GetAsync | 
| `Task<String>` | GetBackupReminderAsync(`Nullable<Guid>` userId = null) | Fetches the backup reminder for the current active user or the provided user ID. | 
| `Task<Byte[]>` | GetContentAsync(`Guid` containerId) | Fetches a Container object's data with the given ID.  See IContainerProvider.GetContentAsync | 
| `Task<ContainerHeader>` | GetHeaderAsync(`Guid` containerId) | Fetches a Container object's custom header with the given ID.  See IContainerProvider.GetHeaderAsync | 
| `Task<List<ContainerEvent>>` | GetLatestEventsAsync(`Nullable<EventActionType>` actionType = null, `Nullable<Int64>` startingId = null, `Nullable<Int64>` endingId = null, `Nullable<Guid>` containerId = null, `String` containerType = null) | Fetches a list of Container-related events from the Absio API server. The list can be filtered based on type, ID  range, and container metadata. | 
| `Task<ContainerMetadata>` | GetMetadataAsync(`Guid` containerId) | Fetches a metadata of the container with the given ID.  See IContainerProvider.GetMetadataAsync | 
| `Guid` | GetUserId() | Fetches the ID of the active user. | 
| `Task` | LogInAsync(`Guid` userId, `String` password, `String` passphrase = null, `Boolean` cacheKeyFileLocal = True) | Authenticates the user locally and against the Absio API Server. | 
| `void` | Logout() | Logout user, equivalent to dispose. The session may not be used after logging out. | 
| `Task<IContainer>` | ReadAsync(`Guid` containerId) | Fetches a Container object with the given Guid. | 
| `Task<Byte[]>` | ReadContentAsync(`Guid` containerId) | Fetches a content of the Container object with the given Guid. | 
| `Task<ContainerHeader>` | ReadHeaderAsync(`Guid` containerId) | Fetches a header ContainerHeader of the Container object with the given Guid. | 
| `Task<ContainerMetadata>` | ReadMetadataAsync(`Guid` containerId) | Fetches a ContainerMetadata  of the Container object with the given Guid. | 
| `Task<Guid>` | RegisterAsync(`String` password, `BackupCredentials` backupCredentials = null) | Registers a user and their cryptographic public keys with the Absio API Server. | 
| `Task` | ResetPasswordAsync(`Guid` userId, `String` currentPassphrase, `String` newPassword) | Resets the user's password using their backup credential passphrase. | 
| `void` | SetCurrentContainerProvider(`IContainerProvider` containerProvider) | Set container provider which is used for operations with containers IContainerProvider. | 
| `void` | SetOfsContainerProvider() | Set Container provider to OfsContainerProvider. This container provider will manage all containers locally, and  will not interact with the Absio API server. | 
| `void` | SetServerCacheOfsContainerProvider() | Set Container provider to ServerCacheOfsContainerProvider. This container provider will manage all containers on  the Absio API server, while also caching them locally in the obfuscating file system and database. | 
| `void` | SetServerContainerProvider() | Set container provider to ServerContainerProvider. This container provider will manage all containers on the Absio  API server, and will not store any data locally. | 
| `Task` | UpdateAsync(`Guid` containerId, `Byte[]` content, `Object` customHeaderObject = null, `List<ContainerAccessLevel>` accessLevels = null, `String` type = null) | Updates the Container object with the provided content. | 
| `Boolean` | ValidateBackupPassphrase(`String` passphrase) | Validates all backup credential passphrases entered as parameters to other SecuredContainerSession  methods.  Default: DefaultPassphraseRegex | 
| `Boolean` | ValidateBackupReminder(`String` reminder) | Validates all backup credential reminders entered as parameters to other SecuredContainerSession  methods.  Default: all reminders are valid. | 
| `Boolean` | ValidatePassword(`String` password) | Validates all passwords entered as parameters to other  methods.  Default: PasswordsRegexPatterns | 


Static Fields

| Type | Name | Summary | 
| --- | --- | --- | 
| `String[]` | PassphraseRegexPatterns | Default Regular Expression patterns used to validate passphrases. | 
| `String[]` | PasswordsRegexPatterns | Default Regular Expression patterns used to validate passwords. | 


## `SessionAttributes`

Defines various mandatory and optional attributes used by the SecuredContainerSession.
```csharp
public class Absio.Sdk.Session.SessionAttributes
    : IEquatable<SessionAttributes>

```

Properties

| Type | Name | Summary | 
| --- | --- | --- | 
| `String` | ApiKey | The Absio SDK API key supplied to the SDK consumer by Absio Corp. | 
| `String` | ApplicationName | The name of the application as registered with Absio | 
| `String` | OfsRootDirectory | The root of the obfuscated file system. | 
| `Boolean` | PartitionDataByUser | Enable to partition the obfuscated file system by user.  Default: false. | 
| `String` | ServerUrl | The URL of the Absio API Server instance to which to connect. | 


Methods

| Type | Name | Summary | 
| --- | --- | --- | 
| `Boolean` | Equals(`SessionAttributes` other) |  | 
| `Boolean` | Equals(`Object` obj) |  | 
| `Int32` | GetHashCode() |  | 



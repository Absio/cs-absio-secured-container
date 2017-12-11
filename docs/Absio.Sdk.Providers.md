## `BaseProvider`

Provides base functionality for provider instances.
```csharp
public class Absio.Sdk.Providers.BaseProvider

```

Properties

| Type | Name | Summary | 
| --- | --- | --- | 
| `ContainerEncryptionService` | ContainerEncryptionService | The container encryption service for the provider. | 
| `KeyRing` | KeyRing | The KeyRing for the authenticated user.  This will be null if the Provider is not authenticated. | 
| `Nullable<Guid>` | UserId | The UserId as specified in the KeyRing.  This will be null if there is no KeyRing for the Provider (not authenticated). | 


Methods

| Type | Name | Summary | 
| --- | --- | --- | 
| `IContainer` | BuildNewContainer(`Byte[]` content, `Object` customHeaderObject = null, `List<ContainerAccessLevel>` accessLevels = null, `String` type = null) | Builds an IContainer instance from the provided parameters.  If the accessLevels list is null or empty,  the creator (the authenticated user) will be added with ContainerAccessLevel.DefaultOwnerPermissions. | 
| `Task<String>` | GetHashAsync(`String` stringToHash) | This is a utility method for all providers.  It will return the hash of the passed in string. | 


## `IAbsioProvider`

```csharp
public interface Absio.Sdk.Providers.IAbsioProvider

```

Properties

| Type | Name | Summary | 
| --- | --- | --- | 
| `KeyRing` | KeyRing |  | 
| `Nullable<Guid>` | UserId |  | 


Methods

| Type | Name | Summary | 
| --- | --- | --- | 
| `Task<SecuredContainer>` | CreateAsync(`Byte[]` content, `Object` customHeaderObject = null, `List<ContainerAccessLevel>` accessLevels = null, `String` type = null) |  | 
| `Task` | DeleteAsync(`Guid` containerId) |  | 
| `Task` | DeleteUserAsync() |  | 
| `Task<IContainer>` | GetAsync(`Guid` containerId) |  | 
| `Task<Byte[]>` | GetContentAsync(`Guid` containerId) |  | 
| `Task<ContainerHeader>` | GetHeaderAsync(`Guid` containerId) |  | 
| `Task<ContainerMetadata>` | GetMetadataAsync(`Guid` containerId) |  | 
| `Task<Byte[]>` | LogInAsync(`Guid` userId, `String` password = null, `String` passphrase = null) |  | 
| `Task` | LogInAsync(`KeyRing` keyRing) |  | 
| `void` | Logout() |  | 
| `Task<List<ContainerAccessLevel>>` | UpdateAccessAsync(`Guid` containerId, `List<ContainerAccessLevel>` accessLevels = null) |  | 
| `Task<SecuredContainer>` | UpdateAsync(`Guid` containerId, `Byte[]` content = null, `Object` customHeaderObject = null, `List<ContainerAccessLevel>` accessLevels = null, `String` type = null) |  | 
| `Task<SecuredContainer>` | UpdateContentAsync(`Guid` containerId, `Byte[]` content = null) |  | 
| `Task<SecuredContainer>` | UpdateHeaderAsync(`Guid` containerId, `Object` customHeaderObject = null) |  | 
| `Task` | UpdateTypeAsync(`Guid` containerId, `String` type = null) |  | 


## `ICachingRules`

An interface used with the ServerCacheOfsProvider to define all caching rules.  This is to avoid passing a full reference to the  provider to classes only concerned with the caching rules.
```csharp
public interface Absio.Sdk.Providers.ICachingRules

```

Properties

| Type | Name | Summary | 
| --- | --- | --- | 
| `Boolean` | ForceLoadFromServer | A flag that determines the usage of the OfsProvider as a source on read operations.  When true, this will skip the OfsProvider  and load data from the ServerProvider.  However, newly read data will still be cached in the OFS. | 


## `OfsProvider`

Provider that sources all data (containers, public keys, key files, users, etc from a local Obfuscating File System (OFS).  This provider serves as a user session with the OFS.  Use LoginAsync to start an authenticated session.
```csharp
public class Absio.Sdk.Providers.OfsProvider
    : BaseProvider, IAbsioProvider

```

Properties

| Type | Name | Summary | 
| --- | --- | --- | 
| `IDataAccess` | DataAccess | The data access used for persisting and reading all data (except for content/header) from the OFS. | 
| `Boolean` | IsAuthenticated | True if a user has successfully called LogInAsync.  Upon calling Logout, the Provider is no longer authenticated. | 
| `KeyFileOfsMapper` | KeyFileMapper | The mapper in charge of managing KeyFile data (all create, read, update and delete operations). | 
| `Ofs` | Ofs | The instance of the OFS itself.  This class is used for determining storage locations. | 
| `String` | OfsRootDirectory | The root of the OFS. | 
| `Boolean` | PartitionDataByUser | If True the data in the OFS Is partitioned by user.  Default: false. | 
| `PublicKeyOfsMapper` | PublicKeyMapper | The mapper in charge of managing public key data (signing and derivation) for other users in the eco-system  (all create, read, update and delete operations). | 
| `SecuredContainerOfsMapper` | SecuredContainerMapper | The mapper in charge of managing secured container data (all create, read, update and delete operations). | 
| `UserOfsMapper` | UserMapper | The mapper in charge of managing user data (all create, read and delete operations). | 


Methods

| Type | Name | Summary | 
| --- | --- | --- | 
| `Task<Byte[]>` | ChangeCredentialsAsync(`String` password, `String` passphrase) | This is used to change the password and/or passphrase of a user.  This will cause the KeyFile to be re-encrypted with the new password (used for the KeyRing portion).  If a passphrase is supplied, the user will be allowed to authenticate (see LoginAsync)  with the passphrase in addition to the password. | 
| `Task<SecuredContainer>` | CreateAsync(`Byte[]` content, `Object` customHeaderObject = null, `List<ContainerAccessLevel>` accessLevels = null, `String` type = null) | Creates and persists a new SecuredContainer in the OFS and returns it.  Use the returned value to learn the assigned ID. | 
| `Task<SecuredContainer>` | CreateAsync(`SecuredContainer` securedContainer) | Creates and persists a new SecuredContainer in the OFS and returns it.  Use the returned value to learn the assigned ID. | 
| `Task<SecuredContainer>` | CreateOrUpdateAsync(`SecuredContainer` securedContainer) | This is a helper method to persists the SecuredContainer in the OFS.  It will determine  if the secured container already exists in the OFS (using the id).  In that case it will update  the existing container. | 
| `Task` | CreateOrUpdateKeyAsync(`PublicKeyMetadata` keyMetadata) | This is used to create or update public key metadata (including the key itself) in the OFS database of the authenticated user. | 
| `Task` | DeleteAsync(`Guid` containerId) | Deletes the secured container with the given ID from the OFS.  The encrypted content and the record  in the encrypted database will be removed. | 
| `Task` | DeleteUserAsync() | Deletes all associated data for the authenticated user.  NOTE: This cannot be undone.  Ensure you really want to perform this operation before doing so.  The KeyFile, encrypted database and all secured containers will be removed from the OFS. | 
| `Task<IContainer>` | GetAsync(`Guid` containerId) | Gets the SecuredContainer from the OFS (content, header and metadata),  decrypts the content and header and returns the resulting IContainer. | 
| `Task<Byte[]>` | GetContentAsync(`Guid` containerId) | Gets the decrypted content of the secured container for the given ID. | 
| `Task<ContainerHeader>` | GetHeaderAsync(`Guid` containerId) | Gets the decrypted header of the secured container for the given ID. | 
| `Task<Byte[]>` | GetKeyFileBytesAsync(`Guid` userId) | This will return the KeyFile for the passed in user from the OFS in its raw form (bytes).  NOTE: KeyFiles are always encrypted.  Thus, they bytes returned are encrypted. | 
| `Task<ContainerMetadata>` | GetMetadataAsync(`Guid` containerId) | Gets the ContainerMetadata of the secured container for the given ID. | 
| `void` | Initialize(`String` ofsRootDirectory = null, `Boolean` partitionDataByUser = False) | Initialize the Container Provider with a the root OFS directory and if data should be partitioned by user. | 
| `Task<Byte[]>` | LogInAsync(`Guid` userId, `String` password = null, `String` passphrase = null) | Authenticates the user locally by decrypting the KeyFile to get their KeyRing  and then logs into the encrypted database in the OFS.  Password and passphrase  are both listed as optional parameters, but at least one must be included.  If  the password is not included, the passphrase will be used to get the password  from the KeyFile.  If no passphrase was included when the KeyFile was created  the operation will fail.  All mappers will be created and initialized. | 
| `Task` | LogInAsync(`KeyRing` keyRing) | Authenticates the user locally by decrypting the KeyFile to get their KeyRing  and then logs into the encrypted database in the OFS.  Password and passphrase  are both listed as optional parameters, but at least one must be included.  If  the password is not included, the passphrase will be used to get the password  from the KeyFile.  If no passphrase was included when the KeyFile was created  the operation will fail.  All mappers will be created and initialized. | 
| `void` | Logout() | This will end an authenticated session.  All mappers, the data access, the OFS and  the KeyRing will be cleared from memory. | 
| `Task<Byte[]>` | RegisterAsync(`String` password, `String` passphrase) | This will create a new user stored in the OFS.  A KeyRing and UserId will be created for the user.  The KeyRing will be stored in the OFS as an encrypted KeyFile. All mappers will be created and initialized.  NOTE: Both the password and passphrase are required.  If they are not supplied an ArgumentException will be thrown. | 
| `Task` | RegisterAsync(`KeyRing` keyRing, `Byte[]` keyFileBytes) | This will create a new user stored in the OFS.  A KeyRing and UserId will be created for the user.  The KeyRing will be stored in the OFS as an encrypted KeyFile. All mappers will be created and initialized.  NOTE: Both the password and passphrase are required.  If they are not supplied an ArgumentException will be thrown. | 
| `Task<List<ContainerAccessLevel>>` | UpdateAccessAsync(`Guid` containerId, `List<ContainerAccessLevel>` accessLevels = null) | Update the access of the secured container with the given ID. | 
| `Task<SecuredContainer>` | UpdateAsync(`Guid` containerId, `Byte[]` content, `Object` customHeaderObject = null, `List<ContainerAccessLevel>` accessLevels = null, `String` type = null) | Update the content, header, access and type of the secured container with the given ID. | 
| `Task<SecuredContainer>` | UpdateContentAsync(`Guid` containerId, `Byte[]` content = null) | Update the content of the secured container with the given ID. | 
| `Task<SecuredContainer>` | UpdateHeaderAsync(`Guid` containerId, `Object` customHeaderObject = null) | Update the header of the secured container with the given ID. | 
| `Task` | UpdateTypeAsync(`Guid` containerId, `String` type = null) | Update the type of the secured container with the given ID. | 
| `Task<Boolean>` | ValidatePasswordAsync(`String` password, `Nullable<Guid>` userId = null) | Checks that the password is valid by decrypting a KeyFile stored in the OFS.  If no userId passed it, it will use the userId of the authenticated user.  If there is no authenticated user (the userId will be null) an InvalidOperationException  will be thrown. | 


## `ServerCacheOfsProvider`

Data provider that combines the ServerProvider and the OfsProvider.  The ServerProvider is treated and the main source for all data.  The OfsProvider is treated as a cache.  So, all create and update operations will be performed on the Absio Broker application and then  cached in the OFS.  All delete operations will be done against both the Absio Broker application and the OFS.  All read operations will  attempt to pull from the OFS (cache) and then from the Absio Broker application if not found.  Use the ForceLoadFromServer property  if you want read operations to skip the cache.  NOTE: the read will still cache the value in the OFS.  It will simply ensure  the data is refreshed from the Absio Broker application.
```csharp
public class Absio.Sdk.Providers.ServerCacheOfsProvider
    : BaseProvider, ICachingRules, IAbsioProvider

```

Properties

| Type | Name | Summary | 
| --- | --- | --- | 
| `Boolean` | ForceLoadFromServer | A flag that determines the usage of the OfsProvider as a source on read operations.  When true, this will skip the OfsProvider  and load data from the ServerProvider.  However, newly read data will still be cached in the OFS. | 
| `Boolean` | IsAuthenticated | True if both the OfsProvider and the ServerProvider are authenticated. | 
| `Boolean` | IsInitialized | True if the ServerProvider is initialized. | 
| `KeyRing` | KeyRing | The KeyRing for the authenticated user.  This will be null if the Provider is not authenticated.  Returns the KeyRing of the  ServerProvider. | 
| `OfsProvider` | OfsProvider | Gets the OfsProvider, which is used as the cache for data. | 
| `ServerProvider` | ServerProvider | Gets the ServerProvider which is used as the main source for data. | 
| `Nullable<Guid>` | UserId | The UserId as specified in the KeyRing.  This will be null if there is no KeyRing for the Provider (not authenticated). | 


Methods

| Type | Name | Summary | 
| --- | --- | --- | 
| `Task<Byte[]>` | ChangeCredentialsAsync(`String` password, `String` passphrase, `String` passphraseReminder = null) | This will change the credentials using the ServerProvider.  The resultant KeyFile bytes will be used to update the  KeyFile in the Ofs using the OfsProviders OfsKeyFileMapper. | 
| `Task<SecuredContainer>` | CreateAsync(`Byte[]` content, `Object` customHeaderObject = null, `List<ContainerAccessLevel>` accessLevels = null, `String` type = null) | Creates and persists a new SecuredContainer on the Absio Broker application, caches it in the OFS and returns it.  Use the returned value to learn the assigned ID. | 
| `Task` | DeleteAsync(`Guid` containerId) | Deletes the container with the given ID from the Absio Broker application and the OFS by calling DeleteAsync on the  ServerProvider and the OfsProvider. | 
| `Task` | DeleteUserAsync() | Deletes all associated data for the authenticated user.  Calls DeleteUserAsync on the ServerProvider and  the OfsProvider.  NOTE: This cannot be undone.  Ensure you really want to perform this operation before doing so.  All data related to the user will be removed from the Absio Broker application. | 
| `Task<IContainer>` | GetAsync(`Guid` containerId) | If the ForceLoadFromServer property is false this gets the Container from the OFS.  If the ForceLoadFromServer  is true or the OFS does not have the container, it is pulled from the server, cached in the OFS and returned  to the user. | 
| `Task<Byte[]>` | GetContentAsync(`Guid` containerId) | If the ForceLoadFromServer property is false this gets the Container content from the OFS.  If the ForceLoadFromServer  is true or the OFS does not have the container, it is pulled from the server, cached in the OFS and returned  to the user. | 
| `Task<List<ContainerEvent>>` | GetEventsAsync(`Nullable<EventActionType>` actionType = null, `Nullable<Int64>` startingId = null, `Nullable<Int64>` endingId = null, `Nullable<Guid>` containerId = null, `String` containerType = null) | This will call GetEventsAsync on the ServerProvider.    Gets all ContainerEvents that match the action type, starting id, ending id, container id and type.  No parameters are required.  If none are supplied this will return all ContainerEvents for all time for  the authenticated user.  This can be used to synchronize accounts or keep a store up-to-date with the  latest container activity related to the user. | 
| `Task<ContainerHeader>` | GetHeaderAsync(`Guid` containerId) | If the ForceLoadFromServer property is false this gets the Container header from the OFS.  If the ForceLoadFromServer  is true or the OFS does not have the container, it is pulled from the server, cached in the OFS and returned  to the user. | 
| `Task<ContainerMetadata>` | GetMetadataAsync(`Guid` containerId) | If the ForceLoadFromServer property is false this gets the Container metadata from the OFS.  If the ForceLoadFromServer  is true or the OFS does not have the container, it is pulled from the server and returned  to the user. | 
| `Task<String>` | GetReminderAsync(`Nullable<Guid>` userId = null) | This will call GetReminderAsync on the ServerProvider.    Gets the reminder for a user.  The reminder can be used to help a user recall their passphrase.  If the session is authenticated the user ID is not required.  If not supplied the authenticated user's  ID will be used. | 
| `void` | Initialize(`String` serverUrl, `String` apiKey, `String` applicationName, `String` ofsRootDirectory = null, `Boolean` partitionDataByUser = False) | Initialize the Ofs and Server Providers with the given parameters. | 
| `Task<Byte[]>` | LogInAsync(`Guid` userId, `String` password = null, `String` passphrase = null) | Authenticates the user by calling LoginAsync on both the OfsProvider and the ServerProvider.  If the  User does not have their KeyFile in the OFS yet, it will be pulled from the server.  The KeyRing will  be used to authenticate with the Absio Broker application.  This will create an authenticated session on  the ServerProvider and the OfsProvider.  If the KeyFile has not been stored in the OFS yet, you must supply  the passphrase. | 
| `Task` | LogInAsync(`KeyRing` keyRing) | Authenticates the user by calling LoginAsync on both the OfsProvider and the ServerProvider.  If the  User does not have their KeyFile in the OFS yet, it will be pulled from the server.  The KeyRing will  be used to authenticate with the Absio Broker application.  This will create an authenticated session on  the ServerProvider and the OfsProvider.  If the KeyFile has not been stored in the OFS yet, you must supply  the passphrase. | 
| `void` | Logout() | This will end the authenticated session by calling Logout on the ServerProvider and the OfsProvider | 
| `Task<Boolean>` | NeedToSyncAccountAsync(`Guid` userId) | This will check to see if the KeyFile stored in the OFS differs from the KeyFile stored on the Absio Broker application. | 
| `Task<Byte[]>` | RegisterAsync(`String` password, `String` passphrase, `String` reminder = null) | Registers a user on the Absio Broker application and then in the OFS. | 
| `Task` | SynchronizeAccountAsync(`String` passphrase, `String` password = null) | This pulls the KeyFile from the Absio Broker application, stores it into the OFS and then updates the KeyRing  in use on the ServerProvider and the OfsProvider. | 
| `Task<List<ContainerAccessLevel>>` | UpdateAccessAsync(`Guid` containerId, `List<ContainerAccessLevel>` accessLevels = null) | Update the access of the secured container with the given ID on the Absio Broker application  and then cache the resulting container in the OFS. | 
| `Task<List<ContainerAccessLevel>>` | UpdateAccessAsync(`Guid` containerId, `ContainerMetadata` metadata, `List<ContainerAccessLevel>` accessLevels = null) | Update the access of the secured container with the given ID on the Absio Broker application  and then cache the resulting container in the OFS. | 
| `Task<SecuredContainer>` | UpdateAsync(`Guid` containerId, `Byte[]` content = null, `Object` customHeaderObject = null, `List<ContainerAccessLevel>` accessLevels = null, `String` type = null) | Update the content, header, access and type of the secured container with the given ID on the Absio Broker application  and then cache the resulting container in the OFS. | 
| `Task<SecuredContainer>` | UpdateContentAsync(`Guid` containerId, `Byte[]` content = null) | Update the content of the secured container with the given ID on the Absio Broker application  and then cache the resulting container in the OFS. | 
| `Task<SecuredContainer>` | UpdateHeaderAsync(`Guid` containerId, `Object` customHeaderObject = null) | Update the header of the secured container with the given ID on the Absio Broker application  and then cache the resulting container in the OFS. | 
| `Task` | UpdateTypeAsync(`Guid` containerId, `String` type = null) | Update the type of the secured container with the given ID on the Absio Broker application  and then cache the resulting container in the OFS. | 


## `ServerProvider`

Provider that sources all data (containers, public keys, key files, users, etc from the Absio Broker application.  This provider serves as a user session with the Absio Broker application.  Use LoginAsync to start an authenticated  session.
```csharp
public class Absio.Sdk.Providers.ServerProvider
    : BaseProvider, IAbsioProvider

```

Properties

| Type | Name | Summary | 
| --- | --- | --- | 
| `EventsServerMapper` | EventsMapper | The mapper in charge of managing event data (all read operations). | 
| `Boolean` | IsAuthenticated | True if a user has successfully called LogInAsync.  Upon calling Logout, the Provider is no longer authenticated. | 
| `Boolean` | IsInitialized | True if the provider has been initialized.  See Initialize for more information. | 
| `KeyFileServerMapper` | KeyFileMapper | The mapper in charge of managing KeyFile data (all create, read, update and delete operations). | 
| `PublicKeyServerMapper` | PublicKeyMapper | The mapper in charge of managing public key data (signing and derivation) for other users in the eco-system  (all create, read, update and delete operations). | 
| `SecuredContainerServerMapper` | SecuredContainerMapper | The mapper in charge of managing secured container data (all create, read, update and delete operations). | 
| `UserServerMapper` | UserMapper | The mapper in charge of managing user data (all create, read and delete operations). | 


Methods

| Type | Name | Summary | 
| --- | --- | --- | 
| `Task<Byte[]>` | ChangeCredentialsAsync(`String` password, `String` passphrase, `String` reminder = null) | This is used to change the password and/or passphrase of a user.  This will cause the KeyFile to be re-encrypted with the new password (used for the KeyRing portion).  The encrypted KeyFile bytes and the hash of the passprhase are passed to the Absio Broker application for backup.  If a reminder is supplied, it will be passed as well.  The reminder can be used to help a user recall  their passphrase. | 
| `Task<SecuredContainer>` | CreateAsync(`Byte[]` content, `Object` customHeaderObject = null, `List<ContainerAccessLevel>` accessLevels = null, `String` type = null) | Creates and persists a new SecuredContainer on the Absio Broker application and returns it.  Use the returned value  to learn the assigned ID. | 
| `Task` | DeleteAsync(`Guid` containerId) | Deletes the secured container with the given ID from the Absio Broker application.  The encrypted content and associated  metadata will be removed. | 
| `Task` | DeleteUserAsync() | Deletes all associated data for the authenticated user.  NOTE: This cannot be undone.  Ensure you really want to perform this operation before doing so.  All data related to the user will be removed from the Absio Broker application. | 
| `void` | Dispose() | Dispose of the provider. Will dispose and null out all mappers. | 
| `Task<IContainer>` | GetAsync(`Guid` containerId) | Fetches an IContainer with the given ID. | 
| `Task<Byte[]>` | GetContentAsync(`Guid` containerId) | Gets the decrypted content for a secured container. | 
| `Task<List<ContainerEvent>>` | GetEventsAsync(`Nullable<EventActionType>` actionType = null, `Nullable<Int64>` startingId = null, `Nullable<Int64>` endingId = null, `Nullable<Guid>` containerId = null, `String` containerType = null) | Gets all ContainerEvents that match the action type, starting id, ending id, container id and type.  No parameters are required.  If none are supplied this will return all ContainerEvents for all time for  the authenticated user.  This can be used to synchronize accounts or keep a store up-to-date with the  latest container activity related to the user. | 
| `Task<ContainerHeader>` | GetHeaderAsync(`Guid` containerId) | Gets the decrypted header for a secured container. | 
| `Task<ContainerMetadata>` | GetMetadataAsync(`Guid` containerId) | Gets the metadata for a secured container. | 
| `Task<PublicKeyMetadata>` | GetPublicKeyIndexAsync(`Guid` userId, `KeyType` type, `Int32` index) | This gets the public key that match the user ID, type and index from the Absio Broker application. | 
| `Task<PublicKeyMetadata>` | GetPublicKeyLatestActiveAsync(`Guid` userId, `KeyType` type) | This gets the latest active public key that match the user ID and type from the Absio Broker application. | 
| `Task<List<PublicKeyMetadata>>` | GetPublicKeyListAsync(`Nullable<Guid>` userId, `Nullable<KeyType>` type = null, `Nullable<Int32>` index = null, `Nullable<Int32>` algorithmIndex = null) | This gets all public keys that match the user ID and type from the Absio Broker application. | 
| `Task<String>` | GetReminderAsync(`Nullable<Guid>` userId = null) | Gets the reminder for a user.  The reminder can be used to help a user recall their passphrase.  If the session is authenticated the user ID is not required.  If not supplied the authenticated user's  ID will be used. | 
| `Task<SecuredContainer>` | GetSecuredContainerAsync(`Guid` containerId) | This will get the secured container and associated metadata from the Absio Broker application.  No decryption will be performed. | 
| `void` | Initialize(`String` serverUrl, `String` apiKey, `String` applicationName) | Initialize the provider with the server URL, API Key and application name. | 
| `Task<Byte[]>` | LogInAsync(`Guid` userId, `String` password = null, `String` passphrase = null) | Authenticates the user with the Absio Broker application by decrypting the KeyFile.  The KeyFile will be requested from the Absio Broker application using the passphrase.  Once decrypted the KeyRing is used to create the authenticated session with  the Absio Broker application.  The Password is optional, but the passphrase is not.  If the password is not included, the passphrase will be used to get the password  from the KeyFile.  If no passphrase was included the operation will fail.  All mappers will be created and initialized. | 
| `Task` | LogInAsync(`KeyRing` keyRing) | Authenticates the user with the Absio Broker application by decrypting the KeyFile.  The KeyFile will be requested from the Absio Broker application using the passphrase.  Once decrypted the KeyRing is used to create the authenticated session with  the Absio Broker application.  The Password is optional, but the passphrase is not.  If the password is not included, the passphrase will be used to get the password  from the KeyFile.  If no passphrase was included the operation will fail.  All mappers will be created and initialized. | 
| `void` | Logout() | This will end an authenticated session.  The KeyRing and the authenticated credentials on the rest client  will be cleared from memory and | 
| `Task<Boolean>` | NeedToSyncAccountAsync(`Guid` userId, `Byte[]` keyFileBytes) | Checks if the encrypted KeyFile bytes are different on the Absio Broker application for the user.  This can be used to  determine if the KeyFile on the Absio Broker application needs to be synced locally. | 
| `Task<Byte[]>` | RegisterAsync(`String` password, `String` passphrase, `String` reminder = null) | This will create a new user stored on the Absio Broker application.  A KeyRing and UserId will be created for the user.  The encrypted KeyRing will be stored on the Absio Broker application as an encrypted KeyFile. All mappers will be created and initialized.  NOTE: Both the password and passphrase are required.  If they are not supplied an ArgumentException will be thrown. | 
| `Task<List<ContainerAccessLevel>>` | UpdateAccessAsync(`Guid` containerId, `List<ContainerAccessLevel>` accessLevels = null) | Update the access of the secured container with the given ID. | 
| `Task<List<ContainerAccessLevel>>` | UpdateAccessAsync(`Guid` containerId, `ContainerMetadata` metadata, `List<ContainerAccessLevel>` accessLevels = null) | Update the access of the secured container with the given ID. | 
| `Task<SecuredContainer>` | UpdateAsync(`Guid` containerId, `Byte[]` content = null, `Object` customHeaderObject = null, `List<ContainerAccessLevel>` accessLevels = null, `String` type = null) | Update the content, header, access and type of the secured container with the given ID. | 
| `Task<SecuredContainer>` | UpdateContentAsync(`Guid` containerId, `Byte[]` content = null) | Update the content of the secured container with the given ID. | 
| `Task<SecuredContainer>` | UpdateHeaderAsync(`Guid` containerId, `Object` customHeaderObject = null) | Update the header of the secured container with the given ID. | 
| `Task` | UpdateTypeAsync(`Guid` containerId, `String` type = null) | Update the type of the secured container with the given ID. | 



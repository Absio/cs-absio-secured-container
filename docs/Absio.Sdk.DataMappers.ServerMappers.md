## `AuthenticatedServerCredentials`

This represents the credentials for an authenticated session with the Absio Broker application.  It holds the signing key used to authenticate, the  KeyRing that came from as well as the token used to help create the one time use, time sensitive tokens for communications with the Absio  Broker application.
```csharp
public class Absio.Sdk.DataMappers.ServerMappers.AuthenticatedServerCredentials

```

Properties

| Type | Name | Summary | 
| --- | --- | --- | 
| `SigningKey` | AuthenticatedSigningKey | The SigningKey that was used to authenticate this user.  Null if not authenticated. | 
| `DateTime` | Expiration | The expiration of the current token | 
| `KeyRing` | KeyRing | The KeyRing of the authenticated user. | 


## `EventsServerMapper`

This maps all Event data from the Absio Broker application.  All read operations relating to container events handled by the mapper.
```csharp
public class Absio.Sdk.DataMappers.ServerMappers.EventsServerMapper
    : AbstractServerMapper

```

Methods

| Type | Name | Summary | 
| --- | --- | --- | 
| `void` | Dispose() | Dispose of the mapper. | 
| `Task<EventPackageJson>` | GetAsync(`Nullable<EventActionType>` actionType = null, `Nullable<EventType>` eventType = null, `Nullable<Int64>` startingId = null, `Nullable<Int64>` endingId = null, `Nullable<Guid>` containerId = null, `String` containerType = null) | Gets all ContainerEvents that match the action type, starting id, ending id, container id and type.  No parameters are required.  If none are supplied this will return all ContainerEvents for all time for  the authenticated user.  This can be used to synchronize accounts or keep a store up-to-date with the  latest container activity related to the user. | 


## `KeyFileServerMapper`

This maps all KeyFile data to/from the Absio Broker application.  All create, read, update and delete operations relating to  raw KeyFile data (encrypted bytes) are handled by the mapper.  No logic for encrypting or decrypting exists in the mapper.
```csharp
public class Absio.Sdk.DataMappers.ServerMappers.KeyFileServerMapper
    : AbstractServerMapper

```

Methods

| Type | Name | Summary | 
| --- | --- | --- | 
| `Task` | CreateOrUpdateAsync(`Guid` userId, `String` passphrase, `String` reminder, `Byte[]` encryptedKeyFileBlob) | Creates or updates the KeyFile on the Absio Broker application.  The passphrase is used to securely store the KeyFile on the server.  The reminder can be used to help the user recall their passphrase. | 
| `Task` | DeleteAsync() | Deletes the authenticated user's KeyFile from the Absio Broker application. | 
| `void` | Dispose() | Dispose of the mapper. This will null out the hashing service and dispose of the rest client. | 
| `Task<Boolean>` | DoesChecksumMatchAsync(`Guid` userId, `Byte[]` encryptedKeyFile) | Determines if the provided KeyFile matches the KeyFile currently loaded on the Absio Broker application for the authenticated user. | 
| `Task<Byte[]>` | GetAsync(`Guid` userId, `String` passphrase) | Gets a user's KeyFile. | 
| `Task<Boolean>` | GetChecksumAsync(`Guid` userId, `String` checksum) | Checks if a KeyFile checksum matches the Absio Broker application's for a user. | 
| `Task<String>` | GetReminderAsync(`Guid` userId) | Gets the reminder for a user.  The reminder can be used to help a user recall their passphrase.  If the session is authenticated the user ID is not required.  If not supplied the authenticated user's  ID will be used. | 
| `Task<Boolean>` | ValidateSecurityPassphrase(`Guid` userId, `String` passphrase) | Validates the user's security passphrase against the current encrypted KeyRing on the Absio Broker application. | 


## `PublicKeyServerMapper`

This maps all public key data to/from the Absio Broker application.  All create, read, update and delete  operations relating to public keys (signing and derivation) for other users in the system are handled by the mapper.  No logic  for encrypting or decrypting exists in the mapper.
```csharp
public class Absio.Sdk.DataMappers.ServerMappers.PublicKeyServerMapper
    : AbstractServerMapper, IPublicKeySource

```

Methods

| Type | Name | Summary | 
| --- | --- | --- | 
| `Task` | CreateOrUpdateAsync(`PublicKeyMetadata` keyMetadata) | Creates or updates the public key on the Absio Broker application. | 
| `Task` | CreateOrUpdateAsync(`Byte[]` publicKey, `KeyType` type, `Int32` index, `Int32` algorithmIndex, `Boolean` active) | Creates or updates the public key on the Absio Broker application. | 
| `void` | Dispose() | Dispose of the mapper. | 
| `Task<PublicKeyMetadata>` | GetIndexAsync(`Guid` userId, `KeyType` type, `Int32` index) | This gets the public key that match the user ID, type and index from the Absio Broker application.  This calls GetAsync.  It is needed for the IPublicKeySource interface. | 
| `Task<PublicKeyMetadata>` | GetLatestActiveAsync(`Guid` userId, `KeyType` type) | This gets the latest active public key that match the user ID and type from the Absio Broker application.  This calls GetListAsync and processes the list for active keys.  It is needed for the IPublicKeySource interface. | 
| `Task<List<PublicKeyMetadata>>` | GetListAsync(`Nullable<Guid>` userId = null, `Nullable<KeyType>` type = null, `Nullable<Int32>` index = null, `Nullable<Int32>` algorithmIndex = null) | Gets all public keys matching the user ID, key type, key ring index and algorithm index. | 


## `SecuredContainerServerMapper`

This maps all secured container data to/from the Absio Broker application (metadata is stored in the server database and the encrypted  data is stored in S3 by default). All create, read, update and delete operations relating to secured containers are handled by  the mapper.  No logic  for encrypting or decrypting exists in the mapper.
```csharp
public class Absio.Sdk.DataMappers.ServerMappers.SecuredContainerServerMapper
    : AbstractServerMapper

```

Methods

| Type | Name | Summary | 
| --- | --- | --- | 
| `Task` | CreateOrUpdateAsync(`SecuredContainer` container) | Writes the encrypted content of the container in S3 by default and the metadata is uploaded to the Absio Broker application. | 
| `Task` | DeleteAsync(`Guid` containerId) | Deletes the encrypted content of the container and deletes the metadata records from the Absio Broker application. | 
| `void` | Dispose() | Dispose of the mapper. | 
| `Task<SecuredContainer>` | GetAsync(`Guid` containerId) | Returns the secured container from the Absio Broker application.  This will have the encrypted data as well as the  metadata from the Absio Broker application. | 
| `Task<ContainerInfoJson>` | GetInfoAsync(`Guid` containerId) | Gets the ContainerInfoJson for a container from the Absio Broker application.  This is the metadata of the container. | 
| `Task` | UpdateAccessLevelsAsync(`Guid` containerId, `List<ContainerAccessLevel>` accessLevels) | Updates the access for a container on the Absio Broker application. | 
| `Task` | UpdateDataAsync(`SecuredContainer` container) | Updates the content on the Absio Broker application (content goes to S3 by default and the metadata changes will go to the server directly). | 
| `Task` | UpdateTypeAsync(`Guid` containerId, `String` type) | Update the type of a container on the Absio Broker application. | 


## `ServerMapperRestClientSession`

This class wraps the IRestClient (the actual REST client used for communication with the Absio Broker application) and the credentials  (AuthenticatedServerCredentials) if the IRestClient is being used in a authenticated session with the Absio Broker application.
```csharp
public class Absio.Sdk.DataMappers.ServerMappers.ServerMapperRestClientSession

```

Properties

| Type | Name | Summary | 
| --- | --- | --- | 
| `AuthenticatedServerCredentials` | Credentials | Get the credentidals for the authenticated session with the Absio Broker application.  This is null if the session is not authenticated. | 
| `IRestClient` | RestClient | Get the REST client used for communicating with the Absio Broker application. | 


Methods

| Type | Name | Summary | 
| --- | --- | --- | 
| `void` | Dispose() | Dispose of the rest client and null the credentials. | 


## `UserServerMapper`

This handles all user operations with the Absio Broker application.  It is used for create, delete and authentication of users.
```csharp
public class Absio.Sdk.DataMappers.ServerMappers.UserServerMapper
    : AbstractServerMapper

```

Methods

| Type | Name | Summary | 
| --- | --- | --- | 
| `Task<Nullable<Guid>>` | CreateAsync(`List<PublicKeyMetadata>` publicKeys) | Create a user on the Absio Broker application. | 
| `Task` | DeleteAsync() | Deletes the user from the server. | 
| `void` | Dispose() | Dispose of the mapper. | 
| `Task` | LogInAsync(`KeyRing` keyRing) | Uses the provided KeyRing to authenticate with the Absio Broker application.  If successful the credentials for the  authenticated session will be set on the ServerMapperRestClientSession of the mapper. | 



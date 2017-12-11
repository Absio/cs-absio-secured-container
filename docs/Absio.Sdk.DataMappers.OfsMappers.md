## `KeyFileOfsMapper`

This maps all KeyFile data to/from the OFS.  All create, read, update and delete operations relating to  raw KeyFile data (encrypted bytes) are handled by the mapper.  No logic for encrypting or decrypting exists in the mapper.
```csharp
public class Absio.Sdk.DataMappers.OfsMappers.KeyFileOfsMapper
    : AbstractOfsMapper

```

Methods

| Type | Name | Summary | 
| --- | --- | --- | 
| `Task<Boolean>` | CreateOrUpdateAsync(`Guid` userId, `Byte[]` encryptedKeyFileBlob, `Boolean` overwrite = True) | Creates or updates the user's KeyFile bytes in the OFS, possibly creating both directory and file. | 
| `Task` | DeleteAsync(`Guid` userId) | Delete the users KeyFile from the OFS. | 
| `Task<Byte[]>` | GetAsync(`Guid` userId) | Get the encrypted KeyFile bytes from the OFS. | 


## `PublicKeyOfsMapper`

This maps all public key data to/from the OFS (it is stored in the encrypted database).  All create, read, update and delete  operations relating to public keys (signing and derivation) for other users in the system are handled by the mapper.  No logic  for encrypting or decrypting exists in the mapper.
```csharp
public class Absio.Sdk.DataMappers.OfsMappers.PublicKeyOfsMapper
    : AbstractOfsMapper, IPublicKeySource

```

Methods

| Type | Name | Summary | 
| --- | --- | --- | 
| `Task` | CreateOrUpdateAsync(`PublicKeyMetadata` keyMetadata) | Creates or updates the public key in the encrypted database in the OFS. | 
| `Task` | CreateOrUpdateAsync(`Guid` userId, `Boolean` active, `Int32` index, `KeyType` type, `Byte[]` key, `Int32` algorithmIndex) | Creates or updates the public key in the encrypted database in the OFS. | 
| `Task` | DeleteAsync(`Guid` userId, `Nullable<KeyType>` type, `Nullable<Int32>` index) | This deletes all public keys that match the user ID, type and index from the encrypted database in the OFS. | 
| `Task<PublicKeyMetadata>` | GetAsync(`Guid` userId, `KeyType` type, `Int32` index) | This gets the public key that match the user ID, type and index from the encrypted database in the OFS. | 
| `Task<PublicKeyMetadata>` | GetIndexAsync(`Guid` userId, `KeyType` type, `Int32` index) | This gets the public key that match the user ID, type and index from the encrypted database in the OFS.  This calls GetAsync.  It is needed for the IPublicKeySource interface. | 
| `Task<PublicKeyMetadata>` | GetLatestActiveAsync(`Guid` userId, `KeyType` type) | This gets the latest active public key that match the user ID and type from the encrypted database in the OFS.  This calls GetListAsync and processes the list for active keys.  It is needed for the IPublicKeySource interface. | 
| `Task<List<PublicKeyMetadata>>` | GetListAsync(`Guid` userId, `KeyType` type) | This gets all public keys that match the user ID and type from the encrypted database in the OFS. | 


## `SecuredContainerOfsMapper`

This maps all secured container data to/from the OFS (metadata is stored in the encrypted database and the encrypted data is stored in the file system).  All create, read, update and delete operations relating to secured containers are handled by the mapper.  No logic  for encrypting or decrypting exists  in the mapper.
```csharp
public class Absio.Sdk.DataMappers.OfsMappers.SecuredContainerOfsMapper
    : AbstractOfsMapper

```

Methods

| Type | Name | Summary | 
| --- | --- | --- | 
| `Task` | CreateAsync(`Byte[]` securedContainerBytes, `ContainerMetadata` metadata, `String` ofsLocation, `Nullable<DateTime>` syncedAt = null) | Writes the encrypted content of the container in the file system and the metadata in the encrypted database. | 
| `Task` | DeleteAllAsync() | This will delete all secured containers from the OFS.  All encrypted content and metadata database records will be deleted. | 
| `Task` | DeleteAsync(`Guid` containerId) | Deletes the encrypted content of the container from the file system and deletes the metadata records from the encrypted database. | 
| `Task<List<ContainerDbInfo>>` | GetAllInfoAsync() | Gets all the ContainerDbInfo (container metadata) that is stored in the encrypted database in the OFS.  Will return an empty list if there are no container records in the database. | 
| `Task<SecuredContainer>` | GetAsync(`Guid` containerId) | Returns the secured container from the Ofs.  This will have the encrypted data from the file system as well as the  metadata from the encrypted database. | 
| `Task<ContainerDbInfo>` | GetInfoAsync(`Guid` containerId) | Gets the ContainerDbInfo for a container from the database.  This is the metadata of the container. | 
| `Task` | UpdateAsync(`Byte[]` securedContainerBytes, `ContainerMetadata` metadata, `String` ofsLocation, `Nullable<DateTime>` syncedAt = null) | Updates the encrypted content of the container in the file system and the metadata in the encrypted database. | 
| `Task` | UpdateInDataAccessAsync(`ContainerMetadata` metadata, `String` ofsLocation, `Nullable<DateTime>` syncedAt) | Updates the container metadata, OFS location and the last synced time in the data access.  This will update the access records  and the container record. | 


## `UserOfsMapper`

This maps all user data to/from the OFS (the user ID is stored in the encrypted database and is the primary key for public keys).  All create, read, update and delete operations relating to users are handled by the mapper.  The public key mapper is used to  ensure that a deleted user has all their public keys removed from the database.  The data access is used to store users ids  in the database, representing "users".
```csharp
public class Absio.Sdk.DataMappers.OfsMappers.UserOfsMapper
    : AbstractOfsMapper

```

Methods

| Type | Name | Summary | 
| --- | --- | --- | 
| `Task` | CreateAsync(`Guid` userId) | Adds the user ID to the encrypted database in the OFS. | 
| `Task` | DeleteAsync(`Guid` userId) | Remove the user ID from the encrypted database in the OFS.  This will also delete all public keys stored in the encrypted database for the user ID. | 
| `Task<Nullable<Guid>>` | GetAsync(`Guid` userId) | Get the user ID from the encrypted database in the OFS.  Returns null if the user is not in the database. | 



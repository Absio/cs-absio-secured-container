## `IPublicKeySource`

An interface to supply public key metadata for users in the Absio eco-system.  This is used by the ContainerEncryptionService for encryption and decryption operations.
```csharp
public interface Absio.Sdk.DataMappers.IPublicKeySource

```

Methods

| Type | Name | Summary | 
| --- | --- | --- | 
| `Task<PublicKeyMetadata>` | GetIndexAsync(`Guid` userId, `KeyType` type, `Int32` index) | This gets the public key that match the user ID, type and index. | 
| `Task<PublicKeyMetadata>` | GetLatestActiveAsync(`Guid` userId, `KeyType` type) | This gets the latest active public key that match the user ID and type. | 



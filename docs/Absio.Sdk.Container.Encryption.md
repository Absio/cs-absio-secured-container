## `ContainerEncryptionService`

Provides encryption and decryption services for IContainer and SecuredContainer for a specific user (KeyRing) and public key source (IPublicKeySource).  The encryption service will perform cryptographic operations for the user that owns the KeyRing (encryption of IContainers into SecuredContainers and  decryption of SecuredContainers into IContainers). The service will use the IPublicKeySource for getting all public signing and derivation keys of other  users needed to perform the cryptographic operations.  Public keys of the user will be retrieved from the KeyRing itself.  Out of the box, the OfsProvider  uses the PublicKeyOfsMapper for all public keys of other users.  The PublicKeyOfsMapper implements IPublicKeySource.  The ServerProvider uses  the PublicKeyServerMapper for all public keys of other users.  The PublicKeyServerMapper also implements IPublicKeySource.  If you want to map public keys  yourself, you will want your class to implement the IPublicKeySource interface so the ContainerEncryptionService can be used.
```csharp
public class Absio.Sdk.Container.Encryption.ContainerEncryptionService

```

Methods

| Type | Name | Summary | 
| --- | --- | --- | 
| `Task<IContainer>` | DecryptAsync(`SecuredContainer` securedContainer) | Decrypts the SecuredContainer content and header and returns an IContainer. | 
| `Task<IContainer>` | DecryptContentAsync(`SecuredContainer` securedContainer) | Decrypts the SecuredContainer content and returns an IContainer with the decrypted content and the metadata. | 
| `Task<IContainer>` | DecryptHeaderAsync(`SecuredContainer` securedContainer) | Decrypts the SecuredContainer header and returns an IContainer with the decrypted header and the metadata. | 
| `Task<SecuredContainer>` | EncryptAsync(`IContainer` container) | Encrypts the IContainer and returns a SecuredContainer. | 
| `Task` | GenerateKeyBlobForEachUserAccessAsync(`List<ContainerAccessLevel>` containerAccessLevels, `Guid` containerId, `ContainerMetadata` containerMetadata) | Generates an encrypted copy of the SecuredContainer encryption key for each user in the access list  of a container.  The encrypted key is then set on each ContainerAccessLevel.KeyBlob property. | 


Static Methods

| Type | Name | Summary | 
| --- | --- | --- | 
| `PublicKeyMetadata` | GetLatestActiveKey(`List<PublicKeyMetadata>` keys) | Given the list of PublicKeyMetadata, find the "latest" active key.  This is key with the highest index that is also active. | 


